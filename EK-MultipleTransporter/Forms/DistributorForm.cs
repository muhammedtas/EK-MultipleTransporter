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

namespace EK_MultipleTransporter.Forms
{
    public partial class DistributorForm : Form
    {

        public static Logger Logger = LogManager.GetCurrentClassLogger();
        public static long projectsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["projectsNodeId"]);
        public static long projectsChildElementsNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["projectsChildElementsNodeId"]);
        public static long generalCategoryNodeId = Convert.ToInt64(ConfigurationManager.AppSettings["generalCategoryNodeId"]);
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

        public void LoadFormsDefault()
        {
            var categoryItems = serviceHelper.GetEntityAttributeGroupOfCategory(generalCategoryNodeId);
            if (categoryItems != null)
            {
                var itemArray = categoryItems.Values[0].ValidValues;
                cmbDocumentType.Items.AddRange(itemArray);
            }

            var childNodes = serviceHelper.GetChildNodesById(projectsChildElementsNodeId);

            //foreach (var childNode in childNodes)
            //{
            //    cmbChildRoot.Items.Add(new ProjectChilds()
            //    {
            //        Id = childNode.Id,
            //        Name = childNode.Name
            //    });

            //    if (serviceHelper.HasChildNode(childNode.Id))
            //    {
            //        var innerChilds = serviceHelper.GetChildNodesById(childNode.Id);

            //        foreach (var innerChild in innerChilds)
            //        {
            //            cmbChildRoot.Items.Add(new ProjectChilds()
            //            {
            //                Id = innerChild.Id,
            //                Name = childNode.Name + "\\" + innerChild.Name
            //            });

            //            if (serviceHelper.HasChildNode(innerChild.Id))
            //            {
            //                var innersOfInnerChild = serviceHelper.GetChildNodesById(innerChild.Id);
            //                foreach (var innerOfInnerChild in innersOfInnerChild)
            //                {
            //                    cmbChildRoot.Items.Add(new ProjectChilds()
            //                    {
            //                        Id = innerOfInnerChild.Id,
            //                        Name = childNode.Name + "\\" + innerChild.Name + "\\" + innerOfInnerChild.Name
            //                    });

            //                }

            //            }

            //        }

            //    }
            //}
        }
    }
}
