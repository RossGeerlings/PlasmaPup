namespace Seeker
{
    partial class ADBrowse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ADBrowse));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pbADTreeWait1 = new System.Windows.Forms.PictureBox();
            this.tvADTree = new System.Windows.Forms.TreeView();
            this.btnQueryRootOU = new System.Windows.Forms.Button();
            this.bgwGetTVChildEntries = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbADTreeWait1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnQueryRootOU);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(641, 464);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pbADTreeWait1);
            this.panel2.Controls.Add(this.tvADTree);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(641, 408);
            this.panel2.TabIndex = 3;
            // 
            // pbADTreeWait1
            // 
            this.pbADTreeWait1.Image = ((System.Drawing.Image)(resources.GetObject("pbADTreeWait1.Image")));
            this.pbADTreeWait1.Location = new System.Drawing.Point(314, 188);
            this.pbADTreeWait1.Name = "pbADTreeWait1";
            this.pbADTreeWait1.Size = new System.Drawing.Size(16, 16);
            this.pbADTreeWait1.TabIndex = 3;
            this.pbADTreeWait1.TabStop = false;
            this.pbADTreeWait1.Visible = false;
            // 
            // tvADTree
            // 
            this.tvADTree.Dock = System.Windows.Forms.DockStyle.Top;
            this.tvADTree.HideSelection = false;
            this.tvADTree.Location = new System.Drawing.Point(0, 0);
            this.tvADTree.Name = "tvADTree";
            this.tvADTree.Size = new System.Drawing.Size(641, 408);
            this.tvADTree.TabIndex = 2;
            this.tvADTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvADTree_AfterSelect);
            // 
            // btnQueryRootOU
            // 
            this.btnQueryRootOU.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQueryRootOU.Location = new System.Drawing.Point(259, 414);
            this.btnQueryRootOU.Name = "btnQueryRootOU";
            this.btnQueryRootOU.Size = new System.Drawing.Size(122, 43);
            this.btnQueryRootOU.TabIndex = 2;
            this.btnQueryRootOU.Text = "Query Workstations Within Selected OU";
            this.btnQueryRootOU.UseVisualStyleBackColor = true;
            this.btnQueryRootOU.Click += new System.EventHandler(this.btnQueryRootOU_Click);
            // 
            // bgwGetTVChildEntries
            // 
            this.bgwGetTVChildEntries.WorkerReportsProgress = true;
            this.bgwGetTVChildEntries.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwGetTVChildEntries_DoWork);
            this.bgwGetTVChildEntries.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwGetTVChildEntries_ProgressChanged);
            this.bgwGetTVChildEntries.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwGetTVChildEntries_RunWorkerCompleted);
            // 
            // ADBrowse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 464);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ADBrowse";
            this.Text = "Select Root OU for Query";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbADTreeWait1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnQueryRootOU;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView tvADTree;
        private System.Windows.Forms.PictureBox pbADTreeWait1;
        private System.ComponentModel.BackgroundWorker bgwGetTVChildEntries;
    }
}