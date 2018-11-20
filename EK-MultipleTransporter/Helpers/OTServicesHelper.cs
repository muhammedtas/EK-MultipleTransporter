using EK_MultipleTransporter.Enums;
using EK_MultipleTransporter.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EK_MultipleTransporter.Web_References.DmsDocumentManagementService;
using EK_MultipleTransporter.Models.HelperModel;

namespace EK_MultipleTransporter.Helpers
{
    public class OtServicesHelper
    {

        public static Logger Logger = LogManager.GetCurrentClassLogger();
       
        public EntityNode[] GetChildNodesById (long id)
        {
            return VariableHelper.Dmo.GetChildNodes(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User), VariableHelper.Token, id, 0, int.MaxValue, false, false);
        }

        public EntityNode[] GetChildNodesByIdTop1000Nodes(long id)
        {
            EntityNode[] data;
            try
            {
                data = VariableHelper.Dmo.GetChildNodes(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User), VariableHelper.Token, id, 0, 10, false, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Logger.Error(ex, Resources.WebServicesNotWorking);
                throw;
            }
            return data;
        }

        public KeyValuePair[] GetFolderListIncludingChildren (long id)
        {
            return VariableHelper.Dmo.GetFolderListIncludingChildren(Resources.Admin, VariableHelper.Token, id);
        }
        public EntityAttributeGroup GetEntityAttributeGroupOfCategory(long id)
        {

            var entityAttributeGroup = VariableHelper.Dmo.GetEntityAttributeGroupOfCategory(Resources.Admin, VariableHelper.Token, id);
            return entityAttributeGroup;
        }

        public EntityNode GetNodeByName (long parentNodeId, string nodeName)
        {
            var node = VariableHelper.Dmo.GetEntityNodeByName(Resources.Admin, VariableHelper.Token, parentNodeId, nodeName, false, false, false);
            return node;
        }

        public EntityNode GetEntityNodeFromId (long id)
        {
            return VariableHelper.Dmo.GetEntityNodeFromId(Resources.Admin, VariableHelper.Token, id, false, false, false);
        }

        public bool HasChildNode (long id)
        {
            return VariableHelper.Dmo.GetChildNodes(Resources.Admin, VariableHelper.Token, id, 0, int.MaxValue, false, false) != null;
        }

        public long FindChildNodeIdByName (long parentNodeId, string childNodeName)
        {
            return VariableHelper.Dmo.GetEntityNodeByName(Resources.Admin, VariableHelper.Token, parentNodeId, childNodeName, false, false, false).Id;
        }
        public bool AddDocumentOrVersion(string docName, byte[] fileByteArray, long targetFolder)
        {
            try
            {
                if (fileByteArray != null && targetFolder > 0)
                {
                    var eaj = new EntityAttachment
                    {
                        Contents = fileByteArray,
                        CreatedDate = DateTime.Now,
                        FileName = docName,
                        ModifiedDate = DateTime.Now,
                        FileSize = fileByteArray.Length
                    };

                    var idj = VariableHelper.Dmo.AddDocumentOrVersion(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User), VariableHelper.Token, eaj, targetFolder, "", true);
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

                var ida =  VariableHelper.Dmo.AddDocumentWithMetadata(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User), VariableHelper.Token, targetFolder, emd, eaj, "");
                if (ida == 0) StreamHelper.MoveUnUploadedDocumentsToBackUpFolder(docName);
                
                return ida > 0;
            }
            catch (Exception ex)
            {
                Logger.Trace(ex, "File named" + docName + "is not exist in this address");
                StreamHelper.MoveUnUploadedDocumentsToBackUpFolder(docName);
                return false;
            }
        }

        public async Task<bool> AddDocumentWithMetaDataAsync(long targetFolder, string docName, byte[] fileByteArray, EntityMetadata emd)
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

                await Task.Run(() => VariableHelper.Dmo.AddDocumentWithMetadataAsync(OtCredentialsEnum.ConvertString(OtCredentialsEnum.OtAdminCredentials.User), VariableHelper.Token, targetFolder, emd, eaj, ""));
                eaj = null;
                return true;
                
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
                var isFileExist = VariableHelper.Dmo.GetEntityNodeByName(Resources.Admin, VariableHelper.Token, folderId, folderName, true, true, true);
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
            return VariableHelper.Dmo.GetEntityNodeListIncludingChildrenUsingTypeFilter(Resources.Admin, VariableHelper.Token, parentNodeId, 1000, typeFilter, false);
        }

        public EntityMetadata CategoryMaker (GeneralCategoryModel categoryModel)
        {
            var eag = VariableHelper.Dmo.GetEntityAttributeGroupOfCategory(Resources.Admin, VariableHelper.Token, categoryModel.NodeId);

            var documentType = eag.Values.First(x => x.Description == OtCategoriesEnum.ConvertString(OtCategoriesEnum.GeneralCategory.DocType));
            documentType.Values = new object[] { categoryModel.DocumentType };

            var docYear = eag.Values.First(x => x.Description == OtCategoriesEnum.ConvertString(OtCategoriesEnum.GeneralCategory.Year));
            docYear.Values = new object[] { categoryModel.Year };

            var docTerm = eag.Values.First(x => x.Description == OtCategoriesEnum.ConvertString(OtCategoriesEnum.GeneralCategory.Quarter));
            docTerm.Values = new object[] { categoryModel.Term };

            var emdNew = new EntityMetadata {AttributeGroups = new[] {eag}};

            return emdNew;

        }

        public async Task<bool> UploadDocuments(Dictionary<Tuple<long, string>, byte[]> docsToUpload, GeneralCategoryModel categoryModel)
        {
            try
            {
                var emdNew = CategoryMaker(categoryModel);
                foreach (var item in docsToUpload)
                {
                    if (VariableHelper.Cts.IsCancellationRequested) return false;
                    await Task.Run(() => AddDocumentWithMetaData(item.Key.Item1, item.Key.Item2, item.Value, emdNew), VariableHelper.Cts.Token);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }   
        }
    }
}
