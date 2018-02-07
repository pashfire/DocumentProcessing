using DocumentProcessing.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DocumentProcessing.Models.Document
{
    public class DocumentsModel
    {
        public int Id { get; set; }
        public DocumentModel Document { get; set; }
        public List<DocumentModel> AllDocuments { get; set; }
        
        public static DocumentsModel Map(List<DocumentModel> allDocs)
        {
            if (allDocs.Count > 0)
            {
                return new DocumentsModel()
                {
                    AllDocuments = allDocs,
                    Id = allDocs.First().Id,
                    Document = allDocs.First()
                };
            }
            else return new DocumentsModel();

        }
    }
}