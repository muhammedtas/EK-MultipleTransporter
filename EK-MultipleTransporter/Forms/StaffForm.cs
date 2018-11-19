using EK_MultipleTransporter.Helpers;
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
using EK_MultipleTransporter.Models.ChildModel;
using EK_MultipleTransporter.Web_References.DmsDocumentManagementService;
using EK_MultipleTransporter.Enums;
using EK_MultipleTransporter.Models.HelperModel;

namespace EK_MultipleTransporter.Forms
{
    public partial class StaffForm : Form
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();
        public static long StaffsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["staffNodeId"]);
        public static long StaffsChildElementsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["staffChildElementsNodeId"]);
        public static long GeneralCategoryNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["generalCategoryNodeId"]);
        private readonly OtServicesHelper _serviceHelper;
        public bool IsProcessing;
        public StaffForm()
        {
            InitializeComponent();

            var dmo = VariableHelper.Dmo;
            var ops = VariableHelper.Ops;
            CheckForIllegalCrossThreadCalls = false;
            _serviceHelper = new OtServicesHelper();

            if (string.IsNullOrEmpty(VariableHelper.Token))
            {
                try
                {
                    var unused = new System.Threading.Timer(
                        e =>
                        {
                            try
                            {
                                VariableHelper.Token =
                                    ops.AuthenticateUser(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User),
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

                    ops.Timeout = 3600000;
                    dmo.Timeout = 3600000;

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
                    VariableHelper.Token =
                        ops.AuthenticateUser(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User),
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

        private async void StaffForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadStaffFormDefault());
        }

        public void LoadStaffFormDefault()
        {
            var categoryItems = _serviceHelper.GetEntityAttributeGroupOfCategory(GeneralCategoryNodeId);
            if (categoryItems != null)
            {
                var itemArray = categoryItems.Values[0].ValidValues;
                cmbStaffDocumentType.Items.AddRange(itemArray);
            }

            var childNodes = _serviceHelper.GetChildNodesById(StaffsChildElementsNodeId);

            foreach (var childNode in childNodes)
            {
                cmbStaffChildRoot.Items.Add(new StaffChilds()
                {
                    Id = childNode.Id,
                    Name = childNode.Name
                });
                if (!_serviceHelper.HasChildNode(childNode.Id)) continue;
                var innerChilds = _serviceHelper.GetChildNodesById(childNode.Id);

                foreach (var innerChild in innerChilds)
                {
                    cmbStaffChildRoot.Items.Add(new StaffChilds()
                    {
                        Id = innerChild.Id,
                        Name = childNode.Name + "\\" + innerChild.Name
                    });
                    if (!_serviceHelper.HasChildNode(innerChild.Id)) continue;
                    var innersOfInnerChild = _serviceHelper.GetChildNodesById(innerChild.Id);
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


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            await Task.Run(() => DoStaffTask());
        }


        public async void DoStaffTask()
        {
            try
            {
                InvokedFormState();

                var targetNodesList = new List<EntityNode>(); // Bu boş liste doldurulup streamer helper methoduna verilecek.

                // Burada da Projeler içerisinde yüklenecek yerlerin nodeId listesini alacağız.
                // Ama ne yazık ki üst parent ten bir kaç kırınım içerideki child ları bulamıyoruz.
                var allChildNodesOfMainStaff = _serviceHelper.GetEntityNodeListIncludingChildrenUsingTypeFilter(StaffsNodeId, (cmbStaffChildRoot.SelectedItem as StaffChilds).Name);
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


                if (txtStaffFolderRoot.Text == string.Empty || cmbStaffChildRoot.SelectedIndex == -1)
                {
                    MessageBox.Show(Resources.WarnMessageChooseTargets);
                    return;
                }
                var docsToUpload = StreamHelper.MakePreparedDocumentListToPush(txtStaffFolderRoot.Text, targetNodesList);

                if (docsToUpload.Count < 1) return;

                var categoryModel = new GeneralCategoryModel()
                {
                    DocumentType = cmbStaffDocumentType.Text,
                    Year = dtpStaffYear.Text,
                    Term = cmbStaffTerm.Text,
                    NodeId = GeneralCategoryNodeId
                };
                //await UploadDocuments(preparedList);
                var result = await _serviceHelper.UploadDocuments(docsToUpload, categoryModel);
                MessageBox.Show(result ? Resources.ProcessIsDone : Resources.ProcessIsNotDone);
                WaitedFormState();
                //await UploadDocuments(docsToUpload);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ot categories update  error :  " + ex);
                MessageBox.Show(Resources.ErrorTypeProccessing);
                Logger.Error(ex , Resources.ErrorTypeProccessing);
                //throw;
            }
        }
        
        private void txtStaffFolderRoot_Click(object sender, EventArgs e)
        {
            var folderDlg = new FolderBrowserDialog {ShowNewFolderButton = true};
            var result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtStaffFolderRoot.Text = folderDlg.SelectedPath;
                var root = folderDlg.RootFolder;
            }

            StreamHelper.RootPathOfUsersFolder = folderDlg.SelectedPath;

        }

        public void InvokedFormState()
        {
            IsProcessing = true;
            txtStaffFolderRoot.Enabled = false;
            cmbStaffChildRoot.Enabled = false;
            cmbStaffDocumentType.Enabled = false;
            dtpStaffYear.Enabled = false;
            cmbStaffTerm.Enabled = false;
            btnOk.Enabled = false;
            btnCancel.Enabled = true;
            lblFolderRoot.Enabled = false;
            lblChild.Enabled = false;
            lblCategoryDocumentType.Enabled = false;
            lblCategoryYear.Enabled = false;
            lblTerm.Enabled = false;

            //this.Enabled = false;
        }

        public void WaitedFormState()
        {
            IsProcessing = false;
            txtStaffFolderRoot.Enabled = true;
            cmbStaffChildRoot.Enabled = true;
            cmbStaffDocumentType.Enabled = true;
            dtpStaffYear.Enabled = true;
            cmbStaffTerm.Enabled = true;
            btnOk.Enabled = true;
            btnCancel.Enabled = false;
            lblFolderRoot.Enabled = true;
            lblChild.Enabled = true;
            lblCategoryDocumentType.Enabled = true;
            lblCategoryYear.Enabled = true;
            lblTerm.Enabled = true;
        }

    }
}
