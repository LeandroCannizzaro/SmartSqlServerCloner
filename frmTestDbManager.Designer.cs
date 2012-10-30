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
            this.btnCloneDatabaseSchema = new System.Windows.Forms.Button();
            this.btnCloneDatabaseData = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnCreateServerXmlDriver = new System.Windows.Forms.Button();
            this.cmbSourceServer = new System.Windows.Forms.ComboBox();
            this.cmbTargetServer = new System.Windows.Forms.ComboBox();
            this.btnCreateDatabaseXmlDriver = new System.Windows.Forms.Button();
            this.btnCloneAllDatabaseSchema = new System.Windows.Forms.Button();
            this.grpAllDatabases = new System.Windows.Forms.GroupBox();
            this.grpSelectedDatabase = new System.Windows.Forms.GroupBox();
            this.lblDatabasesList = new System.Windows.Forms.Label();
            this.lblTablesList = new System.Windows.Forms.Label();
            this.grpServer = new System.Windows.Forms.GroupBox();
            this.grpAllDatabases.SuspendLayout();
            this.grpSelectedDatabase.SuspendLayout();
            this.grpServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSchema
            // 
            this.btnSchema.Location = new System.Drawing.Point(6, 27);
            this.btnSchema.Name = "btnSchema";
            this.btnSchema.Size = new System.Drawing.Size(153, 23);
            this.btnSchema.TabIndex = 0;
            this.btnSchema.Text = "Clone Server only Objects";
            this.btnSchema.UseVisualStyleBackColor = true;
            this.btnSchema.Click += new System.EventHandler(this.btnSchema_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Location = new System.Drawing.Point(6, 67);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(933, 20);
            this.lblStatus.TabIndex = 4;
            // 
            // StatusRows
            // 
            this.StatusRows.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusRows.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StatusRows.Location = new System.Drawing.Point(6, 96);
            this.StatusRows.Name = "StatusRows";
            this.StatusRows.Size = new System.Drawing.Size(933, 20);
            this.StatusRows.TabIndex = 5;
            // 
            // listDatabases
            // 
            this.listDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listDatabases.Location = new System.Drawing.Point(6, 141);
            this.listDatabases.Name = "listDatabases";
            this.listDatabases.Size = new System.Drawing.Size(261, 425);
            this.listDatabases.TabIndex = 7;
            this.listDatabases.UseCompatibleStateImageBehavior = false;
            this.listDatabases.SelectedIndexChanged += new System.EventHandler(this.listDatabases_SelectedIndexChanged);
            // 
            // listTables
            // 
            this.listTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listTables.LabelEdit = true;
            this.listTables.Location = new System.Drawing.Point(273, 141);
            this.listTables.Name = "listTables";
            this.listTables.Size = new System.Drawing.Size(666, 425);
            this.listTables.TabIndex = 9;
            this.listTables.UseCompatibleStateImageBehavior = false;
            // 
            // lblSourceServer
            // 
            this.lblSourceServer.AutoSize = true;
            this.lblSourceServer.Location = new System.Drawing.Point(12, 12);
            this.lblSourceServer.Name = "lblSourceServer";
            this.lblSourceServer.Size = new System.Drawing.Size(73, 13);
            this.lblSourceServer.TabIndex = 0;
            this.lblSourceServer.Text = "Source server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target server";
            // 
            // btnData
            // 
            this.btnData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnData.Location = new System.Drawing.Point(5, 80);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(153, 23);
            this.btnData.TabIndex = 2;
            this.btnData.Text = "Clone All Databases  Data";
            this.btnData.UseVisualStyleBackColor = true;
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // btnCloneDatabaseSchema
            // 
            this.btnCloneDatabaseSchema.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCloneDatabaseSchema.Location = new System.Drawing.Point(6, 65);
            this.btnCloneDatabaseSchema.Name = "btnCloneDatabaseSchema";
            this.btnCloneDatabaseSchema.Size = new System.Drawing.Size(153, 23);
            this.btnCloneDatabaseSchema.TabIndex = 1;
            this.btnCloneDatabaseSchema.Text = "Clone Database Schema";
            this.btnCloneDatabaseSchema.UseVisualStyleBackColor = true;
            this.btnCloneDatabaseSchema.Click += new System.EventHandler(this.btnCloneDatabaseSchema_Click);
            // 
            // btnCloneDatabaseData
            // 
            this.btnCloneDatabaseData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCloneDatabaseData.Location = new System.Drawing.Point(6, 94);
            this.btnCloneDatabaseData.Name = "btnCloneDatabaseData";
            this.btnCloneDatabaseData.Size = new System.Drawing.Size(153, 23);
            this.btnCloneDatabaseData.TabIndex = 2;
            this.btnCloneDatabaseData.Text = "Clone Database Data";
            this.btnCloneDatabaseData.UseVisualStyleBackColor = true;
            this.btnCloneDatabaseData.Click += new System.EventHandler(this.btnCloneDatabaseData_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(6, 572);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1101, 23);
            this.progressBar.TabIndex = 13;
            // 
            // btnCreateServerXmlDriver
            // 
            this.btnCreateServerXmlDriver.Location = new System.Drawing.Point(5, 22);
            this.btnCreateServerXmlDriver.Name = "btnCreateServerXmlDriver";
            this.btnCreateServerXmlDriver.Size = new System.Drawing.Size(153, 23);
            this.btnCreateServerXmlDriver.TabIndex = 0;
            this.btnCreateServerXmlDriver.Text = "Create Xml Driver Files";
            this.btnCreateServerXmlDriver.UseVisualStyleBackColor = true;
            this.btnCreateServerXmlDriver.Click += new System.EventHandler(this.btnCreateServerXmlDriver_Click);
            // 
            // cmbSourceServer
            // 
            this.cmbSourceServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSourceServer.FormattingEnabled = true;
            this.cmbSourceServer.Location = new System.Drawing.Point(88, 9);
            this.cmbSourceServer.Name = "cmbSourceServer";
            this.cmbSourceServer.Size = new System.Drawing.Size(851, 21);
            this.cmbSourceServer.TabIndex = 1;
            this.cmbSourceServer.SelectedIndexChanged += new System.EventHandler(this.cboSourceServer_SelectedIndexChanged);
            // 
            // cmbTargetServer
            // 
            this.cmbTargetServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTargetServer.FormattingEnabled = true;
            this.cmbTargetServer.Location = new System.Drawing.Point(88, 38);
            this.cmbTargetServer.Name = "cmbTargetServer";
            this.cmbTargetServer.Size = new System.Drawing.Size(851, 21);
            this.cmbTargetServer.TabIndex = 3;
            this.cmbTargetServer.SelectedIndexChanged += new System.EventHandler(this.cboTargetServer_SelectedIndexChanged);
            // 
            // btnCreateDatabaseXmlDriver
            // 
            this.btnCreateDatabaseXmlDriver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreateDatabaseXmlDriver.Location = new System.Drawing.Point(6, 36);
            this.btnCreateDatabaseXmlDriver.Name = "btnCreateDatabaseXmlDriver";
            this.btnCreateDatabaseXmlDriver.Size = new System.Drawing.Size(153, 23);
            this.btnCreateDatabaseXmlDriver.TabIndex = 0;
            this.btnCreateDatabaseXmlDriver.Text = "Create Xml Driver File";
            this.btnCreateDatabaseXmlDriver.UseVisualStyleBackColor = true;
            this.btnCreateDatabaseXmlDriver.Click += new System.EventHandler(this.btnCreateDatabaseXmlDriver_Click);
            // 
            // btnCloneAllDatabaseSchema
            // 
            this.btnCloneAllDatabaseSchema.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloneAllDatabaseSchema.Location = new System.Drawing.Point(6, 51);
            this.btnCloneAllDatabaseSchema.Name = "btnCloneAllDatabaseSchema";
            this.btnCloneAllDatabaseSchema.Size = new System.Drawing.Size(153, 23);
            this.btnCloneAllDatabaseSchema.TabIndex = 1;
            this.btnCloneAllDatabaseSchema.Text = "Clone All Databases Schema ";
            this.btnCloneAllDatabaseSchema.UseVisualStyleBackColor = true;
            this.btnCloneAllDatabaseSchema.Click += new System.EventHandler(this.btnCloneAllDatabaseSchema_Click);
            // 
            // grpAllDatabases
            // 
            this.grpAllDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAllDatabases.Controls.Add(this.btnData);
            this.grpAllDatabases.Controls.Add(this.btnCloneAllDatabaseSchema);
            this.grpAllDatabases.Controls.Add(this.btnCreateServerXmlDriver);
            this.grpAllDatabases.Location = new System.Drawing.Point(945, 95);
            this.grpAllDatabases.Name = "grpAllDatabases";
            this.grpAllDatabases.Size = new System.Drawing.Size(164, 122);
            this.grpAllDatabases.TabIndex = 11;
            this.grpAllDatabases.TabStop = false;
            this.grpAllDatabases.Text = "Operations for all databases";
            // 
            // grpSelectedDatabase
            // 
            this.grpSelectedDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSelectedDatabase.Controls.Add(this.btnCreateDatabaseXmlDriver);
            this.grpSelectedDatabase.Controls.Add(this.btnCloneDatabaseSchema);
            this.grpSelectedDatabase.Controls.Add(this.btnCloneDatabaseData);
            this.grpSelectedDatabase.Location = new System.Drawing.Point(945, 222);
            this.grpSelectedDatabase.Name = "grpSelectedDatabase";
            this.grpSelectedDatabase.Size = new System.Drawing.Size(164, 139);
            this.grpSelectedDatabase.TabIndex = 12;
            this.grpSelectedDatabase.TabStop = false;
            this.grpSelectedDatabase.Text = "Operations for selected database";
            // 
            // lblDatabasesList
            // 
            this.lblDatabasesList.AutoSize = true;
            this.lblDatabasesList.Location = new System.Drawing.Point(9, 124);
            this.lblDatabasesList.Name = "lblDatabasesList";
            this.lblDatabasesList.Size = new System.Drawing.Size(77, 13);
            this.lblDatabasesList.TabIndex = 6;
            this.lblDatabasesList.Text = "Databases List";
            // 
            // lblTablesList
            // 
            this.lblTablesList.AutoSize = true;
            this.lblTablesList.Location = new System.Drawing.Point(270, 124);
            this.lblTablesList.Name = "lblTablesList";
            this.lblTablesList.Size = new System.Drawing.Size(58, 13);
            this.lblTablesList.TabIndex = 8;
            this.lblTablesList.Text = "Tables List";
            // 
            // grpServer
            // 
            this.grpServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpServer.Controls.Add(this.btnSchema);
            this.grpServer.Location = new System.Drawing.Point(945, 13);
            this.grpServer.Name = "grpServer";
            this.grpServer.Size = new System.Drawing.Size(164, 71);
            this.grpServer.TabIndex = 10;
            this.grpServer.TabStop = false;
            this.grpServer.Text = "Operations for server";
            // 
            // frmTestDbManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1112, 598);
            this.Controls.Add(this.grpServer);
            this.Controls.Add(this.lblTablesList);
            this.Controls.Add(this.lblDatabasesList);
            this.Controls.Add(this.grpSelectedDatabase);
            this.Controls.Add(this.grpAllDatabases);
            this.Controls.Add(this.cmbTargetServer);
            this.Controls.Add(this.cmbSourceServer);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSourceServer);
            this.Controls.Add(this.listTables);
            this.Controls.Add(this.listDatabases);
            this.Controls.Add(this.StatusRows);
            this.Controls.Add(this.lblStatus);
            this.Name = "frmTestDbManager";
            this.Text = "Test database manager";
            this.Load += new System.EventHandler(this.frmTestDbManager_Load);
            this.grpAllDatabases.ResumeLayout(false);
            this.grpSelectedDatabase.ResumeLayout(false);
            this.grpServer.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnCloneDatabaseSchema;
        private System.Windows.Forms.Button btnCloneDatabaseData;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnCreateServerXmlDriver;
        private System.Windows.Forms.ComboBox cmbSourceServer;
        private System.Windows.Forms.ComboBox cmbTargetServer;
        private System.Windows.Forms.Button btnCreateDatabaseXmlDriver;
        private System.Windows.Forms.Button btnCloneAllDatabaseSchema;
        private System.Windows.Forms.GroupBox grpAllDatabases;
        private System.Windows.Forms.GroupBox grpSelectedDatabase;
        private System.Windows.Forms.Label lblDatabasesList;
        private System.Windows.Forms.Label lblTablesList;
        private System.Windows.Forms.GroupBox grpServer;
    }
}

