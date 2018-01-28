using DocumentProcessing.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentProcessing.Models.Document
{
    public class DocumentTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static DocumentTypeModel Map(DocumentType type)
        {
            return new DocumentTypeModel()
            {
                Id = type.Id,
                Name = type.Name
            };
        }
    }
}