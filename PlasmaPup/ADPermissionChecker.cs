using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Security.AccessControl;
using PlasmaPup;
using System.ComponentModel;
using System.Diagnostics;

class ADPermissionChecker
{
    public delegate void ReportProgressDelegate(int progress);
    private ReportProgressDelegate _reportProgress;
    private BackgroundWorker _worker; // Assuming you pass the BackgroundWorker to use for cancellation checking

    private List<PermissionInfo> permissionInfos = new List<PermissionInfo>();
    ReportParameters rp = new ReportParameters();
    private Dictionary<string, RecursiveGroupMemberCacheItem> groupMembershipCache = new Dictionary<string, RecursiveGroupMemberCacheItem>();
    private Dictionary<string, string> distinguishedNameCache = new Dictionary<string, string>();
    private Dictionary<string, string> dnToDomainUserCache = new Dictionary<string, string>();
    private Dictionary<string, string> dnToObjectTypeCache = new Dictionary<string, string>();
    private Dictionary<string, TimeSpan> methodTimings = new Dictionary<string, TimeSpan>();


    int iTotalPermissions = 0;

    public ADPermissionChecker(ReportParameters rp1, ReportProgressDelegate reportProgress, BackgroundWorker worker)
    {
        rp = rp1;
        _reportProgress = reportProgress;
        _worker = worker; // Assign the passed worker to the local variable
    }

     public List<PermissionInfo> CheckPermissions()
    {
        using (DirectoryEntry de = new DirectoryEntry(rp.RootOU))
        {
            EvaluatePermissions(de);
        }

        // After we've checked everything within the OU, also check permissions on GPOs linked to parent OUs
        CheckParentOUGPOs(rp.RootOU);


        if (_worker.CancellationPending)
        {
            return null;
        }
        return permissionInfos;
    }

