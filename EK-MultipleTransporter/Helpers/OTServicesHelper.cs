﻿using EK_MultipleTransporter.DmsDocumentManagementService;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool AddDocumentOrVersionPrivate(string fileAddress, string alternativeFileAddress, long newFolder)
        {
            try
            {
                EntityAttachment eaj;
                if (File.Exists(fileAddress))
                {
                    var fileByteArray = File.ReadAllBytes(fileAddress);
                    eaj = new EntityAttachment
                    {
                        Contents = fileByteArray,
                        CreatedDate = DateTime.Now,
                        FileName = fileAddress.Split('\\').LastOrDefault(),
                        ModifiedDate = DateTime.Now,
                        FileSize = fileByteArray.Length
                    };


                    var idj = VariableHelper.Dmo.AddDocumentOrVersion("Admin", VariableHelper.Token, eaj, newFolder, "", true);
                    fileByteArray = null;
                    eaj = null;
                    return idj > 0;
                }
                if (File.Exists(alternativeFileAddress))
                {

                    var fileByteArray = File.ReadAllBytes(alternativeFileAddress);
                    eaj = new EntityAttachment
                    {
                        Contents = fileByteArray,
                        CreatedDate = DateTime.Now,
                        FileName = alternativeFileAddress.Split('\\').LastOrDefault(),
                        ModifiedDate = DateTime.Now,
                        FileSize = fileByteArray.Length
                    };

                    var idj = VariableHelper.Dmo.AddDocumentOrVersion("Admin", VariableHelper.Token, eaj, newFolder, "", true);
                    fileByteArray = null;
                    eaj = null;
                    return idj > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "An unexpected error has been occured while transferring this line     :" + alternativeFileAddress + "     :");
            }
            return false;

        }

        public bool AddDocumentWithMetaData(string fileAddress, string alternativeFileAddress, long folderId, EntityMetadata emd, string line, int loadCounter, string ekUnit)
        {
            EntityAttachment eaj = null;
            try
            {
                byte[] fileByteArray;

                if (File.Exists(fileAddress))
                {
                    fileByteArray = File.ReadAllBytes(fileAddress);
                    eaj = new EntityAttachment
                    {
                        Contents = fileByteArray,
                        CreatedDate = DateTime.Now,
                        FileName = fileAddress.Split('\\').LastOrDefault(),
                        ModifiedDate = DateTime.Now,
                        FileSize = fileByteArray.Length
                    };

                }

                if (File.Exists(alternativeFileAddress))
                {
                    fileByteArray = File.ReadAllBytes(fileAddress);
                    eaj = new EntityAttachment
                    {
                        Contents = fileByteArray,
                        CreatedDate = DateTime.Now,
                        FileName = fileAddress.Split('\\').LastOrDefault(),
                        ModifiedDate = DateTime.Now,
                        FileSize = fileByteArray.Length
                    };
                }
                var ida = VariableHelper.Dmo.AddDocumentWithMetadata("Admin", VariableHelper.Token, folderId, emd, eaj, "");

                fileByteArray = null;
                eaj = null;

                return ida > 0;
            }
            catch (Exception ex)
            {
                Logger.Trace(ex, "File is not exist in this address  :" + alternativeFileAddress);
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


    }
}
