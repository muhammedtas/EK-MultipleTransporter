using EK_MultipleTransporter.Helpers;
using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.Configuration;
using System.ServiceModel;
using System.Threading;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using EK_MultipleTransporter.Models.RootModel;
using EK_MultipleTransporter.Web_References.DmsAuthenticationService;
using EK_MultipleTransporter.Web_References.DmsDocumentManagementService;

namespace EK_MultipleTransporter.Forms
{
    public partial class IndependentSectionForm : Form
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();
        public static long IndSecNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["independentSectionNodeId"]);
        public IndependentSectionForm()
        {
            InitializeComponent();

            var dmo = VariableHelper.Dmo;
            var ops = VariableHelper.Ops;
            CheckForIllegalCrossThreadCalls = false;


            if (string.IsNullOrEmpty(VariableHelper.Token))
            {
                try
                {
                    var unused = new System.Threading.Timer(
                        e =>
                        {
                            try
                            {
                                VariableHelper.Token = ops.AuthenticateUser("", "", "Admin", "Dty4208ab1!");
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

                    ops.Timeout = 3600000;
                    dmo.Timeout = 3600000;

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
                    VariableHelper.Token = ops.AuthenticateUser("", "", "Admin", "Dty4208ab1!");
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

        private void DistrictsForm_Load(object sender, EventArgs e)
        {
            AuthOps ao = new AuthOps();
            string adminToken = ao.AuthenticateUser("admin", "token", "admin", "Dty4208ab1!");
            DmsOps dops = new DmsOps();
            EntityNode[] nodes = dops.GetChildNodes("admin", adminToken, 58725, 0, 1000, false, false);
            foreach (EntityNode node in nodes)
            {
               cmbDistrictsList.Items.Add(new IndependentSection()
                {
                    Id = node.Id,
                    Name = node.Name
                });
            }
        }
    }
}