    private void EvaluatePermissions(DirectoryEntry entry)
    {
        ActiveDirectorySecurity security = entry.ObjectSecurity;
        if (security == null) 
        {
            return;
        }
        AuthorizationRuleCollection acl = security.GetAccessRules(true, true, typeof(NTAccount));
        bool bSkip = false;



        // Extract and cache the object type directly from the DirectoryEntry if not already cached
        string distinguishedName = entry.Properties["distinguishedName"].Value.ToString();
        string objectType = entry.SchemaClassName; // Directly use the SchemaClassName as the object type
        if (!dnToObjectTypeCache.ContainsKey(distinguishedName))
        {
            dnToObjectTypeCache[distinguishedName] = objectType; // Cache the object type
        }

        foreach (ActiveDirectoryAccessRule rule in acl)
        {
            if (_worker.CancellationPending)
            {
                return;
            }

            if ((rule.ActiveDirectoryRights & ActiveDirectoryRights.WriteDacl) == ActiveDirectoryRights.WriteDacl ||
                (rule.ActiveDirectoryRights & ActiveDirectoryRights.WriteOwner) == ActiveDirectoryRights.WriteOwner ||
                (rule.ActiveDirectoryRights & ActiveDirectoryRights.WriteProperty) == ActiveDirectoryRights.WriteProperty ||
                (rule.ActiveDirectoryRights & ActiveDirectoryRights.Self) == ActiveDirectoryRights.Self)
            {
                string sInherited = "Unknown";
                if (rule.IsInherited)
                {
                    sInherited = "Yes";
                }
                else
                {
                    sInherited = "No";
                }
                PermissionInfo PermInfo = new PermissionInfo()
                {
                    Subject = rule.IdentityReference.Value,
                    Object = entry.Path.Substring(entry.Path.IndexOf('/') + 2),
                    ObjectType = entry.SchemaClassName,
                    Permissions = rule.ActiveDirectoryRights.ToString(),
                    Path = "",
                    Inherited = sInherited
                };

                if (PermInfo.Subject.Contains("NT AUTHORITY\\SYSTEM") ||
                    PermInfo.Subject.Contains("NT AUTHORITY\\SELF") ||
                    PermInfo.Subject.Contains("CREATOR OWNER"))
                    continue;
                if (rp.IgnoreDAs)
                {
                    if (PermInfo.Subject.Contains("Exchange Trusted Subsystem") ||
                        PermInfo.Subject.Contains("Exchange Enterprise Servers") ||
                        PermInfo.Subject.Contains("Exchange Servers") ||
                        PermInfo.Subject.Contains("Exchange Recipient Administrators") ||
                        PermInfo.Subject.Contains("Exchange Windows Permissions") ||
                        PermInfo.Subject.Contains("Organization Management") ||
                        PermInfo.Subject.Contains("RTCDomainUserAdmins") ||
                        PermInfo.Subject.Contains("RTCUniversalUserAdmins") ||
                        PermInfo.Subject.Contains("BUILTIN\\Administrators") ||
                        PermInfo.Subject.Contains("Enterprise Key Admins") ||
                        PermInfo.Subject.Contains("\\Key Admins") ||
                        PermInfo.Subject.Contains("\\Cert Publishers") ||
                        PermInfo.Subject.Contains("S-1-5-32-548") ||
                        PermInfo.Subject.Contains("S-1-5-32-561") ||
                        PermInfo.Subject.Contains("Enterprise Admins") ||
                        PermInfo.Subject.Contains("Domain Admins")
                        )
                        continue;
                }
                if(rp.IgnoreAccounts.Count > 0) //Add this later
                {
                    bSkip = false;
                    foreach (var account in rp.IgnoreAccounts)
                    {
                        if (PermInfo.Subject.EndsWith("\\" + account, StringComparison.OrdinalIgnoreCase))
                            bSkip = true;
                        if (PermInfo.Subject.Equals(account, StringComparison.OrdinalIgnoreCase))
                            bSkip = true;
                    }
                    if (bSkip)
                        continue;
                }

                //If we have a group for the SUBJECT, we need to evaluate all user/computer objects that gain access through it, recursively.
                if (rule.IdentityReference is NTAccount ntAccount)
                {
                    // Resolve the distinguished name of the object
                    string sDistinguishedName = GetDistinguishedName(rule.IdentityReference);
                    
                    //If it's a group, get recursive users/computers
                    string referencedObjectType = GetObjectType(sDistinguishedName);
                    if (referencedObjectType == "group")
                    {
                        // Recursively evaluate permissions for all members of the group
                        EvaluateGroupMembersForPermissions(sDistinguishedName, PermInfo);             
                    }
                }
                PermInfo.Subject = rule.IdentityReference.Value;
                if (PermInfo.Path.Length < 1)
                {
                    PermInfo.Path = "Direct";
                }
                permissionInfos.Add(PermInfo);
                iTotalPermissions++;
                _reportProgress?.Invoke(iTotalPermissions);
            }
        }

        // Check if the current entry is an OU before evaluating linked GPOs
        if (!rp.IgnoreGPOs)
        {
            if (entry.SchemaClassName == "organizationalUnit")
            {
                EvaluateLinkedGPOs(entry, true);
            }
        }

        // Recursive search into child objects
        if (rp.RecursiveCheck)
        {
            foreach (DirectoryEntry childEntry in entry.Children)
            {
                EvaluatePermissions(childEntry);
            }
        }
    }


    private void CheckParentOUGPOs(string startingOU)
    {
        DirectoryEntry currentEntry = new DirectoryEntry(startingOU);

        while (currentEntry.Parent != null && currentEntry.SchemaClassName == "organizationalUnit")
        {
            if (_worker.CancellationPending)
            {
                return;
            }

            // Move up to the parent OU
            currentEntry = currentEntry.Parent;

            // Check for linked GPOs at this OU level
            EvaluateLinkedGPOs(currentEntry, false);

            // Optional: Check for cancellation
            if (_worker.CancellationPending) break;
        }

        // Now, check for GPOs linked directly to the domain root
        // This assumes 'currentEntry' is now at the highest level OU or the domain root
        // You might need to adjust this logic to ensure you're correctly targeting the domain root
        if (currentEntry.SchemaClassName == "domainDNS" || currentEntry.Parent == null)
        {
            EvaluateLinkedGPOs(currentEntry, false);
        }
    }


