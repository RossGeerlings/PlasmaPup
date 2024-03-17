namespace PlasmaPup
{
    partial class PlasmaPupMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlasmaPupMain));
            this.bgwGetTVChildEntries = new System.ComponentModel.BackgroundWorker();
            this.tpReport = new System.Windows.Forms.TabPage();
            this.rvSubjectSummary = new Microsoft.Reporting.WinForms.ReportViewer();
            this.tpTarget = new System.Windows.Forms.TabPage();
            this.panelFiltersActions = new System.Windows.Forms.Panel();
            this.cbRecursive = new System.Windows.Forms.CheckBox();
            this.btnRemoveIgnored = new System.Windows.Forms.Button();
            this.lbUserAccountsToIgnore = new System.Windows.Forms.ListBox();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblPermsTotal = new System.Windows.Forms.Label();
            this.lblGenerating = new System.Windows.Forms.Label();
            this.lblPermsReported = new System.Windows.Forms.Label();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnIgnoreAccts = new System.Windows.Forms.Button();
            this.cbIncludeGPOs = new System.Windows.Forms.CheckBox();
            this.cbDomainAdmins = new System.Windows.Forms.CheckBox();
            this.lblFilters = new System.Windows.Forms.Label();
            this.panelADTree = new System.Windows.Forms.Panel();
            this.pbADTreeWait1 = new System.Windows.Forms.PictureBox();
            this.tvADTree = new System.Windows.Forms.TreeView();
            this.lblTreeview = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.bgwGenerateReport = new System.ComponentModel.BackgroundWorker();
            this.tpReport.SuspendLayout();
            this.tpTarget.SuspendLayout();
            this.panelFiltersActions.SuspendLayout();
            this.panelProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelADTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbADTreeWait1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgwGetTVChildEntries
            // 
            this.bgwGetTVChildEntries.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwGetTVChildEntries_DoWork);
            this.bgwGetTVChildEntries.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwGetTVChildEntries_ProgressChanged);
            this.bgwGetTVChildEntries.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwGetTVChildEntries_RunWorkerCompleted);
            // 
            // tpReport
            // 
            this.tpReport.Controls.Add(this.rvSubjectSummary);
            this.tpReport.Location = new System.Drawing.Point(4, 22);
            this.tpReport.Name = "tpReport";
            this.tpReport.Padding = new System.Windows.Forms.Padding(3);
            this.tpReport.Size = new System.Drawing.Size(1056, 654);
            this.tpReport.TabIndex = 1;
            this.tpReport.Text = "Report";
            this.tpReport.UseVisualStyleBackColor = true;
            // 
            // rvSubjectSummary
            // 
            this.rvSubjectSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rvSubjectSummary.Location = new System.Drawing.Point(0, 0);
            this.rvSubjectSummary.Name = "rvSubjectSummary";
            this.rvSubjectSummary.PageCountMode = Microsoft.Reporting.WinForms.PageCountMode.Actual;
            this.rvSubjectSummary.ServerReport.BearerToken = null;
            this.rvSubjectSummary.Size = new System.Drawing.Size(1056, 654);
            this.rvSubjectSummary.TabIndex = 1;
            // 
            // tpTarget
            // 
            this.tpTarget.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tpTarget.Controls.Add(this.panelFiltersActions);
            this.tpTarget.Controls.Add(this.panelADTree);
            this.tpTarget.Location = new System.Drawing.Point(4, 22);
            this.tpTarget.Name = "tpTarget";
            this.tpTarget.Padding = new System.Windows.Forms.Padding(3);
            this.tpTarget.Size = new System.Drawing.Size(1056, 654);
            this.tpTarget.TabIndex = 0;
            this.tpTarget.Text = "Target and Filters";
            // 
            // panelFiltersActions
            // 
            this.panelFiltersActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFiltersActions.BackColor = System.Drawing.SystemColors.Control;
            this.panelFiltersActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFiltersActions.Controls.Add(this.cbRecursive);
            this.panelFiltersActions.Controls.Add(this.btnRemoveIgnored);
            this.panelFiltersActions.Controls.Add(this.lbUserAccountsToIgnore);
            this.panelFiltersActions.Controls.Add(this.panelProgress);
            this.panelFiltersActions.Controls.Add(this.btnGenerateReport);
            this.panelFiltersActions.Controls.Add(this.label1);
            this.panelFiltersActions.Controls.Add(this.btnIgnoreAccts);
            this.panelFiltersActions.Controls.Add(this.cbIncludeGPOs);
            this.panelFiltersActions.Controls.Add(this.cbDomainAdmins);
            this.panelFiltersActions.Controls.Add(this.lblFilters);
            this.panelFiltersActions.Location = new System.Drawing.Point(523, 6);
            this.panelFiltersActions.Name = "panelFiltersActions";
            this.panelFiltersActions.Size = new System.Drawing.Size(525, 641);
            this.panelFiltersActions.TabIndex = 1;
            // 
            // cbRecursive
            // 
            this.cbRecursive.AutoSize = true;
            this.cbRecursive.Checked = true;
            this.cbRecursive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRecursive.Location = new System.Drawing.Point(9, 72);
            this.cbRecursive.Name = "cbRecursive";
            this.cbRecursive.Size = new System.Drawing.Size(209, 17);
            this.cbRecursive.TabIndex = 40;
            this.cbRecursive.Text = "Recursively Check Within Child OUs";
            this.cbRecursive.UseVisualStyleBackColor = true;
            // 
            // btnRemoveIgnored
            // 
            this.btnRemoveIgnored.Location = new System.Drawing.Point(184, 278);
            this.btnRemoveIgnored.Name = "btnRemoveIgnored";
            this.btnRemoveIgnored.Size = new System.Drawing.Size(160, 23);
            this.btnRemoveIgnored.TabIndex = 70;
            this.btnRemoveIgnored.Text = "Remove Selected Account";
            this.btnRemoveIgnored.UseVisualStyleBackColor = true;
            this.btnRemoveIgnored.Click += new System.EventHandler(this.btnRemoveIgnored_Click);
            // 
            // lbUserAccountsToIgnore
            // 
            this.lbUserAccountsToIgnore.FormattingEnabled = true;
            this.lbUserAccountsToIgnore.Location = new System.Drawing.Point(171, 175);
            this.lbUserAccountsToIgnore.Name = "lbUserAccountsToIgnore";
            this.lbUserAccountsToIgnore.Size = new System.Drawing.Size(186, 95);
            this.lbUserAccountsToIgnore.TabIndex = 60;
            // 
            // panelProgress
            // 
            this.panelProgress.BackColor = System.Drawing.Color.White;
            this.panelProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProgress.Controls.Add(this.pictureBox1);
            this.panelProgress.Controls.Add(this.btnCancel);
            this.panelProgress.Controls.Add(this.lblPermsTotal);
            this.panelProgress.Controls.Add(this.lblGenerating);
            this.panelProgress.Controls.Add(this.lblPermsReported);
            this.panelProgress.Location = new System.Drawing.Point(108, 523);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Size = new System.Drawing.Size(310, 110);
            this.panelProgress.TabIndex = 10;
            this.panelProgress.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(103, 71);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnCancel.Size = new System.Drawing.Size(102, 34);
            this.btnCancel.TabIndex = 90;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblPermsTotal
            // 
            this.lblPermsTotal.ForeColor = System.Drawing.Color.Green;
            this.lblPermsTotal.Location = new System.Drawing.Point(224, 27);
            this.lblPermsTotal.Name = "lblPermsTotal";
            this.lblPermsTotal.Size = new System.Drawing.Size(75, 13);
            this.lblPermsTotal.TabIndex = 8;
            this.lblPermsTotal.Text = "0";
            // 
            // lblGenerating
            // 
            this.lblGenerating.AutoSize = true;
            this.lblGenerating.ForeColor = System.Drawing.Color.Green;
            this.lblGenerating.Location = new System.Drawing.Point(56, 9);
            this.lblGenerating.Name = "lblGenerating";
            this.lblGenerating.Size = new System.Drawing.Size(247, 13);
            this.lblGenerating.TabIndex = 6;
            this.lblGenerating.Text = "Generating Report, This May Take Some Time...";
            // 
            // lblPermsReported
            // 
            this.lblPermsReported.AutoSize = true;
            this.lblPermsReported.ForeColor = System.Drawing.Color.Green;
            this.lblPermsReported.Location = new System.Drawing.Point(56, 27);
            this.lblPermsReported.Name = "lblPermsReported";
            this.lblPermsReported.Size = new System.Drawing.Size(162, 13);
            this.lblPermsReported.TabIndex = 7;
            this.lblPermsReported.Text = "Permissions Matching Criteria:";
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Font = new System.Drawing.Font("Segoe UI", 15.75F);
            this.btnGenerateReport.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerateReport.Image")));
            this.btnGenerateReport.Location = new System.Drawing.Point(171, 378);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Padding = new System.Windows.Forms.Padding(3);
            this.btnGenerateReport.Size = new System.Drawing.Size(186, 137);
            this.btnGenerateReport.TabIndex = 80;
            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 323);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Actions";
            // 
            // btnIgnoreAccts
            // 
            this.btnIgnoreAccts.Image = ((System.Drawing.Image)(resources.GetObject("btnIgnoreAccts.Image")));
            this.btnIgnoreAccts.Location = new System.Drawing.Point(171, 108);
            this.btnIgnoreAccts.Name = "btnIgnoreAccts";
            this.btnIgnoreAccts.Size = new System.Drawing.Size(186, 61);
            this.btnIgnoreAccts.TabIndex = 50;
            this.btnIgnoreAccts.Text = "Specify Additional Account to Ignore (Shown in List Below)";
            this.btnIgnoreAccts.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnIgnoreAccts.UseVisualStyleBackColor = true;
            this.btnIgnoreAccts.Click += new System.EventHandler(this.btnIgnoreAccts_Click);
            // 
            // cbIncludeGPOs
            // 
            this.cbIncludeGPOs.AutoSize = true;
            this.cbIncludeGPOs.Checked = true;
            this.cbIncludeGPOs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeGPOs.Location = new System.Drawing.Point(9, 30);
            this.cbIncludeGPOs.Name = "cbIncludeGPOs";
            this.cbIncludeGPOs.Size = new System.Drawing.Size(322, 17);
            this.cbIncludeGPOs.TabIndex = 20;
            this.cbIncludeGPOs.Text = "Include Permissions Assessment of Linked GPOs in Report";
            this.cbIncludeGPOs.UseVisualStyleBackColor = true;
            // 
            // cbDomainAdmins
            // 
            this.cbDomainAdmins.AutoSize = true;
            this.cbDomainAdmins.Checked = true;
            this.cbDomainAdmins.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDomainAdmins.Location = new System.Drawing.Point(9, 51);
            this.cbDomainAdmins.Name = "cbDomainAdmins";
            this.cbDomainAdmins.Size = new System.Drawing.Size(460, 17);
            this.cbDomainAdmins.TabIndex = 30;
            this.cbDomainAdmins.Text = "Ignore Accounts in Domain Admins, Exchange Admins, Certain Built-in Accounts, etc" +
    ".";
            this.cbDomainAdmins.UseVisualStyleBackColor = true;
            // 
            // lblFilters
            // 
            this.lblFilters.AutoSize = true;
            this.lblFilters.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilters.Location = new System.Drawing.Point(3, 3);
            this.lblFilters.Name = "lblFilters";
            this.lblFilters.Size = new System.Drawing.Size(46, 17);
            this.lblFilters.TabIndex = 0;
            this.lblFilters.Text = "Filters";
            // 
            // panelADTree
            // 
            this.panelADTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelADTree.BackColor = System.Drawing.SystemColors.Control;
            this.panelADTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelADTree.Controls.Add(this.pbADTreeWait1);
            this.panelADTree.Controls.Add(this.tvADTree);
            this.panelADTree.Controls.Add(this.lblTreeview);
            this.panelADTree.Location = new System.Drawing.Point(7, 6);
            this.panelADTree.Name = "panelADTree";
            this.panelADTree.Size = new System.Drawing.Size(510, 641);
            this.panelADTree.TabIndex = 0;
            // 
            // pbADTreeWait1
            // 
            this.pbADTreeWait1.Image = ((System.Drawing.Image)(resources.GetObject("pbADTreeWait1.Image")));
            this.pbADTreeWait1.Location = new System.Drawing.Point(248, 264);
            this.pbADTreeWait1.Name = "pbADTreeWait1";
            this.pbADTreeWait1.Size = new System.Drawing.Size(16, 16);
            this.pbADTreeWait1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbADTreeWait1.TabIndex = 2;
            this.pbADTreeWait1.TabStop = false;
            this.pbADTreeWait1.Visible = false;
            // 
            // tvADTree
            // 
            this.tvADTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvADTree.Location = new System.Drawing.Point(3, 23);
            this.tvADTree.Name = "tvADTree";
            this.tvADTree.Size = new System.Drawing.Size(502, 613);
            this.tvADTree.TabIndex = 10;
            this.tvADTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvADMTree_AfterSelect);
            // 
            // lblTreeview
            // 
            this.lblTreeview.AutoSize = true;
            this.lblTreeview.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTreeview.Location = new System.Drawing.Point(3, 3);
            this.lblTreeview.Name = "lblTreeview";
            this.lblTreeview.Size = new System.Drawing.Size(70, 17);
            this.lblTreeview.TabIndex = 0;
            this.lblTreeview.Text = "Target OU";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpTarget);
            this.tabControl1.Controls.Add(this.tpReport);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1064, 680);
            this.tabControl1.TabIndex = 0;
            // 
            // bgwGenerateReport
            // 
            this.bgwGenerateReport.WorkerReportsProgress = true;
            this.bgwGenerateReport.WorkerSupportsCancellation = true;
            this.bgwGenerateReport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwGenerateReport_DoWork);
            this.bgwGenerateReport.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwGenerateReport_ProgressChanged);
            this.bgwGenerateReport.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwGenerateReport_RunWorkerCompleted);
            // 
            // PlasmaPupMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 681);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlasmaPupMain";
            this.Text = "PlasmaPup";
            this.Load += new System.EventHandler(this.PlasmaPupMain_Load);
            this.tpReport.ResumeLayout(false);
            this.tpTarget.ResumeLayout(false);
            this.panelFiltersActions.ResumeLayout(false);
            this.panelFiltersActions.PerformLayout();
            this.panelProgress.ResumeLayout(false);
            this.panelProgress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelADTree.ResumeLayout(false);
            this.panelADTree.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbADTreeWait1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker bgwGetTVChildEntries;
        private System.Windows.Forms.TabPage tpReport;
        private System.Windows.Forms.TabPage tpTarget;
        private System.Windows.Forms.Panel panelFiltersActions;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnIgnoreAccts;
        private System.Windows.Forms.CheckBox cbIncludeGPOs;
        private System.Windows.Forms.CheckBox cbDomainAdmins;
        private System.Windows.Forms.Label lblFilters;
        private System.Windows.Forms.Panel panelADTree;
        private System.Windows.Forms.PictureBox pbADTreeWait1;
        private System.Windows.Forms.TreeView tvADTree;
        private System.Windows.Forms.Label lblTreeview;
        private System.Windows.Forms.TabControl tabControl1;
        private Microsoft.Reporting.WinForms.ReportViewer rvSubjectSummary;
        private System.Windows.Forms.Button btnCancel;
        private System.ComponentModel.BackgroundWorker bgwGenerateReport;
        private System.Windows.Forms.Label lblPermsReported;
        private System.Windows.Forms.Label lblGenerating;
        private System.Windows.Forms.Label lblPermsTotal;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelProgress;
        public System.Windows.Forms.ListBox lbUserAccountsToIgnore;
        private System.Windows.Forms.Button btnRemoveIgnored;
        private System.Windows.Forms.CheckBox cbRecursive;
    }
}
