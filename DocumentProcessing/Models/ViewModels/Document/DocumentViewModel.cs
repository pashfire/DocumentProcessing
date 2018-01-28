using DocumentProcessing.Models.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentProcessing.Models.ViewModels.Document
{
    public class DocumentViewModel
    {
        public bool IsDocumentExists { get; set; }
        public DocumentModel DocumentModel { get; set; }
    }
}