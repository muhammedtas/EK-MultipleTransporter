using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EK_MultipleTransporter.Helpers
{
    public class VariableHelper
    {
        public static CancellationTokenSource Cts = new CancellationTokenSource();
        private static DmsAuthenticationService.AuthOps _ops;
        private static DmsDocumentManagementService.DmsOps _dmo;
        public static string Token { get; set; }
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static DmsAuthenticationService.AuthOps Ops
        {
            get
            {
                if (_ops != null) return _ops;

                try
                {
                    _ops = new DmsAuthenticationService.AuthOps();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Web Services is not started...");
                }
                return _ops;
            }
        }
        public static DmsDocumentManagementService.DmsOps Dmo
        {
            get
            {
                if (_dmo != null) return _dmo;
                try
                {
                    _dmo = new DmsDocumentManagementService.DmsOps();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Opentext DmsOps Web Service exception...");
                    throw;
                }
                return _dmo;
            }
        }
    }
}
