using EK_MultipleTransporter.DmsDocumentManagementService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EK_MultipleTransporter.Helpers
{
    public class StreamHelper
    {
        public static Dictionary<Tuple<long,string>, byte[]> MakePreparedDocumentListToPush(string rootFolderPath, List<EntityNode> nodeList)
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
                    
                    var newDocName = file.Split('\\').LastOrDefault().Split('.').FirstOrDefault();
                    
                    if (parentNode.Name.Contains(newDocName))
                    {                        
                        var newFile = File.ReadAllBytes(file);

                        dict.Add(new Tuple<long,string>(targetNode.Id, newDocName),  newFile);
                    }
                    else if (parentOfFirstParentNode.Name.Contains(newDocName))
                    {
                        // 2. Kırınım için
                        var newFile = File.ReadAllBytes(file);

                        dict.Add(new Tuple<long, string>(targetNode.Id, newDocName), newFile);
                    }
                    else if (parentOfSecondParentNode.Name.Contains(newDocName))
                    {
                        // 3.Kırınım için
                        var newFile = File.ReadAllBytes(file);

                        dict.Add(new Tuple<long, string>(targetNode.Id, newDocName), newFile);
                    }
                    else
                    {
                        Console.Write("fuck your nodes. Write all them into your services.");
                    }
                    
                }
                
            }

            return dict;
        }

        public static Dictionary<Tuple<long, string>, byte[]> MakePreparedDocumentListToPushForSecondLayer(string rootFolderPath, List<EntityNode> nodeList)
        {
            if (nodeList == null) return null;

            var docList = new List<byte[]>();
            var dict = new Dictionary<Tuple<long, string>, byte[]>();

            var fileArray = Directory.GetFiles(rootFolderPath);

            foreach (var targetNode in nodeList)
            {
                if (targetNode == null) continue;
                var parentNode = VariableHelper.Dmo.GetEntityNodeFromId("admin", VariableHelper.Token, targetNode.ParentId, false, false, false);

                foreach (var file in fileArray)
                {
                    var newDocName = file.Split('\\').LastOrDefault().Split('.').FirstOrDefault();

                    if (parentNode.Name.Contains(newDocName))
                    {
                        var newFile = File.ReadAllBytes(file);

                        dict.Add(new Tuple<long, string>(targetNode.Id, newDocName), newFile);
                    }

                }

            }

            return dict;
        }

    }
}
