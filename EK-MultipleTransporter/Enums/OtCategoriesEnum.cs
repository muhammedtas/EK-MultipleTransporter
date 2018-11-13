using EK_MultipleTransporter.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EK_MultipleTransporter.Enums
{
    public sealed class OtCategoriesEnum
    {
        private OtCategoriesEnum() { }

        public static readonly string DocType = Resources.DocType;
        public static readonly string Year = Resources.Year;
        public static readonly string Quarter = Resources.Quarter;

        public enum GeneralCategory
        {
            DocType = 1,
            Year = 2,
            Quarter =3
        }
    }
}
