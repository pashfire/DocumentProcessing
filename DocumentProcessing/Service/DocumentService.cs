using DocumentProcessing.DAL;
using DocumentProcessing.Infrastructure.Helpers;
using DocumentProcessing.Models.Document;
using DocumentProcessing.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentProcessing.Service
{
    public class DocumentService
    {
        private GenericRepository<Document> documentsRepo;
        private GenericRepository<DocumentType> documentTypesRepo;
        private GenericRepository<User> userRepo;

        public DocumentService(DocumentProcessingEntities dataContext)
        {
            documentsRepo = new GenericRepository<Document>(dataContext);
            documentTypesRepo = new GenericRepository<DocumentType>(dataContext);
            userRepo = new GenericRepository<User>(dataContext);
        }

        public DocumentModel GetDocument(int id)
        {
            Document document = documentsRepo.GetById(id);
            return document == null ? null : DocumentModel.Map(document);
        }

        public DocumentModel GetDocument(int id, int userId)
        {
            var document = documentsRepo.GetById(id);

            var allTypes = documentTypesRepo.Get()
                    .Select(DocumentTypeModel.Map).ToList();
            var possibleUsersToAssign = userRepo.Get(u => u.Id != userId)
                    ?.Select(UserModel.Map).ToList();

            return document == null ? null : DocumentModel
                    .Map(document, allTypes, possibleUsersToAssign);
        }

        public List<DocumentModel> GetDocumentsByCreator(int userId)
        {
            return documentsRepo.Get(doc => doc.CreatorId == userId)
                ?.Select(doc => DocumentModel.Map(doc)).ToList();
        }

        public List<DocumentModel> GetDocumentsByResponsible(int userId)
        {
            return documentsRepo.Get(doc => doc.CurrentResponsibleId == userId)
                ?.Select(doc => DocumentModel.Map(doc)).ToList();
        }

        public void UpdateDocument(DocumentModel model)
        {
            Document document = documentsRepo.GetById(model.Id);

            FileHelper.DeleteFile(document.Path);

            document.Name = model.Name;
            document.Path = model.Path;
            document.TypeId = model.Type;
            document.CurrentResponsibleId = model.CurrentResponsibleId;

            documentsRepo.Update(document);
            documentsRepo.Save();
        }

        public void DeleteDocument(int id, string serverPath)
        {
            Document document = documentsRepo.GetById(id);
            var path = serverPath + document.Path;
            documentsRepo.Delete(document);
            FileHelper.DeleteFile(path);
        } 

        public DocumentModel CreateDocument(DocumentModel model)
        {
            Document document = new Document()
            {
                CreationDate = DateTime.Now,
                Name = model.Name,
                TypeId = model.Type,
                CreatorId = model.CreatorId,
                CurrentResponsibleId = model.CurrentResponsibleId,
                Path = model.Path
            };

            documentsRepo.Insert(ref document);
            model.Id = document.Id;

            return model;
        }

        public DocumentModel CreateDocumentModel(User user)
        {
            var model = new DocumentModel()
            {
                CreatorId = user.Id,
                Creator = UserModel.Map(user),
                AllTypes = documentTypesRepo.Get()
                    .Select(DocumentTypeModel.Map).ToList(),
                PossibleUsersToAssign = userRepo.Get(u => u.Id != user.Id)
                    ?.Select(UserModel.Map).ToList()
            };

            return model;
        }
    }
}