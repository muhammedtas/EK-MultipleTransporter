using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using EK_MultipleTransporter.Web_References.DmsDocumentManagementService;
using EK_MultipleTransporter.Enums;

namespace EK_MultipleTransporter.Helpers
{
    public class DbEntityHelper
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly string OtcsDbConnStr = ConfigurationManager.ConnectionStrings["OTCSCnnStr"].ConnectionString;
        public static EntityNode GetNodeByName(long parentNodeId, string name)
        {
            EntityNode result = null;
            
            const string query = "Select DataID, OwnerID FROM [OTCS].[dbo].[DTreeCore] Where ABS([ParentID]) = @parentNodeId AND [Name] LIKE @name";
            using(var connection = new SqlConnection(GetConnectionString()))
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
                        result = VariableHelper.Dmo
                            .GetEntityNodeFromId(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User),
                                VariableHelper.Token, itemNodeId, false, false, false);
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
                        result.Add(VariableHelper.Dmo
                            .GetEntityNodeFromId(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User), 
                                VariableHelper.Token, itemNodeId, false, false, false));
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
                        result.Add(VariableHelper.Dmo.GetEntityNodeFromId(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User), VariableHelper.Token, itemNodeId, false, false, false));
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
                        var itemNodeId = Convert.ToInt64(reader["DataID"]);
                        var newNode = VariableHelper.Dmo.GetEntityNodeFromId(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User), VariableHelper.Token, itemNodeId, false, false, false);
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
                        result = VariableHelper.Dmo.GetEntityNodeFromId(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User), VariableHelper.Token, itemNodeId, false, false, false);
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
        /// <summary>
        /// PROJE İçerisindeki PYP NO Item in adının ilk kısmına denk geliyor. Örn 1001/ ALtındağ bina ... nın pyp nosu 1001 --
        /// Onu da ValStr içerisindeki 8 itemden birinde göreceksin. Diperleri projeno, proje tanım, pyp, lansman adı, yüklenici vs ...
        /// </summary>
        /// <param name="defId"></param>
        /// <param name="valStr"></param>
        /// <returns></returns>
        public static List<EntityNode> GetNodesByCategoryAttribute(long defId, string valStr)
        {
            var result = new List<EntityNode>();

            const string query = "Select ID FROM [OTCS].[dbo].[LLAttrData] Where [DefID] = @defId AND [ValStr] LIKE @valStr";

            var connStrBuilder = new SqlConnectionStringBuilder(GetConnectionString()) { ConnectTimeout = 300 };

            using (var connection = new SqlConnection(connStrBuilder.ConnectionString))
            {

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@defId", defId);
                command.Parameters.AddWithValue("@valStr", valStr);
                
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var itemNodeId = Convert.ToInt64(reader["ID"]);
                        result.Add(VariableHelper.Dmo.GetEntityNodeFromId(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User), VariableHelper.Token, itemNodeId, false, false, false));
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
