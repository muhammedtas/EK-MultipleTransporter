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
using System.Diagnostics;
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

    public delegate void Trigger();

    public partial class DistributorForm : Form
    {

        public static Logger Logger = LogManager.GetCurrentClassLogger();
        public static long projectsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["projectsNodeId"]);
        public static long projectsChildElementsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["projectsChildElementsNodeId"]);
        public static long generalCategoryNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["generalCategoryNodeId"]);
        public static long workSpacesNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["workSpacesNodeId"]);
        public static long contentServerDocumentTemplatesNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["contentServerDocumentTemplatesNodeId"]);
        public OTServicesHelper serviceHelper = new OTServicesHelper();
        List<ListViewItem> workPlaceMasterList;
        List<ListViewItem> filteredWorkPlaceMasterList;
        IEnumerable<ListViewItem> itemsToAdd;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;
        private Trigger _worker;
        private static int ItemsPerpage = 100;
        private static int CurrentScrool = 1;

        public DistributorForm()
        {
            InitializeComponent();
            workPlaceMasterList = new List<ListViewItem>();
            filteredWorkPlaceMasterList = new List<ListViewItem>();
            itemsToAdd = new List<ListViewItem>();

            _worker = Worker;
            _cancellationToken = _cts.Token;

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
        private void lstViewScrolled(object sender, ScrollEventArgs e)
        {

        }

        private async void DistributorForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadFormsDefault());
        }
        private async void cmbDistWorkPlaceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cLstBxWorkSpaceType.Items.Clear();
            CurrentScrool = 1;
            //ItemsPerpage = 100;
            cScrollofLst.Maximum = 100;

            await Task.Factory.StartNew(_worker.Invoke, _cancellationToken, TaskCreationOptions.LongRunning,
            TaskScheduler.Current)
            .ConfigureAwait(false);

        }

        public async void Worker()
        {

            var task = Task.Factory.StartNew(() =>
            {

                try
                {
                    Parallel.Invoke(() => LoadSelectedWorkSpacesChilds(), () => LoadSelectedWorkSpacesTargetChildsToListBox());

                }
                catch (Exception)
                {
                    //throw;
                }


            }).ConfigureAwait(false);

            await task;

        }

        private void cmbDistOTFolder_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        public void LoadFormsDefault()
        {
            // Categories loaded.
            var categoryItems = serviceHelper.GetEntityAttributeGroupOfCategory(generalCategoryNodeId);
            if (categoryItems != null)
            {
                var itemArray = categoryItems.Values[0].ValidValues;
                cmbDocumentType.Items.AddRange(itemArray);
            }

            // Work Spaces Loaded.
            var workSpacesTypes = serviceHelper.GetChildNodesById(workSpacesNodeId);

            if (workSpacesTypes != null)
            {
                foreach (var workSpace in workSpacesTypes)
                {
                    cmbDistWorkPlaceType.Items.Add(new DistributorChilds()
                    {
                        Id = workSpace.Id,
                        Name = workSpace.Name
                    });

                }
            }

        }
        public void LoadSelectedWorkSpacesChilds()
        {
            // Üstteki çalışma alanı değiştikçe burası asyn bir şekilde document template ten gelmesini istiyoruz.
            // Ancak bunlar bizim asıl target node larımız olmayacak. Döküman atmak istediğimiz zaman buraya eklediğimiz node ları
            // Adı ile aratarak target nodumuzu bulacağız.

            //var task = Task.Run(() =>
            //{

            try
            {
                cmbDistOTFolder.Items.Clear();

                var docTemplateNode = DbEntityHelper.GetAncestorNodeByName(contentServerDocumentTemplatesNodeId, (cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Name);

                var childFoldersNodes = serviceHelper.GetChildNodesById(docTemplateNode.Id);

                foreach (var childNode in childFoldersNodes)
                {
                    cmbDistOTFolder.Items.Add(new DistributorChilds()
                    {
                        Id = childNode.Id,
                        Name = childNode.Name
                    });

                    if (serviceHelper.HasChildNode(childNode.Id))
                    {
                        var innerChilds = serviceHelper.GetChildNodesById(childNode.Id);

                        foreach (var innerChild in innerChilds)
                        {
                            cmbDistOTFolder.Items.Add(new DistributorChilds()
                            {
                                Id = innerChild.Id,
                                Name = childNode.Name + "\\" + innerChild.Name
                            });

                            if (serviceHelper.HasChildNode(innerChild.Id))
                            {
                                var innersOfInnerChild = serviceHelper.GetChildNodesById(innerChild.Id);
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
                }

            }
            catch (Exception)
            {
                Logger.Error("Büyük ihtimal bir workspace itemleri listlere doldurulurken workspace değişti");

                //throw;
            }

            //}).ConfigureAwait(false);

            //await task;


        }

        public void LoadSelectedWorkSpacesTargetChildsToListBox()
        {  // cmbDistWorkPlaceType

            //var task = Task.Run(() =>
            //{

            try
            {
                cLstBxWorkSpaceType.Items.Clear();

                var workSpaceTargetNodes = serviceHelper.GetChildNodesById((cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Id);

                foreach (var targetNode in workSpaceTargetNodes)
                {
                    var listItem = new ListViewItem();
                    listItem.Text = targetNode.Name;
                    listItem.Tag = new DistributorChilds() { Id = targetNode.Id, Name = targetNode.Name };
                    workPlaceMasterList.Add(listItem);
                    //  workPlaceMasterList.ToArray();
                    //cLstBxWorkSpaceType.Items.Add(listItem);
                    //cLstBxWorkSpaceType.Items.AddRange(workPlaceMasterList);
                }
                // Skip(ItemsPerpage * (CurrentPage-1)).Таке(ItemsPerpage);
                var itemsToAdd = workPlaceMasterList.ToArray().Skip(ItemsPerpage * (CurrentScrool - 1)).Take(100);
                cLstBxWorkSpaceType.Items.AddRange(itemsToAdd.ToArray());
            }
            catch (Exception)
            {
                Logger.Error("Workspace dolarken değiştirilmiş olmalı...");
                //throw;
            }

            //}).ConfigureAwait(false);

            //await task;

        }

        private void txtDistDocumentRoot_Click(object sender, EventArgs e)
        {
            ofdDocument.Title = Resources.ChooseDocument;
            ofdDocument.Filter = Resources.AllowedTypes;
            ofdDocument.Multiselect = false;
            ofdDocument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (ofdDocument.ShowDialog() != DialogResult.OK) return;
            // txtDistDocumentRoot.Text = ofdDocument.FileName.Split('\\').Last();
            txtDistDocumentRoot.Text = ofdDocument.FileName;
            StreamHelper.RootPathOfUsersFolder = ofdDocument.FileName;

        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            await Task.Run(() => DoDistributorWorks());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            filteredWorkPlaceMasterList.Clear();
            cLstBxWorkSpaceType.Items.Clear();

            if (txtFilter.Text != String.Empty || txtFilter.Text == Resources.filterText) FilterItems();

        }

        private void txtFilter_MouseClick(object sender, MouseEventArgs e)
        {
            txtFilter.Text = String.Empty;
        }

        private void FilterItems()
        {
            CurrentScrool = 1;
            cScrollofLst.Maximum = 100;
            // ListView filter ediliyor.
            foreach (ListViewItem item in workPlaceMasterList.Where(lvi => lvi.Text.ToLower().Contains(txtFilter.Text.ToLower().Trim())))
            {
                filteredWorkPlaceMasterList.Add(item);
            }

            cLstBxWorkSpaceType.Items.AddRange(filteredWorkPlaceMasterList.ToArray());

        }

        public async void DoDistributorWorks()
        {
            Debugger.NotifyOfCrossThreadDependency();

            var selectedItemList = cLstBxWorkSpaceType.CheckedItems;

            // SelectedItemList içerisinde EmlakKonut İş Alanı Türünü seçtiğimiz en alt taki checkedListBox taki node ların id sini tutar.
            // Şimdi biz bu node ların içerisinde dönerek 2. Combobox olan "Klasör Seçimi" Node larını bulacağız, Ki bunlar TARGET Node ID lerimiz olacak.
            var targetNodesList = new List<EntityNode>();
            var selectedNodeList = new List<DistributorChilds>();
            var selectedNodeIdList = new List<long>();
            var targetOTAddress = cmbDistOTFolder.Text;
            var countDeepness = cmbDistOTFolder.Text.Split('\\').Count();

            foreach (var item in selectedItemList)
            {

                var listViewItem = ((ListViewItem)item);
                var objectItem = ((listViewItem.Tag) as DistributorChilds);
                selectedNodeList.Add(objectItem);

                var itemNodeId = (((listViewItem.Tag) as DistributorChilds)).Id;
                var itemNodeName = (((listViewItem.Tag) as DistributorChilds)).Name;

                selectedNodeIdList.Add(itemNodeId);



                if (countDeepness == 1)
                {
                    // var targetNode = serviceHelper.GetNodeByName(itemNodeId, itemNodeName);
                    var oneOfTargetNode = DbEntityHelper.GetNodeByName(itemNodeId, itemNodeName);
                    targetNodesList.Add(oneOfTargetNode);
                }
                else if (countDeepness == 2)
                {
                    var generalFirstStepTargetNodeName = targetOTAddress.Split('\\')[0];
                    var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(itemNodeId, generalFirstStepTargetNodeName).FirstOrDefault();

                    var generalSecondStepTargetNodeName = targetOTAddress.Split('\\')[1];

                    var trgtChldnd = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                    if (trgtChldnd != null)
                        targetNodesList.Add(trgtChldnd);
                }
                else if (countDeepness == 3)
                {
                    var generalFirstStepTargetNodeName = targetOTAddress.Split('\\')[0];
                    var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(itemNodeId, generalFirstStepTargetNodeName).FirstOrDefault();

                    var generalSecondStepTargetNodeName = targetOTAddress.Split('\\')[1];

                    var secondChldnd = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                    var generalThirdStepTargetNodeName = targetOTAddress.Split('\\')[2];

                    var trgtChldnd = DbEntityHelper.GetNodeByName(secondChldnd.Id, generalThirdStepTargetNodeName);

                    if (trgtChldnd != null)
                        targetNodesList.Add(trgtChldnd);
                }
                else
                {
                    Console.WriteLine("Fuck your nodes!!");

                }

            }

            var preparedList = StreamHelper.PrepareDocumentToSendMultipleTarger(targetNodesList, txtDistDocumentRoot.Text);

            if (preparedList.Count < 1) return;
            // nodeId, dosya adı, ve hedef nodeId ile yarattığımız dictionary i opentext e yüklenebilir hale getireceğiz.

            await UploadDocuments(preparedList);
        }

        private async Task UploadDocuments(Dictionary<Tuple<long, string>, byte[]> docsToUpload)
        {
            var emdNew = serviceHelper.CategoryMaker(cmbDocumentType.Text, dtpDistributorYear.Text, cmbDistriborTerm.Text, generalCategoryNodeId);
            foreach (var item in docsToUpload)
            {
                await Task.Run(() => serviceHelper.AddDocumentWithMetaData(item.Key.Item1, item.Key.Item2, item.Value, emdNew));
            }
        }

        private void cScrollofLst_ValueChanged(object sender, EventArgs e)
        {
            if (cScrollofLst.Maximum >= workPlaceMasterList.Count) return;
            
            if (cScrollofLst.Maximum - 20 > ((VScrollBar)sender).Value) return;

            cScrollofLst.Maximum += 100;
            
            CurrentScrool++;

            // Filter text'e bir şey girmemişse filtered listten getir.
            if (!String.Equals(txtFilter.Text, Resources.filterText) && !String.IsNullOrEmpty(txtFilter.Text))
            {
                cLstBxWorkSpaceType.Items.Clear();
                itemsToAdd = filteredWorkPlaceMasterList.ToArray().Skip(ItemsPerpage * (CurrentScrool - 1)).Take(ItemsPerpage);
                if (itemsToAdd != null)
                    cLstBxWorkSpaceType.Items.AddRange(itemsToAdd.ToArray());
                itemsToAdd = null;
            }
            else // Filtered liste hiç dokunulmadıysa masterList ten getir.
            {
                itemsToAdd = workPlaceMasterList.ToArray().Skip(ItemsPerpage * (CurrentScrool - 1)).Take(ItemsPerpage);
                if (itemsToAdd != null)
                    cLstBxWorkSpaceType.Items.AddRange(itemsToAdd.ToArray());

                itemsToAdd = null;
            }
            
        }

        private void cScrollofLst_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (cScrollofLst.Maximum >= e.NewValue)
                {
                    cLstBxWorkSpaceType.EnsureVisible(e.NewValue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Occur moccur bro" + ex);
            }
            
        }
    }
}
