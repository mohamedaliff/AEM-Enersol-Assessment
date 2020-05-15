using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AEM_Enersol_Assessment.Models
{
    public class Well
    {
        [KeyAttribute()]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int PlatformID { get; set; }


        public string UniqueName { get; set; }
     
        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

    }
}
