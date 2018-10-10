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

                foreach (var file in fileArray)
                {
                    var newDocName = file.Split('\\').LastOrDefault().Split('.').FirstOrDefault();

                    if (targetNode.Name.Contains(newDocName))
                    {
                        //var mainNodeResult = serviceHelper.GetEntityNodeFromId(parentNodeId);

                        // var nodeResult = DbEntityHelper.GetNodeByName(parentNodeId, nodeName);
                        
                        var newFile = File.ReadAllBytes(file);

                        dict.Add(new Tuple<long,string>(targetNode.Id, newDocName),  newFile);
                    }
                    
                }
                
            }

            return dict;
        }

    }
}
