namespace TestDatabaseCreation
{
    partial class frmTestDbManager
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
            this.btnSchema = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.StatusRows = new System.Windows.Forms.Label();
            this.listDatabases = new System.Windows.Forms.ListView();
            this.listTables = new System.Windows.Forms.ListView();
            this.lblSourceServer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnData = new System.Windows.Forms.Button();
            this.btnUpdateDatabaseSchema = new System.Windows.Forms.Button();
            this.btnUpdateDatabaseData = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnUpdateServerXml = new System.Windows.Forms.Button();
            this.cmbSourceServer = new System.Windows.Forms.ComboBox();
            this.cmbTargetServer = new System.Windows.Forms.ComboBox();
            this.btnUpdateDatabaseXml = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSchema
            // 
            this.btnSchema.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSchema.Location = new System.Drawing.Point(781, 9);
            this.btnSchema.Name = "btnSchema";
            this.btnSchema.Size = new System.Drawing.Size(153, 23);
            this.btnSchema.TabIndex = 0;
            this.btnSchema.Text = "Update Server Schema";
            this.btnSchema.UseVisualStyleBackColor = true;
            this.btnSchema.Click += new System.EventHandler(this.btnSchema_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Location = new System.Drawing.Point(6, 100);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(684, 20);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "status";
            // 
            // StatusRows
            // 
            this.StatusRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusRows.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StatusRows.Location = new System.Drawing.Point(696, 100);
            this.StatusRows.Name = "StatusRows";
            this.StatusRows.Size = new System.Drawing.Size(238, 20);
            this.StatusRows.TabIndex = 2;
            this.StatusRows.Text = "StatusRowsCopied";
            // 
            // listDatabases
            // 
            this.listDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listDatabases.Location = new System.Drawing.Point(6, 128);
            this.listDatabases.Name = "listDatabases";
            this.listDatabases.Size = new System.Drawing.Size(261, 438);
            this.listDatabases.TabIndex = 3;
            this.listDatabases.UseCompatibleStateImageBehavior = false;
            this.listDatabases.SelectedIndexChanged += new System.EventHandler(this.listDatabases_SelectedIndexChanged);
            // 
            // listTables
            // 
            this.listTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listTables.LabelEdit = true;
            this.listTables.Location = new System.Drawing.Point(273, 128);
            this.listTables.Name = "listTables";
            this.listTables.Size = new System.Drawing.Size(661, 438);
            this.listTables.TabIndex = 4;
            this.listTables.UseCompatibleStateImageBehavior = false;
            this.listTables.SelectedIndexChanged += new System.EventHandler(this.listTables_SelectedIndexChanged);
            // 
            // lblSourceServer
            // 
            this.lblSourceServer.AutoSize = true;
            this.lblSourceServer.Location = new System.Drawing.Point(12, 12);
            this.lblSourceServer.Name = "lblSourceServer";
            this.lblSourceServer.Size = new System.Drawing.Size(73, 13);
            this.lblSourceServer.TabIndex = 7;
            this.lblSourceServer.Text = "Source server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Target server";
            // 
            // btnData
            // 
            this.btnData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnData.Location = new System.Drawing.Point(781, 37);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(153, 23);
            this.btnData.TabIndex = 9;
            this.btnData.Text = "Update Server Data";
            this.btnData.UseVisualStyleBackColor = true;
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // btnUpdateDatabaseSchema
            // 
            this.btnUpdateDatabaseSchema.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdateDatabaseSchema.Location = new System.Drawing.Point(6, 71);
            this.btnUpdateDatabaseSchema.Name = "btnUpdateDatabaseSchema";
            this.btnUpdateDatabaseSchema.Size = new System.Drawing.Size(153, 23);
            this.btnUpdateDatabaseSchema.TabIndex = 10;
            this.btnUpdateDatabaseSchema.Text = "Update Database Schema";
            this.btnUpdateDatabaseSchema.UseVisualStyleBackColor = true;
            this.btnUpdateDatabaseSchema.Click += new System.EventHandler(this.btnUpdateDatabaseSchema_Click);
            // 
            // btnUpdateDatabaseData
            // 
            this.btnUpdateDatabaseData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdateDatabaseData.Location = new System.Drawing.Point(176, 71);
            this.btnUpdateDatabaseData.Name = "btnUpdateDatabaseData";
            this.btnUpdateDatabaseData.Size = new System.Drawing.Size(153, 23);
            this.btnUpdateDatabaseData.TabIndex = 11;
            this.btnUpdateDatabaseData.Text = "Update Database Data";
            this.btnUpdateDatabaseData.UseVisualStyleBackColor = true;
            this.btnUpdateDatabaseData.Click += new System.EventHandler(this.btnUpdateDatabaseData_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(6, 572);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(928, 23);
            this.progressBar.TabIndex = 12;
            // 
            // btnUpdateServerXml
            // 
            this.btnUpdateServerXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdateServerXml.Location = new System.Drawing.Point(781, 66);
            this.btnUpdateServerXml.Name = "btnUpdateServerXml";
            this.btnUpdateServerXml.Size = new System.Drawing.Size(153, 23);
            this.btnUpdateServerXml.TabIndex = 13;
            this.btnUpdateServerXml.Text = "Update Server Xml";
            this.btnUpdateServerXml.UseVisualStyleBackColor = true;
            this.btnUpdateServerXml.Click += new System.EventHandler(this.btnUpdateServerXml_Click);
            // 
            // cmbSourceServer
            // 
            this.cmbSourceServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSourceServer.FormattingEnabled = true;
            this.cmbSourceServer.Location = new System.Drawing.Point(88, 9);
            this.cmbSourceServer.Name = "cmbSourceServer";
            this.cmbSourceServer.Size = new System.Drawing.Size(687, 21);
            this.cmbSourceServer.TabIndex = 14;
            this.cmbSourceServer.SelectedIndexChanged += new System.EventHandler(this.cboSourceServer_SelectedIndexChanged);
            // 
            // cmbTargetServer
            // 
            this.cmbTargetServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTargetServer.FormattingEnabled = true;
            this.cmbTargetServer.Location = new System.Drawing.Point(88, 38);
            this.cmbTargetServer.Name = "cmbTargetServer";
            this.cmbTargetServer.Size = new System.Drawing.Size(687, 21);
            this.cmbTargetServer.TabIndex = 15;
            this.cmbTargetServer.SelectedIndexChanged += new System.EventHandler(this.cboTargetServer_SelectedIndexChanged);
            // 
            // btnUpdateDatabaseXml
            // 
            this.btnUpdateDatabaseXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdateDatabaseXml.Location = new System.Drawing.Point(346, 71);
            this.btnUpdateDatabaseXml.Name = "btnUpdateDatabaseXml";
            this.btnUpdateDatabaseXml.Size = new System.Drawing.Size(153, 23);
            this.btnUpdateDatabaseXml.TabIndex = 16;
            this.btnUpdateDatabaseXml.Text = "Update Database Xml";
            this.btnUpdateDatabaseXml.UseVisualStyleBackColor = true;
            this.btnUpdateDatabaseXml.Click += new System.EventHandler(this.btnUpdateDatabaseXml_Click);
            // 
            // frmTestDbManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 598);
            this.Controls.Add(this.btnUpdateDatabaseXml);
            this.Controls.Add(this.cmbTargetServer);
            this.Controls.Add(this.cmbSourceServer);
            this.Controls.Add(this.btnUpdateServerXml);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnUpdateDatabaseData);
            this.Controls.Add(this.btnUpdateDatabaseSchema);
            this.Controls.Add(this.btnData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSourceServer);
            this.Controls.Add(this.listTables);
            this.Controls.Add(this.listDatabases);
            this.Controls.Add(this.StatusRows);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnSchema);
            this.Name = "frmTestDbManager";
            this.Text = "Test database manager";
            this.Load += new System.EventHandler(this.frmTestDbManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSchema;
        private System.Windows.Forms.Label lblStatus;
        public System.Windows.Forms.Label StatusRows;
        private System.Windows.Forms.ListView listDatabases;
        private System.Windows.Forms.ListView listTables;
        private System.Windows.Forms.Label lblSourceServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnData;
        private System.Windows.Forms.Button btnUpdateDatabaseSchema;
        private System.Windows.Forms.Button btnUpdateDatabaseData;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnUpdateServerXml;
        private System.Windows.Forms.ComboBox cmbSourceServer;
        private System.Windows.Forms.ComboBox cmbTargetServer;
        private System.Windows.Forms.Button btnUpdateDatabaseXml;
    }
}

