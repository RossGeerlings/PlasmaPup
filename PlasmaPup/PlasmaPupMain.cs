using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using Microsoft.Reporting.WinForms;


namespace PlasmaPup
{
    public partial class PlasmaPupMain : Form
    {
        private DirectoryEntry Base;
        private bool bIsDrillthroughSubscribed = false;
        List<PermissionInfo> permissions = new List<PermissionInfo>();

        private class TrNodeAndDEList
        {
            public TreeViewEventArgs TVEA;
            public List<DirectoryEntry> ChildNodeList = new List<DirectoryEntry>();
        }

        public PlasmaPupMain()
        {
            InitializeComponent();
            Connect();
            panelFiltersActions.Paint += new PaintEventHandler(FilterActionPanel_Paint);

            // Create a PictureBox for the information icon
            PictureBox informationIcon = new PictureBox();

            // Set the icon to the Information system icon
            informationIcon.Image = SystemIcons.Information.ToBitmap();

            // Set the size and mode for the PictureBox
            informationIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            informationIcon.Size = new Size(24, 24); // Ensure this is a suitable size for your panel

            // Set the location within the panel (this will be relative to the panel's top-left corner)
            informationIcon.Location = new Point(480, 600); // Change this to position the icon where you want it within the panel

            // Optionally, add a tooltip to the icon
            ToolTip infoToolTip = new ToolTip();
            infoToolTip.SetToolTip(informationIcon, "About this application");

            // Add a click event handler to show the About
            string InfoString = "PlasmaPup\nVersion 1.0\n\n" + 
                                "PlasmaPup is designed to help central and departmental IT personnel understand their exposures in Active Directory by showing " +
                                "which accounts have permissions to make changes within their OU(s) or modify group policy applying to thier OU(s).\n\n" +
                                "PlasmaPup was written by Ross Geerlings and is licensed under the terms of the MIT license, reproduced below.\n\nThe MIT License\n\n" +
                                "Copyright (c) 2024 The University of Michigan Board of Regents.\n\nPermission is hereby granted, free of charge, to any person obtaining " +
                                "a copy of this software and associated documentation fime-les (the \"Software\"), to deal in the Software without restriction, including " +
                                "without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to " +
                                "permit persons to whom the Software is furnished to do so, subject to the following conditions:\n\nThe above copyright notice and this " + 
                                "permission notice shall be included in all copies or substantial portions of the Software.\n\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT " +
                                "WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE " +
                                "AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN " +
                                "ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE " + 
                                "SOFTWARE.\n\n" +
                                "The icon used on the additional account button in the PlasmaPup application is made by Icons8. Their icons can be viewed at " + 
                                "https://www.icons8.com.";
            informationIcon.Click += (sender, e) => MessageBox.Show(InfoString);

            // Add the PictureBox to the panel's controls
            panelFiltersActions.Controls.Add(informationIcon);

        }

        private void Connect()
        {
            try
            {
                Base = new DirectoryEntry();
                this.MinimumSize = this.Size;

                //Read the root:
                if (Base != null)
                {
                    tvADTree.Nodes.Clear();
                    tvADTree.BeginUpdate();
                    TreeNode childNode = tvADTree.Nodes.Add(Base.Name);
                    childNode.Tag = Base;

                    try
                    {
                        foreach (DirectoryEntry rootIter in Base.Children)
                        {
                            if (rootIter.SchemaClassName == "organizationalUnit" || rootIter.SchemaClassName == "container" || rootIter.SchemaClassName == "computer")
                            {
                                TreeNode RootNode = childNode.Nodes.Add(rootIter.Name);
                                RootNode.Tag = rootIter;
                            }
                        }
                    }
                    finally
                    {
                        childNode.Expand();
                        tvADTree.EndUpdate();
                    }
                }
            }
            catch (Exception eEx)
            {
                //HandleError(eEx.Message + "...Are you running as a domain user?");
            }
        }

