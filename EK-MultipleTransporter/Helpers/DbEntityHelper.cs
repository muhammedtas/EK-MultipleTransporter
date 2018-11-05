using EK_MultipleTransporter.DmsDocumentManagementService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace EK_MultipleTransporter.Helpers
{
    public class DbEntityHelper
    {
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
                    Console.WriteLine("Reading from OTCS db error. " + ex.ToString());
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
                    Console.WriteLine("Reading from OTCS db error. " + ex.ToString());
                    throw;
                }
            }

        }

        public static List<EntityNode> GetNodesByNameByPart(long parentNodeId, int startPoint, int endPoint)
        {
            var result = new List<EntityNode>();
            //string query = "Select * FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId AND [Name] LIKE @name";
            const string query = "Select DataID FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId AND [Id] BETWEEN @startPoint AND @endPoint" ;

            var connStrBuilder = new SqlConnectionStringBuilder(GetConnectionString()) {ConnectTimeout = 300};

            using (var connection = new SqlConnection(connStrBuilder.ConnectionString))
            {
                
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@startPoint",  startPoint);
                command.Parameters.AddWithValue("@endPoint", endPoint);
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
                    Console.WriteLine("Reading from OTCS db error. " + ex.ToString());
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
                    Console.WriteLine("Reading from OTCS db error. " + ex.ToString());
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
                    Console.WriteLine("Reading from OTCS db error. " + ex.ToString());
                    throw;
                }
            }

        }

        private static string GetConnectionString()
        {
            return OtcsDbConnStr;
        }

    }
}
