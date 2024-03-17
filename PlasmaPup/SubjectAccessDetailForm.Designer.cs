namespace PlasmaPup
{
    partial class SubjectAccessDetailForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubjectAccessDetailForm));
            this.rvSubjectDetail = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // rvSubjectDetail
            // 
            this.rvSubjectDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rvSubjectDetail.Location = new System.Drawing.Point(0, 0);
            this.rvSubjectDetail.Name = "rvSubjectDetail";
            this.rvSubjectDetail.PageCountMode = Microsoft.Reporting.WinForms.PageCountMode.Actual;
            this.rvSubjectDetail.ServerReport.BearerToken = null;
            this.rvSubjectDetail.Size = new System.Drawing.Size(1424, 831);
            this.rvSubjectDetail.TabIndex = 0;
            // 
            // SubjectAccessDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1424, 831);
            this.Controls.Add(this.rvSubjectDetail);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SubjectAccessDetailForm";
            this.Text = "Subject Access Details";
            this.Load += new System.EventHandler(this.SubjectAccessDetailForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rvSubjectDetail;
    }
}