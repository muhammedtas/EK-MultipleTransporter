using EK_MultipleTransporter.Data;
using EK_MultipleTransporter.DmsDocumentManagementService;
using System;
using System.Data.SqlClient;

namespace EK_MultipleTransporter.Helpers
{
    public class DbEntityHelper
    {
        public static OTCSDbContext dbContext;
        public static EntityNode GetNodeByName(long nodeId, string name)
        {
            EntityNode result = null;
            string query = "Select * FROM [OTCS].[dbo].[DTreeCore] Where [ParentID] = @nodeId AND [Name] LIKE @name";
            //string query = "Select * FROM [OTCS].[dbo].[DTreeCore]";
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", "%"+name+"%");
                command.Parameters.AddWithValue("@nodeId", nodeId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        //Console.WriteLine(reader[0]);
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

        static private string GetConnectionString()
        {
            return "Data Source=192.168.50.120; Initial Catalog=OTCS; Persist Security Info=True;User ID=sa; Password=TstAdminOT*; MultipleActiveResultSets=true; Connection Timeout=300";
        }

    }
}
