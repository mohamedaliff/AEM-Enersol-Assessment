using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEM_Enersol_Assessment.Models
{
    public class Platform
    {
        [KeyAttribute()]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }


        public string UniqueName { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }


        public string CreatedAt { get; set; }


        public string UpdatedAt { get; set; }

        public List<Well> Well { get; set; }
    }
}
