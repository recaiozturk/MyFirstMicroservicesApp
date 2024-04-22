using MyMicroService.Services.Catalog.DTOs;
using MyMicroservice.Shared.Dtos;
using MyMicroService.Services.Catalog.Models;

namespace MyMicroService.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();
        Task<Response<CategoryDto>> CreateAsync(CategoryDto category);
        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
