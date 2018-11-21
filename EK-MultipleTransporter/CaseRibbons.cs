using System.Threading.Tasks;
using EK_MultipleTransporter.Forms;
using Microsoft.Office.Tools.Ribbon;

namespace EK_MultipleTransporter
{
    public partial class CaseRibbons
    {
        private void CaseRibbons_Load(object sender, RibbonUIEventArgs e)
        {

        }

 
        private async void rbtnProjectList_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new ProjectsForm();
            await Task.Run(() => frm.ShowDialog());
        }

        private async void rbtnPlotList_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new PlotForm();
            await Task.Run(() => frm.ShowDialog());
        }

        private async void rbtnDistrictLst_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new IndependentSectionForm();
            await Task.Run(() => frm.ShowDialog());
        }

        private async void rbtnLitigationList_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new LitigationForm();
            await Task.Run(() => frm.ShowDialog());
        }

        private async void rbtnPersonelList_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new StaffForm();
            await Task.Run(() => frm.ShowDialog());
        }

        private async void btnDistributor_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new DistributorForm();
            await Task.Run(() => frm.ShowDialog());
        }
    }
}
