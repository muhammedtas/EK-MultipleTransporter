using EK_MultipleTransporter.Helpers;
using Microsoft.Win32.SafeHandles;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EK_MultipleTransporter.Web_References.DmsAuthenticationService;
using EK_MultipleTransporter.Web_References.DmsDocumentManagementService;

namespace EK_MultipleTransporter.Entity
{
    public abstract class BaseEntity : IBaseEntity
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public OtServicesHelper ServiceHelper { get; } = new OtServicesHelper();

        public DmsOps Dmo { get; } = VariableHelper.Dmo;

        public AuthOps Ops { get; } = VariableHelper.Ops;

        public BaseEntity()
        {
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Disposed;
        public readonly SafeHandle Handle = new SafeFileHandle(IntPtr.Zero, true);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                Handle.Dispose();

            }

            Disposed = true;
        }

        public Task OneToMultipleLoader<T>(object source)
        {
            throw new NotImplementedException();
        }

        public Task MultipleToOneLoader<T>(IList<T> source)
        {
            throw new NotImplementedException();
        }
    }
}
