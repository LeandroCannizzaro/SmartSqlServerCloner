using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace TestDatabaseCreation
{
    public partial class frmTestDbManager : Form
    {
        private List<ServerInfo> sourceServers = new List<ServerInfo>();
        private List<ServerInfo> targetServers = new List<ServerInfo>();
 

        private List<DatabaseData> databaseList;
        private TestDbManager testDbManager = new TestDbManager();
        public frmTestDbManager()
        {
            InitializeComponent();
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

        void FillSourceServers()
        {
            ServerInfo yapp15 = new ServerInfo();
            yapp15.ConnectionString = "YVIKINGDEV01BLQ";
            yapp15.Name = "YVIKINGDEV01BLQ";
            sourceServers.Add(yapp15);
            cmbSourceServer.DisplayMember = "Name";
            cmbSourceServer.ValueMember = "ConnectionString";
            cmbSourceServer.DataSource = sourceServers;

            //DataTable servers = SmoApplication.EnumAvailableSqlServers();
            //foreach (var server in servers.Rows)
            //{
            //    cmbSourceServer.Items.Add(server);
            //}

        }

        void FillTargetServers()
        {
            ServerInfo yapp15local = new ServerInfo();
            yapp15local.ConnectionString = @"localhost\viking";
            yapp15local.Name = "local viking";
            targetServers.Add(yapp15local);
            cmbTargetServer.DisplayMember = "Name";
            cmbTargetServer.ValueMember = "ConnectionString";
            cmbTargetServer.DataSource = targetServers;

            //DataTable servers = SmoApplication.EnumAvailableSqlServers();
            //foreach (var server in servers.Rows)
            //{
            //    cmbSourceServer.Items.Add(server);
            //}

        }


        private void frmTestDbManager_Load(object sender, EventArgs e)
        {
            FillSourceServers();
            FillTargetServers();
            //return;
            //Set Your SQL Server Name Here
            testDbManager.statusLabel = lblStatus;
            testDbManager.statusRows = StatusRows;
            testDbManager.progressBar = progressBar;
            //testDbManager.sourceSQLServer = @"172.18.31.33\ISTDB,1671";
            //txtSourceServer.Text = testDbManager.sourceSQLServer;
            ////Set Destination SQL Server Name Here
            //txtTargetServer.Text 
            //testDbManager.targetSQLServer = @"doG";
            //txtTargetServer.Text = testDbManager.targetSQLServer;
            
            //testDbManager.ServerConnect();
            //databaseList = testDbManager.LoadDatabaseList();
            //foreach (DatabaseData dbd in databaseList)
            //{
            //    ListViewItem i = listDatabases.Items.Add(dbd.database.Name);
            //    ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem();
            //    subItem.Name = @"Tables";
            //    subItem.Text = dbd.database.Tables.Count.ToString();
            //    i.Tag = dbd;
            //    i.SubItems.Add(subItem);
            //}
            //i.Tag = database;

        }

        private void btnSchema_Click(object sender, EventArgs e)
        {
            testDbManager.UpdateServerSchema();

        }
        
        private void btnData_Click(object sender, EventArgs e)
        {
            testDbManager.UpdateServerData();
        }

        private void btnUpdateDatabaseSchema_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (listDatabases.FocusedItem != null)
                testDbManager.UpdateDatabaseSchema(listDatabases.FocusedItem.Text);
            this.Cursor = Cursors.Default;
        }

        private void btnUpdateDatabaseData_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (listDatabases.FocusedItem != null)
                testDbManager.UpdateDatabaseData(((DatabaseData)listDatabases.FocusedItem.Tag).database.Name );
            this.Cursor = Cursors.Default;

        }

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
                    //testDbManager.LoadTablesList(listDatabases.FocusedItem.Text, listTables);
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

        private void btnUpdateServerXml_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            testDbManager.UpdateServerXml();
            this.Cursor = Cursors.Default;
        }

        private void cboSourceServer_SelectedIndexChanged(object sender, EventArgs e)
        {

            testDbManager.SourceServerConnect((ServerInfo)cmbSourceServer.SelectedItem);
            databaseList = testDbManager.LoadDatabaseList();
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

        private void cboTargetServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            testDbManager.TargetServerConnect((ServerInfo)cmbTargetServer.SelectedItem);
        }

        private void listTables_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdateDatabaseXml_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (listDatabases.FocusedItem != null)
                testDbManager.UpdateDatabaseXml(((DatabaseData)listDatabases.FocusedItem.Tag).database);
            this.Cursor = Cursors.Default;
        }

    }


    public class TestDbManager
    {
        public Label statusLabel;
        public Label statusRows;
        public ProgressBar progressBar;
        private Server sourceServer;
        private ServerConnection sourceServerConnection;
        public string sourceSQLServer;
        //private string sourceDatabase;
        
        private Server targetServer;
        private ServerConnection targetServerConnection;
        public string targetSQLServer;
        //private string targetDatabase;
        public string basepath;
        public const int OfferDaysRange = 5;
        public TestDbManager()
        {
            
        }

        public void SourceServerConnect(ServerInfo server)
        {
            sourceServerConnection = new ServerConnection(server.ConnectionString);
            sourceServer = new Server(sourceServerConnection);
        }

        public void TargetServerConnect(ServerInfo server)
        {
            targetServerConnection = new ServerConnection(server.ConnectionString);
            targetServer = new Server(targetServerConnection);
        }

        public List<DatabaseData> LoadDatabaseList()
        {
            List<DatabaseData> list = new List<DatabaseData>();
            foreach (Database database in sourceServer.Databases)
            {
                if (!database.IsSystemObject)
                {
                    DatabaseData dbd = new DatabaseData();
                    dbd.database = database;

                    dbd.xmlDocument = new XmlDocument();
                    String fileName = Path.Combine(Path.Combine(Application.StartupPath, "DatabaseXmlDrivers"), database.Name + ".xml");

                    //string databaseBasePath = Path.Combine(ServerBasePath(), "Databases");
                    //databaseBasePath = Path.Combine(databaseBasePath, database.Name);
                    //String fileName = Path.Combine(databaseBasePath, database.Name + ".xml");
                    XmlElement tables = null;
                    if (File.Exists(fileName))
                    {
                        dbd.xmlDocument.Load(fileName);
                        dbd.tables = dbd.xmlDocument.DocumentElement;
                    }
                    list.Add(dbd);
                }
            }
            return list;
        }

        String ServerBasePath()
        {
            return  Path.Combine(Application.StartupPath, sourceServer.Name.Replace(@"\", "_"));
        }

        public void UpdateServerSchema()
        {
            //foreach (Database database in sourceServer.Databases)
            //{
            //    if (!database.IsSystemObject)
            //    {
            //        if (!targetServer.Databases.Contains(database.Name))
            //        {

            //            Database targetDatabase = new Database(targetServer, database.Name);
            //            targetDatabase.Create();
            //        }
            //    }
            //}
            CopyServerObject<Database>(ServerBasePath(), sourceServer, targetServer, sourceServer.Databases, targetServer.Databases);


            CopyServerObject<Login>(ServerBasePath(), sourceServer, targetServer, sourceServer.Logins, targetServer.Logins);

            String TBasePath = Path.Combine(ServerBasePath(), typeof(Login).Name + "s"); //.Replace("Collection","s"));
            Directory.CreateDirectory(TBasePath);
            String fileName = Path.Combine(TBasePath, "UpdateLogin") + ".sql";


            foreach (Login login in targetServer.Logins)
            {
                if (!login.IsSystemObject)
                {
                    if (sourceServer.Logins.Contains(login.Name))
                    {
                        try
                        {
                            login.Enable();
                            login.ChangePassword(login.Name);
                        }
                        catch (Exception e)
                        {
                            Log(fileName, e, "");
                        }
                    }
                }
            }
            //CopyServerObject<Job>(ServerBasePath(), sourceServer, targetServer, sourceServer.JobServer.Jobs, targetServer.JobServer.Jobs);
            
            UpdateDatabasesSchema();
        }

        public void UpdateDatabaseSchema(string databaseName)
        {
            Database targetDatabase = null;
            Database sourceDatabase = null;
            sourceDatabase = sourceServer.Databases[databaseName];
            targetServer.Refresh();
            targetServer.Databases.Refresh();
            if (!targetServer.Databases.Contains(databaseName))
            {
                Debugger.Break();
                //targetDatabase = new Database(targetServer, databaseName);
                //targetDatabase.Create();
            }else
            {
                targetDatabase = targetServer.Databases[databaseName];
            }
            if (targetDatabase != null)
                CopyDatabaseStructure(sourceDatabase, targetDatabase);
            
        }

        public void UpdateDatabasesSchema()
        {

            foreach (Database sourceDatabase in sourceServer.Databases)
            {
                if (!sourceDatabase.IsSystemObject)
                {
                    UpdateDatabaseSchema(sourceDatabase.Name);
                }
            }
        }

        public bool Contains(ICollection collection, String Name )
        {
            foreach (Object o in collection)
            {

                String OName = (String)o.GetType().GetProperty("Name").GetValue(o, null);
                if (OName == Name)
                    return true;
            }
            return false;
        }

        public void CopyDatabaseObject<T>(string databaseBasePath, Database sourceDatabase, Database targetDatabase, ICollection sourceCollection, ICollection targetCollection)  
        {
            ScriptingOptions scriptingOptions = new ScriptingOptions();
            String TBasePath = Path.Combine(databaseBasePath, typeof(T).Name + "s"); //.Replace("Collection","s"));
            Directory.CreateDirectory(TBasePath);
            int errCount = 0;
            progressBar.Maximum = 0;
            progressBar.Maximum = sourceCollection.Count;  
            List<T> list = new List<T>();
            foreach (T t in sourceCollection)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                bool IsSystemObject = false;
                try
                {
                    IsSystemObject = (bool) t.GetType().GetProperty("IsSystemObject").GetValue(t, null);
                }catch(Exception e)
                {
                    IsSystemObject = false;
                }
                String Name = (String)t.GetType().GetProperty("Name").GetValue(t, null);
                StatusAdvancing("PREPARING", typeof(T).Name + " - " + sourceServer.Name + "." + sourceDatabase.Name + "." + Name, progressBar.Value, progressBar.Maximum, 0);
                if (!IsSystemObject) 
                    list.Add(t);
            }
            progressBar.Value = 0;
            progressBar.Maximum = list.Count;  

            foreach (T t in list)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                String Name = (String)t.GetType().GetProperty("Name").GetValue(t, null);
                StatusAdvancing("EXECUTING", typeof(T).Name + " - " + sourceServer.Name + "." + sourceDatabase.Name + "." + Name, progressBar.Value, progressBar.Maximum, errCount);
                if (!Contains(targetCollection, Name))
                {
                    String fileName = Path.Combine(TBasePath, Name.Replace(@"\", "_").Replace(@":", "_")) + ".sql";
                    scriptingOptions.FileName = fileName;
                    scriptingOptions.Triggers = true;
                    scriptingOptions.ExtendedProperties = true;
                    scriptingOptions.IncludeDatabaseContext = true; 
                    MethodInfo mi = t.GetType().GetMethod("Script", new Type[] { typeof(ScriptingOptions) });
                    StringCollection scriptRows = null;
                    try
                    {
                        scriptRows = (StringCollection)mi.Invoke(t, new object[] { scriptingOptions });
                    }
                    catch (Exception e)
                    {
                        errCount++;
                        Log(fileName, e, "");
                    }
                    if (scriptRows != null)
                    {
                        foreach (String stm in scriptRows)
                        {
                            try
                            {
                                targetDatabase.ExecuteNonQuery(stm);
                            }
                            catch (Exception e)
                            {
                                errCount++;
                                Log(fileName, e, stm);
                            }
                        }
                    }
                }
            }
            progressBar.Value = 0;
            
        }


        public void CopyDatabaseTables(string databaseBasePath, Database sourceDatabase, Database targetDatabase, ICollection sourceCollection, ICollection targetCollection)  
        {
            ScriptingOptions scriptingOptions = new ScriptingOptions();
            String TBasePath = Path.Combine(databaseBasePath, typeof(Table).Name + "s"); //.Replace("Collection","s"));
            Directory.CreateDirectory(TBasePath);
            int errCount = 0;
            progressBar.Maximum = 0;
            progressBar.Maximum = sourceCollection.Count;
            List<Table> list = new List<Table>();
            foreach (Table t in sourceCollection)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                bool IsSystemObject = (bool)t.GetType().GetProperty("IsSystemObject").GetValue(t, null);
                String Name = (String)t.GetType().GetProperty("Name").GetValue(t, null);
                StatusAdvancing("PREPARING", typeof(Table).Name + " - " + sourceServer.Name + "." + sourceDatabase.Name + "." + Name, progressBar.Value, progressBar.Maximum, 0);
                if (!IsSystemObject)
                    list.Add(t);
            }
            progressBar.Value = 0;
            progressBar.Maximum = list.Count;

            foreach (Table t in list)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                String Name = (String)t.GetType().GetProperty("Name").GetValue(t, null);
                StatusAdvancing("EXECUTING", typeof(Table).Name + " - " + sourceServer.Name + "." + sourceDatabase.Name + "." + Name, progressBar.Value, progressBar.Maximum, errCount);
                if (!Contains(targetCollection, Name))
                {
                    String fileName = Path.Combine(TBasePath, Name.Replace(@"\", "_").Replace(@":", "_")) + ".sql";
                    scriptingOptions.FileName = fileName;
                    scriptingOptions.Triggers = true;
                    scriptingOptions.ExtendedProperties = true;
                    MethodInfo mi = t.GetType().GetMethod("Script", new Type[] { typeof(ScriptingOptions) });
                    StringCollection scriptRows = null;
                    try
                    {
                        scriptRows = (StringCollection)mi.Invoke(t, new object[] { scriptingOptions });
                    }
                    catch (Exception e)
                    {
                        errCount++;
                        Log(fileName, e, "");
                    }
                    if (scriptRows != null)
                    {
                        foreach (String stm in scriptRows)
                        {
                            try
                            {
                                targetDatabase.ExecuteNonQuery(stm);
                            }
                            catch (Exception e)
                            {
                                errCount++;
                                Log(fileName, e, stm);
                            }
                        }
                    }
                }
            }
            progressBar.Value = 0;

        }

        
        public void CopyServerObject<T>(string serverBasePath, Server sourceServer, Server targetServer, ICollection sourceCollection, ICollection targetCollection)
        {
            ScriptingOptions scriptingOptions = new ScriptingOptions();
            String TBasePath = Path.Combine(serverBasePath, typeof(T).Name + "s"); //.Replace("Collection","s"));
            Directory.CreateDirectory(TBasePath);

            int errCount = 0;
            progressBar.Maximum = 0;
            progressBar.Maximum = sourceCollection.Count;
            List<T> list = new List<T>();
            foreach (T t in sourceCollection)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                PropertyInfo propertyIsSystemObject = t.GetType().GetProperty("IsSystemObject");
                bool isSystemObject = false;
                if (propertyIsSystemObject != null)
                {
                    isSystemObject = (bool)propertyIsSystemObject.GetValue(t, null);
                }
                String Name = (String)t.GetType().GetProperty("Name").GetValue(t, null);
                StatusAdvancing("PREPARING", typeof(T).Name + " - " + sourceServer.Name + "." + Name, progressBar.Value, progressBar.Maximum, 0);
                if (!isSystemObject) // && !Contains(targetCollection, Name))
                    list.Add(t);
            }
            progressBar.Value = 0;
            progressBar.Maximum = list.Count;  

            foreach (T t in list)
            {
                String name = (String)t.GetType().GetProperty("Name").GetValue(t, null);
                Application.DoEvents();
                progressBar.Value += 1;
                StatusAdvancing("EXECUTING", typeof(T).Name + " - " + sourceServer.Name + "." + name,progressBar.Value ,progressBar.Maximum, errCount);
                if (!Contains(targetCollection, name))
                {
                    String fileName = Path.Combine(TBasePath, name.Replace(@"\","_").Replace(@":","_")) + ".sql";
                    scriptingOptions.FileName = fileName;
                    MethodInfo mi = t.GetType().GetMethod("Script", new Type[] { typeof(ScriptingOptions) });

                    StringCollection scriptRows = null;
                    try
                    {
                        scriptRows = (StringCollection)mi.Invoke(t, new object[] { scriptingOptions });
                    }
                    catch (Exception e)
                    {
                        errCount++;
                        Log(fileName, e, "");
                    }
                    if (scriptRows != null)
                    {
                        foreach (String stm in scriptRows)
                        {
                            try
                            {
                                if (stm.Contains("CREATE DATABASE"))
                                {
                                    string newStm = "";
                                    /*
                                     CREATE DATABASE [crs] 
                                     ON  PRIMARY ( NAME = N'crs_dat', FILENAME = N'M:\Microsoft SQL Server\MSSQL.1\MSSQL\Data\crs.mdf' , SIZE = 40960000KB , MAXSIZE = UNLIMITED, FILEGROWTH = 3072000KB )
                                       LOG ON ( NAME = N'crs_log', FILENAME = N'S:\Microsoft SQL Server\MSSQL.1\MSSQL\Data\crs_log.ldf' , SIZE = 12032512KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024000KB )
                                     COLLATE SQL_Latin1_General_CP1_CI_AS
                                     */
                                    int pos1 = stm.IndexOf("ON"); // " PRIMARY");
                                    int pos2 = stm.LastIndexOf(")");
                                    newStm = stm.Substring(0, pos1 - 1);
                                    newStm += stm.Substring(pos2 + 1);

                                    targetServer.Databases["master"].ExecuteNonQuery(newStm);
                                }
                                else
                                {
                                    targetServer.Databases["master"].ExecuteNonQuery(stm);
                                }
                            }
                            catch (Exception e)
                            {
                                errCount++;
                                Log(fileName, e, stm);
                            }
                        }
                    }

                }
            }

        }

        public void StatusAdvancing(string phase, string label, int elabCount, int totCount,int errCount)
        {
            string s = "";
            s += phase + ": ";
            s += elabCount.ToString("0000") + "/" + totCount.ToString("0000") + " ";
            if (errCount > 0)
                s += "Errors=" + errCount.ToString("0000");
            s += " - " + label;
            Status(s);    
        }

        public void Log(String FileName, Exception e, String statement)
        {
            String strLog = "LOGSTART".PadRight(100, '-') + @"\r\n";
            strLog += e.ToString() + @"\r\n";
            strLog += "OFFENDING_STATEMENT".PadRight(100, '-') + @"\r\n";
            strLog += statement + @"\r\n";
            strLog += "LOGEND".PadRight(100,'-') + @"\r\n";

            File.AppendAllText(FileName + @".log", strLog);
        }

        public void CopyDatabaseStructure(Database sourceDatabase,Database targetDatabase)
        {
            string databaseBasePath = Path.Combine(ServerBasePath(), "Databases");
            databaseBasePath = Path.Combine(databaseBasePath, sourceDatabase.Name);
            Directory.CreateDirectory(databaseBasePath);

            CopyDatabaseObject<User>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Users, targetDatabase.Users);
            CopyDatabaseObject<Microsoft.SqlServer.Management.Smo.Rule>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Rules, targetDatabase.Rules);
            CopyDatabaseObject<Schema>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Schemas, targetDatabase.Schemas);
            CopyDatabaseObject<Table>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Tables, targetDatabase.Tables);

            //UpdateDatabaseXml(databaseBasePath, sourceDatabase);
            
            CopyDatabaseObject<Microsoft.SqlServer.Management.Smo.View>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Views, targetDatabase.Views);
            CopyDatabaseObject<StoredProcedure>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.StoredProcedures, targetDatabase.StoredProcedures);
            CopyDatabaseObject<UserDefinedFunction>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.UserDefinedFunctions, targetDatabase.UserDefinedFunctions);
            
            CopyDatabaseObject<DatabaseDdlTrigger>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Triggers, targetDatabase.Triggers);
            Status("Update schema for database " + sourceDatabase.Name + " completed.");
        }

        public void UpdateServerXml()
        {
            foreach (Database database in sourceServer.Databases)
            {
                if (!database.IsSystemObject)
                {
                    UpdateDatabaseXml( database);
                }
            }

        }

        public void UpdateDatabaseXml(Database database)
        {
            String databaseBasePath = Path.Combine(Application.StartupPath, "DatabaseXmlDrivers");

            XmlDocument document = new XmlDocument();
            String fileName = Path.Combine(databaseBasePath, database.Name + ".xml");
            XmlElement tables=null;
            XmlElement tableNode = null;
            if (File.Exists(fileName))
            {
                document.Load(fileName);
                tables = document.DocumentElement;
            }
            else
            {
                tables = document.CreateElement("tables");
                document.AppendChild(tables);
            }

            progressBar.Maximum = 0;
            progressBar.Maximum = database.Tables.Count;
            List<Table> list = new List<Table>();
            foreach (Table t in database.Tables)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                StatusAdvancing("PREPARING", typeof(Table).Name + " - " + sourceServer.Name + "." + database.Name + "." + t.Schema +"." + t.Name, progressBar.Value, progressBar.Maximum, 0);
                if (!t.IsSystemObject) // && !Contains(targetCollection, Name))
                    list.Add(t);
            }
            progressBar.Value = 0;
            progressBar.Maximum = list.Count;  

            foreach (Table table in list)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                StatusAdvancing("EXECUTING", typeof(Table).Name + " - " + sourceServer.Name + "." + database.Name + "." + table.Schema + "." + table.Name, progressBar.Value, progressBar.Maximum, 0);
                {
                    tableNode = (XmlElement)document.SelectSingleNode("/tables/table[@name='[" + database.Name + "].[" + table.Schema + "].[" +table.Name.Replace("'","").Trim() + "]']");
                    if (tableNode == null)
                    {
                        tableNode = document.CreateElement("table");
                        tables.AppendChild(tableNode);
                    }
                    
                    XmlAttribute attrName = tableNode.Attributes["name"];
                    if (attrName == null)
                    {
                        attrName = document.CreateAttribute("name");
                        tableNode.Attributes.Append(attrName);
                        attrName.Value = "[" + database.Name + "].[" + table.Schema + "].[" + table.Name + "]";
                    }
                        
                    
                    XmlAttribute attrOp = tableNode.Attributes["operation"];
                    if (attrOp == null)
                    {
                        attrOp = document.CreateAttribute("operation");
                        attrOp.Value = "NoUpdate";
                        tableNode.Attributes.Append(attrOp);
                    }

                    XmlAttribute attrOpDdl = tableNode.Attributes["operationddl"];
                    if (attrOpDdl == null)
                    {
                        attrOpDdl = document.CreateAttribute("operationddl");
                        attrOpDdl.Value = "NoUpdate";
                        tableNode.Attributes.Append(attrOpDdl);
                    }

                    XmlAttribute attrCount = tableNode.Attributes["count"];
                    if (attrCount == null)
                    {
                        attrCount = document.CreateAttribute("count");
                        tableNode.Attributes.Append(attrCount);
                    }
                    attrCount.Value = table.RowCount.ToString();
                    //DataSet dt = targetDatabase.ExecuteWithResults("SELECT Count(*) FROM [" + table.Name + "] WITH(NOLOCK) ");
                    //int rows = (int)dt.Tables[0].Rows[0][0];
                }

            }
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            document.Save(fileName);
        }

        public void UpdateServerData()
        {
            Status("Start update data for server " + sourceServer.Name + ".");
            foreach (Database database in sourceServer.Databases)
            {
                if (!database.IsSystemObject)
                    UpdateDatabaseData(database.Name);
            }
            Status("Update data for server " + sourceServer.Name + " completed.");
        }

        public void UpdateDatabaseData(String sourceDatabaseName)
        {
            Database sourceDatabase = sourceServer.Databases[sourceDatabaseName];
            
            string databaseBasePath = Path.Combine(ServerBasePath(), "Databases");
            databaseBasePath = Path.Combine(databaseBasePath, sourceDatabase.Name);

            XmlDocument document = new XmlDocument();
            String fileName = Path.Combine(Path.Combine(Application.StartupPath, "DatabaseXmlDrivers"), sourceDatabase.Name + ".xml");
            //String fileName = Path.Combine(databaseBasePath, sourceDatabase.Name + ".xml");
            XmlElement tablesNode = null;
            XmlElement tableNode = null;
            if (File.Exists(fileName))
            {
                document.Load(fileName);
                tablesNode = document.DocumentElement;
            }

            Database targetDatabase = targetServer.Databases[sourceDatabase.Name];
            List<Table> tables = new List<Table>();
            progressBar.Maximum = 0;
            progressBar.Maximum = sourceDatabase.Tables.Count;
            foreach (Table sourceTable in sourceDatabase.Tables)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                StatusAdvancing("PREPARING", typeof(Table).Name + " - " + sourceServer.Name + "." + sourceDatabase.Name + "." + sourceTable.Schema + "." + sourceTable.Name, progressBar.Value, progressBar.Maximum, 0);
                if (!sourceTable.IsSystemObject)
                {
                    tables.Add(sourceTable);
                }
            }
            progressBar.Value = 0;
            progressBar.Maximum = tables.Count;  

            foreach (Table sourceTable in tables)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                
                StatusAdvancing("EXECUTING", typeof(Table).Name + " - " + sourceServer.Name + "." + sourceDatabase.Name + "." + sourceTable.Schema + "." + sourceTable.Name, progressBar.Value, progressBar.Maximum, 0);
                
                String fullName = @"[" + sourceDatabase.Name + @"].[" + sourceTable.Schema + @"].[" + sourceTable.Name + @"]";
                
                tableNode = (XmlElement)tablesNode.SelectSingleNode("table[@name='" + fullName + "']");
                if (tableNode != null)
                {
                    string operation = tableNode.Attributes["operation"].Value;
                    //if (operation == "TableUpdate" && (fullName == "[crs].[crm_user].[user]"))
                    //if (operation == "DestinationUpdate")
                    //if (operation == "TableUpdate")
                    //    Debugger.Break();
                    //if (operation == "FlightUpdate" || operation == "FlightSegmentUpdate")
                    {
                        MethodInfo m = this.GetType().GetMethod(operation);
                        if (m != null)
                        {

                            m.Invoke(this, new object[] { sourceDatabase, targetDatabase, sourceTable, tableNode, "", true });
                        }
                    }
                }
            }
            Status("Update data for database " + sourceServer.Name + "." + sourceDatabase.Name + " completed.");
            progressBar.Value = 0;
            progressBar.Maximum = 0;

            tables = null;
            sourceDatabase = null;
            targetDatabase = null;
        }

        void Status(string t)
        {
            statusLabel.Text = t;
            statusLabel.Refresh();
            Application.DoEvents();
            Debug.Print(t);

        }

        public List<CustomOperationData> SearchCustomUpdateTableList(String CustomOperationName)
        {
            List<CustomOperationData> list = new List<CustomOperationData>();

            foreach (Database database in targetServer.Databases)
            {
                string databaseBasePath = Path.Combine(ServerBasePath(), "Databases");
                databaseBasePath = Path.Combine(databaseBasePath, database.Name);

                XmlDocument document = new XmlDocument();
                String fileName = Path.Combine(databaseBasePath, database.Name + ".xml");
                XmlElement tablesNode = null;
                XmlElement tableNode = null;
                if (File.Exists(fileName))
                {
                    document.Load(fileName);
                    tablesNode = document.DocumentElement;
                    foreach (XmlElement table in tablesNode.ChildNodes)
                    {
                        string operation = table.Attributes["operation"].Value;
                        if (operation == CustomOperationName)
                        {
                            string CustomOpData = table.Attributes["CustomOperation"].Value;
                            string[] fields = CustomOpData.Split('=');
                            CustomOperationData cu = new CustomOperationData();
                            cu.childField = fields[0];
                            if (fields.Length == 2)
                                cu.parentField = fields[1];
                            cu.Table = table.Attributes["name"].Value;
                            list.Add(cu);
                        }

                    }
                }
            }
            return list;
        }

        public CustomOperationData CustomUpdateDataGet(String databaseName,String tableSchema, String tableName)
        {
            CustomOperationData cu = null;

            string databaseBasePath = Path.Combine(ServerBasePath(), "Databases");
            databaseBasePath = Path.Combine(databaseBasePath, databaseName);

            XmlDocument document = new XmlDocument();
            String fileName = Path.Combine(Path.Combine(Application.StartupPath, "DatabaseXmlDrivers"), databaseName + ".xml");
            //String fileName = Path.Combine(databaseBasePath, databaseName + ".xml");
            XmlElement tablesNode = null;
            XmlElement tableNode = null;
            if (File.Exists(fileName))
            {
                document.Load(fileName);
                tablesNode = document.DocumentElement;
                //String fullName = @"[" + databaseName + @"].[" + tableSchema + @"].[" + tableName + @"]";
                tableNode = (XmlElement)tablesNode.SelectSingleNode("table[@name='" + tableName + "']");
                string CustomOpData = tableNode.Attributes["CustomOperation"].Value;
                string[] fields = CustomOpData.Split('=');
                cu = new CustomOperationData();
                cu.childField = fields[0];
                cu.parentField = fields[1];
                cu.Table = tableNode.Attributes["name"].Value;
            }
            
            return cu;
        }

        public void IdentitiesUpdate(Database sourceDatabase, Database targetDatabase, Table sourceTable, XmlElement tableNode, String w, bool deleteFirst)
        {
            
        }


        public void TableUpdate(Database sourceDatabase, Database targetDatabase, Table sourceTable,XmlElement tableNode, String w, bool deleteFirst)
        {
            Table targetTable;
            String tbName = "[" + sourceDatabase.Name + "].[" + sourceTable.Schema + "].[" + sourceTable.Name + "]";
            //SqlDataReader reader = sourceServerConnection.ExecuteReader( @"SELECT TOP 1000 * FROM " + tbName);
            SqlDataReader reader = null;
            DateTime start = DateTime.Now;
            targetTable = targetDatabase.Tables[sourceTable.Name, sourceTable.Schema];
            //if (targetTable.RowCount == 0)
            {
                try
                {
                    sourceServer.ConnectionContext.Connect();
                    String s = @"SELECT TOP 100000 * FROM " + tbName + " with(nolock) ";
                    if (w.Length > 0)
                        s += " WHERE " + w;


                    statusRows.Text = @"Start Copy " + tbName;
                    statusRows.Refresh();

                    reader = sourceServer.ConnectionContext.ExecuteReader(s);

                    // ensure that our destination table is empty:
                    targetServer.ConnectionContext.Connect();
                    if (deleteFirst)
                        targetServer.ConnectionContext.ExecuteNonQuery("DELETE FROM " + tbName);

                    SqlBulkCopy sqlBulk = null;
                    //if (targetTable.Name == "SoftwareCRS")
                    {
                        //targetTable.i
                        //targetServer.ConnectionContext.ExecuteNonQuery("SET IDENTITY_INSERT " + tbName + " ON ");
                        sqlBulk = new SqlBulkCopy(targetServer.ConnectionContext.SqlConnectionObject,
                            SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction,null);
                    }
                    //else
                    //{
                    //    sqlBulk = new SqlBulkCopy(targetServer.ConnectionContext.SqlConnectionObject);
                    //}
                    //targetServer.ConnectionContext.ExecuteNonQuery("DELETE FROM " + tbName);
                    //Debug.Print("Beginning Copy " + tbName);
                    //targetServerConnection.SqlConnectionObject.Open();
                    // using SqlDataReader to copy the rows:
                    

                    //using (SqlBulkCopy sqlBulk = new SqlBulkCopy(targetServer.ConnectionContext.SqlConnectionObject))
                        //using (SqlBulkCopy s = new SqlBulkCopy(targetServerConnection.SqlConnectionObject))
                    {
                        sqlBulk.BulkCopyTimeout = 120;
                        sqlBulk.BatchSize = 100;
                        sqlBulk.DestinationTableName = tbName;
                        sqlBulk.NotifyAfter = 100;
                        sqlBulk.SqlRowsCopied += new SqlRowsCopiedEventHandler(s_SqlRowsCopied);
                        sqlBulk.WriteToServer(reader);
                        sqlBulk.Close();
                    }
                    sqlBulk = null;

                }
                catch (Exception e)
                {
                    Debugger.Break();
                }
                finally
                {
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                            reader.Close();
                        reader = null;
                    }
                    sourceServer.ConnectionContext.Disconnect();
                    targetServer.ConnectionContext.Disconnect();
                }
                //Debug.Print("Copy complete in {0}  seconds.", DateTime.Now.Subtract(start).Seconds);
                statusRows.Text = String.Format("Copy complete in {0}  seconds.", DateTime.Now.Subtract(start).Seconds);
                statusRows.Refresh();
                Application.DoEvents();
            }
        }

        private void s_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            statusRows.Text= "Copied " + e.RowsCopied.ToString() + " rows.";
            statusRows.Refresh();
            Application.DoEvents();
        }
    }

    public class DatabaseData
    {
        public XmlDocument xmlDocument;
        public XmlElement tables;
        public Database database;
    }

    public class CustomOperationData
    {
        public String parentField;
        public String Table;
        public String childField;
    }

    public class ServerInfo
    {
        public String ConnectionString;
        public String Name;
        public override String ToString()
        {
            return ConnectionString;
        }
    }

}