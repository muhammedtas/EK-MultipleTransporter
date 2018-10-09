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
        public static Dictionary<long,byte[]> ReadAllDocumentsAsByte(string rootFolderPath, OTServicesHelper serviceHelper, string token, long parentNodeId)
        {
            var docList = new List<byte[]>();
            var dict = new Dictionary<long, byte[]>();

            var fileArray = Directory.GetFiles(rootFolderPath);

            foreach (var item in fileArray)
            {
                // var nodeName = item.Split('\\').LastOrDefault();
                // C:\\Users\\Taş\\Desktop\\TestFolder\\1000.pdf
                // var nodeName = item.Split('\\').LastOrDefault().Split('.').FirstOrDefault();
                var nodeName = "1000 / TEST";
                long parentNode = 59055;
                var nodeResult = serviceHelper.GetNodeByName(token, parentNode, nodeName);
                var newFile = File.ReadAllBytes(item);

                dict.Add(nodeResult.Id, newFile);

            }
            return dict;
        }

    }
}
