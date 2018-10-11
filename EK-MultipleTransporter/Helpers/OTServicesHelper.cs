using EK_MultipleTransporter.DmsDocumentManagementService;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EK_MultipleTransporter.Helpers
{
    public class OTServicesHelper
    {

        public static Logger Logger = LogManager.GetCurrentClassLogger();
        private static string _desktopPath;

        public OTServicesHelper()
        {
            _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        public EntityNode[] GetChildNodesById (long id)
        {
           return VariableHelper.Dmo.GetChildNodes("admin", VariableHelper.Token, id, 0, 1000, false, false);

        }

        public KeyValuePair[] GetFolderListIncludingChildren (long id)
        {
            return VariableHelper.Dmo.GetFolderListIncludingChildren("admin", VariableHelper.Token, id);
        }
        public EntityAttributeGroup GetEntityAttributeGroupOfCategory(long id)
        {
            var entityAttributeGroup = VariableHelper.Dmo.GetEntityAttributeGroupOfCategory("Admin", VariableHelper.Token, id);
            return entityAttributeGroup;
        }

        public EntityNode GetNodeByName (long parentNodeId, string nodeName)
        {
            var node = VariableHelper.Dmo.GetEntityNodeByName("admin", VariableHelper.Token, parentNodeId, nodeName, false, false, false);
            return node;
        }

        public EntityNode GetEntityNodeFromId (long id)
        {
            return VariableHelper.Dmo.GetEntityNodeFromId("admin", VariableHelper.Token, id, false, false, false);
        }

        public bool HasChildNode (long id)
        {
            return VariableHelper.Dmo.GetChildNodes("admin", VariableHelper.Token, id, 0, 1000, false, false) == null ? false : true;
        }

        public long FindChildNodeIdByName (long parentNodeId, string childNodeName)
        {
            return VariableHelper.Dmo.GetEntityNodeByName("admin", VariableHelper.Token, parentNodeId, childNodeName, false, false, false).Id;
        }
        public bool AddDocumentOrVersion(string docName, byte[] fileByteArray, long targetFolder)
        {
            try
            {
                EntityAttachment eaj;
                if (fileByteArray != null && targetFolder > 0)
                {
                    eaj = new EntityAttachment
                    {
                        Contents = fileByteArray,
                        CreatedDate = DateTime.Now,
                        FileName = docName,
                        ModifiedDate = DateTime.Now,
                        FileSize = fileByteArray.Length
                    };


                    var idj = VariableHelper.Dmo.AddDocumentOrVersion("Admin", VariableHelper.Token, eaj, targetFolder, "", true);
                    fileByteArray = null;
                    eaj = null;
                    return idj > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "File named " + docName + "   could not be found.");
            }
            return false;

        }

        public bool AddDocumentWithMetaData(long targetFolder,  string docName, byte[] fileByteArray,EntityMetadata emd)
        {
            EntityAttachment eaj = null;
            try
            {
                if (fileByteArray != null && targetFolder > 0)
                {
                    eaj = new EntityAttachment
                    {
                        Contents = fileByteArray,
                        CreatedDate = DateTime.Now,
                        FileName = docName,
                        ModifiedDate = DateTime.Now,
                        FileSize = fileByteArray.Length
                    };

                }

                var ida = VariableHelper.Dmo.AddDocumentWithMetadata("Admin", VariableHelper.Token, targetFolder, emd, eaj, "");

                fileByteArray = null;
                eaj = null;

                return ida > 0;
            }
            catch (Exception ex)
            {
                Logger.Trace(ex, "File named" + docName + "is not exist in this address");
                return false;
            }

        }

        public bool IsFileExist(long folderId, string folderName)
        {
            try
            {
                var isFileExist = VariableHelper.Dmo.GetEntityNodeByName("Admin", VariableHelper.Token, folderId, folderName, true, true, true);
                return isFileExist != null;
            }
            catch (Exception ex)
            {
                Logger.Trace(ex, "An Error has occured when trying to find if File exist in Opentext. Details Are    :" + ex.Message + "Inner exception  :" + ex.InnerException?.Message);
                return false;
            }
        }

        public EntityNode[] GetEntityNodeListIncludingChildrenUsingTypeFilter(long parentNodeId, string typeFilter)
        {
            return VariableHelper.Dmo.GetEntityNodeListIncludingChildrenUsingTypeFilter("admin", VariableHelper.Token, parentNodeId, 1000, typeFilter, false);
        }

    }
}
