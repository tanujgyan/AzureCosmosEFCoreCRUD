using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace AzureCosmosEFCoreCRUD.Models
{
    public class Videogames
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Genere { get; set; }
        public string Platform { get; set; }
        public Company Company { get; set; }
    }
}
