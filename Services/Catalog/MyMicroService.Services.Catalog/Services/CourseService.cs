using AutoMapper;
using MongoDB.Driver;
using MyMicroservice.Shared.Dtos;
using MyMicroService.Services.Catalog.DTOs;
using MyMicroService.Services.Catalog.Models;
using MyMicroService.Services.Catalog.Settings;

namespace MyMicroService.Services.Catalog.Services
{
    public class CourseService:ICourseService
    {
        private readonly IMongoCollection<Course> _courseColleciton;
        private readonly IMongoCollection<Category> _categoryColleciton;

        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionStrings);
            var database = client.GetDatabase(databaseSettings.DatabasName);
            _courseColleciton = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryColleciton = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            //iliskisel olmadıgı için tek tek ulasıcaz

            var courses = await _courseColleciton.Find(course => true).ToListAsync();


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
            var course = await _courseColleciton.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null)
                return Response<CourseDto>.Fail("Course not found", 404);

            course.Category = await _categoryColleciton.Find<Category>(x => x.Id == course.CatgegoryId).FirstAsync();

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseColleciton.Find(course => course.UserId== userId).ToListAsync();

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

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse=_mapper.Map<Course>(courseCreateDto);
            
            newCourse.CreatedTime=DateTime.Now;
            
            await _courseColleciton.InsertOneAsync(newCourse);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);

        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);

            var result = await _courseColleciton.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);

            if (result == null)
            {
                return Response<NoContent>.Fail("Course Not Found", 404);
            }

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id) 
        {
            var result = await _courseColleciton.DeleteOneAsync(x => x.Id == id);

            if(result.DeletedCount>0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("Course Not Found", 404);
            }
        }


        //user s course list
    }
}
