using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EK_MultipleTransporter.Enums
{
    public sealed class OtCredentialsEnum
    {

        public enum OtAdminCredentials
        {
            User,
            Password,
            Token
        }


        public static string ConvertString(OtAdminCredentials me)
        {
            switch (me)
            {
                case OtAdminCredentials.User:
                    return ConfigurationManager.AppSettings["otUserName"];
                case OtAdminCredentials.Password:
                    return ConfigurationManager.AppSettings["otPassword"];
                case OtAdminCredentials.Token:
                    return ConfigurationManager.AppSettings["otToken"];
                default:
                    return "";
            }
        }


    }
}
