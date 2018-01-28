using DocumentProcessing.Infrastructure.Helpers;
using DocumentProcessing.Models.Document;
using DocumentProcessing.Models.ViewModels.Document;
using DocumentProcessing.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DocumentProcessing.Controllers
{

    public class DocumentController : ApplicationController
    {
        private DocumentService documentService;

        public DocumentController()
        {
            documentService = new DocumentService(DataContext);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index(int id = 0)
        {
            DocumentViewModel model = new DocumentViewModel()
            {
                DocumentModel = documentService.GetDocument(id, this.CurrentUser.Id),
                IsDocumentExists = true
            };
            if (model.DocumentModel == null)
            {
                model.IsDocumentExists = false;
                model.DocumentModel = documentService.CreateDocumentModel(this.CurrentUser);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(DocumentModel model)
        {
            var file = Request.Files["DocumentFile"];
            if (ModelState.IsValid && file != null)
            {
                var path = Server.MapPath("~");
                model.Path = FileHelper.SaveDiplomaPdf(path, file, this.CurrentUser.AspNetUser.Email);
                if(model.Id == 0)
                {
                    documentService.CreateDocument(model);
                }
                else
                {
                    documentService.UpdateDocument(model);
                }
           

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Authorize]
        [HttpGet]
        public ActionResult DownloadDocument(int id)
        {
            var document = documentService.GetDocument(id);
            if (document == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return FileHelper.GetFilePathResult(Server.MapPath("~") + document.Path);
        }
    }
}