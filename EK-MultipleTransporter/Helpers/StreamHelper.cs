using EK_MultipleTransporter.DmsDocumentManagementService;
using EK_MultipleTransporter.Model.ChildModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EK_MultipleTransporter.Helpers
{
    public class StreamHelper
    {
        public static Dictionary<Tuple<long,string>, byte[]> MakePreparedDocumentListToPush(string rootFolderPath, List<EntityNode> nodeList)
        {

            try
            {
                if (nodeList == null) return null;

                var docList = new List<byte[]>();
                var dict = new Dictionary<Tuple<long, string>, byte[]>();

                var fileArray = Directory.GetFiles(rootFolderPath);

                foreach (var targetNode in nodeList)
                {
                    if (targetNode == null) continue;
                    var parentNode = VariableHelper.Dmo.GetEntityNodeFromId("admin", VariableHelper.Token, targetNode.ParentId, false, false, false);
                    var parentOfFirstParentNode = VariableHelper.Dmo.GetEntityNodeFromId("admin", VariableHelper.Token, parentNode.ParentId, false, false, false);
                    var parentOfSecondParentNode = VariableHelper.Dmo.GetEntityNodeFromId("admin", VariableHelper.Token, parentOfFirstParentNode.ParentId, false, false, false);
                    foreach (var file in fileArray)
                    {
                        if (dict.Count == fileArray.Count()) return dict;

                        var newDocName = file.Split('\\').LastOrDefault().Split('.').FirstOrDefault().Split('-').FirstOrDefault();
                        var newDocNameWithTail = file.Split('\\').LastOrDefault();

                        if (parentNode.Name.Contains(newDocName))
                        {
                            var newFile = File.ReadAllBytes(file);

                            dict.Add(new Tuple<long, string>(targetNode.Id, newDocNameWithTail), newFile);
                        }
                        else if (parentOfFirstParentNode != null)
                        {
                            if (parentOfFirstParentNode.Name.Contains(newDocName))
                            {
                                // 2. Kırınım için
                                var newFile = File.ReadAllBytes(file);

                                dict.Add(new Tuple<long, string>(targetNode.Id, newDocNameWithTail), newFile);
                            }
                           
                        }
                        else if (parentOfSecondParentNode != null)
                        {
                            if (parentOfSecondParentNode.Name.Contains(newDocName))
                            {
                                var newFile = File.ReadAllBytes(file);

                                dict.Add(new Tuple<long, string>(targetNode.Id, newDocNameWithTail), newFile);
                            }
                            // 3.Kırınım için
                           
                        }
                        else
                        {
                            Console.Write("fuck your nodes. Write all them into your services.");
                        }

                    }

                }

                return dict;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static Dictionary<Tuple<long, string>, byte[]> PrepareDocumentToSendMultipleTarger(List<EntityNode> nodeIdList, string rootFolderPath)
        {
            var dict = new Dictionary<Tuple<long, string>, byte[]>();

            var docName = rootFolderPath.Split('\\').LastOrDefault();

            var documentStream = File.ReadAllBytes(rootFolderPath);

            foreach (var item in nodeIdList)
            {
                dict.Add(new Tuple<long, string>(item.Id, docName), documentStream);
            }
            return dict;
        }

    }
}
