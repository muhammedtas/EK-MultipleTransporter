using NLog;
using System;
using System.ServiceModel;
using System.Threading;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using EK_MultipleTransporter.Properties;
using EK_MultipleTransporter.Web_References.DmsAuthenticationService;
using EK_MultipleTransporter.Web_References.DmsDocumentManagementService;
using EK_MultipleTransporter.Enums;

namespace EK_MultipleTransporter.Helpers
{
    public class VariableHelper
    {
        private static CancellationTokenSource _cts;
        private static AuthOps _ops;
        private static DmsOps _dmo;
        public static string Token { get; set; }
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static CancellationTokenSource Cts
        {
            get
            {
                if (_cts != null) return _cts;
                try
                {
                    _cts = new CancellationTokenSource();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                return _cts;
            }
            set => _cts = value;
        }
        public static AuthOps Ops
        {
            get
            {
                if (_ops != null) return _ops;

                try
                {
                    _ops = new AuthOps();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Web Services is not started...");
                }
                return _ops;
            }
        }
        public static DmsOps Dmo
        {
            get
            {
                if (_dmo != null) return _dmo;
                try
                {
                    _dmo = new DmsOps();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Opentext DmsOps Web Service exception...");
                    throw;
                }
                return _dmo;
            }
        }
      
        public static void InitializeVariables()
        {
            if (string.IsNullOrEmpty(Token))
            {
                try
                {
                    var unused = new System.Threading.Timer(
                        e =>
                        {
                            try
                            {
                                Token =
                                    Ops.AuthenticateUser(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User),
                                        OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.Token),
                                        OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User),
                                        OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.Password));
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex, "Web Services is not working...");
                                MessageBox.Show(Resources.WebServicesNotWorking);
                            }
                        },
                        SynchronizationContext.Current,
                        TimeSpan.Zero,
                        TimeSpan.FromMinutes(5));

                    Ops.Timeout = 3600000;
                    Dmo.Timeout = 3600000;

                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, Resources.ErrorTypeFaultException);
                    Thread.Sleep(1200000);
                }
                catch (SoapException ex)
                {
                    Logger.Error(ex, Resources.ErrorTypeSOAPException);
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
                    Token =
                        Ops.AuthenticateUser(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User),
                            OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.Token),
                            OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User),
                            OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.Password));
                }
                catch (FaultException ex)
                {
                    Logger.Error(ex, Resources.ErrorTypeFaultException);
                    Thread.Sleep(1200000);
                }
                catch (SoapException ex)
                {
                    Logger.Error(ex, Resources.ErrorTypeSOAPException);
                    Thread.Sleep(1200000);
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex, "Error Details   :" + ex.Message + ex.InnerException?.Message);
                }
            }
        }

        /// <summary>
        /// Burada Bir Form kapatılıp yeniden açıldığında CancellationToken Cancel request i iptal etmek için
        /// obje tekrar initialize edilir.
        /// </summary>
        public static void InitializeNewCancellationTokenSource()
        {
            if (Cts.Token.IsCancellationRequested) Cts = new CancellationTokenSource();
        }
    }
}
