using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlasmaPup
{
    public partial class AddAccountExclusion : Form
    {
        private PlasmaPupMain frmParent;

        public AddAccountExclusion(PlasmaPupMain frmCalling)
        {
            InitializeComponent();
            frmParent = frmCalling;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {   
            try
            {
                DirectoryEntry root = new DirectoryEntry();

                string sFilter = "(&(objectClass=User)(CN=" + tbUserToExclude.Text + "))";

                DirectorySearcher searcherResults = new DirectorySearcher(root);
                searcherResults.Filter = sFilter;

                SearchResult searchResults = searcherResults.FindOne();

                if (searchResults != null)
                {
                    btnAdd.Enabled = true;
                    btnValidateUser.Enabled = false;
                    btnClear.Enabled = true;
                    tbUserToExclude.Enabled = false;
                    lblValidated.Visible = true;
                }
                else
                {
                    MessageBox.Show("User Not Found");
                }
            }
            catch
            {
                MessageBox.Show("Error retreiving information from Active Directory.");
            }
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmParent.AddUniqueItemToUserExclusions(tbUserToExclude.Text);
            this.Close();
        }

        private void tbUserToExclude_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnValidateUser.Select();
                btnValidate_Click(sender, e);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbUserToExclude.Text = "";
            tbUserToExclude.Enabled = true;
            btnValidateUser.Enabled = true;
            btnAdd.Enabled = false;
            lblValidated.Visible = false;
        }

    }
}
