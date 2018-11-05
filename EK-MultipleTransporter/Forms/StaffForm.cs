using EK_MultipleTransporter.DmsDocumentManagementService;
using EK_MultipleTransporter.Helpers;
using EK_MultipleTransporter.Model.ChildModel;
using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;

namespace EK_MultipleTransporter.Forms
{
    public partial class StaffForm : Form
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();
        public static long StaffsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["staffNodeId"]);
        public static long StaffsChildElementsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["staffChildElementsNodeId"]);
        public static long GeneralCategoryNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["generalCategoryNodeId"]);
        public OtServicesHelper ServiceHelper = new OtServicesHelper();

        public StaffForm()
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

        private async void StaffForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadStaffFormDefault());
        }

        public void LoadStaffFormDefault()
        {
            var categoryItems = ServiceHelper.GetEntityAttributeGroupOfCategory(GeneralCategoryNodeId);
            if (categoryItems != null)
            {
                var itemArray = categoryItems.Values[0].ValidValues;
                cmbStaffDocumentType.Items.AddRange(itemArray);
            }

            var childNodes = ServiceHelper.GetChildNodesById(StaffsChildElementsNodeId);

            foreach (var childNode in childNodes)
            {
                cmbStaffChildRoot.Items.Add(new StaffChilds()
                {
                    Id = childNode.Id,
                    Name = childNode.Name
                });

                if (ServiceHelper.HasChildNode(childNode.Id))
                {
                    var innerChilds = ServiceHelper.GetChildNodesById(childNode.Id);

                    foreach (var innerChild in innerChilds)
                    {
                        cmbStaffChildRoot.Items.Add(new StaffChilds()
                        {
                            Id = innerChild.Id,
                            Name = childNode.Name + "\\" + innerChild.Name
                        });

                        if (ServiceHelper.HasChildNode(innerChild.Id))
                        {
                            var innersOfInnerChild = ServiceHelper.GetChildNodesById(innerChild.Id);
                            foreach (var innerOfInnerChild in innersOfInnerChild)
                            {
                                cmbStaffChildRoot.Items.Add(new StaffChilds()
                                {
                                    Id = innerOfInnerChild.Id,
                                    Name = childNode.Name + "\\" + innerChild.Name + "\\" + innerOfInnerChild.Name
                                });

                            }

                        }

                    }

                }
            }

        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            await Task.Run(() => DoStaffTask());
        }


        public async void DoStaffTask()
        {
            try
            {

                // var mainChildRootNodeId = Convert.ToInt64((cmbStaffChildRoot.SelectedItem as StaffChilds).Id); // Şimdi Bu nodeId ye karşılık gelen 

                // var mainChildRootElement = ServiceHelper.GetEntityNodeFromId(mainChildRootNodeId);

                var targetNodesList = new List<EntityNode>(); // Bu boş liste doldurulup streamer helper methoduna verilecek.

                // Burada da Projeler içerisinde yüklenecek yerlerin nodeId listesini alacağız.
                // Ama ne yazık ki üst parent ten bir kaç kırınım içerideki child ları bulamıyoruz.
                var allChildNodesOfMainStaff = ServiceHelper.GetEntityNodeListIncludingChildrenUsingTypeFilter(StaffsNodeId, (cmbStaffChildRoot.SelectedItem as StaffChilds).Name);
                var targetRootAddres = (cmbStaffChildRoot.SelectedItem as StaffChilds).Name;
                var countDeepness = targetRootAddres.Split('\\').Count();

                if (countDeepness > 3)
                {
                    MessageBox.Show(Resources.NodeDeepnessExceed);
                    return;
                }

                foreach (var childNodeOfMainStaff in allChildNodesOfMainStaff)
                {
                    switch (countDeepness)
                    {
                        case 1:
                        {
                            var oneOfTargetNode = DbEntityHelper.GetNodeByName(childNodeOfMainStaff.Id, targetRootAddres);
                            targetNodesList.Add(oneOfTargetNode);
                            break;
                        }
                        case 2:
                        {
                            var generalFirstStepTargetNodeName = targetRootAddres.Split('\\')[0];
                            var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(childNodeOfMainStaff.Id, generalFirstStepTargetNodeName).FirstOrDefault();

                            var generalSecondStepTargetNodeName = targetRootAddres.Split('\\')[1];

                            var targetChildNode = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                            if (targetChildNode != null)
                                targetNodesList.Add(targetChildNode);
                            break;
                        }
                        case 3:
                        {
                            var generalFirstStepTargetNodeName = targetRootAddres.Split('\\')[0];
                            var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(childNodeOfMainStaff.Id, generalFirstStepTargetNodeName).FirstOrDefault();

                            var generalSecondStepTargetNodeName = targetRootAddres.Split('\\')[1];
                            if (firstStepTargetNode == null) continue;
                            var secondChldnd = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                            var generalThirdStepTargetNodeName = targetRootAddres.Split('\\')[2];

                            var targetChildNode = DbEntityHelper.GetNodeByName(secondChldnd.Id, generalThirdStepTargetNodeName);

                            if (targetChildNode != null)
                                targetNodesList.Add(targetChildNode);
                            break;
                        }
                        default:
                            Console.WriteLine(Resources.NodeDeepnessExceed);
                            break;
                    }
                }

                // var mainNodeResult = ServiceHelper.GetEntityNodeFromId(StaffsNodeId); 


                if (txtStaffFolderRoot.Text == string.Empty || cmbStaffChildRoot.SelectedIndex == -1)
                {
                    MessageBox.Show("Lütfen Yüklenecek klasörü ve Hedef dizini seçiniz.");
                    return;
                }
                var docsToUpload = StreamHelper.MakePreparedDocumentListToPush(txtStaffFolderRoot.Text, targetNodesList);

                if (docsToUpload.Count < 1) return;

                await UploadDocuments(docsToUpload);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ot categories update  error :  " + ex);
                throw;
            }
        }

        public async Task UploadDocuments(Dictionary<Tuple<long, string>, byte[]> docsToUpload)
        {
            var eag = ServiceHelper.GetEntityAttributeGroupOfCategory(GeneralCategoryNodeId);

            var docType = eag.Values.First(x => x.Description == "Doküman Türü");
            docType.Values = new object[] { cmbStaffDocumentType.Text };

            var year = eag.Values.First(x => x.Description == "Yıl");
            year.Values = new object[] { dtpStaffYear.Text };

            var term = eag.Values.First(x => x.Description == "Çeyrek");
            term.Values = new object[] { cmbStaffTerm.Text };

            var emdNew = new EntityMetadata
            {
                AttributeGroups = new[] { eag }
            };

            foreach (var item in docsToUpload)
            {
                await Task.Run(() => ServiceHelper.AddDocumentWithMetaData(item.Key.Item1, item.Key.Item2, item.Value, emdNew));
            }
        }

        private void txtStaffFolderRoot_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog {ShowNewFolderButton = true};
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtStaffFolderRoot.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }

            StreamHelper.RootPathOfUsersFolder = folderDlg.SelectedPath;

        }

    }
}
