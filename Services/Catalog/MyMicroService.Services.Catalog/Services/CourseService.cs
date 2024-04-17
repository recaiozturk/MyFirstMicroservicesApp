using AutoMapper;
using MongoDB.Driver;
using MyMicroservice.Shared.Dtos;
using MyMicroService.Services.Catalog.DTOs;
using MyMicroService.Services.Catalog.Models;
using MyMicroService.Services.Catalog.Settings;

namespace MyMicroService.Services.Catalog.Services
{
    public class CourseService
    {
        private readonly IMongoCollection<Course> _courseColleciton;
        private readonly IMongoCollection<Category> _categoryColleciton;

        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(databaseSettings.DatabasName);
            _courseColleciton = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryColleciton= database.GetCollection<Course>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            //iliskisel olmadıgı için tek tek ulasıcaz

            var courses= await _courseColleciton.Find(course=>true).ToListAsync();


            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryColleciton.Find<Category>(x => x.Id == course.CatgegoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course=await _courseColleciton.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null)
                return Response<CourseDto>.Fail("Course not found", 404);

            course.Category = await _categoryColleciton.Find<Category>(x => x.Id == course.CatgegoryId).FirstAsync();

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        //user s course list
    }
}
