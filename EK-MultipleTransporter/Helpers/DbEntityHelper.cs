﻿using EK_MultipleTransporter.DmsDocumentManagementService;
using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace EK_MultipleTransporter.Helpers
{
    public class DbEntityHelper
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly string OtcsDbConnStr = ConfigurationManager.ConnectionStrings["OTCSCnnStr"].ConnectionString;
        public static EntityNode GetNodeByName(long parentNodeId, string name)
        {
            EntityNode result = null;
            //string query = "Select * FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId AND [Name] LIKE @name";
            // Index i kullanalım hacı.
            const string query = "Select DataID, OwnerID FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId AND [Name] LIKE @name";
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", "%"+name+"%");
                command.Parameters.AddWithValue("@parentNodeId", parentNodeId);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var itemNodeId = Convert.ToInt64(reader["DataID"]);
                        Console.WriteLine("Item Node Id is that :: " + itemNodeId);
                        result = VariableHelper.Dmo.GetEntityNodeFromId("admin", VariableHelper.Token, itemNodeId, false, false, false);
                    }
                    reader.Close();
                    connection.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Resources.ErrorTypeDbTransacts + ex.ToString());
                    Logger.Error(ex, Resources.ErrorTypeDbTransacts);
                    throw;
                }
            }

        }

        public static List<EntityNode> GetNodesByNameInExactParent(long parentNodeId, string name)
        {
            var result = new List<EntityNode>();
            //string query = "Select * FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId AND [Name] LIKE @name";

            const string query = "Select DataID FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId AND [Name] LIKE @name";


            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", "%" + name + "%");
                command.Parameters.AddWithValue("@parentNodeId", parentNodeId);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var itemNodeId = Convert.ToInt64(reader["DataID"]);
                        Console.WriteLine("Item Node Id is that :: " + itemNodeId);
                        result.Add(VariableHelper.Dmo.GetEntityNodeFromId("admin", VariableHelper.Token, itemNodeId, false, false, false));
                    }
                    reader.Close();
                    connection.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Resources.ErrorTypeDbTransacts + ex.ToString());
                    Logger.Error(ex, Resources.ErrorTypeDbTransacts);

                    throw;
                }
            }

        }

        public static List<EntityNode> GetNodesByIdPartially(long parentNodeId, int offSetPoint, int fetchNextAmount)
        {
            var result = new List<EntityNode>();

            // OFFSET 10 ROWS
            // FETCH NEXT 10 ROWS ONLY;
            //string query = "Select * FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId AND [Name] LIKE @name";
            const string query = "Select DataID FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId ORDER BY (SELECT NULL) OFFSET @offSetPoint ROWS FETCH NEXT @fetchNextAmount ROWS ONLY";

            var connStrBuilder = new SqlConnectionStringBuilder(GetConnectionString()) {ConnectTimeout = 300};

            using (var connection = new SqlConnection(connStrBuilder.ConnectionString))
            {
                
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@offSetPoint", offSetPoint);
                command.Parameters.AddWithValue("@fetchNextAmount", fetchNextAmount);
                command.Parameters.AddWithValue("@parentNodeId", parentNodeId);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var itemNodeId = Convert.ToInt64(reader["DataID"]);
                        Console.WriteLine("Item Node Id is that :: " + itemNodeId);
                        result.Add(VariableHelper.Dmo.GetEntityNodeFromId("admin", VariableHelper.Token, itemNodeId, false, false, false));
                    }
                    reader.Close();
                    connection.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Resources.ErrorTypeDbTransacts + ex.ToString());
                    Logger.Error(ex, Resources.ErrorTypeDbTransacts);

                    throw;
                }
            }
        }

        public static List<EntityNode> GetNodesByName(string name)
        {
            var result = new List<EntityNode>();
            //string query = "Select * FROM [OTCS].[dbo].[DTreeCore] Where [Name] LIKE @name";
            const string query = "Select DataID FROM [OTCS].[dbo].[DTreeCore] Where [Name] LIKE @name";

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", "%" + name + "%");

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        //Console.WriteLine(reader[0]);
                        var itemNodeId = Convert.ToInt64(reader["DataID"]);
                        Console.WriteLine("Item Node Id is that :: " + itemNodeId);
                        var newNode = VariableHelper.Dmo.GetEntityNodeFromId("admin", VariableHelper.Token, itemNodeId, false, false, false);
                        result.Add(newNode);
                    }
                    reader.Close();
                    connection.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Resources.ErrorTypeDbTransacts + ex.ToString());
                    Logger.Error(ex, Resources.ErrorTypeDbTransacts);

                    throw;
                }
            }

        }

        public static EntityNode GetAncestorNodeByName(long parentNodeId, string name)
        {
            EntityNode result = null;
            //string query = "Select * FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId AND UPPER([Name]) LIKE @name";
            const string query = "Select DataID FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId AND UPPER([Name]) LIKE @name";


            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", "%" + name + "%");
                command.Parameters.AddWithValue("@parentNodeId", parentNodeId);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var itemNodeId = Convert.ToInt64(reader["DataID"]);
                        Console.WriteLine("Item Node Id is that :: " + itemNodeId);
                        result = VariableHelper.Dmo.GetEntityNodeFromId("admin", VariableHelper.Token, itemNodeId, false, false, false);
                    }
                    reader.Close();
                    connection.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Resources.ErrorTypeDbTransacts + ex.ToString());
                    Logger.Error(ex, Resources.ErrorTypeDbTransacts);
                    throw;
                }
            }

        }

        private static string GetConnectionString()
        {
            return OtcsDbConnStr;
        }

        public static List<EntityNode> GetNodesByCategoryAttribute(long defId, string valStr)
        {

            // PROJE İçerisindeki PYP NO Item in adının ilk kısmına denk geliyor. Örn 1001/ ALtındağ bina ... nın pyp nosu 1001 --
            // Onu da ValStr içerisindeki 8 itemden birinde göreceksin. Diperleri projeno, proje tanım, pyp, lansman adı, yüklenici vs ...

            var result = new List<EntityNode>();

            // OFFSET 10 ROWS
            // FETCH NEXT 10 ROWS ONLY;
            //string query = "Select * FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId AND [Name] LIKE @name";
            const string query = "Select ID FROM [OTCS].[dbo].[LLAttrData] Where [DefID] = @defId AND [ValStr] LIKE @valStr";

            var connStrBuilder = new SqlConnectionStringBuilder(GetConnectionString()) { ConnectTimeout = 300 };

            using (var connection = new SqlConnection(connStrBuilder.ConnectionString))
            {

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@defId", defId);
                //command.Parameters.AddWithValue("@valStr", "%" + valStr);
                command.Parameters.AddWithValue("@valStr", valStr);


                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var itemNodeId = Convert.ToInt64(reader["ID"]);
                        result.Add(VariableHelper.Dmo.GetEntityNodeFromId("admin", VariableHelper.Token, itemNodeId, false, false, false));
                    }
                    reader.Close();
                    connection.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Resources.ErrorTypeDbTransacts + ex.ToString());
                    Logger.Error(ex, Resources.ErrorTypeDbTransacts);
                    throw;
                }
            }
        }

    }
}