        private void tvADMTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                DirectoryEntry deCurrent = (DirectoryEntry)e.Node.Tag;
                tvADTree.Enabled = false;
                bgwGetTVChildEntries.RunWorkerAsync(e);
            }
        }

        private void bgwGetTVChildEntries_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 1;
            TrNodeAndDEList TrNodeAndDEChildren = new TrNodeAndDEList();
            TrNodeAndDEChildren.TVEA = e.Argument as TreeViewEventArgs;
            DirectoryEntry parent = (DirectoryEntry)TrNodeAndDEChildren.TVEA.Node.Tag;
            //TrNodeAndDEChildren.ChildNodeList = new List<DirectoryEntry>();

            if (parent != null)
            {
                if ((parent.Children != null) && (TrNodeAndDEChildren.TVEA.Node.Checked == false))
                {
                    int iLimit = 3000;
                    foreach (DirectoryEntry Iter in parent.Children)
                    {
                        if (Iter.SchemaClassName == "organizationalUnit" || Iter.SchemaClassName == "container")
                        {
                            TrNodeAndDEChildren.ChildNodeList.Add(Iter);
                        }

                        i++;

                        if (i == 300) { bgwGetTVChildEntries.ReportProgress(1); }

                        if (i == iLimit)
                        {
                            //If there are a ton of objects directly under OU/container, ask if we should continue
                            if (MessageBox.Show("Iterating through this OU/container's child objects has " +
                                        "already yielded over " + iLimit.ToString() + " objects (incluing some which " +
                                        "may not be of types displayed in this tree).  Select yes to continue " +
                                        "retrieving objects (may take several moments) or no to use only objects " +
                                        "retrieved so far.", "Continue?", MessageBoxButtons.YesNo)
                                                                            == DialogResult.No)
                            {
                                break;
                            }

                            else { iLimit *= 5; }
                        }
                    }
                }
            }
            e.Result = TrNodeAndDEChildren;
        }

        private void bgwGetTVChildEntries_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TrNodeAndDEList TreeNodeAndDEChildren = e.Result as TrNodeAndDEList;
            tvADTree.BeginUpdate();
            int i;
            for (i = 0; i < TreeNodeAndDEChildren.ChildNodeList.Count; i++)
            {
                TreeNode childNode = TreeNodeAndDEChildren.TVEA.Node.Nodes.Add(TreeNodeAndDEChildren.ChildNodeList[i].Name);
                childNode.Tag = TreeNodeAndDEChildren.ChildNodeList[i];
            }
            TreeNodeAndDEChildren.TVEA.Node.Checked = true;
            tvADTree.EndUpdate();
            pbADTreeWait1.Visible = false;
            tvADTree.Enabled = true;
            tvADTree.Focus();
        }

        private void bgwGetTVChildEntries_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbADTreeWait1.Visible = true;
        }

        private void FilterActionPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Determine the position of the line
            //int lineY = panelFiltersActions.Height / 2; // For example, in the middle of the panel
            int lineY = 320;

            // Create pens with different colors for the shadow and highlight lines
            Pen shadowPen = new Pen(Color.Gray); // Darker color for shadow
            Pen highlightPen = new Pen(Color.White); // Lighter color for highlight

            // Draw the shadow line
            g.DrawLine(shadowPen, new Point(0, lineY), new Point(panelFiltersActions.Width, lineY));

            // Draw the highlight line right below the shadow line to create the indented effect
            g.DrawLine(highlightPen, new Point(0, lineY + 1), new Point(panelFiltersActions.Width, lineY + 1));
        }

        private void PlasmaPupMain_Load(object sender, EventArgs e)
        {

            this.rvSubjectSummary.RefreshReport();
        }

        private void bgwGenerateReport_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            ReportParameters rp = (ReportParameters)e.Argument;

            ADPermissionChecker checker = new ADPermissionChecker(rp, progress =>
            {
                // Use Invoke to marshal the call to the UI thread if needed
                this.Invoke((MethodInvoker)delegate
                {
                    bgwGenerateReport.ReportProgress(0, progress); // Use userState to pass the actual count
                });
            }, worker);
            permissions = new List<PermissionInfo>(); // Wipe in case we had something previous
            permissions = checker.CheckPermissions();

            // Check for cancellation request
            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return; // Exit early if cancellation was requested
            }


            e.Result = permissions;

        }

        private void bgwGenerateReport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int count = (int)e.UserState;
            lblPermsTotal.Text = count.ToString();
        }

        private void bgwGenerateReport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                List<PermissionInfo> permissions = e.Result as List<PermissionInfo>;

                var subjectCounts = permissions
                            .GroupBy(info => info.Subject)
                            .Select(group => new
                            {
                                Subject = group.Key,
                                Count = group.Count()
                            })
                            .OrderByDescending(item => item.Count)
                            .ToList();

                DataTable dtSubjectCounts = new DataTable();
                dtSubjectCounts.Columns.Add("Subject", typeof(string));
                dtSubjectCounts.Columns.Add("Count", typeof(int));

                foreach (var item in subjectCounts)
                {
                    dtSubjectCounts.Rows.Add(item.Subject, item.Count);
                }

                //For ease of using reportviewer with data, we're making the datatable rows instances of a class
                var permissionInfoSummaries = from DataRow row in dtSubjectCounts.Rows
                                              select new PermissionInfoSummary
                                              {
                                                  Subject = row["Subject"].ToString(),
                                                  Count = Convert.ToInt32(row["Count"])
                                              };

                rvSubjectSummary.LocalReport.ReportPath = "SubjectSummary.rdlc";
                ReportDataSource dataSource = new ReportDataSource("DataSet1", permissionInfoSummaries.ToList());
                rvSubjectSummary.LocalReport.DataSources.Clear();
                rvSubjectSummary.LocalReport.DataSources.Add(dataSource);
                rvSubjectSummary.RefreshReport();
                if (bIsDrillthroughSubscribed)
                {
                    rvSubjectSummary.Drillthrough -= reportViewer_Drillthrough;
                }
                rvSubjectSummary.Drillthrough += new Microsoft.Reporting.WinForms.DrillthroughEventHandler(this.reportViewer_Drillthrough);

                bIsDrillthroughSubscribed = true;

                tabControl1.SelectedIndex = 1;
            }

            if (e.Cancelled)
                MessageBox.Show("Cancelled.");

            lblPermsTotal.Text = "0";
            tvADTree.Enabled = true;
            cbIncludeGPOs.Enabled = true;
            cbDomainAdmins.Enabled = true;
            cbRecursive.Enabled = true;
            btnIgnoreAccts.Enabled = true;
            btnRemoveIgnored.Enabled = true;
            btnGenerateReport.Enabled = true;
            panelProgress.Visible = false;
        }


        private void reportViewer_Drillthrough(object sender, DrillthroughEventArgs e)
        {
            // Prevent the drillthrough action from actually navigating to another report
            e.Cancel = true;

            LocalReport localReport = (LocalReport)e.Report;
            //var parameters = localReport.GetParameters();
            ReportParameterInfoCollection parameters = localReport.GetParameters();

            // Get the data from the report
            string sSubject = parameters["Subject"].Values[0];
            DataTable permissionsTable = new DataTable();
            permissionsTable.Columns.Add("Subject", typeof(string));
            permissionsTable.Columns.Add("Object", typeof(string));
            permissionsTable.Columns.Add("ObjectType", typeof(string));
            permissionsTable.Columns.Add("Permissions", typeof(string));
            permissionsTable.Columns.Add("Path", typeof(string));
            permissionsTable.Columns.Add("Inherited", typeof(string));

            foreach (var permission in permissions)
            {
                if (permission.Subject == sSubject)
                {
                    permissionsTable.Rows.Add(permission.Subject, permission.Object, permission.ObjectType, permission.Permissions, permission.Path, permission.Inherited);
                }
            }


            SubjectAccessDetailForm SubjectAccessDetailFrm = new SubjectAccessDetailForm(sSubject, permissionsTable);
            SubjectAccessDetailFrm.StartPosition = FormStartPosition.CenterParent;
            SubjectAccessDetailFrm.Show();

        }



        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (tvADTree.SelectedNode == null)
            {
                MessageBox.Show("You must first select a node in the treeview that represents an OU in Active Directory.");
                return;
            }
            if (!tvADTree.SelectedNode.Text.StartsWith("OU="))
            {
                MessageBox.Show("You must first select a node in the treeview that represents an OU in Active Directory.");
                return;
            }
            tvADTree.Enabled = false;
            cbIncludeGPOs.Enabled = false;
            cbDomainAdmins.Enabled = false;
            cbRecursive.Enabled = false;
            btnIgnoreAccts.Enabled = false;
            btnRemoveIgnored.Enabled = false;
            btnGenerateReport.Enabled = false;
            panelProgress.Visible = true;
            ReportParameters rp = new ReportParameters();
            DirectoryEntry de = (DirectoryEntry)tvADTree.SelectedNode.Tag;
            rp.RootOU = de.Path;
            rp.IgnoreAccounts = new List<string>();
            foreach (var item in lbUserAccountsToIgnore.Items)
            {
                rp.IgnoreAccounts.Add(item.ToString());
            }
            rp.IgnoreDAs = cbDomainAdmins.Checked;
            rp.IgnoreGPOs = !cbIncludeGPOs.Checked;
            rp.RecursiveCheck = cbRecursive.Checked;
            bgwGenerateReport.RunWorkerAsync(rp);
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel collection and report generation?  The cancellation may take a moment to occur.", "Confirm Cancel", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                // Check if the background worker is running
                if (bgwGenerateReport.IsBusy)
                {
                    // Request cancellation
                    bgwGenerateReport.CancelAsync();
                }
            }
            else
            {
                //Nothing to do at this time.
            }
        }


        private void btnIgnoreAccts_Click(object sender, EventArgs e)
        {
            AddAccountExclusion addAccountExclusion = new AddAccountExclusion(this);
            addAccountExclusion.StartPosition = FormStartPosition.CenterParent;
            addAccountExclusion.ShowDialog();
        }


        public void AddUniqueItemToUserExclusions(string sUser)
        {
            if (!lbUserAccountsToIgnore.Items.Contains(sUser))
            {
                lbUserAccountsToIgnore.Items.Add(sUser);
            }
            else
            {
                MessageBox.Show("User already exists in the list.");
            }
        }


        private void btnRemoveIgnored_Click(object sender, EventArgs e)
        {
            if (lbUserAccountsToIgnore.SelectedIndex != -1)
            {
                lbUserAccountsToIgnore.Items.RemoveAt(lbUserAccountsToIgnore.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Please select an item to delete.");
            }
        }


        static string GetDomainLdapPath()
        {
            try
            {
                Domain domain = Domain.GetCurrentDomain();
                return $"LDAP://{domain.Name.Replace(".", ",DC=")}";
            }
            catch (ActiveDirectoryOperationException)
            {
                // Handle the case where the domain is not available or the computer is not domain-joined
                return null;
            }
        }

    }


    public class ReportParameters
    {
        public string RootOU { get; set; }
        public List<string> IgnoreAccounts { get; set; }
        public bool IgnoreDAs { get; set; }
        public bool IgnoreGPOs { get; set; }
        public bool RecursiveCheck { get; set; }
    }


    public class PermissionInfo
    {
        public string Subject { get; set; }
        public string Object { get; set; }
        public string ObjectType { get; set; }
        public string Permissions { get; set; }
        public string Path { get; set; }
        public string Inherited { get; set; }
    }


    public class RecursiveGroupMemberCacheItem
    {
        // Maps member names to their respective list of recursion paths
        public Dictionary<string, List<string>> MemberNames { get; set; } = new Dictionary<string, List<string>>();
        public bool IsComplete { get; set; }
    }


    public class PermissionInfoSummary
    {
        public string Subject { get; set; }
        public int Count { get; set; }
    }

}
