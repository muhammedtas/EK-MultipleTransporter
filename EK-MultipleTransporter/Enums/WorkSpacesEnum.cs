using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EK_MultipleTransporter.Enums
{
    public class WorkSpacesEnum
    {

        public enum WorkSpaces
        {
            IndependentSectionNodeId,
            IndependentSectionChildElementsNodeId,
            LitigationNodeId,
            LitigationChildElementsNodeId,
            StaffNodeId,
            StaffChildElementsNodeId,
            PlotsNodeId,
            PlotsChildElementsNodeId,
            ProjectsNodeId,
            ProjectsChildElementsNodeId,
            ContentServerDocumentTemplatesNodeId,
            GeneralCategoryNodeId,
            WorkSpacesNodeId,
            BackUpFolderRoot
        }

        public static long GetValue(WorkSpaces me)
        {
            switch (me)
            {
                case WorkSpaces.IndependentSectionNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["independentSectionNodeId"]);
                case WorkSpaces.IndependentSectionChildElementsNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["independentSectionChildElementsNodeId"]);
                case WorkSpaces.LitigationNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["litigationNodeId"]);
                case WorkSpaces.LitigationChildElementsNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["litigationChildElementsNodeId"]);
                case WorkSpaces.StaffNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["staffNodeId"]);
                case WorkSpaces.StaffChildElementsNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["staffChildElementsNodeId"]);
                case WorkSpaces.PlotsNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["plotsNodeId"]);
                case WorkSpaces.PlotsChildElementsNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["plotsChildElementsNodeId"]);
                case WorkSpaces.ProjectsNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["projectsNodeId"]);
                case WorkSpaces.ProjectsChildElementsNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["projectsChildElementsNodeId"]);
                case WorkSpaces.ContentServerDocumentTemplatesNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["contentServerDocumentTemplatesNodeId"]);
                case WorkSpaces.GeneralCategoryNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["generalCategoryNodeId"]);
                case WorkSpaces.WorkSpacesNodeId:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["workSpacesNodeId"]);
                case WorkSpaces.BackUpFolderRoot:
                    return Convert.ToInt64(ConfigurationManager.AppSettings["BackUpFolderRoot"]);

                default:
                    return -1;
            }
        }
    }
}
