//Author: leandro.cannizzaro@gmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace SmartSqlServerCloner
{
    //Copia la struttura completa da un server/database sorgente a un server/database destinazione
    //Copia i dati da un server/database sorgente a un server/database destinazione. Usando se richiesto una copia personalizzabile per tabella
    public class SmartSqlServerCloner
    {
        //Numero massimo di righe copiate dalla sorgente alla destinazione
        //E' basso per il debug (Modificare o eliminare quando si usa in produzione)
        const int MAXROWS_FETCHED = 10000;

        //Oggetti di interfaccia per il feedback visivo delle operazioni.
        //Devono essere impostati prima dell'uso dei metodi che effettuano le elaborazioni.
        public Label statusLabel;
        public Label statusRows;
        public ProgressBar progressBar;

        //Server e connessionesorgenti
        private Server sourceServer;
        private ServerConnection sourceServerConnection;
        public string sourceSQLServer;

        //Server e connessione di destinazione
        private Server targetServer;
        private ServerConnection targetServerConnection;
        public string targetSQLServer;

        //Si connette al server sorgente
        public void SourceServerConnect(ServerInfo server)
        {
            sourceServerConnection = new ServerConnection(server.ConnectionString);
            sourceServer = new Server(sourceServerConnection);
        }

        //Si connette al server di destinazione
        public void TargetServerConnect(ServerInfo server)
        {
            targetServerConnection = new ServerConnection(server.ConnectionString);
            targetServer = new Server(targetServerConnection);
        }

        //Ritorna la lista dei database non di sistema presenti nel server sorgente
        //La lista e' una lista di oggetti DatabaseData che contengono un riferimento al db sql e
        //un riferimento alla collezione di nodi xml relativi alle tabelle e alle operazioni da fare sulle stesse.
        //La lista dei nodi e' ottenuta dai file di fonfigurazione presenti nella cartella DatabaseXmlDrivers.
        public List<DatabaseData> DatabaseList()
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

        //Ritorna il path di base dove ricreare i file .sql con tutti gli script usati per la replicazione della struttura del database.
        String ServerBasePath()
        {
            return Path.Combine(Application.StartupPath, sourceServer.Name.Replace(@"\", "_"));
        }

        //Replica la struttura del server sorgente sul server destinazione.
        public void ServerSchemaClone()
        {

            //Ricrea sul server di destinazione tutti i database non di sistema presenti sul server sorgente.
            CopyServerObject<Database>(ServerBasePath(), sourceServer, targetServer, sourceServer.Databases, targetServer.Databases);
            
            //Aggiorna le struttura nella classi locali smo per comprendere i nuovi db appena creati nel target.
            //Corrisponde a fare un refresh in Management Studio
            targetServer.Refresh();
            targetServer.Databases.Refresh();


            //Ricrea sul server di destinazione tutti gli oggetti Login del server sorgente. I login vengono ricreati disabilitati.
            CopyServerObject<Login>(ServerBasePath(), sourceServer, targetServer, sourceServer.Logins, targetServer.Logins);

            //Crea un file UpdateLogins.log che viene usata per loggare i problemi incontrati nel riattivare i login sul
            //database destinazione.
            //Ci possono essere problemi di diversa natura a ricreare i logins sul db di destinazione legati al fatto 
            //che l'infrastruttura window di destinazione e' diversa e possono mancare gli oggetti windows necessari.
            //Se necessario ricreare l'ecosistema windows necessario manualmente.
            String TBasePath = Path.Combine(ServerBasePath(), typeof(Login).Name + "s"); //.Replace("Collection","s"));
            Directory.CreateDirectory(TBasePath);
            String fileName = Path.Combine(TBasePath, "UpdateLogins") + ".log";

            //Per ogni login cerca di abililitarlo e di impostare un apassword uguale al login name stesso.
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
            
            //Se necessario si possono replicare anche i jobs
            //CopyServerObject<Job>(ServerBasePath(), sourceServer, targetServer, sourceServer.JobServer.Jobs, targetServer.JobServer.Jobs);

            //Aggiorno lo stato visuale indicando che l'elaborazione e' terminata.
            Status("Update schema for server " + targetServer.Name + " completed.");
            progressBar.Value = 0;

        }

        //Ricrea sul server di destinazione tutti i database (non di sistema) presenti sul server sorgente
        public void DatabasesSchemaClone()
        {

            foreach (Database sourceDatabase in sourceServer.Databases)
            {
                if (!sourceDatabase.IsSystemObject)
                {
                    DatabaseSchemaClone(sourceDatabase.Name);
                }
            }
            //Aggiorno lo stato visuale indicando che l'elaborazione e' terminata.
            Status("Update all databases schema for server " + targetServer.Name + " completed.");
            progressBar.Value = 0;
        }

        //Ricrea sul server di destinazione lo schema del database indicato come parametro.
        public void DatabaseSchemaClone(string databaseName)
        {
            Database targetDatabase = null;
            Database sourceDatabase = null;
            sourceDatabase = sourceServer.Databases[databaseName];
            
            if (!targetServer.Databases.Contains(databaseName))
            {
                //Ricrea sul server di destinazione il database dal server sorgente. se non esistente.
                ICollection dblist = new Collection<Database>(){sourceServer.Databases[databaseName]};
                CopyServerObject<Database>(ServerBasePath(), sourceServer, targetServer, dblist, targetServer.Databases);
                //aggiorno le strutture interne di smo.
                targetServer.Refresh();
                targetServer.Databases.Refresh();
                //prendo il riferimento al nuovo db creato.
                targetDatabase = targetServer.Databases[databaseName];
            }
            else
            {
                //prendo il riferimento al db
                targetDatabase = targetServer.Databases[databaseName];
            }

            //Se sono riuscito a individuare il server di destinazione lancio il metodo che ricrea la struttura del database
            //sorgente sul database destinazione
            if (targetDatabase != null)
                CopyDatabaseStructure(sourceDatabase, targetDatabase);

        }

        //Replica sul database di destinazione la struttura del db sorgente
        public void CopyDatabaseStructure(Database sourceDatabase, Database targetDatabase)
        {
            //Creo il path locale dove scrivere tutti i file di script sql che uso per ricreare il db.
            string databaseBasePath = Path.Combine(ServerBasePath(), "Databases");
            databaseBasePath = Path.Combine(databaseBasePath, sourceDatabase.Name);
            Directory.CreateDirectory(databaseBasePath);


            //Per ogni tipo di oggetto del database lancio un metodo generico che ricrea tutti gli oggetti db del tipo passato come parametro generico
            //sul database di destinazione
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
            CopyDatabaseObject<User>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Users, targetDatabase.Users);
            CopyDatabaseObject<Microsoft.SqlServer.Management.Smo.Rule>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Rules, targetDatabase.Rules);
            CopyDatabaseObject<Schema>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Schemas, targetDatabase.Schemas);
            CopyDatabaseObject<Table>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Tables, targetDatabase.Tables);

            
            //TODO Creazione dei trigger per tutte le tabelle
            //Vedi nota nel metodo CopyDatabaseObject
            //foreach (Table sourceTable in sourceDatabase.Tables)
            //{
            //    if (!sourceTable.IsSystemObject)
            //    {
            //        Table targetTable = targetDatabase.Tables[sourceTable.Name];
            //        if (sourceTable.Triggers.Count > 0)
            //            CopyDatabaseObject<Trigger>(databaseBasePath, sourceDatabase, targetDatabase, sourceTable.Triggers, targetTable.Triggers);
            //    }
            //}

            CopyDatabaseObject<Microsoft.SqlServer.Management.Smo.View>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Views, targetDatabase.Views);
            CopyDatabaseObject<StoredProcedure>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.StoredProcedures, targetDatabase.StoredProcedures);
            CopyDatabaseObject<UserDefinedFunction>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.UserDefinedFunctions, targetDatabase.UserDefinedFunctions);
            CopyDatabaseObject<DatabaseDdlTrigger>(databaseBasePath, sourceDatabase, targetDatabase, sourceDatabase.Triggers, targetDatabase.Triggers);
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //Aggiorno lo stato visuale indicando che l'elaborazione e' terminata.
            Status("Update schema for database " + sourceDatabase.Name + " completed.");
        }

        //Metodo generico he copia tutti gli oggetti sql server, figli di un database e di un determinato tipo, da un db sorgente a un db destinazione.
        //Inoltre crea per ogni oggetto il file di script sql usato per la creazione.
        //Gli script da usare vengono richiesti al framework smo e eseguiti.
        public void CopyDatabaseObject<T>(string databaseBasePath, Database sourceDatabase, Database targetDatabase, ICollection sourceCollection, ICollection targetCollection)
        {
            ScriptingOptions scriptingOptions = new ScriptingOptions();

            //Creo la directory dove creare gli script sql
            String TBasePath = Path.Combine(databaseBasePath, typeof(T).Name + "s"); 
            Directory.CreateDirectory(TBasePath);
            
            //Inizializzo la progress bar per la notifica del progresso dell'esecuzione
            int errCount = 0;
            progressBar.Maximum = 0;
            progressBar.Maximum = sourceCollection.Count;
            
            //Scorro tutti gli oggetti del tipo T nel db sorgente e se non sono di sistema li inserisco in una lista degli oggetti da ricreare nella destinazione
            List<T> list = new List<T>();
            foreach (T t in sourceCollection)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                
                //Uso un try catch perche alcuni oggetti possono non avere la proprieta' IsSystemObject
                bool IsSystemObject = false;
                try
                {
                    IsSystemObject = (bool)t.GetType().GetProperty("IsSystemObject").GetValue(t, null);
                }
                catch (Exception e)
                {
                    IsSystemObject = false;
                }
                
                //Avanzamento dello stato
                String Name = (String)t.GetType().GetProperty("Name").GetValue(t, null);
                StatusAdvancing("PREPARING", typeof(T).Name + " - " + sourceServer.Name + "." + sourceDatabase.Name + "." + Name, progressBar.Value, progressBar.Maximum, 0);
                
                if (!IsSystemObject)
                    list.Add(t);
            }

            //Reinizializzo la progress bar sulla lista degli oggetti effettivamente da ricreare sulla destinazione.
            progressBar.Value = 0;
            progressBar.Maximum = list.Count;

            //Ciclo che fa l'operazione di copia effettiva
            foreach (T t in list)
            {
                Application.DoEvents();
                
                //Aggiorno lo stato visivo
                progressBar.Value += 1;
                String Name = (String)t.GetType().GetProperty("Name").GetValue(t, null);
                StatusAdvancing("EXECUTING", typeof(T).Name + " - " + sourceServer.Name + "." + sourceDatabase.Name + "." + Name, progressBar.Value, progressBar.Maximum, errCount);
                
                //Se l'oggetto esiste nella destinazione non faccio nulla.
                //TODO Da implementare uno schema piu' smart che magari testa la data di modifica per ricreare il nuovo oggetto sulla destinazione
                if (!Contains(targetCollection, Name))
                {
                    //Imposto il nome del file di script sql da creare
                    String fileName = Path.Combine(TBasePath, Name.Replace(@"\", "_").Replace(@":", "_").Replace("\\", "")) + ".sql";
                    
                    //Chiedo a smo di restiruirmi lo script di creazione dell'oggetto
                    //In caso di fallimento scrivo sul log e vado avanti
                    scriptingOptions.FileName = fileName; //Chiedo di salvare lo script su un file.
                    
                    //Nel caso di oggetto tabella genera gli script per la creazione dei trigger
                    //TODO Da valutare se crearli tabella per tabella o tutti insieme dopo la creazione di tutte le tabelle
                    //Bisogna verificare se ci sono problemi di creazione se inserisco un trigger il cui testo fa' riferimento a
                    //Tabelle e/o altri oggetti db non ancora creati.
                    //In questo caso bisogna creare tutti i trigger dopo la creazione delle tabelle.
                    //Il codice per far questo e' inserito per ora commentato (e ancora da testare)
                    //nel metodo CopyDatabaseStructure
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

                    //Se ho le righe dello scrip da eseguire le eseguo una alla volta sul db di destinazione
                    if (scriptRows != null)
                    {
                        foreach (String stm in scriptRows)
                        {
                            try
                            {
                                //CREATE TABLE Name(
                                //) ON [DATAFILEGROUP]
                                //Elimino la specifica del datafile group e opzioni varie lascio il default del database di destinazione
                                if (stm.Contains("CREATE TABLE"))
                                {
                                    String newStm = stm;
                                    int i = stm.IndexOf(") ON");
                                    if (i >= 0)
                                        newStm = stm.Substring(0,i + 1);
                                    targetDatabase.ExecuteNonQuery(newStm);

                                }else
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

        //Copia di tutti gli oggetti a livello server di un determinato tipo.
        public void CopyServerObject<T>(string serverBasePath, Server sourceServer, Server targetServer, ICollection sourceCollection, ICollection targetCollection)
        {
            ScriptingOptions scriptingOptions = new ScriptingOptions();
            
            //Creo una directory dove scrivere i file degli script generati
            String TBasePath = Path.Combine(serverBasePath, typeof(T).Name + "s"); 
            Directory.CreateDirectory(TBasePath);

            int errCount = 0;
            
            //Aggiorno interfaccia
            progressBar.Maximum = 0;
            progressBar.Maximum = sourceCollection.Count;
        
            //Creo una lista degli oggetti non di sistema da elaborare
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

            //Aggiorno l'interfaccia
            progressBar.Value = 0;
            progressBar.Maximum = list.Count;

            //Per ogno oggetto nella lista da elaborare eseguo la creazione nel server di destinazione
            foreach (T t in list)
            {
                String name = (String)t.GetType().GetProperty("Name").GetValue(t, null);
                
                //Aggiorno l'interfaccia utente
                Application.DoEvents();
                progressBar.Value += 1;
                StatusAdvancing("EXECUTING", typeof(T).Name + " - " + sourceServer.Name + "." + name, progressBar.Value, progressBar.Maximum, errCount);

                //Se l'oggetto non esiste nel server di destinazione lo creo
                //TODO Da gestire meccaninso legato alla data di modifica in modo da tenere aggiornato ilserver di destinazione
                if (!Contains(targetCollection, name))
                {
                    //Recupero dal db sorgente lo script per la creazione dell'oggetto
                    String fileName = Path.Combine(TBasePath, name.Replace(@"\", "_").Replace(@":", "_").Replace("\\","_")) + ".sql";
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

                    //Se ho recuperato lo script lo eseguo sul server di destinazione
                    if (scriptRows != null)
                    {
                        foreach (String stm in scriptRows)
                        {
                            try
                            {
                                //Nel caso di script database elimino le informazioni sulla dislocazione fisica dei file db e lascio che venga usato il default nel server di destinazione
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
                                else //esecuzione standard
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
        
        //Imposto lo stato di avanzamento visivo delle elaborazioni
        public void StatusAdvancing(string phase, string label, int elabCount, int totCount, int errCount)
        {
            string s = "";
            s += phase + ": ";
            s += elabCount.ToString("0000") + "/" + totCount.ToString("0000") + " ";
            if (errCount > 0)
                s += "Errors=" + errCount.ToString("0000");
            s += " - " + label;
            Status(s);
        }

        //Scrivo un item sul file di log
        public void Log(String FileName, Exception e, String statement)
        {
            String strLog = "LOG_START".PadRight(100, '-') + "\r\n";
            strLog += "OFFENDING_STATEMENT" + "\r\n";
            strLog += statement + "\r\n";
            strLog += "EXCEPTION_TEXT" + "\r\n";
            strLog += e.ToString() + "\r\n";
            strLog += "LOG_END".PadRight(100, '-') + "\r\n";

            File.AppendAllText(FileName + @".log", strLog);
        }

        //Creo i file xml di guida per l'elaborazione con la configurazione di default per tutti i databases di un server
        public void XmlDriverFileForServerCreate()
        {
            foreach (Database database in sourceServer.Databases)
            {
                if (!database.IsSystemObject)
                {
                    XmlDriverFileForDatabaseCreate(database);
                }
            }
            //Aggiorno lo stato visuale indicando che l'elaborazione e' terminata.
            Status("Create xml driver files for server " + targetServer.Name + " completed.");
            progressBar.Value = 0;

        }

        //Creo il file xml di guida per l'elaborazione per un singolo database
        public void XmlDriverFileForDatabaseCreate(Database database)
        {
            //Creo i file nella direcotry DatabaseXmlDrivers figlia della directory dell'eseguibile
            String databaseBasePath = Path.Combine(Application.StartupPath, "DatabaseXmlDrivers");

            //Creo o apro il file di guida corrispondente al database
            XmlDocument document = new XmlDocument();
            String fileName = Path.Combine(databaseBasePath, database.Name + ".xml");
            XmlElement tables = null;
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

            //Aggiorno l'interfaccia
            progressBar.Maximum = 0;
            progressBar.Maximum = database.Tables.Count;
            
            //Creo la solita lista degli oggetti non di sistema da elaborare
            List<Table> list = new List<Table>();
            foreach (Table t in database.Tables)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                StatusAdvancing("PREPARING", typeof(Table).Name + " - " + sourceServer.Name + "." + database.Name + "." + t.Schema + "." + t.Name, progressBar.Value, progressBar.Maximum, 0);
                if (!t.IsSystemObject) // && !Contains(targetCollection, Name))
                    list.Add(t);
            }

            //Aggiorno l'interfaccia
            progressBar.Value = 0;
            progressBar.Maximum = list.Count;

            //PEr ogni tabella da elaborare creo un nodo sul file xml
            foreach (Table table in list)
            {
                Application.DoEvents();
                progressBar.Value += 1;
                StatusAdvancing("EXECUTING", typeof(Table).Name + " - " + sourceServer.Name + "." + database.Name + "." + table.Schema + "." + table.Name, progressBar.Value, progressBar.Maximum, 0);
           
                //Se non esiste il nodo per la tabella lo creo.
                tableNode = (XmlElement)document.SelectSingleNode("/tables/table[@name='[" + database.Name + "].[" + table.Schema + "].[" + table.Name.Replace("'", "").Trim() + "]']");
                if (tableNode == null)
                {
                    tableNode = document.CreateElement("table");
                    tables.AppendChild(tableNode);
                }

                //Nome della tabella
                XmlAttribute attrName = tableNode.Attributes["name"];
                if (attrName == null)
                {
                    attrName = document.CreateAttribute("name");
                    tableNode.Attributes.Append(attrName);
                    attrName.Value = "[" + database.Name + "].[" + table.Schema + "].[" + table.Name + "]";
                }

                //Operazione da eseguire nel caso di caricamento dati.
                //E' il nome di un metodo in questa classe che esegue l'operazione
                XmlAttribute attrOp = tableNode.Attributes["operation"];
                if (attrOp == null)
                {
                    attrOp = document.CreateAttribute("operation");
                    attrOp.Value = "TableClone";
                    tableNode.Attributes.Append(attrOp);
                }

                //Operazione da eseguire nel caso di creazione schema
                //TODO Per ora non e' gestito questo attributo.
                //Bisogna prevedere il caso per escludere le tabelle spurie nel db di sorgente
                //XmlAttribute attrOpDdl = tableNode.Attributes["operationddl"];
                //if (attrOpDdl == null)
                //{
                //    attrOpDdl = document.CreateAttribute("operationddl");
                //    attrOpDdl.Value = "NoUpdate";
                //    tableNode.Attributes.Append(attrOpDdl);
                //}

                //Numero di righe della tabella
                XmlAttribute attrCount = tableNode.Attributes["count"];
                if (attrCount == null)
                {
                    attrCount = document.CreateAttribute("count");
                    tableNode.Attributes.Append(attrCount);
                }
                attrCount.Value = table.RowCount.ToString();

                //Metood alternativo di prendere il numeo di righe della tabella
                //DataSet dt = targetDatabase.ExecuteWithResults("SELECT Count(*) FROM [" + table.Name + "] WITH(NOLOCK) ");
                //int rows = (int)dt.Tables[0].Rows[0][0];

            }
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            document.Save(fileName);

            //Aggiorno lo stato visuale indicando che l'elaborazione e' terminata.
            Status("Create xml driver files for database " + database.Name + " completed.");
            progressBar.Value = 0;

        }

        //Replica i dati sul server sorgente nel server destinazione
        public void ServerDataClone()
        {
            Status("Start update data for server " + sourceServer.Name + ".");
            //PEr ogni database lancio il metodo di clone data per il database
            foreach (Database database in sourceServer.Databases)
            {
                if (!database.IsSystemObject)
                    DatabaseDataClone(database.Name);
            }
            Status("Update data for server " + sourceServer.Name + " completed.");
        }

        //Replica i dati dal database del server sorgente al database corrispondente nle server destinazione.
        //L'operazione da eseguirre puo essere configurata mettendo il nome di un metodo custom nel campo operation del file xml di guida.
        public void DatabaseDataClone(String sourceDatabaseName)
        {
            //Recupero l'oggetto database sorgente
            Database sourceDatabase = sourceServer.Databases[sourceDatabaseName];

            //Apro il file xml di guida per l'elaborazione
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

            //Recupero l'oggetto database di destinazione
            Database targetDatabase = targetServer.Databases[sourceDatabase.Name];
            
            //Aggiorno l'interfaccia
            progressBar.Maximum = 0;
            progressBar.Maximum = sourceDatabase.Tables.Count;

            //Recupero tutte le tabelle non di sistema sul db sorgente
            List<Table> tables = new List<Table>();
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
            
            //Aggiorno l'interfaccia
            progressBar.Value = 0;
            progressBar.Maximum = tables.Count;

            //PEr ogni tabella da elaborare
            foreach (Table sourceTable in tables)
            {
                //Aggiorno l'interfaccia
                Application.DoEvents();
                progressBar.Value += 1;
                StatusAdvancing("EXECUTING", typeof(Table).Name + " - " + sourceServer.Name + "." + sourceDatabase.Name + "." + sourceTable.Schema + "." + sourceTable.Name, progressBar.Value, progressBar.Maximum, 0);

                //Recupero il nodo della tabella dal file xml di guida.
                String fullName = @"[" + sourceDatabase.Name + @"].[" + sourceTable.Schema + @"].[" + sourceTable.Name + @"]";
                tableNode = (XmlElement)tablesNode.SelectSingleNode("table[@name='" + fullName + "']");
                if (tableNode != null)
                {
                    //Recupero l'operazione da eseguire.
                    //TODO PEr ora deve essere un metodo pubblico di questa classe
                    //Si puo' pensare a uno schema di plugin piu' sofisticato che prevede la creazione di una classe specifica dinamicamente
                    //Sono gia' implementate le operazioni:
                    //NoUpdate che non fa nulla
                    //TableUpdate che copia tutta la tabella (limitata a 100000 records)
                    string operation = tableNode.Attributes["operation"].Value;
                    
                    //Recupero e chiamo il metodo per la copia dei dati
                    MethodInfo m = this.GetType().GetMethod(operation);
                    if (m != null)
                    {
                        //Vedi Metodo TableUpdate per la firma del metodo e il significato dei parametri.
                        //Ogni metodo custom deve rispettare questa firma.
                        m.Invoke(this, new object[] { sourceDatabase, targetDatabase, sourceTable, tableNode, "", true });
                    }
                }
            }

            //Aggiorno l'interfaccia
            Status("Update data for database " + sourceServer.Name + "." + sourceDatabase.Name + " completed.");
            progressBar.Value = 0;
            progressBar.Maximum = 0;

            tables = null;
            sourceDatabase = null;
            targetDatabase = null;
        }

        //Aggiorno lo stato dell'elaborazione
        void Status(string t)
        {
            statusLabel.Text = t;
            statusLabel.Refresh();
            Application.DoEvents();
            Debug.Print(t);

        }
        
        //Metodo per la copia di tutti i dati di una tabella dalla sorgente alla destinazione
        //Questo metodo puo' essere personalizzato per avere comportamenti di copia diversi in base alla tabella.
        //Mettere nel campo operation del file xml di guida il nome di un metodo custom con la stessa firma e 
        //durante la copia dei dati verra' chiamato il metodo indicato nell'xml.
        public void TableClone(Database sourceDatabase, Database targetDatabase, Table sourceTable, XmlElement tableNode, String w, bool deleteFirst)
        {
            //Creo una directory dove scrivere i file di log degli errori
            String TBasePath = Path.Combine(ServerBasePath(), typeof(Database).Name + "s"); 
            Directory.CreateDirectory(TBasePath);

            //Determino nome tabella e nome file di log
            String tbName = "[" + sourceDatabase.Name + "].[" + sourceTable.Schema + "].[" + sourceTable.Name + "]";
            String logFileName = Path.Combine(TBasePath, tbName.Replace("[","").Replace("]","").Replace(@"\", "_").Replace(@":", "_")) + ".CloneData.log";

            SqlDataReader reader = null;

            //Memorizzo timestamp per inizio elaborazione
            DateTime start = DateTime.Now;
            
            try
            {
                //Mi connetto al server sorgente
                sourceServer.ConnectionContext.Connect();

                //Costruisco la select per il reader da creare nel database sorgente
                String s = @"SELECT TOP " + MAXROWS_FETCHED.ToString() + " * FROM " + tbName + " with(nolock) ";
                //Select senza limiti
                //String s = @"SELECT * FROM " + tbName + " with(nolock) ";
                if (w.Length > 0)
                    s += " WHERE " + w;

                //Aggiorno l'interfaccia
                statusRows.Text = @"Start Copy " + tbName;
                statusRows.Refresh();

                //Creo il reader nel server sorgente
                reader = sourceServer.ConnectionContext.ExecuteReader(s);

                //Mi connetto al server di destinazione
                targetServer.ConnectionContext.Connect();

                //Se richiesta la cancellazione dei dati precedenti cancello i dati
                if (deleteFirst)
                    targetServer.ConnectionContext.ExecuteNonQuery("DELETE FROM " + tbName);

                SqlBulkCopy sqlBulk = null;
                sqlBulk = new SqlBulkCopy(targetServer.ConnectionContext.SqlConnectionObject,
                            SqlBulkCopyOptions.KeepIdentity | 
                            SqlBulkCopyOptions.UseInternalTransaction 
                        , null);
                sqlBulk.BulkCopyTimeout = 120;
                sqlBulk.BatchSize = 100;
                sqlBulk.DestinationTableName = tbName;
                sqlBulk.NotifyAfter = 100;
                //Aggancio l'evento generato dal blulk loader per il progresso dell'elaborazione
                sqlBulk.SqlRowsCopied += new SqlRowsCopiedEventHandler(SqlRowsCopied); 
                sqlBulk.WriteToServer(reader);
                sqlBulk.Close();
                sqlBulk = null;

            }
            catch (Exception e)
            {
                Log(logFileName, e, "Copy table data");
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

            //Aggiorno interfaccia
            statusRows.Text = String.Format("Copy complete in {0}  seconds.", DateTime.Now.Subtract(start).Seconds);
            statusRows.Refresh();
            Application.DoEvents();
        }

        //Aggiorno lo stato della copia dei dati
        private void SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            statusRows.Text = "Copied " + e.RowsCopied.ToString() + " rows.";
            statusRows.Refresh();
            Application.DoEvents();
        }


        //Implementazione del metodo contains per le collezioni smo (non e' presente nativamente)
        //E' una semplice ricerca lineare ma non e' necessario di piu' per gli usi che ne faccio.
        public bool Contains(ICollection collection, String Name)
        {
            foreach (Object o in collection)
            {

                String OName = (String)o.GetType().GetProperty("Name").GetValue(o, null);
                if (OName == Name)
                    return true;
            }
            return false;
        }

    }

    //Classe di appoggio per i dati di configurazione per un database
    public class DatabaseData
    {
        public XmlDocument xmlDocument;
        public XmlElement tables;
        public Database database;
    }

    //Classe di appoggio per la definizione delle connessioni ai server
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
