using EK_MultipleTransporter.DmsAuthenticationService;
using EK_MultipleTransporter.DmsDocumentManagementService;
using EK_MultipleTransporter.Helpers;
using EK_MultipleTransporter.Model;
using EK_MultipleTransporter.Model.ChildModel;
using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.Configuration;
using System.Linq;
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

            //EntityNode[] nodes = VariableHelper.Dmo.GetChildNodes("admin", VariableHelper.Token, projectsNodeId, 0, 1000, false, false);

            //foreach (EntityNode node in nodes)
            //{
            //    cmbProjects.Items.Add(new Project()
            //    {
            //        Id = node.Id,
            //        Name = node.Name
            //    });
            //}

            var childNodes = serviceHelper.GetChildNodesById(projectsChildElementsNodeId);

            foreach (var childNode in childNodes)
            {
                cmbChildRoot.Items.Add(new ProjectChilds()
                {
                    Id = childNode.Id,
                    Name = childNode.Name
                });

                if (serviceHelper.HasChildNode(childNode.Id))
                {
                    var innerChilds = serviceHelper.GetChildNodesById(childNode.Id);

                    foreach (var innerChild in innerChilds)
                    {
                        cmbChildRoot.Items.Add(new ProjectChilds()
                        {
                            Id = innerChild.Id,
                            Name = childNode.Name + "\\" + innerChild.Name
                        });

                        if (serviceHelper.HasChildNode(innerChild.Id))
                        {
                            var innersOfInnerChild = serviceHelper.GetChildNodesById(innerChild.Id);
                            foreach (var innerOfInnerChild in innersOfInnerChild)
                            {
                                cmbChildRoot.Items.Add(new ProjectChilds()
                                {
                                    Id = innerOfInnerChild.Id,
                                    Name = childNode.Name + "\\" + innerChild.Name + "\\" + innerOfInnerChild.Name
                                });

                            }

                        }

                    }

                }
            }

            var childNodesWithChildren = serviceHelper.GetFolderListIncludingChildren(projectsChildElementsNodeId);

            foreach (var childNode in childNodesWithChildren)
            {
                cmbChildRoot.Items.Add(new ProjectChilds()
                {
                    Id = Convert.ToInt64(childNode.Key),
                    Name = childNode.Value
                });
            }
        }

        private void cmbProjects_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            var mainChildRootNodeId = Convert.ToInt64((cmbChildRoot.SelectedItem as ProjectChilds).Id); // Şimdi Bu nodeId ye karşılık gelen 

            // if (mainChildRootNodeId != projectsChildElementsNodeId) return;
           
            var mainChildRootElement = serviceHelper.GetEntityNodeFromId(mainChildRootNodeId);

            var loadingPlaceForEachDocumentsList = DbEntityHelper.GetNodesByName((cmbChildRoot.SelectedItem as ProjectChilds).Name);

            var mainNodeResult = serviceHelper.GetEntityNodeFromId(projectsNodeId); // ProjectsNodeId asıl dökümanların atılacağı yer.


            if (txtFolderRoot.Text == String.Empty || cmbChildRoot.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen Yüklenecek klasörü ve Hedef dizini seçiniz.");
                return;
            }
            var docsToUpload = StreamHelper.MakePreparedDocumentListToPush(txtFolderRoot.Text, loadingPlaceForEachDocumentsList);

            try
            {
                var eag = serviceHelper.GetEntityAttributeGroupOfCategory(generalCategoryNodeId);

                var docType = eag.Values.First(x => x.Description == "Doküman Türü");
                docType.Values = new object[] { cmbDocumentType.Text };

                var year = eag.Values.First(x => x.Description == "Yıl");
                year.Values = new object[] { txtYear.Text };

                var term = eag.Values.First(x => x.Description == "Çeyrek");
                term.Values = new object[] { cmbTerm.Text };

                var emdNew = new EntityMetadata();

                emdNew.AttributeGroups = new[] { eag };

                foreach (var item in docsToUpload)
                {
                    serviceHelper.AddDocumentWithMetaData(item.Key.Item1, item.Key.Item2, item.Value, emdNew);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ot categories update  error :  " + ex);
                throw;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void txtFolderRoot_Click(object sender, EventArgs e)
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

        }
    }
}
