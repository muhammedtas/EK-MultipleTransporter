using EK_MultipleTransporter.DmsDocumentManagementService;
using EK_MultipleTransporter.Helpers;
using EK_MultipleTransporter.Model.ChildModel;
using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using JetBrains.Interop.WinApi;
using MessageBox = System.Windows.Forms.MessageBox;

namespace EK_MultipleTransporter.Forms
{

    public delegate void Trigger();

    public partial class DistributorForm : Form
    {

        public static Logger Logger = LogManager.GetCurrentClassLogger();
        public static long GeneralCategoryNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["generalCategoryNodeId"]);
        public static long WorkSpacesNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["workSpacesNodeId"]);
        public static long ContentServerDocumentTemplatesNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["contentServerDocumentTemplatesNodeId"]);
        public static long IndependentSectionNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["independentSectionNodeId"]); 
        public static long ProjectsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["projectsNodeId"]);
        private readonly OtServicesHelper _serviceHelper;
        private readonly List<ListViewItem> _workPlaceMasterList;
        private readonly List<ListViewItem> _filteredWorkPlaceMasterList;
        private readonly List<ListViewItem> _independentSectionsOfProjectList;
        private IEnumerable<ListViewItem> _itemsToAdd;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;
        private readonly Trigger _worker;
        private static readonly int ItemsPerPage = 100;
        private static int CurrentScroll = 1;
        public bool IsChecked;
        public bool IsProcessing;
        public bool IsDistrictsSelected;
        public static int SelectedItemCounter;
        public readonly List<ListViewItem> _distributedWorkSpaceList;

        public DistributorForm()
        {
            InitializeComponent();
            _workPlaceMasterList = new List<ListViewItem>();
            _filteredWorkPlaceMasterList = new List<ListViewItem>();
            _independentSectionsOfProjectList = new List<ListViewItem>();
            _itemsToAdd = new List<ListViewItem>();
            _distributedWorkSpaceList = new List<ListViewItem>();
            _serviceHelper = new OtServicesHelper();
            _worker = Worker;
            _cancellationToken = _cts.Token;

            var dmo = VariableHelper.Dmo;
            var ops = VariableHelper.Ops;
            CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += DistributorForm_FormClosing;

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
        private void LstViewScrolled(object sender, ScrollEventArgs e)
        {

        }

        private async void DistributorForm_Load(object sender, EventArgs e)
        {
            // var item = VariableHelper.Dmo.GetEntityAttributeGroupOfCategory("admin", VariableHelper.Token, 118237);

            await Task.Run(() => LoadFormsDefault());

            
        }
        private async void cmbDistWorkPlaceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cmbDistOTFolder.Items.Clear();
            //cLstBxWorkSpaceType.Items.Clear();
            //SelectedItemCounter = 0;
            //CurrentScroll = 1;
            //cScrollofLst.Maximum = 100;
            SelectedWorkSpaceChangedFormRefresher();

            if (string.Equals(cmbDistWorkPlaceType.Text, Resources.IndependentWorkSpace))
            {
                //ChangeStateOfFormAsDistricts();
                lblProjectsOfDistricts.Visible = true;
                cmbProjectsOfDistricts.Visible = true;
                lblProjectsOfDistricts.Enabled = true;
                cmbProjectsOfDistricts.Enabled = true;
                var distCmbLoaderTask = Task.Run(() => FillProjectsOfDistricts(), _cancellationToken);
                await distCmbLoaderTask;
                return;
            }
            else
            {
                cmbProjectsOfDistricts.Visible = false;
                lblProjectsOfDistricts.Enabled = false;
                cmbProjectsOfDistricts.Enabled = false;
                lblProjectsOfDistricts.Visible = false;
                //ChangeStateOfFormAsDistricts();
                await Task.Factory.StartNew(() => { VariableHelper.Cts.Cancel(); }, _cancellationToken, TaskCreationOptions.LongRunning,
                        TaskScheduler.FromCurrentSynchronizationContext()).
                    ConfigureAwait(false);


                await Task.Factory.StartNew(_worker.Invoke, _cancellationToken, TaskCreationOptions.LongRunning,
                        TaskScheduler.Current)
                    .ConfigureAwait(false);
            }
            

        }

        public async void Worker()
        {
            cmbDistOTFolder.Items.Clear();
            _workPlaceMasterList.Clear();
            cLstBxWorkSpaceType.Items.Clear();
         
            var task = Task.Factory.StartNew(() =>
            {

                try
                {
                    Parallel.Invoke(LoadSelectedWorkSpacesChilds, LoadSelectedWorkSpacesTargetChildsToListBox);
                }
                catch (Exception)
                {
                    //throw;
                }


            }, _cancellationToken).ConfigureAwait(false);

            await task;

        }

        private void cmbDistOTFolder_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        public void LoadFormsDefault()
        {
            // Categories loaded.
            var categoryItems = _serviceHelper.GetEntityAttributeGroupOfCategory(GeneralCategoryNodeId);
            if (categoryItems != null)
            {
                var itemArray = categoryItems.Values[0].ValidValues;
                cmbDocumentType.Items.AddRange(itemArray);
            }

            // Work Spaces Loaded.
            var workSpacesTypes = _serviceHelper.GetChildNodesById(WorkSpacesNodeId);
            if (workSpacesTypes == null) return;
            foreach (var workSpace in workSpacesTypes)
            {
                cmbDistWorkPlaceType.Items.Add(new DistributorChilds()
                {
                    Id = workSpace.Id,
                    Name = workSpace.Name
                });

            }

        }
        public void LoadSelectedWorkSpacesChilds()
        {
            // Üstteki çalışma alanı değiştikçe burası asyn bir şekilde document template ten gelmesini istiyoruz.
            // Ancak bunlar bizim asıl target node larımız olmayacak. Döküman atmak istediğimiz zaman buraya eklediğimiz node ları
            // Adı ile aratarak target nodumuzu bulacağız.

            try
            {
                cmbDistOTFolder.Items.Clear();

                var docTemplateNode = DbEntityHelper.GetAncestorNodeByName(ContentServerDocumentTemplatesNodeId, (cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Name);

                var childFoldersNodes = _serviceHelper.GetChildNodesById(docTemplateNode.Id);

                foreach (var childNode in childFoldersNodes)
                {
                    cmbDistOTFolder.Items.Add(new DistributorChilds()
                    {
                        Id = childNode.Id,
                        Name = childNode.Name
                    });
                    if (!_serviceHelper.HasChildNode(childNode.Id)) continue;
                    var innerChilds = _serviceHelper.GetChildNodesById(childNode.Id);

                    foreach (var innerChild in innerChilds)
                    {
                        cmbDistOTFolder.Items.Add(new DistributorChilds()
                        {
                            Id = innerChild.Id,
                            Name = childNode.Name + "\\" + innerChild.Name
                        });
                        if (!_serviceHelper.HasChildNode(innerChild.Id)) continue;
                        var innersOfInnerChild = _serviceHelper.GetChildNodesById(innerChild.Id);
                        foreach (var innerOfInnerChild in innersOfInnerChild)
                        {
                            cmbDistOTFolder.Items.Add(new ProjectChilds()
                            {
                                Id = innerOfInnerChild.Id,
                                Name = childNode.Name + "\\" + innerChild.Name + "\\" + innerOfInnerChild.Name
                            });

                        }

                    }
                }

            }
            catch (Exception)
            {
                Logger.Error("Büyük ihtimal bir workspace itemleri listlere doldurulurken workspace değişti");
            }

        }

        public void LoadSelectedWorkSpacesTargetChildsToListBox()
        {
            try
            {
                cLstBxWorkSpaceType.Items.Clear();
                
                var workSpaceTargetNodes = _serviceHelper.GetChildNodesById((cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Id);

                //var data1 = VariableHelper.Dmo.GetChildNodes("admin", VariableHelper.Token, (cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Id, 0, 100, false, false);
                //var data2 = VariableHelper.Dmo.GetChildNodes("admin", VariableHelper.Token, (cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Id, 100, 100, false, false);
                //var data3 = VariableHelper.Dmo.GetChildNodes("admin", VariableHelper.Token, (cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Id, 200, 100, false, false);

                foreach (var targetNode in workSpaceTargetNodes)
                {
                    var listItem = new ListViewItem
                    {
                        Text = targetNode.Name,
                        Tag = new DistributorChilds() { Id = targetNode.Id, Name = targetNode.Name }
                    };
                    _workPlaceMasterList.Add(listItem);

                }

                 var itemsToAdd = _workPlaceMasterList.ToArray().Skip(ItemsPerPage * (CurrentScroll - 1)).Take(100);
                cLstBxWorkSpaceType.Items.AddRange(itemsToAdd.ToArray());
            }
            catch (Exception ex)
            {
                Logger.Error("Workspace dolarken değiştirilmiş olmalı...");
                Logger.Error("Details   :" + ex);
                //throw;
            }


        }

        private void txtDistDocumentRoot_Click(object sender, EventArgs e)
        {
            ofdDocument.Title = Resources.ChooseDocument;
            ofdDocument.Filter = Resources.AllowedTypes;
            ofdDocument.Multiselect = false;
            ofdDocument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (ofdDocument.ShowDialog() != DialogResult.OK) return;

            txtDistDocumentRoot.Text = ofdDocument.FileName;
            StreamHelper.RootPathOfUsersFolder = ofdDocument.FileName;

        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            if (CheckFormsIfSuitableForUpload()) await Task.Run(() => DoDistributorWorks(), _cancellationToken);
            else MessageBox.Show(Resources.ChooseAllRequiredFields);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                cLstBxWorkSpaceType.Items.Clear();
                _filteredWorkPlaceMasterList.Clear();
                //var thisIsHedgeStone = SelectedItemCounter;
                cLstBxWorkSpaceType.Items.AddRange(_workPlaceMasterList.ToArray());
                lblCounter.Text = Resources.SelectedItemNumber + cLstBxWorkSpaceType.SelectedItems.Count; // Burası güzel hard code oldu. O değil çok güzel hardcode oldu.
                return;
            }

            _filteredWorkPlaceMasterList.Clear();
            cLstBxWorkSpaceType.Items.Clear();

            if (txtFilter.Text != string.Empty || txtFilter.Text == Resources.filterText) FilterItems();
            //if (txtFilter.Text == Resources.filterText) FilterItems();

        }

        private void txtFilter_MouseClick(object sender, MouseEventArgs e)
        {
            txtFilter.Text = string.Empty;
        }

        private void FilterItems()
        {
            if (string.IsNullOrEmpty(txtFilter.Text))  return;

            CurrentScroll = 1;
            cScrollofLst.Maximum = 100;
            // ListView filter ediliyor.
            foreach (var item in _workPlaceMasterList.Where(lvi => lvi.Text.ToLower().Contains(txtFilter.Text.ToLower().Trim())))
            {
                _filteredWorkPlaceMasterList.Add(item);
            }
            cLstBxWorkSpaceType.Items.Clear(); // **
            cLstBxWorkSpaceType.Items.AddRange(_filteredWorkPlaceMasterList.ToArray());

        }

        public async void DoDistributorWorks()
        {
            InvokedFormState();
            Debugger.NotifyOfCrossThreadDependency();

            var selectedItemList = cLstBxWorkSpaceType.CheckedItems;

            // SelectedItemList içerisinde EmlakKonut İş Alanı Türünü seçtiğimiz en alt taki checkedListBox taki node ların id sini tutar.
            // Şimdi biz bu node ların içerisinde dönerek 2. Combobox olan "Klasör Seçimi" Node larını bulacağız, Ki bunlar TARGET Node ID lerimiz olacak.
            var targetNodesList = new List<EntityNode>();
            var selectedNodeList = new List<DistributorChilds>();
            var selectedNodeIdList = new List<long>();
            var targetOpenTextAddress = cmbDistOTFolder.Text;
            var countDeepness = cmbDistOTFolder.Text.Split('\\').Count();
            if (countDeepness > 3)
            {
                MessageBox.Show(Resources.NodeDeepnessExceed);
                return;
            }

            foreach (var item in selectedItemList)
            {

                var listViewItem = ((ListViewItem)item);
                var objectItem = ((listViewItem.Tag) as DistributorChilds);
                selectedNodeList.Add(objectItem);

                var itemNodeId = (((listViewItem.Tag) as DistributorChilds)).Id;
                var itemNodeName = (((listViewItem.Tag) as DistributorChilds)).Name;

                selectedNodeIdList.Add(itemNodeId);
                
                switch (countDeepness)
                {
                    case 1:
                    {
                        // var targetNode = serviceHelper.GetNodeByName(itemNodeId, itemNodeName);
                        var oneOfTargetNode = DbEntityHelper.GetNodeByName(itemNodeId, itemNodeName);
                        targetNodesList.Add(oneOfTargetNode);
                        break;
                    }
                    case 2:
                    {
                        var generalFirstStepTargetNodeName = targetOpenTextAddress.Split('\\')[0];
                        var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(itemNodeId, generalFirstStepTargetNodeName).FirstOrDefault();

                        var generalSecondStepTargetNodeName = targetOpenTextAddress.Split('\\')[1];
                        if (firstStepTargetNode == null) continue;
                        var targetChildNode = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                        if (targetChildNode != null)
                            targetNodesList.Add(targetChildNode);
                        break;
                    }
                    case 3:
                    {
                        var generalFirstStepTargetNodeName = targetOpenTextAddress.Split('\\')[0];
                        var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(itemNodeId, generalFirstStepTargetNodeName).FirstOrDefault();

                        var generalSecondStepTargetNodeName = targetOpenTextAddress.Split('\\')[1];
                        if (firstStepTargetNode == null) continue;
                        var secondChildNode = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                        var generalThirdStepTargetNodeName = targetOpenTextAddress.Split('\\')[2];

                        var targetChildNode = DbEntityHelper.GetNodeByName(secondChildNode.Id, generalThirdStepTargetNodeName);

                        if (targetChildNode != null)
                            targetNodesList.Add(targetChildNode);
                        break;
                    }
                    default:
                        Console.WriteLine(Resources.NodeDeepnessExceed);
                        //Console.WriteLine("Node derinliği beklenen değerin üzerinde.");
                        break;
                }

            }

            var preparedList = StreamHelper.PrepareDocumentToSendMultipleTarget(targetNodesList, txtDistDocumentRoot.Text);

            if (preparedList.Count < 1) return;
            // nodeId, dos-ya adı, ve hed-ef nodeId ile ya-rat-tı-ğı-mız dictionary i open-text e yük-le-ne-bi-lir hale getireceğiz.

            await UploadDocuments(preparedList);
            WaitedFormState();
            MessageBox.Show(Resources.ProcessIsDone);

        }

        private async Task UploadDocuments(Dictionary<Tuple<long, string>, byte[]> docsToUpload)
        {
            var emdNew = _serviceHelper.CategoryMaker(cmbDocumentType.Text, dtpDistributorYear.Text, cmbDistriborTerm.Text, GeneralCategoryNodeId);
            foreach (var item in docsToUpload)
            {
                await Task.Run(() => _serviceHelper.AddDocumentWithMetaData(item.Key.Item1, item.Key.Item2, item.Value, emdNew), _cancellationToken);
            }
        }

        private void cScrollofLst_ValueChanged(object sender, EventArgs e)
        {
            if (cScrollofLst.Maximum >= _workPlaceMasterList.Count) return;
            if (cScrollofLst.Maximum - 20 > ((VScrollBar)sender).Value) return;

            cScrollofLst.Maximum += 100;

            CurrentScroll++;

            bool a = string.Equals(txtFilter.Text, Resources.filterText);
            bool b = string.IsNullOrEmpty(txtFilter.Text);
            bool c = txtFilter.Text == "" ? true : false;
            // Filter text'e bir şey girmemişse filtered listten getir.
            if (!string.Equals(txtFilter.Text, Resources.filterText) && !string.IsNullOrEmpty(txtFilter.Text))
            {
                if (cScrollofLst.Maximum >= _filteredWorkPlaceMasterList.Count && _filteredWorkPlaceMasterList.Count > 0) return;
                cLstBxWorkSpaceType.Items.Clear();
                _itemsToAdd = _filteredWorkPlaceMasterList.ToArray().Skip(ItemsPerPage * (CurrentScroll - 1)).Take(ItemsPerPage);
                if (_itemsToAdd != null)
                    cLstBxWorkSpaceType.Items.AddRange(_itemsToAdd.ToArray());
                _itemsToAdd = null;
            }
            else // Filtered liste hiç dokunulmadıysa masterList ten getir.
            {
                _filteredWorkPlaceMasterList.Clear();
                
                _itemsToAdd = _workPlaceMasterList.ToArray().Skip(ItemsPerPage * (CurrentScroll - 1)).Take(ItemsPerPage);
                if (_itemsToAdd != null)
                {
                    cLstBxWorkSpaceType.Items.AddRange(_itemsToAdd.ToArray());
                }

                _itemsToAdd = null;
            }

        }

        private void cScrollofLst_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (cScrollofLst.Maximum >= e.NewValue || cScrollofLst.Maximum >= _filteredWorkPlaceMasterList.Count())
                {
                    cLstBxWorkSpaceType.EnsureVisible(e.NewValue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Scrooling is not working properly..." + ex);
            }

        }

        public void InvokedFormState()
        {
            IsProcessing = true;
            cbCheckAll.Enabled = false;
            cmbDistWorkPlaceType.Enabled = false;
            cmbDistOTFolder.Enabled = false;
            txtDistDocumentRoot.Enabled = false;
            cmbDocumentType.Enabled = false;
            dtpDistributorYear.Enabled = false;
            cmbDistriborTerm.Enabled = false;
            txtFilter.Enabled = false;
            cLstBxWorkSpaceType.Enabled = false;
            cScrollofLst.Enabled = false;
            btnOk.Enabled = false;
            btnCancel.Enabled = true;
            lblProjectsOfDistricts.Enabled = false;
            cmbProjectsOfDistricts.Enabled = false;
            //this.Enabled = false;
        }

        public void WaitedFormState()
        {
            IsProcessing = false;
            cbCheckAll.Enabled = true;
            cmbDistWorkPlaceType.Enabled = true;
            cmbDistOTFolder.Enabled = true;
            txtDistDocumentRoot.Enabled = true;
            cmbDocumentType.Enabled = true;
            dtpDistributorYear.Enabled = true;
            cmbDistriborTerm.Enabled = true;
            txtFilter.Enabled = true;
            cLstBxWorkSpaceType.Enabled = true;
            cScrollofLst.Enabled = true;
            btnOk.Enabled = true;
            btnCancel.Enabled = true;
            lblProjectsOfDistricts.Enabled = true;
            cmbProjectsOfDistricts.Enabled = true;
            this.Enabled = true;
        }

        private void cbCheckAll_Click(object sender, EventArgs e)
        {
            if ((_workPlaceMasterList.Count == 0 || _filteredWorkPlaceMasterList.Count == 0) & cLstBxWorkSpaceType.SelectedItems.Count != 0)
            {
                SelectedItemCounter = 0;
                lblCounter.Text = Resources.SelectedItemNumber + SelectedItemCounter;
            }
            
            if (!IsChecked)
            {
                IsChecked = true;
                cLstBxWorkSpaceType.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = IsChecked);
            }
            else
            {
                IsChecked = false;
                cLstBxWorkSpaceType.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = IsChecked);
                SelectedItemCounter = 0;
                lblCounter.Text = Resources.SelectedItemNumber + SelectedItemCounter;
            }
        }

        private void cLstBxWorkSpaceType_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFilter.Text) && !string.Equals(txtFilter.Text, Resources.filterText)) lblCounter.Text = Resources.SelectedItemNumber + cLstBxWorkSpaceType.SelectedItems.Count;
            //if (string.IsNullOrEmpty(txtFilter.Text)) return;
            switch (e.NewValue)
            {
                case CheckState.Unchecked:
                    SelectedItemCounter--;
                    break;
                case CheckState.Checked:
                    SelectedItemCounter++;
                    break;
                case CheckState.Indeterminate:
                    break;
                default:
                    break;
            }
            lblCounter.Text = Resources.SelectedItemNumber + SelectedItemCounter;
        }

        private void DistributorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CurrentScroll = 1;
            IsChecked = false;
            IsProcessing = false;
            SelectedItemCounter = 0;
        }

        private bool CheckFormsIfSuitableForUpload()
        {
            return cmbDocumentType.SelectedItem != null && cmbDistWorkPlaceType.SelectedItem != null && cmbDistOTFolder.SelectedItem != null && !string.IsNullOrEmpty(txtDistDocumentRoot.Text);
        }

        public void ListViewLoaderForDistributedWorkSpace(int startIndex, int length)
        {
            var itemsToLoad = _distributedWorkSpaceList.Skip(startIndex).Take(length);
            cLstBxWorkSpaceType.Items.AddRange(itemsToLoad.ToArray());
        }

        public void FillProjectsOfDistricts()
        {
            var projectsOfDistricts = _serviceHelper.GetChildNodesById(ProjectsNodeId);

            foreach (var projectOfDistrict in projectsOfDistricts)
            {
                cmbProjectsOfDistricts.Items.Add(new DistributorChilds()
                {
                    Id = projectOfDistrict.Id,
                    Name = projectOfDistrict.Name
                });
            }
        }

        public void FillDistributedWorkSpacesList()
        {
            var distList = _serviceHelper.GetChildNodesById((cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Id);

            foreach (var item in distList)
            {
                var listItem = new ListViewItem
                {
                    Text = item.Name,
                    Tag = new DistributorChilds() { Id = item.Id, Name = item.Name }
                };
                _distributedWorkSpaceList.Add(listItem);
            }
        }

        public void ChangeStateOfFormAsDistricts()
        {

            if (IsDistrictsSelected == false)
            {
                IsDistrictsSelected = true;
                lblProjectsOfDistricts.Visible = true;
                cmbProjectsOfDistricts.Visible = true;
                lblProjectsOfDistricts.Enabled = true;
                cmbProjectsOfDistricts.Enabled = true;
            }
            else
            {
                IsDistrictsSelected = false;
                lblProjectsOfDistricts.Visible = false;
                cmbProjectsOfDistricts.Visible = false;
                lblProjectsOfDistricts.Enabled = false;
                cmbProjectsOfDistricts.Enabled = false;
            }
            
        }

        private void cmbProjectsOfDistricts_SelectedIndexChanged(object sender, EventArgs e)
        {
            Task.Run(() => GetIndependentSectionsOfProject());
        }

        private async void GetIndependentSectionsOfProject()
        {
            cLstBxWorkSpaceType.Clear();
            // Burada seçilmiş olan projeye göre bağımsız bölümleri getirme işi, proje adının ilk kısmının Bağımsız bölüm kategori adında 
            // aratarak bulunacağı halidir. 
            // Kategoride proje adı Örn : 1001 ise, bağ. böl kategorisinde bu alan 00001001 olarak kayıtlıdır. Format hepsinde aynı, başa 4 sıfır alıyor.
            var strVal = cmbProjectsOfDistricts.SelectedItem as DistributorChilds;
            var strVel = strVal?.Name.Split('/')[0];

            var projectsOfIndependentSection =
                DbEntityHelper.GetNodesByCategoryAttribute(118237, strVel);

            foreach (var targetNode in projectsOfIndependentSection)
            {
                var listItem = new ListViewItem
                {
                    Text = targetNode.Name,
                    Tag = new DistributorChilds() { Id = targetNode.Id, Name = targetNode.Name }
                };
                _independentSectionsOfProjectList.Add(listItem);

            }

            cLstBxWorkSpaceType.Items.AddRange(_independentSectionsOfProjectList.ToArray());

        }

        private async void SelectedWorkSpaceChangedFormRefresher()
        {
            cmbDistOTFolder.Items.Clear();
            cLstBxWorkSpaceType.Items.Clear();
            SelectedItemCounter = 0;
            lblCounter.Text = Resources.SelectedItemNumber + SelectedItemCounter;
            CurrentScroll = 1;
            cScrollofLst.Maximum = 100;
        }
    }
}
