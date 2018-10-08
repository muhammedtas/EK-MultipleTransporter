using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EK_MultipleTransporter.Entity
{
    internal interface IBaseEntity : IDisposable
    {
        bool Equals(object obj);
        int GetHashCode();
        string ToString();

        Task OneToMultipleLoader<T>(object source);
        Task MultipleToOneLoader<T>(IList<T> source);
    }
}
