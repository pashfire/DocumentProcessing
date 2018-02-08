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
        [Authorize(Roles= "Admin, Clerk")]
        public ActionResult Index(int id = 0)
        {
            DocumentViewModel model = new DocumentViewModel()
            {
                DocumentModel = documentService.GetDocument(id, this.CurrentUser.Id),
                //DocumentModel = documentService.GetDocument(id),
                IsDocumentExists = true
            };
            if (model.DocumentModel == null)
            {
                model.IsDocumentExists = false;
                model.DocumentModel = documentService.CreateDocumentModel(this.CurrentUser);
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Clerk")]
        public ActionResult All()
        {
            if (CurrentUser.Role == "Clerk")
            {
                DocumentViewModel model = new DocumentViewModel()
                {
                    AllDocumentsModel = documentService.GetAllDocuments(CurrentUser.Id),
                    //DocumentModel = documentService.GetDocument(id),
                    IsDocumentExists = true
                };
                if (model.AllDocumentsModel == null)
                {
                    model.IsDocumentExists = false;
                    model.DocumentModel = documentService.CreateDocumentModel(this.CurrentUser);
                }

                return View(model);
            } else if (CurrentUser.Role == "Admin")
            {
                DocumentViewModel model = new DocumentViewModel()
                {

                    AllDocumentsModel = documentService.GetAllDocuments(),
                    //DocumentModel = documentService.GetDocument(id),
                    IsDocumentExists = true
                };
                if (model.AllDocumentsModel == null)
                {
                    model.IsDocumentExists = false;
                    model.DocumentModel = documentService.CreateDocumentModel(this.CurrentUser);
                }

                return View(model);
            }
            return null;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Clerk")]
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

        //for managers
        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Resolutions()
        {
            if (CurrentUser.Role == "Manager")
            {
                DocumentViewModel model = new DocumentViewModel()
                {
                    AllDocumentsModel = documentService.GetDocumentsByManager(CurrentUser.Id),
                    //DocumentModel = documentService.GetDocument(id),
                    IsDocumentExists = true
                };

                return View(model);
            }
            else if (CurrentUser.Role == "Admin")
            {
                DocumentViewModel model = new DocumentViewModel()
                {

                    AllDocumentsModel = documentService.GetAllDocuments(),
                    //DocumentModel = documentService.GetDocument(id),
                    IsDocumentExists = true
                };
                if (model.AllDocumentsModel == null)
                {
                    model.IsDocumentExists = false;
                    model.DocumentModel = documentService.CreateDocumentModel(this.CurrentUser);
                }

                return View(model);
            }
            return null;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Resolution(int id = 0)
        {
            DocumentViewModel model = new DocumentViewModel()
            {
                DocumentModel = documentService.GetDocument(id, this.CurrentUser.Id),
                //DocumentModel = documentService.GetDocument(id),
                IsDocumentExists = true
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Resolution(DocumentModel model)
        {
            if (ModelState.IsValid)
            {
                
                if (model.Id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    documentService.UpdateDocument(model);
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //for executors
        [HttpGet]
        [Authorize(Roles = "Admin, Executor")]
        public ActionResult Tasks()
        {
            if (CurrentUser.Role == "Executor")
            {
                DocumentViewModel model = new DocumentViewModel()
                {
                    AllDocumentsModel = documentService.GetDocumentsByExecutor(CurrentUser.Id),
                    IsDocumentExists = true
                };

                return View(model);
            }
            else if (CurrentUser.Role == "Admin")
            {
                DocumentViewModel model = new DocumentViewModel()
                {

                    AllDocumentsModel = documentService.GetAllDocuments(),
                    //DocumentModel = documentService.GetDocument(id),
                    IsDocumentExists = true
                };
                if (model.AllDocumentsModel == null)
                {
                    model.IsDocumentExists = false;
                    model.DocumentModel = documentService.CreateDocumentModel(this.CurrentUser);
                }

                return View(model);
            }
            return null;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Executor")]
        public ActionResult Task(DocumentModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.Id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    documentService.UpdateDocument(model);
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Executor")]
        public ActionResult Task(int id = 0)
        {
            DocumentViewModel model = new DocumentViewModel()
            {
                DocumentModel = documentService.GetDocument(id, this.CurrentUser.Id),
                IsDocumentExists = true
            };

            return View(model);
        }

        //for inspectors
        [HttpGet]
        [Authorize(Roles = "Admin, Inspector")]
        public ActionResult Inspections()
        {
            if (CurrentUser.Role == "Inspector")
            {
                DocumentViewModel model = new DocumentViewModel()
                {
                    AllDocumentsModel = documentService.GetDocumentsByController(CurrentUser.Id),
                    IsDocumentExists = true
                };

                return View(model);
            }
            else if (CurrentUser.Role == "Admin")
            {
                DocumentViewModel model = new DocumentViewModel()
                {

                    AllDocumentsModel = documentService.GetAllDocuments(),
                    //DocumentModel = documentService.GetDocument(id),
                    IsDocumentExists = true
                };
                if (model.AllDocumentsModel == null)
                {
                    model.IsDocumentExists = false;
                    model.DocumentModel = documentService.CreateDocumentModel(this.CurrentUser);
                }

                return View(model);
            }
            return null;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Inspector")]
        public ActionResult Inspect(int id = 0)
        {
            DocumentViewModel model = new DocumentViewModel()
            {
                DocumentModel = documentService.GetDocument(id, this.CurrentUser.Id),
                IsDocumentExists = true
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Inspector")]
        public ActionResult Inspect(DocumentModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.Id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    documentService.UpdateDocument(model);
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}