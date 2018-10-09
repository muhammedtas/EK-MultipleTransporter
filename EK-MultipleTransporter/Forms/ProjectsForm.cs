using EK_MultipleTransporter.DmsAuthenticationService;
using EK_MultipleTransporter.DmsDocumentManagementService;
using EK_MultipleTransporter.Helpers;
using EK_MultipleTransporter.Model;
using EK_MultipleTransporter.Model.ChildModel;
using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.Configuration;
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
        public static long projectsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["projectsNodeId"]);
        public static long projectsChildElementsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["projectsChildElementsNodeId"]);
        public static long generalCategoryNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["projectsNodeId"]);
        public OTServicesHelper serviceHelper = new OTServicesHelper();



        public ProjectsForm()
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
                                VariableHelper.Token = ops.AuthenticateUser("admin", "token", "admin", "Dty4208ab1!");
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
                    VariableHelper.Token = ops.AuthenticateUser("admin", "token", "admin", "Dty4208ab1!");
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

            //DmsOps dops = new DmsOps();

            EntityNode[] nodes = VariableHelper.Dmo.GetChildNodes("admin", VariableHelper.Token, projectsNodeId, 0, 1000, false, false);

            foreach (EntityNode node in nodes)
            {
                cmbProjects.Items.Add(new Project()
                {
                    Id = node.Id,
                    Name = node.Name                    
                });
            }

            var childNodes = serviceHelper.GetChildNodesById(VariableHelper.Token, projectsChildElementsNodeId);

            foreach (var childNode in childNodes)
            {
                cmbChildRoot.Items.Add(new ProjectChilds()
                {
                    Id = childNode.Id,
                    Name = childNode.Name
                });
            }
        }

        private void cmbProjects_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cmbProjects.SelectedIndex < 0 || txtFolderRoot.Text==String.Empty)
            {
                MessageBox.Show("Lütfen proje ve Hedef dizini seçiniz.");
                return;
            }
            var docs = StreamHelper.ReadAllDocumentsAsByte(txtFolderRoot.Text, serviceHelper, VariableHelper.Token, projectsChildElementsNodeId);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void txtFolderRoot_Click(object sender, EventArgs e)
        {
            if (cmbProjects.SelectedIndex == -1)
            {
                MessageBox.Show("Önce Proje Türünü Seçiniz.");
            }
            else
            {

                FolderBrowserDialog folderDlg = new FolderBrowserDialog();
                folderDlg.ShowNewFolderButton = true;
                // Show the FolderBrowserDialog.  
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtFolderRoot.Text = folderDlg.SelectedPath;
                    Environment.SpecialFolder root = folderDlg.RootFolder;
                }

                //ofdRootFolder.Title = Resources.ChooseFolder;
                //ofdRootFolder.Filter = Resources.AllowedTypes;
                //ofdRootFolder.FileName = "";
                //ofdRootFolder.Multiselect = false;
                //ofdRootFolder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                //if (ofdRootFolder.ShowDialog() != DialogResult.OK) return;
                //var folderRoot = ofdRootFolder.FileName;

            }
        }
    }
}
