namespace PlasmaPup
{
    partial class AddAccountExclusion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddAccountExclusion));
            this.tbUserToExclude = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblValidated = new System.Windows.Forms.Label();
            this.btnValidateUser = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbUserToExclude
            // 
            this.tbUserToExclude.Location = new System.Drawing.Point(28, 25);
            this.tbUserToExclude.Name = "tbUserToExclude";
            this.tbUserToExclude.Size = new System.Drawing.Size(207, 22);
            this.tbUserToExclude.TabIndex = 0;
            this.tbUserToExclude.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUserToExclude_KeyDown);
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(45, 71);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(44, 37);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(176, 71);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(44, 37);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblValidated);
            this.panel1.Controls.Add(this.btnValidateUser);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbUserToExclude);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 120);
            this.panel1.TabIndex = 4;
            // 
            // lblValidated
            // 
            this.lblValidated.AutoSize = true;
            this.lblValidated.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValidated.ForeColor = System.Drawing.Color.Green;
            this.lblValidated.Location = new System.Drawing.Point(99, 50);
            this.lblValidated.Name = "lblValidated";
            this.lblValidated.Size = new System.Drawing.Size(66, 17);
            this.lblValidated.TabIndex = 6;
            this.lblValidated.Text = "Validated";
            this.lblValidated.Visible = false;
            // 
            // btnValidateUser
            // 
            this.btnValidateUser.Location = new System.Drawing.Point(95, 71);
            this.btnValidateUser.Name = "btnValidateUser";
            this.btnValidateUser.Size = new System.Drawing.Size(75, 37);
            this.btnValidateUser.TabIndex = 5;
            this.btnValidateUser.Text = "Validate User Name";
            this.btnValidateUser.UseVisualStyleBackColor = true;
            this.btnValidateUser.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Name of User Account to Exclude";
            // 
            // AddAccountExclusion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(283, 135);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddAccountExclusion";
            this.Text = "Add User Account to Exclude";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbUserToExclude;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnValidateUser;
        private System.Windows.Forms.Label lblValidated;
    }
}