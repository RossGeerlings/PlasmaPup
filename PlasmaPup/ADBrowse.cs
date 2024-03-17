using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;

namespace Seeker
{
    public partial class ADBrowse : Form
    {
        private DirectoryEntry Base;
        public string sTreeType;
        private class TrNodeAndDEList
        {
            public TreeViewEventArgs TVEA;
            public List<DirectoryEntry> ChildNodeList = new List<DirectoryEntry>();
        }

        public ADBrowse()
        {
            //sTreeType = sCallType;
            InitializeComponent();
            //if (sTreeType == "ADQ")
            //{
            this.Text = "Select Root OU for Query";
            btnQueryRootOU.Text = "Query Workstations Within Selected OU";
            //}
            //else
            //{
            //    this.Text = "Select Nodes to be Returned";
            //    btnQueryRootOU.Text = "Return Selected OUs and Computers";
            //}
            Connect();
        }


        private void Connect()
        {
            Base = new DirectoryEntry();
            
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
                        //MessageBox.Show(rootIter.Properties["objectClass"].ToString());
                        //if ( rootIter.Properties["objectClass"].ToString() == "O);
                        if (rootIter.SchemaClassName == "organizationalUnit" || rootIter.SchemaClassName == "container")
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

        private void tvADTree_AfterSelect(object sender, TreeViewEventArgs e)
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
                if ( (parent.Children != null) && (TrNodeAndDEChildren.TVEA.Node.Checked == false))
                {
                    //NodesToAdd.Add(parent);
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
                //pbADTreeWait1.Visible = false;
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
                //MessageBox.Show(NodeList[i].Name.ToString());
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

        private void btnQueryRootOU_Click(object sender, EventArgs e)
        {
            List<string> lRoots = new List<string>();
            if (tvADTree.SelectedNode == null)
            {
                //tvADTree.SelectedNode = N
                TreeNodeCollection tncNodes = tvADTree.Nodes;
                if (tncNodes.Count > 0)
                {
                    // Select the root node
                    tvADTree.SelectedNode = tncNodes[0];
                }

            }
            DirectoryEntry selItem = (DirectoryEntry)tvADTree.SelectedNode.Tag;
            //string sRootString = selItem.Path;
            lRoots.Add(selItem.Path);
            //*** Function gone, frmParent.RunADQuery(lRoots);
            this.Close();
        }
    
    }
}
