using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentProcessing.Models.Document
{
    public class NomenclatureModel
    {
        public int Id { get; set; }

        public string IndexOfCase { get; set; }

        public string SectionName { get; set; }

        public string Headline { get; set; }

        public static NomenclatureModel Map(DAL.AffairsNomenclature nomenclature)
        {
            return new NomenclatureModel()
            {
                Id = nomenclature.Id,
                IndexOfCase = nomenclature.IndexOfCase,
                SectionName = nomenclature.SectionName,
                Headline = nomenclature.Headline
            };
        }
    }
}