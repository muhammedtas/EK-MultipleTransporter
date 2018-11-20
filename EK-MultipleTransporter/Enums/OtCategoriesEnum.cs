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
  
        public enum GeneralCategory
        {
            DocType = 1,
            Year = 2,
            Quarter =3
        }

        public static string ConvertString(GeneralCategory me)
        {
            switch (me)
            {
                case GeneralCategory.DocType:
                    return Resources.DocType;
                case GeneralCategory.Year:
                    return Resources.Year;
                case GeneralCategory.Quarter:
                    return Resources.Quarter;
                default:
                    return "";
            }
        }
    }
}
