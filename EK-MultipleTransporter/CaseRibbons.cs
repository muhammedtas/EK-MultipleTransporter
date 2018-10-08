using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EK_MultipleTransporter.Forms;
using Microsoft.Office.Tools.Ribbon;

namespace EK_MultipleTransporter
{
    public partial class CaseRibbons
    {
        private void CaseRibbons_Load(object sender, RibbonUIEventArgs e)
        {

        }

 
        private void rbtnProjectList_Click(object sender, RibbonControlEventArgs e)
        {
            ProjectsForm frm = new ProjectsForm();
            frm.ShowDialog();
        }

        private void rbtnPlotList_Click(object sender, RibbonControlEventArgs e)
        {
            PlotForm frm = new PlotForm();
            frm.ShowDialog();
        }

        private void rbtnDistrictLst_Click(object sender, RibbonControlEventArgs e)
        {
            IndependentSectionForm frm = new IndependentSectionForm();
            frm.ShowDialog();
        }

        private void rbtnLitigationList_Click(object sender, RibbonControlEventArgs e)
        {
            LitigationForm frm = new LitigationForm();
            frm.ShowDialog();
        }

        private void rbtnPersonelList_Click(object sender, RibbonControlEventArgs e)
        {
            StaffForm frm = new StaffForm();
            frm.ShowDialog();
        }
    }
}