    private void EvaluateLinkedGPOs(DirectoryEntry ouEntry, bool checkChildOUs)
    {
        // Read the gPLink attribute, which contains linked GPOs
        string gPLinkValue = ouEntry.Properties["gPLink"]?.Value as string;

        //DN of the OU it's linked to
        string ouDistinguishedName = ouEntry.Properties["distinguishedName"].Value.ToString();

        if (!string.IsNullOrEmpty(gPLinkValue))
        {
            // gPLinkValue format: [LDAP://cn={GPO-ID},cn=policies,cn=system,DC=domain,DC=com;0]
            string[] linkedGPOs = gPLinkValue.Split('[');
            foreach (string gpoLink in linkedGPOs)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(gpoLink))
                    {
                        string gpoDn = gpoLink.Split(';')[0];
                        DirectoryEntry gpoEntry = new DirectoryEntry(gpoDn);
                        string gpoName = gpoEntry.Properties["displayName"].Value as string;
                        // Evaluate permissions for the GPO
                        EvaluatePermissionsForGPO(gpoEntry, ouDistinguishedName, gpoName);
                    }
                }
                catch { } //For now, if an exception is thrown reading these, we move on.  May add a debug reporting option later.

            }
        }

        if (checkChildOUs)
        {
            // Recursive search into child OUs
            foreach (DirectoryEntry childEntry in ouEntry.Children)
            {
                if (_worker.CancellationPending)
                {
                    return;
                }
                if (childEntry.SchemaClassName == "organizationalUnit")
                {
                    EvaluateLinkedGPOs(childEntry, true);
                }
            }
        }

    }


    private void EvaluatePermissionsForGPO(DirectoryEntry gpoEntry, string linkedOUName, string gpoName)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();


        ActiveDirectorySecurity gpoSecurity = gpoEntry.ObjectSecurity;
        AuthorizationRuleCollection acl = gpoSecurity.GetAccessRules(true, true, typeof(NTAccount));

        foreach (ActiveDirectoryAccessRule rule in acl)
        {
            if (_worker.CancellationPending)
            {
                return;
            }
            if (ShouldSkipAccount(GetDistinguishedName(rule.IdentityReference)))
                continue;
            // Check if permission allows changes to the GPO
            if ((rule.ActiveDirectoryRights & ActiveDirectoryRights.GenericAll) == ActiveDirectoryRights.GenericAll ||
                (rule.ActiveDirectoryRights & ActiveDirectoryRights.WriteDacl) == ActiveDirectoryRights.WriteDacl ||
                (rule.ActiveDirectoryRights & ActiveDirectoryRights.WriteProperty) == ActiveDirectoryRights.WriteProperty)
            {
                // Determine if the subject is a group
                if (IsGroup(rule.IdentityReference))
                {
                    // If the subject is a group, evaluate permissions for its members
                    string groupDistinguishedName = GetDistinguishedName(rule.IdentityReference);

                    PermissionInfo permInfo = new PermissionInfo
                    {
                        Subject = rule.IdentityReference.Value, // The DN of the group or user in domain\user format
                        Object = $"{gpoName}, linked to:\n{linkedOUName}", // Description of the GPO being accessed
                        ObjectType = "GPO", // The type of object, in this case, a GPO
                        Permissions = rule.ActiveDirectoryRights.ToString(), // The permissions granted by the rule
                        Path = "Direct", // Initially, the path is direct since we're evaluating the GPO itself
                        Inherited = rule.IsInherited ? "Yes" : "No" // Whether the permission is inherited
                    };
                    AddPermissionInfoForGPO(rule.IdentityReference.Value, "Direct", rule, gpoEntry, linkedOUName, gpoName); // Pass "Direct" as the path for direct permissions
                    permInfo.Path = "";
                    EvaluateGroupMembersForGPOPermissions(groupDistinguishedName, permInfo);
                }
                else
                {
                    // Direct user permissions on GPO
                    AddPermissionInfoForGPO(rule.IdentityReference.Value, "Direct", rule, gpoEntry, linkedOUName, gpoName); // Pass "Direct" as the path for direct permissions

                }
            }
        }


        stopwatch.Stop();

        // Update the cumulative time
        lock (methodTimings)
        {
            if (methodTimings.ContainsKey(nameof(EvaluatePermissionsForGPO)))
            {
                methodTimings[nameof(EvaluatePermissionsForGPO)] += stopwatch.Elapsed;
            }
            else
            {
                methodTimings[nameof(EvaluatePermissionsForGPO)] = stopwatch.Elapsed;
            }
        }
    }

    private void EvaluateGroupMembersForGPOPermissions(string groupDistinguishedName, PermissionInfo permInfo)
    {
        var memberDistinguishedNames = GetMembers(groupDistinguishedName); // Assuming this retrieves DNs of members
        string membershipPath = permInfo.Path + " => " + ConvertToDomainAccountFormat(groupDistinguishedName);

        foreach (string memberDn in memberDistinguishedNames)
        {
            string memberType = GetObjectType(memberDn);
            if (memberType == "user")
            {
                // Construct or modify a PermissionInfo instance for the user
                var memberPermInfo = new PermissionInfo
                {
                    Subject = ConvertToDomainAccountFormat(memberDn),
                    Object = permInfo.Object,
                    ObjectType = permInfo.ObjectType,
                    Permissions = permInfo.Permissions,
                    Path = membershipPath, // Update path to include group membership hierarchy
                    Inherited = permInfo.Inherited
                };

                // Add the PermissionInfo to your permissions list or process it as needed
                permissionInfos.Add(memberPermInfo);
            }
            else if (memberType == "group")
            {
                // For nested groups, recursively call this method with updated PermissionInfo
                var nestedGroupPermInfo = new PermissionInfo
                {
                    // Update or copy fields from permInfo 
                    Subject = ConvertToDomainAccountFormat(memberDn), // Update the subject to the current group/user
                    Object = permInfo.Object, // The object being accessed remains the same
                    ObjectType = permInfo.ObjectType, // The type of object being accessed remains the same
                    Permissions = permInfo.Permissions, // The permissions being evaluated remain the same
                    Path = membershipPath, // Update path to include this group
                    Inherited = permInfo.Inherited // Inherited status remains as per the parent PermissionInfo
                };
                permissionInfos.Add(nestedGroupPermInfo);
                EvaluateGroupMembersForGPOPermissions(memberDn, nestedGroupPermInfo);
            }
        }
    }


    private void AddPermissionInfoForGPO(string sDomainUser, string path, ActiveDirectoryAccessRule rule, DirectoryEntry gpoEntry, string linkedOUName, string gpoName)
    {
        PermissionInfo permInfo = new PermissionInfo()
        {
            Subject = sDomainUser,
            Object = $"{gpoName}, linked to:\n{linkedOUName}",
            ObjectType = "GPO",
            Permissions = rule.ActiveDirectoryRights.ToString(),
            Path = path, // Use the passed path, which can be "Direct" or a group membership path
            Inherited = rule.IsInherited ? "Yes" : "No"
        };

        permissionInfos.Add(permInfo);
        iTotalPermissions++;
        _reportProgress?.Invoke(iTotalPermissions);
    }



    private bool IsGroup(IdentityReference identityReference)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();



        // Attempt to resolve the distinguished name from cache
        string distinguishedName;
        if (!distinguishedNameCache.TryGetValue(identityReference.Value, out distinguishedName))
        {
            // If not in cache, resolve distinguished name
            distinguishedName = GetDistinguishedName(identityReference);
            if (distinguishedName != null)
            {
                // Cache the distinguished name for future use
                distinguishedNameCache[identityReference.Value] = distinguishedName;
            }
        }

        if (string.IsNullOrEmpty(distinguishedName))
        {
            return false; // Could not resolve distinguished name
        }

        // Check if the object type is cached for the distinguished name
        string objectType;
        if (!dnToObjectTypeCache.TryGetValue(distinguishedName, out objectType))
        {
            // If not in cache, determine the object type
            objectType = GetObjectType(distinguishedName);
            if (objectType != null)
            {
                // Cache the object type for future use
                dnToObjectTypeCache[distinguishedName] = objectType;
            }
        }



        stopwatch.Stop();

        // Update the cumulative time
        lock (methodTimings)
        {
            if (methodTimings.ContainsKey(nameof(IsGroup)))
            {
                methodTimings[nameof(IsGroup)] += stopwatch.Elapsed;
            }
            else
            {
                methodTimings[nameof(IsGroup)] = stopwatch.Elapsed;
            }
        }



        return objectType.Equals("group", StringComparison.OrdinalIgnoreCase);
    }



    private bool ShouldSkipAccount(string accountName)
    {
        if (accountName.Contains("NT AUTHORITY\\SYSTEM") ||
            accountName.Contains("NT AUTHORITY\\SELF") ||
            accountName.Contains("CREATOR OWNER"))
            return true;
        if (rp.IgnoreDAs)
        {
            if (accountName.Contains("Exchange Trusted Subsystem") ||
                accountName.Contains("Exchange Enterprise Servers") ||
                accountName.Contains("Exchange Servers") ||
                accountName.Contains("Exchange Windows Permissions") ||
                accountName.Contains("Organization Management") ||
                accountName.Contains("RTCDomainUserAdmins") ||
                accountName.Contains("RTCUniversalUserAdmins") ||
                accountName.Contains("BUILTIN\\Administrators") ||
                accountName.Contains("Enterprise Key Admins") ||
                accountName.Contains("\\Key Admins") ||
                accountName.Contains("\\Cert Publishers") ||
                accountName.Contains("S-1-5-32-548") ||
                accountName.Contains("S-1-5-32-561") ||
                accountName.Contains("Enterprise Admins") ||
                accountName.Contains("Domain Admins")
                )
                return true;
        }
        if (rp.IgnoreAccounts.Count > 0) //Add this later
        {
            foreach (var account in rp.IgnoreAccounts)
            {
                if (accountName.EndsWith("\\" + account, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (accountName.Equals(account, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }
        return false; // Placeholder implementation
    }


    public string GetDistinguishedName(IdentityReference identityReference)
    {
        string accountName = identityReference.Value;

        // Check cache first
        if (distinguishedNameCache.TryGetValue(accountName, out string cachedDistinguishedName))
        {
            return cachedDistinguishedName;
        }

        // If not in cache, query AD
        string distinguishedName = null;
        using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
        {
            Principal principal = Principal.FindByIdentity(context, accountName);
            if (principal != null)
            {
                DirectoryEntry de = principal.GetUnderlyingObject() as DirectoryEntry;
                distinguishedName = de.Properties["distinguishedName"].Value as string;

                // Cache the result
                if (distinguishedName != null)
                {
                    distinguishedNameCache[accountName] = distinguishedName;
                }
            }
            else
            {
                // Just send back what we have, as we can't get DN
                distinguishedName = accountName;
                // Also, place this in the cache so we don't take forever in these cases
                distinguishedNameCache[accountName] = accountName;
            }
        }

        return distinguishedName ?? throw new InvalidOperationException($"Distinguished Name not found for {accountName}.");
    }


    public string GetObjectType(string distinguishedName)
    {
        // Check if the object type is already cached
        if (dnToObjectTypeCache.TryGetValue(distinguishedName, out string objectType))
        {
            return objectType; // Return the cached object type
        }

        // Object type not in cache, so query Active Directory
        try
        {
            using (DirectoryEntry de = new DirectoryEntry($"LDAP://{distinguishedName}"))
            {
                // The objectClass property contains the type(s) of the AD object
                PropertyValueCollection objectClasses = de.Properties["objectClass"];
                if (objectClasses != null)
                {
                    // Typically, the most specific class type is the last one in the list
                    string mostSpecificType = objectClasses[objectClasses.Count - 1].ToString();

                    // Map the specific class type to user, group, or computer
                    // This mapping might need adjustments based on your AD schema
                    if (mostSpecificType.Equals("user", StringComparison.OrdinalIgnoreCase))
                    {
                        objectType = "user";
                    }
                    else if (mostSpecificType.Equals("group", StringComparison.OrdinalIgnoreCase))
                    {
                        objectType = "group";
                    }
                    else if (mostSpecificType.Equals("computer", StringComparison.OrdinalIgnoreCase))
                    {
                        objectType = "computer";
                    }
                    else
                    {
                        objectType = "unknown"; // Or adjust based on your needs
                    }

                    // Cache the determined object type
                    dnToObjectTypeCache[distinguishedName] = objectType;

                    return objectType;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, such as access denied, network issues, etc.
            Console.WriteLine($"Error determining object type for {distinguishedName}: {ex.Message}");
        }

        return "error"; // Indicate an error occurred in determining the object type
    }


    private void EvaluateGroupMembersForPermissions(string distinguishedName, PermissionInfo PermInfo)
    {
        RecursiveGroupMemberCacheItem cacheItem;
        bool bSkip = false;

        // Check if this group's members have already been evaluated and cached
        if (groupMembershipCache.TryGetValue(distinguishedName, out cacheItem))
        {
            // If the group's members are fully resolved, use the cached data
            if (cacheItem.IsComplete)
            {
                foreach (var member in cacheItem.MemberNames)
                {
                    foreach (var memberPath in member.Value)
                    {
                        if (_worker.CancellationPending)
                        {
                            return;
                        }

                        string sPathToAdd = "";
                        //Now we need to get the short name for this DN (memberDN)...
                        string sDomainUser = ConvertToDomainAccountFormat(member.Key);

                        if (memberPath.Length < 1)
                        {
                            sPathToAdd = "Direct";
                        }
                        else
                        {
                            sPathToAdd = memberPath;
                        }
                        if (rp.IgnoreAccounts.Count > 0) //Add this later
                        {
                            bSkip = false;
                            foreach (var account in rp.IgnoreAccounts)
                            {
                                if (sDomainUser.EndsWith("\\" + account, StringComparison.OrdinalIgnoreCase))
                                    bSkip = true;
                                if (sDomainUser.Equals(account, StringComparison.OrdinalIgnoreCase))
                                    bSkip = true;
                            }
                            if (bSkip)
                                continue;
                        }
                        permissionInfos.Add(new PermissionInfo
                        {
                            Subject = sDomainUser,
                            Object = PermInfo.Object,
                            ObjectType = PermInfo.ObjectType,
                            Permissions = PermInfo.Permissions,
                            Path = sPathToAdd,
                            Inherited = PermInfo.Inherited
                        });
                        iTotalPermissions++;
                        _reportProgress?.Invoke(iTotalPermissions);
                    }
                }
                return; // Exit as we've processed this group from the cache
            }
        }
        else
        {
            // Initialize cache item if this group hasn't been processed
            cacheItem = new RecursiveGroupMemberCacheItem
            {
                MemberNames = new Dictionary<string, List<string>>(),
                IsComplete = false // Will set to true once all members are evaluated
            };
            groupMembershipCache[distinguishedName] = cacheItem;
        }

        // If here, we need to recursively evaluate the group's members
        foreach (var memberDn in GetMembers(distinguishedName))
        {
            string memberType = GetObjectType(memberDn);
            string memberPath = PermInfo.Path + " => " + ConvertToDomainAccountFormat(distinguishedName);
            string sPathToAddforGroup = "";

            if (_worker.CancellationPending)
            {
                return;
            }

            if (memberType == "user" || memberType == "computer")
            {
                //Now we need to get the short name for this DN (memberDN)...
                string sDomainUser = ConvertToDomainAccountFormat(memberDn);

                if (memberPath.Length < 1)
                {
                    sPathToAddforGroup = "Direct";
                }
                else
                {
                    sPathToAddforGroup = memberPath;
                }
                if (rp.IgnoreAccounts.Count > 0) //Add this later
                {
                    bSkip = false;
                    foreach (var account in rp.IgnoreAccounts)
                    {
                        if (sDomainUser.EndsWith("\\" + account, StringComparison.OrdinalIgnoreCase))
                            bSkip = true;
                        if (sDomainUser.Equals(account, StringComparison.OrdinalIgnoreCase))
                            bSkip = true;
                    }
                    if (bSkip)
                        continue;
                }
                permissionInfos.Add(new PermissionInfo
                {
                    Subject = sDomainUser,
                    Object = PermInfo.Object,
                    ObjectType = PermInfo.ObjectType,
                    Permissions = PermInfo.Permissions,
                    Path = sPathToAddforGroup,
                    Inherited = PermInfo.Inherited
                });
                iTotalPermissions++;
                _reportProgress?.Invoke(iTotalPermissions);

                // Update the cache
                if (!cacheItem.MemberNames.ContainsKey(memberDn))
                {
                    cacheItem.MemberNames[memberDn] = new List<string>();
                }
                cacheItem.MemberNames[memberDn].Add(memberPath);
            }
            else if (memberType == "group")
            {
                // Recursive call for nested group
                EvaluateGroupMembersForPermissions(memberDn, new PermissionInfo
                {
                    Subject = PermInfo.Subject,
                    Object = PermInfo.Object,
                    ObjectType = PermInfo.ObjectType,
                    Permissions = PermInfo.Permissions,
                    Path = memberPath,
                    Inherited = PermInfo.Inherited
                });
            }
        }

        // After processing all members, mark this group as complete in the cache
        cacheItem.IsComplete = true;
    }



    public IEnumerable<string> GetMembers(string groupDistinguishedName)
    {
        List<string> memberDistinguishedNames = new List<string>();

        try
        {
            using (DirectoryEntry groupEntry = new DirectoryEntry($"LDAP://{groupDistinguishedName}"))
            {
                // Retrieve the member attribute
                groupEntry.RefreshCache(new string[] { "member" });
                PropertyValueCollection members = groupEntry.Properties["member"];

                foreach (var member in members)
                {
                    string memberDN = member.ToString();
                    memberDistinguishedNames.Add(memberDN);
                }
            }
        }
        catch (Exception ex)
        {
            // Log or handle the exception as needed
            Console.WriteLine($"Error retrieving members for {groupDistinguishedName}: {ex.Message}");
        }

        return memberDistinguishedNames;
    }


    private string GetDomainUsernameOrGroupName(string distinguishedName)
    {
        string domainUsernameOrGroupName = string.Empty;

        using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
        {
            // Attempt to find as user
            UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.DistinguishedName, distinguishedName);
            if (user != null)
            {
                SecurityIdentifier sid = user.Sid;
                NTAccount account = (NTAccount)sid.Translate(typeof(NTAccount));
                domainUsernameOrGroupName = account.Value;
            }
            else
            {
                // Attempt to find as group if user is not found
                GroupPrincipal group = GroupPrincipal.FindByIdentity(context, IdentityType.DistinguishedName, distinguishedName);
                if (group != null)
                {
                    SecurityIdentifier sid = group.Sid;
                    NTAccount account = (NTAccount)sid.Translate(typeof(NTAccount));
                    domainUsernameOrGroupName = account.Value;
                }
            }
        }

        if (domainUsernameOrGroupName.Length > 0)
            return domainUsernameOrGroupName;
        else
            return distinguishedName;
    }


    private string ConvertToDomainAccountFormat(string identityReferenceValue)
    {
        // Check if the value is already in DOMAIN\User format or if it's cached
        if (identityReferenceValue.Contains("\\"))
        {
            return identityReferenceValue; // Already in DOMAIN\User format
        }
        else if (dnToDomainUserCache.TryGetValue(identityReferenceValue, out string cachedValue))
        {
            return cachedValue; // Return the cached DOMAIN\User value
        }
        else
        {
            // Perform the conversion, assuming GetDomainUsername does the actual conversion
            string domainUser = GetDomainUsernameOrGroupName(identityReferenceValue);

            // Cache the result for future use
            dnToDomainUserCache[identityReferenceValue] = domainUser;

            return domainUser;
        }
    }

}

public class GroupMembershipCache
{
    private Dictionary<string, RecursiveGroupMemberCacheItem> cache = new Dictionary<string, RecursiveGroupMemberCacheItem>();

    public bool TryGetMembers(string groupName, out RecursiveGroupMemberCacheItem cacheItem)
    {
        return cache.TryGetValue(groupName, out cacheItem);
    }

    public void AddOrUpdate(string groupName, RecursiveGroupMemberCacheItem cacheItem)
    {
        cache[groupName] = cacheItem;
    }
}


