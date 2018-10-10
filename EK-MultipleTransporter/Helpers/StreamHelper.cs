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
        public static Dictionary<long?,byte[]> ReadAllDocumentsAsByte(string rootFolderPath, OTServicesHelper serviceHelper, long parentNodeId)
        {
            var docList = new List<byte[]>();
            var dict = new Dictionary<long?, byte[]>();

            var fileArray = Directory.GetFiles(rootFolderPath);

            foreach (var item in fileArray)
            {
                var nodeName = item.Split('\\').LastOrDefault().Split('.').FirstOrDefault();
                var nodeResult = DbEntityHelper.GetNodeByName(parentNodeId, nodeName);

                if (nodeResult == null) continue;

                var newFile = File.ReadAllBytes(item);

                dict.Add(nodeResult?.Id, newFile);

            }
            return dict;
        }

    }
}
