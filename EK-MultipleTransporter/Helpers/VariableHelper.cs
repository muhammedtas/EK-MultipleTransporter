using NLog;
using System;
using System.ServiceModel;
using System.Threading;
using System.Web.Services.Protocols;
using EK_MultipleTransporter.Properties;

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

        public VariableHelper()
        {

            if (string.IsNullOrEmpty(VariableHelper.Token))
            {
                try
                {
                    var unused = new System.Threading.Timer(
                        e =>
                        {
                            try
                            {
                                VariableHelper.Token = _ops.AuthenticateUser("admin", "token", "admin", "Dty4208ab1!");
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex, "Web Services is not working...");
                            }
                        },
                        SynchronizationContext.Current,
                        TimeSpan.Zero,
                        TimeSpan.FromMinutes(5));

                    _ops.Timeout = 3600000;
                    _dmo.Timeout = 3600000;

                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, "SOAP Exception has been thrown. Web services will not return any answer for a while. Program is stopping itself for this period. Please do not do anything. ");
                    Thread.Sleep(1200000);
                }
                catch (SoapException ex)
                {
                    Logger.Error(ex, "SOAP Exception has been thrown. Web services will not return any answer for a while. Program is stopping itself for this period. Please do not do anything. ");
                    Thread.Sleep(1200000);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error Details   :" + ex.Message + ex.InnerException?.Message);
                }

            }
            else
            {
                try
                {
                    VariableHelper.Token = _ops.AuthenticateUser("admin", "token", "admin", "Dty4208ab1!");
                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, "Fault Exception has been thrown. Web services will not return any answer for a while. Program is stopping itself for this period. Please do not do anything. ");
                    Thread.Sleep(1200000);

                }
                catch (SoapException ex)
                {

                    Logger.Error(ex, "SOAP Exception has been thrown. Web services will not return any answer for a while. Program is stopping itself for this period. Please do not do anything. ");
                    Thread.Sleep(1200000);
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex, "Error Details   :" + ex.Message + ex.InnerException?.Message);
                }
            }
        } 
    }
}
