using EK_MultipleTransporter.DmsAuthenticationService;
using EK_MultipleTransporter.DmsDocumentManagementService;
using EK_MultipleTransporter.Helpers;
using EK_MultipleTransporter.Model;
using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.ServiceModel;
using System.Threading;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace EK_MultipleTransporter.Forms
{
    public partial class ProjectsForm : Form
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public ProjectsForm()
        {
            InitializeComponent();
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

        private void ProjectsForm_Load(object sender, EventArgs e)
        {
            DmsAuthenticationService.AuthOps ao = new AuthOps();
            string adminToken = ao.AuthenticateUser("admin", "token", "admin", "Dty4208ab1!");
            //string userToken = ao.ImpersonateUser("admin", "token", adminToken);
            DmsOps dops = new DmsOps();
            EntityNode[] nodes = dops.GetChildNodes("admin", adminToken, 59055, 0, 1000, false, false);
            foreach (EntityNode node in nodes)
            {
                cmbProjects.Items.Add(new Project()
                {
                    Id = node.Id,
                    Name = node.Name                    
                });
            }
        }

        private void cmbProjects_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cmbProjects.SelectedIndex < 0)
            {
                MessageBox.Show("Lütfen proje seçiniz.");
                return;
            }

            DmsAuthenticationService.AuthOps ao = new AuthOps();
            string adminToken = ao.AuthenticateUser("admin", "token", "admin", "Dty4208ab1!");

            Excel.Worksheet ws = Globals.ThisAddIn.Application.ActiveSheet;
            ws.Cells[1, 1].Value = "Proje Adı:";
            ws.Cells[2, 1].Value = "Proje ID:";
            ws.Cells[1, 2].Value = ((Project)cmbProjects.SelectedItem).Name;
            ws.Cells[2, 2].Value = ((Project)cmbProjects.SelectedItem).Id;

            ws.Cells[4, 1].Value = "Seçim";
            ws.Cells[4, 2].Value = "ID";
            ws.Cells[4, 3].Value = "Bağımsız Bölüm";
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
