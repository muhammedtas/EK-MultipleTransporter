using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EK_MultipleTransporter.Entity
{
    public class BaseChildEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
