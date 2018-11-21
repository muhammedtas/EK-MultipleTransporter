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
    public partial class ProjectsForm : Form
    {
        public static Logger Logger;
        public bool IsProcessing;

        private readonly OtServicesHelper _serviceHelper;

        public ProjectsForm()
        {
            InitializeComponent();
            VariableHelper.InitializeVariables();
            VariableHelper.InitializeNewCancellationTokenSource();
            _serviceHelper = new OtServicesHelper();
            CheckForIllegalCrossThreadCalls = false;
            Logger = LogManager.GetCurrentClassLogger();
        }

        private async void ProjectsForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadFormsDefault());
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            await Task.Run(() => DoProjectWorks());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtFolderRoot_Click(object sender, EventArgs e)
        {

            var folderDlg = new FolderBrowserDialog {ShowNewFolderButton = true};
            var result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFolderRoot.Text = folderDlg.SelectedPath;
                //var root = folderDlg.RootFolder;
            }
            StreamHelper.RootPathOfUsersFolder = folderDlg.SelectedPath;

        }

        public async void DoProjectWorks()
        {
            try
            {
                InvokedFormState();
                // Burada da Projeler içerisinde yüklenecek yerlerin nodeId listesini alacağız.
                // Ama ne yazık ki üst parent ten bir kaç kırınım içerideki child ları bulamıyoruz.
                var targetNodesList = new List<EntityNode>(); // Bu boş liste doldurulup streamer helper methoduna verilecek.
                var allChildNodesOfMainProject = _serviceHelper.GetEntityNodeListIncludingChildrenUsingTypeFilter(WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.ProjectsNodeId), (cmbChildRoot.SelectedItem as ProjectChilds).Name);
                var targetRootAddress = (cmbChildRoot.SelectedItem as ProjectChilds).Name;
                var countDeepness = targetRootAddress.Split('\\').Count();
                if (countDeepness > 3)
                {
                    MessageBox.Show(Resources.NodeDeepnessExceed);
                    return;
                }
                // Bu yüzden tüm node ların içerisine girip, node derinliğini hesaplayarak iç nodelara ulaşacağız ve maplemek üzere node keyleri tek tek StreamHelper a göndereceğiz.
                // Streamer helper aldığı bu node id ile bir ilişki kurabilirse map in içerisine koyup bize verecek, kuramazsa eklemeyecek.
                foreach (var childNodeOfMainProject in allChildNodesOfMainProject)
                {
                    switch (countDeepness)
                    {
                        
                        case 1:
                            {
                                var oneOfTargetNode = DbEntityHelper.GetNodeByName(childNodeOfMainProject.Id, targetRootAddress);
                                targetNodesList.Add(oneOfTargetNode);
                                break;
                            }
                        case 2:
                            {
                                var generalFirstStepTargetNodeName = targetRootAddress.Split('\\')[0];
                                var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(childNodeOfMainProject.Id, generalFirstStepTargetNodeName).FirstOrDefault();

                                var generalSecondStepTargetNodeName = targetRootAddress.Split('\\')[1];

                                var targetChildNode = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                                if (targetChildNode != null)
                                    targetNodesList.Add(targetChildNode);

                                // Child kırınımı 2 ise 
                                break;
                            }
                        case 3:
                            {
                                var generalFirstStepTargetNodeName = targetRootAddress.Split('\\')[0];
                                var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(childNodeOfMainProject.Id, generalFirstStepTargetNodeName).FirstOrDefault();

                                var generalSecondStepTargetNodeName = targetRootAddress.Split('\\')[1];

                                var secondChildNode = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                                var generalThirdStepTargetNodeName = targetRootAddress.Split('\\')[2];

                                var targetChildNode = DbEntityHelper.GetNodeByName(secondChildNode.Id, generalThirdStepTargetNodeName);

                                // var targetChildNode = firstStepTargetNode.Where(x => x.Name == generalFirstStepTargetNodeName).FirstOrDefault();

                                if (targetChildNode != null)
                                    targetNodesList.Add(targetChildNode);
                                // Child kırınım 3 ise
                                break;
                            }
                        default:
                            Console.WriteLine(Resources.NodeDeepnessExceed);
                            break;
                    }
                }

                // Bu mainChildRootElement => Document Templates içerisinde seçmiş olduğumuz child 
                // mainChildRootElement inin adı ile Emlak Konut iş alanları altındaki Projeler içerisinde ne kadar aynı isimde child element varsa Bunlardan bir dictionary yap.

                if (txtFolderRoot.Text == string.Empty || cmbChildRoot.SelectedIndex == -1)
                {
                    MessageBox.Show(Resources.WarnMessageChooseTargets);
                    return;
                }
                var docsToUpload = StreamHelper.MakePreparedDocumentListToPush(txtFolderRoot.Text, targetNodesList);
                
                if (docsToUpload.Count < 1) return;
                // nodeId, dosya adı, ve hedef nodeId ile yarattığımız dictionary i opentext e yüklenebilir hale getireceğiz.

                var categoryModel = new GeneralCategoryModel()
                {
                    DocumentType = cmbDocumentType.Text,
                    Year = dtpYear.Text,
                    Term = cmbTerm.Text,
                    NodeId = WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.GeneralCategoryNodeId)
                };
                var result = await _serviceHelper.UploadDocuments(docsToUpload, categoryModel);
                MessageBox.Show(result ? Resources.ProcessIsDone : Resources.ProcessIsNotDone);
                WaitedFormState();
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ErrorTypeProccessing + ex);
                //throw;
                MessageBox.Show(Resources.ErrorTypeProccessing);
            }

        }

        public void LoadFormsDefault ()
        {
            var categoryItems = _serviceHelper.GetEntityAttributeGroupOfCategory(WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.GeneralCategoryNodeId));
            if (categoryItems != null)
            {
                var itemArray = categoryItems.Values[0].ValidValues;
                cmbDocumentType.Items.AddRange(itemArray);
            }

            var childNodes = _serviceHelper.GetChildNodesById(WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.ProjectsChildElementsNodeId));

            foreach (var childNode in childNodes)
            {
                cmbChildRoot.Items.Add(new ProjectChilds()
                {
                    Id = childNode.Id,
                    Name = childNode.Name
                });
                if (!_serviceHelper.HasChildNode(childNode.Id)) continue;
                var innerChildList = _serviceHelper.GetChildNodesById(childNode.Id);

                foreach (var innerChild in innerChildList)
                {
                    cmbChildRoot.Items.Add(new ProjectChilds()
                    {
                        Id = innerChild.Id,
                        Name = childNode.Name + "\\" + innerChild.Name
                    });
                    if (!_serviceHelper.HasChildNode(innerChild.Id)) continue;
                    var innerListOfInnerChild = _serviceHelper.GetChildNodesById(innerChild.Id);
                    foreach (var innerOfInnerChild in innerListOfInnerChild)
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

        public void InvokedFormState()
        {
            IsProcessing = true;
            txtFolderRoot.Enabled = false;
            cmbChildRoot.Enabled = false;
            cmbDocumentType.Enabled = false;
            dtpYear.Enabled = false;
            cmbTerm.Enabled = false;
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
            txtFolderRoot.Enabled = true;
            cmbChildRoot.Enabled = true;
            cmbDocumentType.Enabled = true;
            dtpYear.Enabled = true;
            cmbTerm.Enabled = true;
            btnOk.Enabled = true;
            btnCancel.Enabled = false;
            lblFolderRoot.Enabled = true;
            lblChild.Enabled = true;
            lblCategoryDocumentType.Enabled = true;
            lblCategoryYear.Enabled = true;
            lblTerm.Enabled = true;
        }

        #region DeletedMethods

        public async Task UploadDocuments(Dictionary<Tuple<long, string>, byte[]> docsToUpload)
        {
            var categoryModel = new GeneralCategoryModel()
            {
                DocumentType = cmbDocumentType.Text,
                Year = dtpYear.Text,
                Term = cmbTerm.Text,
                NodeId = WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.GeneralCategoryNodeId)
            };

            var emdNew = _serviceHelper.CategoryMaker(categoryModel);

            foreach (var item in docsToUpload)
            {
                await Task.Run(() => _serviceHelper.AddDocumentWithMetaData(item.Key.Item1, item.Key.Item2, item.Value, emdNew));
            }
        }

        #endregion

    }
}
