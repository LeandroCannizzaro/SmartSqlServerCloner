using System;
using System.Collections.Generic;
using System.Text;

//public void UpdateServerStructure()
//{
//    sourceServerConnection = new ServerConnection(sourceSQLServer, "sa", "EMs4$_!gt");
//    //Set Source SQL Server Instance Information
//    sourceServer = new Server(sourceServerConnection);

//    targetServerConnection = new ServerConnection(targetSQLServer, "sa", "password1");
//    //Set Source SQL Server Instance Information
//    targetServer = new Server(targetServerConnection);

//    basepath = Application.StartupPath;
//    basepath = Path.Combine(basepath, sourceServer.Name.Replace(@"\", "_"));



//    foreach (Database database in sourceServer.Databases)
//    {
//        if (!database.IsSystemObject)
//        {
//            if (!targetServer.Databases.Contains(database.Name))
//            {
//                Database targetDatabase = new Database(targetServer, database.Name);
//                targetDatabase.Create();
//            }
//        }
//    }

//    CopyServerObject<Login>(basepath, sourceServer, targetServer, sourceServer.Logins, targetServer.Logins);

//    String TBasePath = Path.Combine(basepath, typeof(Login).Name + "s"); //.Replace("Collection","s"));
//    Directory.CreateDirectory(TBasePath);
//    String fileName = Path.Combine(TBasePath, "Update") + ".sql";


//    foreach (Login login in targetServer.Logins)
//    {
//        if (!login.IsSystemObject)
//        {
//            if (sourceServer.Logins.Contains(login.Name))
//            {
//            try
//            {
//                login.Enable();
//                login.ChangePassword(login.Name);
//            }
//            catch (Exception e)
//            {
//                Log(fileName, e, "");
//            }
//            }
//        }
//    }
//    CopyServerObject<Job>(basepath, sourceServer, targetServer, sourceServer.JobServer.Jobs, targetServer.JobServer.Jobs);

//    foreach (Database database in sourceServer.Databases)
//    {
//        if (!database.IsSystemObject)
//        {
//            //if (database.Name == "kettle")
//            {
//                Database targetDatabase;
//                targetDatabase = targetServer.Databases[database.Name];

//                CopyDatabaseStructure(database,targetDatabase);
//                //CopyDatabaseData(database);
//            }
//        }
//    }


//    sourceServer = null;
//    targetServer = null;
//}

