using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MyMicroService.Services.Catalog.Models;

namespace MyMicroService.Services.Catalog.DTOs
{
    public class CourseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string UserId { get; set; }

        public string Picture { get; set; }
        public string Description { get; set; }

        public DateTime CreatedTime { get; set; }

        public FeatureDto Feature { get; set; }

        public string CatgegoryId { get; set; }

        public CategoryDto Category { get; set; }
    }
}
