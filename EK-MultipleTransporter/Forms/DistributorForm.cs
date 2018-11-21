using EK_MultipleTransporter.Enums;
using EK_MultipleTransporter.Helpers;
using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EK_MultipleTransporter.Models.ChildModel;
using EK_MultipleTransporter.Models.HelperModel;
using EK_MultipleTransporter.Web_References.DmsDocumentManagementService;
using MessageBox = System.Windows.Forms.MessageBox;

namespace EK_MultipleTransporter.Forms
{
    public delegate void Trigger();

    public partial class DistributorForm : Form
    {
        private readonly Trigger _worker;

        private readonly OtServicesHelper _serviceHelper;
        private readonly List<ListViewItem> _workPlaceMasterList;
        private readonly List<ListViewItem> _filteredWorkPlaceMasterList;
        private readonly List<ListViewItem> _independentSectionsOfProjectList;
        private readonly List<ListViewItem> _distributedWorkSpaceList;
        private IEnumerable<ListViewItem> _itemsToAdd;

        public static Logger Logger;
        public static int ItemsPerPage;
        public static int CurrentScroll;
        public static int SelectedItemCounter;
        public bool IsChecked;
        public bool IsProcessing;
        public bool IsDistrictsSelected;

        public DistributorForm()
        {
            InitializeComponent();
            VariableHelper.InitializeVariables();
            VariableHelper.InitializeNewCancellationTokenSource();
            Logger = LogManager.GetCurrentClassLogger();
            _workPlaceMasterList = new List<ListViewItem>();
            _filteredWorkPlaceMasterList = new List<ListViewItem>();
            _independentSectionsOfProjectList = new List<ListViewItem>();
            _itemsToAdd = new List<ListViewItem>();
            _distributedWorkSpaceList = new List<ListViewItem>();
            _serviceHelper = new OtServicesHelper();
            _worker = Worker;
            ItemsPerPage = 100;
            CurrentScroll = 1;
           
            CheckForIllegalCrossThreadCalls = false;
            FormClosing += DistributorForm_FormClosing;

        }
        private async void DistributorForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadFormsDefault(), VariableHelper.Cts.Token);   
        }
        private async void cmbDistWorkPlaceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedWorkSpaceChangedFormRefresher();

            if (string.Equals(cmbDistWorkPlaceType.Text, Resources.IndependentWorkSpace))
            {
                lblProjectsOfDistricts.Visible = true;
                cmbProjectsOfDistricts.Visible = true;
                lblProjectsOfDistricts.Enabled = true;
                cmbProjectsOfDistricts.Enabled = true;
                var distCmbLoaderTask = Task.Run(() => FillProjectsOfDistricts(), VariableHelper.Cts.Token);
                await distCmbLoaderTask;
            }
            else
            {
                cmbProjectsOfDistricts.Visible = false;
                lblProjectsOfDistricts.Enabled = false;
                cmbProjectsOfDistricts.Enabled = false;
                lblProjectsOfDistricts.Visible = false;
            
                await Task.Factory.StartNew(_worker.Invoke, VariableHelper.Cts.Token, TaskCreationOptions.LongRunning,
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
                    Console.WriteLine(Resources.WorkerMethodException);
                }


            }, VariableHelper.Cts.Token).ConfigureAwait(false);

            await task;

        }

        /// <summary>
        /// Kategori ve work spaces leri ilgili comboboxlara yükler.
        /// </summary>
        public void LoadFormsDefault()
        {
            // Categories loaded.
            var categoryItems = _serviceHelper.GetEntityAttributeGroupOfCategory(WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.GeneralCategoryNodeId));
            if (categoryItems != null)
            {
                var itemArray = categoryItems.Values[0].ValidValues;
                cmbDocumentType.Items.AddRange(itemArray);
            }

            // Work Spaces Loaded.
            var workSpacesTypes = _serviceHelper.GetChildNodesById(WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.WorkSpacesNodeId));
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
        /// <summary>
        /// Üstteki çalışma alanı değiştikçe burası asyn bir şekilde document template ten gelmesini istiyoruz.
        /// Ancak bunlar bizim asıl target node larımız olmayacak. Döküman atmak istediğimiz zaman buraya eklediğimiz node ları
        /// Adı ile aratarak target nodumuzu bulacağız.
        /// </summary>
        public void LoadSelectedWorkSpacesChilds()
        {
           
            try
            {
                cmbDistOTFolder.Items.Clear();

                var docTemplateNode = DbEntityHelper.GetAncestorNodeByName(WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.ContentServerDocumentTemplatesNodeId), (cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Name);

                var childFoldersNodes = _serviceHelper.GetChildNodesById(docTemplateNode.Id);

                foreach (var childNode in childFoldersNodes)
                {
                    cmbDistOTFolder.Items.Add(new DistributorChilds()
                    {
                        Id = childNode.Id,
                        Name = childNode.Name
                    });
                    if (!_serviceHelper.HasChildNode(childNode.Id)) continue;
                    var innerChildList = _serviceHelper.GetChildNodesById(childNode.Id);

                    foreach (var innerChild in innerChildList)
                    {
                        cmbDistOTFolder.Items.Add(new DistributorChilds()
                        {
                            Id = innerChild.Id,
                            Name = childNode.Name + "\\" + innerChild.Name
                        });
                        if (!_serviceHelper.HasChildNode(innerChild.Id)) continue;
                        var innerListOfInnerChild = _serviceHelper.GetChildNodesById(innerChild.Id);
                        foreach (var innerOfInnerChild in innerListOfInnerChild)
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
            catch (Exception ex)
            {
                Logger.Error(ex, "Workspace itemleri listlere doldurulurken workspace değişmiş olabilir. Adres : LoadSelectedWorkSpacesChilds");
            }

        }

        public void LoadSelectedWorkSpacesTargetChildsToListBox()
        {
            try
            {
                cLstBxWorkSpaceType.Items.Clear();
                
                var workSpaceTargetNodes = _serviceHelper.GetChildNodesById((cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Id);

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
                Logger.Error(ex, "Workspace dolarken değiştirilmiş olmalı...");
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
            if (CheckFormsIfSuitableForUpload()) await Task.Run(() => DoDistributorWorks(), VariableHelper.Cts.Token);
            else MessageBox.Show(Resources.ChooseAllRequiredFields);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Liste filter edilirken yapılacak işlemler. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                cLstBxWorkSpaceType.Items.Clear();
                _filteredWorkPlaceMasterList.Clear();
                cLstBxWorkSpaceType.Items.AddRange(_workPlaceMasterList.ToArray());
                lblCounter.Text = Resources.SelectedItemNumber + cLstBxWorkSpaceType.CheckedItems.Count; 
                return;
            }

            _filteredWorkPlaceMasterList.Clear();
            cLstBxWorkSpaceType.Items.Clear();

            if (txtFilter.Text != string.Empty || txtFilter.Text == Resources.filterText) FilterItems();
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
            // ListView is filtering
            foreach (var item in _workPlaceMasterList.Where(lvi => lvi.Text.ToLower().Contains(txtFilter.Text.ToLower().Trim())))
            {
                _filteredWorkPlaceMasterList.Add(item);
            }
            cLstBxWorkSpaceType.Items.Clear(); 
            cLstBxWorkSpaceType.Items.AddRange(_filteredWorkPlaceMasterList.ToArray());
            lblCounter.Text = Resources.SelectedItemNumber + cLstBxWorkSpaceType.CheckedItems.Count;
        }
        /// <summary>
        /// SelectedItemList içerisinde EmlakKonut İş Alanı Türünü seçtiğimiz en alt taki checkedListBox taki node ların id sini tutar.
        /// Şimdi biz bu node ların içerisinde dönerek 2. Combobox olan "Klasör Seçimi" Node larını bulacağız, Ki bunlar TARGET Node ID lerimiz olacak.
        /// </summary>
        public async void DoDistributorWorks()
        {
            InvokedFormState();
            Debugger.NotifyOfCrossThreadDependency();
            var selectedItemList = cLstBxWorkSpaceType.CheckedItems;
            
            var targetNodesList = new List<EntityNode>();
            var targetOpenTextAddress = cmbDistOTFolder.Text;
            var countDeepness = cmbDistOTFolder.Text.Split('\\').Count();

            if (countDeepness > 3)
            {
                MessageBox.Show(Resources.NodeDeepnessExceed);
                return;
            }

            foreach (var item in selectedItemList)
            {

                var listViewItem = (ListViewItem)item;
                var itemNodeId = (listViewItem.Tag as DistributorChilds).Id;
                var itemNodeName = (listViewItem.Tag as DistributorChilds).Name;
                
                switch (countDeepness)
                {
                    case 1:
                    {
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
                        break;
                }

            }

            var preparedList = StreamHelper.PrepareDocumentToSendMultipleTarget(targetNodesList, txtDistDocumentRoot.Text);

            if (preparedList.Count < 1) return;
            // nodeId, dos-ya adı, ve he-def nodeId ile ya-rat-tı-ğı-mız dictionary i open-text e yük-le-ne-bi-lir hale ge-ti-re-ce-ğiz.
            var categoryModel = new GeneralCategoryModel()
            {
                DocumentType = cmbDocumentType.Text,
                Year = dtpDistributorYear.Text,
                Term = cmbDistriborTerm.Text,
                NodeId = WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.GeneralCategoryNodeId)
            };
            var result = await _serviceHelper.UploadDocuments(preparedList, categoryModel);
            MessageBox.Show(result ? Resources.ProcessIsDone : Resources.ProcessIsNotDone);
            WaitedFormState();

        }
        /// <summary>
        /// scRool listViewDen ayrı bir item. Ve bu limitine yaklaştıkça listview in itemlerini burada dolduracağız.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cScrollofLst_ValueChanged(object sender, EventArgs e)
        {
            if (cScrollofLst.Maximum >= _workPlaceMasterList.Count) return;
            if (cScrollofLst.Maximum - 20 > ((VScrollBar)sender).Value) return;

            cScrollofLst.Maximum += 100;

            CurrentScroll++;

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
            else if (string.IsNullOrEmpty(txtFilter.Text)) //Filtered liste dokunulmuş ama aratılmamışsa masterList ten getir.
            {
                try
                {
                    _filteredWorkPlaceMasterList.Clear();

                    _itemsToAdd = _workPlaceMasterList.ToArray().Skip(ItemsPerPage * (CurrentScroll - 1)).Take(ItemsPerPage);
                    if (_itemsToAdd != null)
                    {
                        cLstBxWorkSpaceType.Items.AddRange(_itemsToAdd.ToArray());
                    }

                    _itemsToAdd = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Resources.AvaredErrorTypes + ex);
                    //throw;
                }
            }
            else // Hiç dokunulmamış halindeyse sadece masterlist i scrool boyutunda getir. 
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
                if (cScrollofLst.Maximum >= e.NewValue || cScrollofLst.Maximum >= _filteredWorkPlaceMasterList.Count()) cLstBxWorkSpaceType.EnsureVisible(e.NewValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.AvaredErrorTypes + ex);
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
            Enabled = true;
        }

        private void cbCheckAll_Click(object sender, EventArgs e)
        {
            if ((_workPlaceMasterList.Count == 0 || _filteredWorkPlaceMasterList.Count == 0) & cLstBxWorkSpaceType.CheckedItems.Count != 0)
            {
                SelectedItemCounter = 0;
                lblCounter.Text = Resources.SelectedItemNumber + SelectedItemCounter;
            }
            
            if (!IsChecked)
            {
                IsChecked = true;

                // PERFORMANCE TESTS WILL BE DONE IN HERE! 
                for (var i = 0; i < cLstBxWorkSpaceType.Items.Count; i++) cLstBxWorkSpaceType.Items[i].Checked = IsChecked; // 3.42

                //var source = cLstBxWorkSpaceType.Items.OfType<ListViewItem>();
                //source.AsParallel().WithDegreeOfParallelism(10).ForEach(x => x.Checked = IsChecked); // 3.45 dk
                //cLstBxWorkSpaceType.Items.OfType<ListViewItem>().ToList().AsParallel().ForAll(item => item.Checked = IsChecked); // 4 dk.
                //cLstBxWorkSpaceType.Items.OfType<ListViewItem>().AsQueryable().ForEach(x => x.Checked = IsChecked);
                //cLstBxWorkSpaceType.Items.OfType<ListViewItem>().ForEach(item => item.Checked = IsChecked);
                //cLstBxWorkSpaceType.Items.OfType<ListViewItem>().AsParallel().ForEach(x => x.Checked = IsChecked); // 3.50
                //Parallel.ForEach(cLstBxWorkSpaceType.Items.Cast<ListViewItem>(), (item) => { item.Checked = IsChecked; }); // 4 dk.
            }
            else
            {
                // PERFORMANCE TESTS WILL BE DONE IN HERE! 
                IsChecked = false;

                for (var i = 0; i < cLstBxWorkSpaceType.Items.Count; i++) cLstBxWorkSpaceType.Items[i].Checked = IsChecked; 

                //var source = cLstBxWorkSpaceType.Items.OfType<ListViewItem>();
                //source.AsParallel().WithDegreeOfParallelism(10).ForEach(x => x.Checked = IsChecked);
                //cLstBxWorkSpaceType.Items.OfType<ListViewItem>().ToList().AsParallel().ForAll(item => item.Checked = IsChecked);
                //cLstBxWorkSpaceType.Items.OfType<ListViewItem>().AsQueryable().ForEach(x => x.Checked = IsChecked);
                //cLstBxWorkSpaceType.Items.OfType<ListViewItem>().ForEach(item => item.Checked = IsChecked);
                //cLstBxWorkSpaceType.Items.OfType<ListViewItem>().AsParallel().ForEach(x => x.Checked = IsChecked);
                //Parallel.ForEach(cLstBxWorkSpaceType.Items.Cast<ListViewItem>(), (item) => { item.Checked = IsChecked; });
                SelectedItemCounter = 0;
                lblCounter.Text = Resources.SelectedItemNumber + SelectedItemCounter;
            }
        }

        private async void cLstBxWorkSpaceType_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFilter.Text) && !string.Equals(txtFilter.Text, Resources.filterText)) lblCounter.Text = Resources.SelectedItemNumber + cLstBxWorkSpaceType.CheckedItems.Count;

            SelectedItemCounter = cLstBxWorkSpaceType.CheckedItems.Count;
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
            Task.Factory.StartNew(() =>
            {
                VariableHelper.Cts.Cancel();
            }, VariableHelper.Cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);
        }

        private bool CheckFormsIfSuitableForUpload()
        {
            return cmbDocumentType.SelectedItem != null && cmbDistWorkPlaceType.SelectedItem != null && cmbDistOTFolder.SelectedItem != null && !string.IsNullOrEmpty(txtDistDocumentRoot.Text);
        }

        public void FillProjectsOfDistricts()
        {
            var projectsOfDistricts = _serviceHelper.GetChildNodesById(WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.ProjectsNodeId));
            foreach (var projectOfDistrict in projectsOfDistricts)
            {
                cmbProjectsOfDistricts.Items.Add(new DistributorChilds()
                {
                    Id = projectOfDistrict.Id,
                    Name = projectOfDistrict.Name
                });
            }
        }
        
        private void cmbProjectsOfDistricts_SelectedIndexChanged(object sender, EventArgs e)
        {
            Task.Run(() => GetIndependentSectionsOfProject(), VariableHelper.Cts.Token);
        }
        /// <summary>
        /// Burada seçilmiş olan projeye göre bağımsız bölümleri getirme işi, proje adının ilk kısmının Bağımsız bölüm kategori adında
        /// aratarak bulunacağı halidir.Kategoride proje adı Örn : 1001 ise, bağ.böl kategorisinde bu alan 00001001 olarak kayıtlıdır. Format hepsinde aynı, başa 4 sıfır alıyor.
        /// </summary>
        private void GetIndependentSectionsOfProject()
        {
            cLstBxWorkSpaceType.Clear();
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

        private void SelectedWorkSpaceChangedFormRefresher()
        {
            cmbDistOTFolder.Items.Clear();
            cLstBxWorkSpaceType.Items.Clear();
            cbCheckAll.Checked = false;
            SelectedItemCounter = 0;
            lblCounter.Text = Resources.SelectedItemNumber + SelectedItemCounter;
            CurrentScroll = 1;
            cScrollofLst.Maximum = 100;
        }


        #region DeletedMethods

        public void ListViewLoaderForDistributedWorkSpace(int startIndex, int length)
        {
            var itemsToLoad = _distributedWorkSpaceList.Skip(startIndex).Take(length);
            cLstBxWorkSpaceType.Items.AddRange(itemsToLoad.ToArray());
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

        private async Task UploadDocuments(Dictionary<Tuple<long, string>, byte[]> docsToUpload)
        {
            var categoryModel = new GeneralCategoryModel()
            {
                DocumentType = cmbDocumentType.Text,
                Year = dtpDistributorYear.Text,
                Term = cmbDistriborTerm.Text,
                NodeId = WorkSpacesEnum.GetValue(WorkSpacesEnum.WorkSpaces.GeneralCategoryNodeId)
            };

            var emdNew = _serviceHelper.CategoryMaker(categoryModel);
            foreach (var item in docsToUpload)
            {
                if (VariableHelper.Cts.IsCancellationRequested) return;
                await Task.Run(() => _serviceHelper.AddDocumentWithMetaData(item.Key.Item1, item.Key.Item2, item.Value, emdNew), VariableHelper.Cts.Token);
            }
            MessageBox.Show(Resources.ProcessIsDone);
        }

        #endregion

    }
}
