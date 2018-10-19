﻿using EK_MultipleTransporter.DmsDocumentManagementService;
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
    public partial class DistributorForm : Form
    {

        public static Logger Logger = LogManager.GetCurrentClassLogger();
        public static long projectsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["projectsNodeId"]);
        public static long projectsChildElementsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["projectsChildElementsNodeId"]);
        public static long generalCategoryNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["generalCategoryNodeId"]);
        public static long workSpacesNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["workSpacesNodeId"]); 
        public static long contentServerDocumentTemplatesNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["contentServerDocumentTemplatesNodeId"]);
        public OTServicesHelper serviceHelper = new OTServicesHelper();
        public DistributorForm()
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

        private async void DistributorForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadFormsDefault());
        }
        private void cmbDistWorkPlaceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //await Task.Run(() => LoadSelectedWorkSpacesChilds());
            //await Task.Run(() => LoadSelectedWorkSpacesTargetChildsToListBox());
            Parallel.Invoke(() => LoadSelectedWorkSpacesChilds(), () => LoadSelectedWorkSpacesTargetChildsToListBox());
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

            cmbDistOTFolder.Items.Clear();

            Task worker = Task.Run(() => {

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


            } );

            await worker;

            //Task.WaitAny(worker);

        }

        public async void LoadSelectedWorkSpacesTargetChildsToListBox()
        {  // cmbDistWorkPlaceType

            cLstBxWorkSpaceType.Items.Clear();

            Task worker = Task.Run(() => {
                var workSpaceTargetNodes = serviceHelper.GetChildNodesById((cmbDistWorkPlaceType.SelectedItem as DistributorChilds).Id);

                foreach (var targetNode in workSpaceTargetNodes)
                {
                    var listItem = new ListViewItem();

                    listItem.Text = targetNode.Name;
                    listItem.Tag = new DistributorChilds() { Id = targetNode.Id, Name = targetNode.Name };

                    cLstBxWorkSpaceType.Items.Add(listItem);
                }
            });

            await worker;

            //Task.WaitAny(worker);
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
            // 
            // await Task.Run(() => { });

            var selectedNodeIdList = new List<long>();
            var selectedItemList = cLstBxWorkSpaceType.SelectedItems;
            // SelectedItemList içerisinde EmlakKonut İş Alanı Türünü seçtiğimiz en alt taki checkedListBox taki node ların id sini tutar.
            // Şimdi biz bu node ların içerisinde dönerek 2. Combobox olan "Klasör Seçimi" Node larını bulacağız, Ki bunlar TARGET Node ID lerimiz olacak.

            var selectedNodList = new List<DistributorChilds>();

            foreach (var item in selectedItemList)
            {
                //DataRowView castedItem = item as DataRowView;
                //var id = Convert.ToInt64(castedItem["Id"]);
                // var name = castedItem["Name"].ToString();
                selectedNodList.Add((DistributorChilds)item);

                selectedNodeIdList.Add(((DistributorChilds)item).Id);
            }

            var preparedList = StreamHelper.PrepareDocumentToSendMultipleTarger(selectedNodeIdList, txtDistDocumentRoot.Text);

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
    }
}