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
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;
        private Trigger _worker;

        public DistributorForm()
        {
            InitializeComponent();
            workPlaceMasterList = new List<ListViewItem>();
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

        private async void DistributorForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadFormsDefault());
        }
        private async void cmbDistWorkPlaceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cLstBxWorkSpaceType.Items.Clear();
            //await Task.Run(() => LoadSelectedWorkSpacesChilds());
            //await Task.Run(() => LoadSelectedWorkSpacesTargetChildsToListBox());

            await Task.Factory.StartNew(_worker.Invoke, _cancellationToken, TaskCreationOptions.LongRunning,
                    TaskScheduler.Current)
                .ConfigureAwait(false);

            

            //List<Task> tasks = new List<Task>();
            //tasks.Add(Task.Run(() => { LoadSelectedWorkSpacesChilds(); }));
            //tasks.Add(Task.Run(() => { LoadSelectedWorkSpacesTargetChildsToListBox(); }));
            //Task.WaitAll(tasks.ToArray());

            //var task1 = Task.Run(() => LoadSelectedWorkSpacesChilds());
            //var task2 = Task.Run(() => LoadSelectedWorkSpacesTargetChildsToListBox());
            //Task task1 = LoadSelectedWorkSpacesChilds();
            //Task task2 = LoadSelectedWorkSpacesTargetChildsToListBox();

            // Task.WhenAll(task1, task2);

        }

        public async void Worker()
        {
            var task = Task.Factory.StartNew(() =>
            {
                Parallel.Invoke(() => LoadSelectedWorkSpacesChilds(), () => LoadSelectedWorkSpacesTargetChildsToListBox());

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
        public async void LoadSelectedWorkSpacesChilds()
        {
            // Üstteki çalışma alanı değiştikçe burası asyn bir şekilde document template ten gelmesini istiyoruz.
            // Ancak bunlar bizim asıl target node larımız olmayacak. Döküman atmak istediğimiz zaman buraya eklediğimiz node ları
            // Adı ile aratarak target nodumuzu bulacağız.

            var task = Task.Run(() => {

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


            }).ConfigureAwait(false);


           await task;
      

        }

        public async void LoadSelectedWorkSpacesTargetChildsToListBox()
        {  // cmbDistWorkPlaceType

            var task = Task.Run(() =>
            {
                cLstBxWorkSpaceType.Items.Clear();

                var workSpaceTargetNodes = serviceHelper.GetChildNodesById((cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Id);

                foreach (var targetNode in workSpaceTargetNodes)
                {
                    var listItem = new ListViewItem();
                    listItem.Text = targetNode.Name;
                    listItem.Tag = new DistributorChilds() { Id = targetNode.Id, Name = targetNode.Name };
                    workPlaceMasterList.Add(listItem);
                    cLstBxWorkSpaceType.Items.Add(listItem);
                    //cLstBxWorkSpaceType.Items.AddRange(workPlaceMasterList);
                }
            }).ConfigureAwait(false);

            await task;
        
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

        private void FilterItems()
        {

            cLstBxWorkSpaceType.Items.Clear();

            //var task = Task.Run(() => {
            // This filters and adds your filtered items to listView1
            foreach (ListViewItem item in workPlaceMasterList.Where(lvi => lvi.Text.ToLower().Contains(txtFilter.Text.ToLower().Trim())))
            {
                cLstBxWorkSpaceType.Items.Add(item);
            }
            // });

            // await task;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (txtFilter.Text != String.Empty) FilterItems();
        }

        private void txtFilter_MouseClick(object sender, MouseEventArgs e)
        {
            txtFilter.Text = String.Empty;
        }
    }
}
