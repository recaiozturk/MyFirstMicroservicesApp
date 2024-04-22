using AutoMapper;
using MongoDB.Driver;
using MyMicroservice.Shared.Dtos;
using MyMicroService.Services.Catalog.DTOs;
using MyMicroService.Services.Catalog.Models;
using MyMicroService.Services.Catalog.Settings;

namespace MyMicroService.Services.Catalog.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryColleciton;

        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper,IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(databaseSettings.DatabasName);
            _categoryColleciton=database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories=await _categoryColleciton.Find(category=>true).ToListAsync();
            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }

        public async Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var category=_mapper.Map<Category>(categoryDto);
            await _categoryColleciton.InsertOneAsync(category);

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 204);

        }

        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var category = _categoryColleciton.Find<Category>(c => c.Id == id).FirstOrDefaultAsync();

            if (category == null)
                return Response<CategoryDto>.Fail("Category Not Found", 404);

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
    } 
}
