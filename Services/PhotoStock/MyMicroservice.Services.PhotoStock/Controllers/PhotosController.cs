using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMicroservice.Services.PhotoStock.DTOs;
using MyMicroservice.Shared.Dtos;
using MyMicroService.Shared.ControllerBases;

namespace MyMicroservice.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        public async Task<IActionResult> PhotoSave(IFormFile photo,CancellationToken cancellationToken)
        {
            if(photo != null && photo.Length>0) 
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);

                await photo.CopyToAsync(stream,cancellationToken);

                var returnPath="photos/"+photo.FileName;

                PhotoDto photoDto = new() { Url = returnPath };

                return CreateActioNResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            }

            return CreateActioNResultInstance(Response<PhotoDto>.Fail("photo is empty", 400));
        }

        public  IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);

            if(!System.IO.File.Exists(path)) 
            {
                return CreateActioNResultInstance(Response<NoContent>.Fail("photo not found", 404));
            }

            System.IO.File.Delete(path);

            return CreateActioNResultInstance(Response<NoContent>.Success(204));
        }
    }
}
