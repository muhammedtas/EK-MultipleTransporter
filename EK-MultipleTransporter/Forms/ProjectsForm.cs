using EK_MultipleTransporter.DmsAuthenticationService;
using EK_MultipleTransporter.DmsDocumentManagementService;
using EK_MultipleTransporter.Helpers;
using EK_MultipleTransporter.Model;
using EK_MultipleTransporter.Model.ChildModel;
using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
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
        public static long generalCategoryNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["generalCategoryNodeId"]);
        public OTServicesHelper serviceHelper = new OTServicesHelper();
        // public delegate Task StartProcess();


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

        private async void ProjectsForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadFormsDefault());
        }

        private void cmbProjects_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            //StartProcess startProcess = DoProjectWorks;
            //await startProcess();
            await Task.Run(() => DoProjectWorks());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFolderRoot_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFolderRoot.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }

        }

        public async void DoProjectWorks()
        {

            try
            {

                var mainChildRootNodeId = Convert.ToInt64((cmbChildRoot.SelectedItem as ProjectChilds).Id); // Şimdi Bu nodeId ye karşılık gelen 

                var mainChildRootElement = serviceHelper.GetEntityNodeFromId(mainChildRootNodeId);

                var targetNodesList = new List<EntityNode>(); // Bu boş liste doldurulup streamer helper methoduna verilecek.

                // Burada da Projeler içerisinde yüklenecek yerlerin nodeId listesini alacağız.
                // Ama ne yazık ki üst parent ten bir kaç kırınım içerideki child ları bulamıyoruz.
                var allChildNodesOfMainProject = serviceHelper.GetEntityNodeListIncludingChildrenUsingTypeFilter(projectsNodeId, (cmbChildRoot.SelectedItem as ProjectChilds).Name);

                // Bu yüzden tüm node ların içerisine girip, node derinliğini hesaplayarak iç nodelara ulaşacağız ve maplemek üzere node keyleri tek tek StreamHelper a göndereceğiz.
                // Streamer helper aldığı bu node id ile bir ilişki kurabilirse map in içerisine koyup bize verecek, kuramazsa eklemeyecek.
                foreach (var childNodeOfMainProject in allChildNodesOfMainProject)
                {
                    var targetRootAddres = (cmbChildRoot.SelectedItem as ProjectChilds).Name;
                    var countDeepness = targetRootAddres.Split('\\').Count();

                    if (countDeepness == 1)
                    {
                        var oneOfTargetNode = DbEntityHelper.GetNodeByName(childNodeOfMainProject.Id, targetRootAddres);
                        targetNodesList.Add(oneOfTargetNode);
                    }
                    else if (countDeepness == 2)
                    {
                        var generalFirstStepTargetNodeName = targetRootAddres.Split('\\')[0];
                        var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(childNodeOfMainProject.Id, generalFirstStepTargetNodeName).FirstOrDefault();

                        var generalSecondStepTargetNodeName = targetRootAddres.Split('\\')[1];

                        var trgtChldnd = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                        // var targetChildNode = firstStepTargetNode.Where(x => x.Name == generalFirstStepTargetNodeName).FirstOrDefault();

                        if (trgtChldnd != null)
                            targetNodesList.Add(trgtChldnd);


                        // Child kırınımı 2 ise 
                    }
                    else if (countDeepness == 3)
                    {
                        var generalFirstStepTargetNodeName = targetRootAddres.Split('\\')[0];
                        var firstStepTargetNode = DbEntityHelper.GetNodesByNameInExactParent(childNodeOfMainProject.Id, generalFirstStepTargetNodeName).FirstOrDefault();

                        var generalSecondStepTargetNodeName = targetRootAddres.Split('\\')[1];

                        var secondChldnd = DbEntityHelper.GetNodeByName(firstStepTargetNode.Id, generalSecondStepTargetNodeName);

                        var generalThirdStepTargetNodeName = targetRootAddres.Split('\\')[2];

                        var trgtChldnd = DbEntityHelper.GetNodeByName(secondChldnd.Id, generalThirdStepTargetNodeName);

                        // var targetChildNode = firstStepTargetNode.Where(x => x.Name == generalFirstStepTargetNodeName).FirstOrDefault();

                        if (trgtChldnd != null)
                            targetNodesList.Add(trgtChldnd);
                        // Child kırınımı 3 ise
                    }
                    else
                    {
                        Console.WriteLine("Dont leak the water into donkey's cunt!!");
                    }
                }

                // Bu mainChildRootElement => Document Templates içerisinde seçmiş olduğumuz child 
                // mainChildRootElement inin adı ile Emlak Konut iş alanları altındaki Projeler içerisinde ne kadar aynı isimde child element varsa Bunlardan bir dictionary yap.

                var mainNodeResult = serviceHelper.GetEntityNodeFromId(projectsNodeId); // ProjectsNodeId asıl dökümanların atılacağı yer.


                if (txtFolderRoot.Text == String.Empty || cmbChildRoot.SelectedIndex == -1)
                {
                    MessageBox.Show("Lütfen Yüklenecek klasörü ve Hedef dizini seçiniz.");
                    return;
                }
                // var docsToUpload = StreamHelper.MakePreparedDocumentListToPush(txtFolderRoot.Text, loadingPlaceForEachDocumentsList);
                // ** Değişti.
                var docsToUpload = StreamHelper.MakePreparedDocumentListToPush(txtFolderRoot.Text, targetNodesList);

                if (docsToUpload.Count < 1) return;
                // nodeId, dosya adı, ve hedef nodeId ile yarattığımız dictionary i opentext e yüklenebilir hale getireceğiz.

                await UploadDocuments(docsToUpload);

                // Hazır hale gelmiş olan  dictionary nin her bir elementine bir kategori bilgisi gir.
                //var eag = serviceHelper.GetEntityAttributeGroupOfCategory(generalCategoryNodeId);

                //var docType = eag.Values.First(x => x.Description == "Doküman Türü");
                //docType.Values = new object[] { cmbDocumentType.Text };

                //var year = eag.Values.First(x => x.Description == "Yıl");
                //year.Values = new object[] { dtpYear.Text };

                //var term = eag.Values.First(x => x.Description == "Çeyrek");
                //term.Values = new object[] { cmbTerm.Text };

                //var emdNew = new EntityMetadata();

                //emdNew.AttributeGroups = new[] { eag };

                //foreach (var item in docsToUpload)
                //{
                //   serviceHelper.AddDocumentWithMetaData(item.Key.Item1, item.Key.Item2, item.Value, emdNew);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ot categories update  error :  " + ex);
                throw;
            }

        }

        public async Task UploadDocuments(Dictionary<Tuple<long, string>, byte[]> docsToUpload)
        {
            var eag = serviceHelper.GetEntityAttributeGroupOfCategory(generalCategoryNodeId);

            var docType = eag.Values.First(x => x.Description == "Doküman Türü");
            docType.Values = new object[] { cmbDocumentType.Text };

            var year = eag.Values.First(x => x.Description == "Yıl");
            year.Values = new object[] { dtpYear.Text };

            var term = eag.Values.First(x => x.Description == "Çeyrek");
            term.Values = new object[] { cmbTerm.Text };

            var emdNew = new EntityMetadata();

            emdNew.AttributeGroups = new[] { eag };

            foreach (var item in docsToUpload)
            {
                await Task.Run(()=>serviceHelper.AddDocumentWithMetaData(item.Key.Item1, item.Key.Item2, item.Value, emdNew));
            }
        }

        public void LoadFormsDefault ()
        {
            var categoryItems = serviceHelper.GetEntityAttributeGroupOfCategory(generalCategoryNodeId);
            if (categoryItems != null)
            {
                var itemArray = categoryItems.Values[0].ValidValues;
                cmbDocumentType.Items.AddRange(itemArray);
            }

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
        }
    }
}
