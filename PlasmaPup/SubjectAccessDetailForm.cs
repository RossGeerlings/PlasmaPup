using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlasmaPup
{
    public partial class SubjectAccessDetailForm : Form
    {
        public DataTable dtSubjectDetail = new DataTable();

        public SubjectAccessDetailForm(string sSubject, DataTable permissionsTable)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            dtSubjectDetail = permissionsTable;
            this.Text += " for " + sSubject;
        }

        private void SubjectAccessDetailForm_Load(object sender, EventArgs e)
        {

            //this.rvSubjectDetail.RefreshReport();

            var permissionInfoDetail = from DataRow row in dtSubjectDetail.Rows
                                          select new PermissionInfo
                                          {
                                              Subject = row["Subject"].ToString(),
                                              Object = row["Object"].ToString(),
                                              ObjectType = row["ObjectType"].ToString(),
                                              Permissions = row["Permissions"].ToString(),
                                              Path = row["Path"].ToString(),
                                              Inherited = row["Inherited"].ToString()
                                          };

            rvSubjectDetail.LocalReport.ReportPath = "SubjectDetail.rdlc";
            ReportDataSource dataSource = new ReportDataSource("DataSetSubjectDetail", permissionInfoDetail.ToList());
            rvSubjectDetail.LocalReport.DataSources.Clear();
            rvSubjectDetail.LocalReport.DataSources.Add(dataSource);
            rvSubjectDetail.RefreshReport();
        }
    }

}
