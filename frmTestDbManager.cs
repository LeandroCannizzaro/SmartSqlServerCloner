using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace TestDatabaseCreation
{
    //Classe per l'interfaccia utente di tipo windows.forms.
    public partial class frmTestDbManager : Form
    {
        private List<ServerInfo> sourceServers = new List<ServerInfo>();
        private List<ServerInfo> targetServers = new List<ServerInfo>();
 

        private List<DatabaseData> databaseList;
        
        //Riferimento alla Classe che effettua il lavoro di sincronizzazione
        private TestDbManager testDbManager = new TestDbManager();
        
        //Costruttore della window form
        public frmTestDbManager()
        {
            InitializeComponent();

            //Inizializza la listview per la lista dei database di un server
            listDatabases.View = System.Windows.Forms.View.Details;
            listDatabases.FullRowSelect = true;
            listDatabases.MultiSelect = false;
            listDatabases.CheckBoxes = true;
            listDatabases.GridLines = true;
            listDatabases.HideSelection = false;

            ColumnHeader chDatabase = listDatabases.Columns.Add("Database");
            chDatabase.Width = 180;
            ColumnHeader chTablesCount = listDatabases.Columns.Add("Tables");
            chTablesCount.TextAlign = HorizontalAlignment.Right;
            chTablesCount.Width = 70;

            //Inizializza la listview con la lista delle tabelle di un database
            listTables.View = System.Windows.Forms.View.Details;
            listTables.FullRowSelect = true;
            listTables.MultiSelect = false;
            listTables.CheckBoxes = true;
            listTables.GridLines = true;
            listTables.HideSelection = false;

            ColumnHeader chTable = listTables.Columns.Add("Tables");
            chTable.Width = 350;
            ColumnHeader chTableRows = listTables.Columns.Add("Rows");
            chTableRows.TextAlign = HorizontalAlignment.Right;
            chTableRows.Width = 80;
            ColumnHeader chTableOp = listTables.Columns.Add("Operation");
            chTableOp.Width = 200;
            ColumnHeader chTableOpDdl = listTables.Columns.Add("Schema Operation");
            chTableOpDdl.Width = 200;
        
        }

        //Carica la lista dei server sorgenti
        void FillSourceServers()
        {
            //Riempe il combo con la lista dei server di destinazione.
            //TODO Ora carica un db fisso da parametrizzare su un file xml o in altro modo
            ServerInfo yapp15 = new ServerInfo();
            yapp15.ConnectionString = "YVIKINGDEV01BLQ";
            yapp15.Name = "YVIKINGDEV01BLQ";
            sourceServers.Add(yapp15);
            cmbSourceServer.DisplayMember = "Name";
            cmbSourceServer.ValueMember = "ConnectionString";
            cmbSourceServer.DataSource = sourceServers;

        }

        //Carica la lista dei server di destinazione
        void FillTargetServers()
        {
            //Riempe il combo con la lista dei server di destinazione.
            //TODO Ora carica un db fisso da parametrizzare su un file xml o in altro modo
            ServerInfo yapp15local = new ServerInfo();
            yapp15local.ConnectionString = @"localhost\viking";
            yapp15local.Name = "local viking";
            targetServers.Add(yapp15local);
            cmbTargetServer.DisplayMember = "Name";
            cmbTargetServer.ValueMember = "ConnectionString";
            cmbTargetServer.DataSource = targetServers;

        }

        //Procedura di inizializzazione della window
        private void frmTestDbManager_Load(object sender, EventArgs e)
        {
            //Carica i combo box per i server sorgente e destinazione disponibili
            FillSourceServers();
            FillTargetServers();

            //Imposta i riferimenti ai controlli utilizzati dalla classe di gestione per dare feedback visivo
            testDbManager.statusLabel = lblStatus;
            testDbManager.statusRows = StatusRows;
            testDbManager.progressBar = progressBar;

        }

        //Lancia la procedura di creazione locale della stuttura dei databases del server selezionato.
        //ATTENZIONE La creazione non e' incrementale e non cancella la struttura presente nel database/server di destinazione.
        //Cancellare i database di destinazione manualmente (se necessario) prima di lanciare la procedura
        private void btnSchema_Click(object sender, EventArgs e)
        {
            testDbManager.ServerSchemaClone();

        }

        //Lancia la procedura di aggiornamento dei dati di tutti i database del server selezionato
        private void btnData_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            testDbManager.UpdateServerData();
            this.Cursor = Cursors.Default;
        }

        //Lancia la procedura di creazione locale della stuttura del database selezionato
        //ATTENZIONE La creazione non e' incrementale e non cancella la struttura presente nel database/server di destinazione.
        //Cancellare i database di destinazione manualmente (se necessario) prima di lanciare la procedura
        private void btnUpdateDatabaseSchema_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (listDatabases.FocusedItem != null)
                testDbManager.DatabaseSchemaClone(listDatabases.FocusedItem.Text);
            this.Cursor = Cursors.Default;
        }

        //Lancia la procedura di aggiornamento dei dati del database selezionato
        private void btnUpdateDatabaseData_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (listDatabases.FocusedItem != null)
                testDbManager.UpdateDatabaseData(((DatabaseData)listDatabases.FocusedItem.Tag).database.Name );
            this.Cursor = Cursors.Default;

        }

        //Cambia il database selezionato e viene ricreata la lista delle tabelle 
        //I dati vengono presi dal file xml di guida alla duplicazione del db.
        private void listDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (listDatabases.FocusedItem != null)
            {
                ListViewItem item = listDatabases.FocusedItem;
                XmlElement tables = ((DatabaseData) item.Tag).tables;
                listTables.Items.Clear();
                if (tables != null)
                {
                    foreach (XmlElement table in tables.ChildNodes)
                    {
                        ListViewItem i = listTables.Items.Add(table.Attributes["name"].Value);

                        ListViewItem.ListViewSubItem subItemRowa = new ListViewItem.ListViewSubItem();
                        subItemRowa.Name = @"Rows";
                        subItemRowa.Text = table.Attributes["count"].Value;
                        i.SubItems.Add(subItemRowa);

                        ListViewItem.ListViewSubItem subItemOp = new ListViewItem.ListViewSubItem();
                        subItemOp.Name = @"Operation";
                        subItemOp.Text = table.Attributes["operation"].Value;
                        i.SubItems.Add(subItemOp);

                        ListViewItem.ListViewSubItem subItemOpDdl = new ListViewItem.ListViewSubItem();
                        subItemOpDdl.Name = @"Operation Ddl";
                        subItemOpDdl.Text = table.Attributes["operationddl"].Value;
                        i.SubItems.Add(subItemOpDdl);

                    }
                }
            }
            this.Cursor = Cursors.Default;
        }
        
        //Crea a partire dalla lista delle tabelle dei database del server il file xml di guida per l'elaborazione
        private void btnUpdateServerXml_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            testDbManager.UpdateServerXml();
            this.Cursor = Cursors.Default;
        }

        //Cambia il server sorgente mi connetto al nuovo server e aggiorno la lista dei database disponibili
        private void cboSourceServer_SelectedIndexChanged(object sender, EventArgs e)
        {

            testDbManager.SourceServerConnect((ServerInfo)cmbSourceServer.SelectedItem);
            databaseList = testDbManager.DatabaseList();
            foreach (DatabaseData dbd in databaseList)
            {
                ListViewItem i = listDatabases.Items.Add(dbd.database.Name);
                ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
                subItem.Name = @"Tables";
                subItem.Text = dbd.database.Tables.Count.ToString();
                i.Tag = dbd;
                i.SubItems.Add(subItem);
            }

        }

        //Cambia il server di destinazione mi connetto al nuovo server
        private void cboTargetServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            testDbManager.TargetServerConnect((ServerInfo)cmbTargetServer.SelectedItem);
        }

        //Aggiorno il file di xml di guida all'elaborazione relativo al database selezionato
        private void btnUpdateDatabaseXml_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (listDatabases.FocusedItem != null)
                testDbManager.UpdateDatabaseXml(((DatabaseData)listDatabases.FocusedItem.Tag).database);
            this.Cursor = Cursors.Default;
        }

    }



}