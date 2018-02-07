using DocumentProcessing.DAL;
using DocumentProcessing.Infrastructure.Helpers;
using DocumentProcessing.Models;
using DocumentProcessing.Models.Document;
using DocumentProcessing.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentProcessing.Service
{
    public class DocumentService
    {
        private GenericRepository<Document> _documentsRepo;
        private GenericRepository<DocumentType> _documentTypesRepo;
        private GenericRepository<User> _userRepo;
        private GenericRepository<AffairsNomenclature> _nomenclatureRepo;

        public DocumentService(DocumentProcessingEntities dataContext)
        {
            _documentsRepo = new GenericRepository<Document>(dataContext);
            _documentTypesRepo = new GenericRepository<DocumentType>(dataContext);
            _userRepo = new GenericRepository<User>(dataContext);
            _nomenclatureRepo = new GenericRepository<AffairsNomenclature>(dataContext);
        }

        public DocumentModel GetDocument(int id)
        {
            Document document = _documentsRepo.GetById(id);
            return document == null ? null : DocumentModel.Map(document);
        }

        public DocumentModel GetDocument(int id, int userId)
        {
          
            var document = _documentsRepo.GetById(id);

            var allTypes = _documentTypesRepo.Get()
                    .Select(DocumentTypeModel.Map).ToList();

            var allNomenclature = _nomenclatureRepo.Get()
                   .Select(NomenclatureModel.Map).ToList();

            var possibleManagers = _userRepo.Get(u => u.Role == "Manager")
                        ?.Select(UserModel.Map).ToList();

            var possibleExecutors = _userRepo.Get(u => u.Role == "Executor")
                        ?.Select(UserModel.Map).ToList();

            var possibleControllers = _userRepo.Get(u => u.Role == "Inspector")
                        ?.Select(UserModel.Map).ToList();
       
           
            return document == null ? null : DocumentModel
                    .Map(document, allTypes, allNomenclature, possibleManagers, possibleExecutors, possibleControllers);
        }
        public DocumentsModel GetAllDocuments(int userId)
        {

            return DocumentsModel.Map(_documentsRepo.Get(doc => doc.CreatorId == userId)
                ?.Select(doc => DocumentModel.Map(doc)).ToList());
        }

        public DocumentsModel GetAllDocuments()
        {
            return DocumentsModel.Map(_documentsRepo.Get(doc => doc.CreatorId >= 0)
                ?.Select(doc => DocumentModel.Map(doc)).ToList());
        }

        public List<DocumentModel> GetDocumentsByCreator(int userId)
        {
            return _documentsRepo.Get(doc => doc.CreatorId == userId)
                ?.Select(doc => DocumentModel.Map(doc)).ToList();
        }

        public DocumentsModel GetDocumentsByManager(int userId)
        {
            return DocumentsModel.Map(_documentsRepo.Get(doc => doc.ManagerId == userId)
                ?.Select(doc => DocumentModel.Map(doc)).ToList());
        }

        public List<DocumentModel> GetDocumentsByExecutor(int userId)
        {
            return _documentsRepo.Get(doc => doc.ExecutorId == userId)
                ?.Select(doc => DocumentModel.Map(doc)).ToList();
        }

        public List<DocumentModel> GetDocumentsByController(int userId)
        {
            return _documentsRepo.Get(doc => doc.ControllerId == userId)
                ?.Select(doc => DocumentModel.Map(doc)).ToList();
        }

        public void UpdateDocument(DocumentModel model)
        {
            Document document = _documentsRepo.GetById(model.Id);
            
            FileHelper.DeleteFile(document.Path);

            document.Name = model.Name;
            document.Path = model.Path;
            document.TypeId = model.Type;
            document.DocHeader = model.DocHeader;
            
            document.ManagerId = model.ManagerId;
            document.Resolution = model.Resolution;
            document.ExecutorId = model.ExecutorId;
            document.ExecutorNote = model.ExecutorNote;
            document.ControllerId = model.ControllerId;
            document.ControllerNote = model.ControllerNote;

            _documentsRepo.Update(document);
            _documentsRepo.Save();
        }

        public void DeleteDocument(int id, string serverPath)
        {
            Document document = _documentsRepo.GetById(id);
            var path = serverPath + document.Path;
            _documentsRepo.Delete(document);
            FileHelper.DeleteFile(path);
        } 

        public DocumentModel CreateDocument(DocumentModel model)
        {
            Document document = new Document()
            {

                Name = model.Name,
                Path = model.Path,
                CreatorId = model.CreatorId,
                CreationDate = model.Created,
                TypeId = model.Type,
                ExecutionPeriod = model.ExecutionPeriod,
                RegistrationDate = DateTime.Now.ToShortDateString(),
                NomenclatureId = model.NomenclatureId,
                
                DocIndex = model.DocIndex,
                DocHeader = model.DocHeader,
                ManagerId = model.ManagerId,
                
                Resolution = model.Resolution,
                ExecutorId = model.ExecutorId,
                ExecutorNote = model.ExecutorNote,
                ControllerId = model.ControllerId,
                ControllerNote = model.ControllerNote
            };

            _documentsRepo.Insert(ref document);
            model.Id = document.Id;

            var nomenclature = _nomenclatureRepo.GetById(model.NomenclatureId);
            document.DocIndex = nomenclature.IndexOfCase + "/" + model.Id +
                          " от " + model.Created;

            _documentsRepo.Update(document);
            _documentsRepo.Save();

            return model;
        }

        public DocumentModel CreateDocumentModel(User user)
        {
            var model = new DocumentModel()
            {
                CreatorId = user.Id,
                Creator = UserModel.Map(user),
                AllNomenclature = _nomenclatureRepo.Get()
                   .Select(NomenclatureModel.Map).ToList(),
                AllTypes = _documentTypesRepo.Get()
                    .Select(DocumentTypeModel.Map).ToList(),
                Managers = _userRepo.Get(u => u.Role == "Manager")
                    ?.Select(UserModel.Map).ToList()
            };

            return model;
        }
    }
}