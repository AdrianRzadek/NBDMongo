using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MongoNBD.Models
{
    public class Computers
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [Display(Name="Computer name")]
        public string? Name { get; set; }   
        [Display(Name="Creation year")]
        public int? Year { get; set; }  
        public string? Image { get; set; }
       // public bool HasImage()=> !string.IsNullOrWhiteSpace(Image);
    }
}
