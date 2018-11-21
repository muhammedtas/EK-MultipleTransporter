using EK_MultipleTransporter.Helpers;
using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EK_MultipleTransporter.Enums;
using EK_MultipleTransporter.Models.ChildModel;
using EK_MultipleTransporter.Web_References.DmsDocumentManagementService;
using EK_MultipleTransporter.Models.HelperModel;

namespace EK_MultipleTransporter.Forms
{
    public partial class StaffForm : Form
    {
        public static Logger Logger;
        public bool IsProcessing;

        private readonly OtServicesHelper _serviceHelper;

        public StaffForm()
        {
            InitializeComponent();
            VariableHelper.InitializeVariables();
            VariableHelper.InitializeNewCancellationTokenSource();
            CheckForIllegalCrossThreadCalls = false;
            _serviceHelper = new OtServicesHelper();
            Logger = LogManager.GetCurrentClassLogger();
        }

        private async void StaffForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadStaffFormDefault());
        }

        public void LoadStaffFormDefault()
        {
            var categoryItems = _serviceHelper.GetEntityAttributeGroupOfCategory(WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.GeneralCategoryNodeId));
            if (categoryItems != null)
            {
                var itemArray = categoryItems.Values[0].ValidValues;
                cmbStaffDocumentType.Items.AddRange(itemArray);
            }

            var childNodes = _serviceHelper.GetChildNodesById(WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.StaffChildElementsNodeId));

            foreach (var childNode in childNodes)
            {
                cmbStaffChildRoot.Items.Add(new StaffChilds()
                {
                    Id = childNode.Id,
                    Name = childNode.Name
                });
                if (!_serviceHelper.HasChildNode(childNode.Id)) continue;
                var innerChildList = _serviceHelper.GetChildNodesById(childNode.Id);

                foreach (var innerChild in innerChildList)
                {
                    cmbStaffChildRoot.Items.Add(new StaffChilds()
                    {
                        Id = innerChild.Id,
                        Name = childNode.Name + "\\" + innerChild.Name
                    });
                    if (!_serviceHelper.HasChildNode(innerChild.Id)) continue;
                    var innerChildListOfInnerChild = _serviceHelper.GetChildNodesById(innerChild.Id);
                    foreach (var innerOfInnerChild in innerChildListOfInnerChild)
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
                var allChildNodesOfMainStaff = _serviceHelper.GetEntityNodeListIncludingChildrenUsingTypeFilter(WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.StaffNodeId), (cmbStaffChildRoot.SelectedItem as StaffChilds)?.Name);
                var targetRootAddress = (cmbStaffChildRoot.SelectedItem as StaffChilds)?.Name;
                var countDeepness = targetRootAddress?.Split('\\').Count();

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
                            var oneOfTargetNode = DbEntityHelper.GetNodeByName(childNodeOfMainStaff.Id, targetRootAddress);
                            targetNodesList.Add(oneOfTargetNode);
                            break;
                        }
                        case 2:
                        {
                            var generalFirstStepTargetNodeName = targetRootAddress.Split('\\')[0];
                            var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(childNodeOfMainStaff.Id, generalFirstStepTargetNodeName).FirstOrDefault();

                            var generalSecondStepTargetNodeName = targetRootAddress.Split('\\')[1];
                            if (firstStepTargetNode != null)
                            {
                                var targetChildNode = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                                if (targetChildNode != null)
                                    targetNodesList.Add(targetChildNode);
                            }

                            break;
                        }
                        case 3:
                        {
                            var generalFirstStepTargetNodeName = targetRootAddress.Split('\\')[0];
                            var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(childNodeOfMainStaff.Id, generalFirstStepTargetNodeName).FirstOrDefault();

                            var generalSecondStepTargetNodeName = targetRootAddress.Split('\\')[1];
                            if (firstStepTargetNode == null) continue;
                            var secondChildNode = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                            var generalThirdStepTargetNodeName = targetRootAddress.Split('\\')[2];

                            var targetChildNode = DbEntityHelper.GetNodeByName(secondChildNode.Id, generalThirdStepTargetNodeName);

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
                    NodeId = WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.GeneralCategoryNodeId)
                };
                //await UploadDocuments(preparedList);
                var result = await _serviceHelper.UploadDocuments(docsToUpload, categoryModel);
                MessageBox.Show(result ? Resources.ProcessIsDone : Resources.ProcessIsNotDone);
                WaitedFormState();
                //await UploadDocuments(docsToUpload);

            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ErrorTypeProccessing + ex);
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
                //var root = folderDlg.RootFolder;
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
