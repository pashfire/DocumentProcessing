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
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public int CreatorId { get; set; }

        public UserModel Creator { get; set; }

        [Required]
        public string Created { get; set; }

        [Required]
        public int Type { get; set; }

        public DocumentTypeModel TypeModel { get; set; }

        public List<DocumentTypeModel> AllTypes { get; set; }

        public string DocIndex { get; set; }

        [Required]
        public string ExecutionPeriod { get; set; }

        [Required]
        public string RegistrationDate { get; set; }

        public string DocHeader { get; set; }
        
        public int ManagerId { get; set; }

        public UserModel Manager { get; set; }
        public List<UserModel> Managers { get; set; }

        public string Resolution { get; set; }

        public int ExecutorId { get; set; }

        public UserModel Executor { get; set; }
        public List<UserModel> Executors { get; set; }

        public string ExecutorNote { get; set; }

        public int ControllerId { get; set; }

        public UserModel Inspector { get; set; }
        public List<UserModel> Inspectors { get; set; }

        public string ControllerNote { get; set; }

        [Required]
        public int NomenclatureId { get; set; }

        public NomenclatureModel Nomenclature { get; set; }
        public List<NomenclatureModel> AllNomenclature { get; set; }

        //public List<DocumentModel> AllDocuments { get; set; }

        public static DocumentModel Map(DAL.Document document)
        {
            return new DocumentModel()
            {
                Id = document.Id,
                Name = document.Name,
                Path = document.Path,
                CreatorId = document.CreatorId,
                Creator = UserModel.Map(document.Creator),
                Created = document.CreationDate,
                Type = document.TypeId,
                TypeModel = DocumentTypeModel.Map(document.DocumentType),
                DocIndex = document.DocIndex,
                ExecutionPeriod = document.ExecutionPeriod,
                RegistrationDate = document.RegistrationDate,
                DocHeader = document.DocHeader,
                ManagerId = (int)document.ManagerId,
                Manager = UserModel.Map(document.Manager),
                Resolution = document.Resolution,
                ExecutorId = (int)document.ExecutorId,
                Executor = UserModel.Map(document.Executor),
                ExecutorNote = document.ExecutorNote,
                ControllerId = (int)document.ControllerId,
                Inspector = UserModel.Map(document.Inspector),
                ControllerNote = document.ControllerNote,
                NomenclatureId = document.NomenclatureId
            };
        }

        public static DocumentModel Map(DAL.Document document,
            List<DocumentTypeModel> allTypes, List<NomenclatureModel> allNomenclature,
            List<UserModel> managers,  List<UserModel> executors, List<UserModel> inspectors)
        {
            DocumentModel model = DocumentModel.Map(document);

            model.AllTypes = allTypes;
            model.Managers = managers;
            model.Inspectors = inspectors;
            model.Executors = executors;
            model.AllNomenclature = allNomenclature;

            return model;
        }
    }
}