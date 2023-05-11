using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HPlusSport.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required] //For Sku
        public string Sku { get; set; } = string.Empty;
        [Required] //For Name
        public string Name { get; set; } = string.Empty;
        [Required] //For Description
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public bool IsAvailable { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category? Category { get; set; }

    }
}
