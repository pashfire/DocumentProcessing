using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentProcessing.Infrastructure.Helpers
{
    public class FileHelper
    {
        public static string SaveDiplomaPdf(string rootPath, HttpPostedFileBase file, string userEmail)
        {
            string filePath = null;
            var folder = rootPath + @"App_Data\Documents\"
                + userEmail + @"\";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            filePath = folder + file.FileName;
            while (File.Exists(filePath))
            {
                folder += "new_";
                filePath = folder + file.FileName;
            }

            file.SaveAs(filePath);
            return filePath.Replace(rootPath, "");
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public static FilePathResult GetFilePathResult(string path)
        {
            var mimeType = MimeMapping.GetMimeMapping(path);

            return new FilePathResult(path, mimeType)
            {
                FileDownloadName = "document" + Path.GetExtension(path)
            };
        }
    }
}