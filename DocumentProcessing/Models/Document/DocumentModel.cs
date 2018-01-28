using DocumentProcessing.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DocumentProcessing.Models.Document
{
    public class DocumentModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Path { get; set; }

        public string Created { get; set; }

        [Required]
        public int Type { get; set; }
        public DocumentTypeModel TypeModel { get; set; }
        public List<DocumentTypeModel> AllTypes { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        public int CurrentResponsibleId { get; set; }

        public UserModel Creator { get; set; }
        public UserModel Responsible { get; set; }
        public List<UserModel> PossibleUsersToAssign { get; set; }

        public static DocumentModel Map(DAL.Document document)
        {
            return new DocumentModel()
            {
                Id = document.Id,
                Name = document.Name,
                Path = document.Path,
                CreatorId = document.CreatorId,
                Creator = UserModel.Map(document.Creator),
                CurrentResponsibleId = document.CurrentResponsibleId,
                Responsible = UserModel.Map(document.ResponsibleUser),
                Type = document.TypeId,
                TypeModel = DocumentTypeModel.Map(document.DocumentType),
                Created = document.CreationDate.ToString("dd.MM.yyyy hh:mm"),
            };
        }

        public static DocumentModel Map(DAL.Document document,
            List<DocumentTypeModel> allTypes, List<UserModel> possibleUsersToAssign)
        {
            DocumentModel model = DocumentModel.Map(document);

            model.AllTypes = allTypes;
            model.PossibleUsersToAssign = possibleUsersToAssign;

            return model;
        }
    }
}