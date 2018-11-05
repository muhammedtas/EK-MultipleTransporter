﻿using EK_MultipleTransporter.DmsDocumentManagementService;
using EK_MultipleTransporter.Model.ChildModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace EK_MultipleTransporter.Helpers
{
    public class StreamHelper
    {
        public static string BackUpFolderRoot = ConfigurationManager.AppSettings["BackUpFolderRoot"];
        public static string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static Logger Logger;
        public static string RootPathOfUsersFolder { get; set; }

        public StreamHelper()
        {
            if (!Directory.Exists(DesktopPath + @"\" + BackUpFolderRoot)) Directory.CreateDirectory(DesktopPath + @"\" + BackUpFolderRoot);

        }

        public static Dictionary<Tuple<long,string>, byte[]> MakePreparedDocumentListToPush(string rootFolderPath, List<EntityNode> nodeList)
        {

            try
            {
                if (nodeList == null) return null;

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

                        var newDocName = file.Split('\\').LastOrDefault()?.Split('.').FirstOrDefault()?.Split('-').FirstOrDefault();
                        var newDocNameWithTail = file.Split('\\').LastOrDefault();

                        if (newDocName != null && parentNode.Name.Contains(newDocName))
                        {
                            var newFile = File.ReadAllBytes(file);

                            dict.Add(new Tuple<long, string>(targetNode.Id, newDocNameWithTail), newFile);
                        }
                        else if (parentOfFirstParentNode.Id > 0)
                        {
                            if (newDocName == null || !parentOfFirstParentNode.Name.Contains(newDocName)) continue;
                            // 2. Kırınım için
                            var newFile = File.ReadAllBytes(file);

                            dict.Add(new Tuple<long, string>(targetNode.Id, newDocNameWithTail), newFile);

                        }
                        else if (parentOfSecondParentNode.Id > 0)
                        {
                            if (newDocName != null && !parentOfSecondParentNode.Name.Contains(newDocName)) continue;
                            var newFile = File.ReadAllBytes(file);

                            dict.Add(new Tuple<long, string>(targetNode.Id, newDocNameWithTail), newFile);
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

        public static Dictionary<Tuple<long, string>, byte[]> PrepareDocumentToSendMultipleTarget(List<EntityNode> nodeIdList, string rootFolderPath)
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

        public static bool MoveUnUploadedDocumentsToBackUpFolder(string docName)
        {
            //var docName = FileRoot.Split('\\').LastOrDefault();
            //File.Move(RootPathOfUsersFolder + docName , DesktopPath + @"\" + BackUpFolderRoot);

            File.Move(RootPathOfUsersFolder, DesktopPath + @"\" + BackUpFolderRoot);

            Logger.Info("Document named" + docName + "Could not be uploaded to Opentext");
            return true;
        }

    }
}
