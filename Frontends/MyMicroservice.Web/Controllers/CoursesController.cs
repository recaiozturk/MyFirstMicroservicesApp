using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyMicroservice.Web.Services.Interfaces;
using MyMicroService.Shared.Services;

namespace MyMicroservice.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharerdIdentityService _sharerdIdentityService;

        public CoursesController(ICatalogService catalogService, ISharerdIdentityService sharerdIdentityService)
        {
            _catalogService = catalogService;
            _sharerdIdentityService = sharerdIdentityService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCoursesByIdAsync(_sharerdIdentityService.GetUserId));
        }

        public async Task<IActionResult> Create()
        {
            var categories=await _catalogService.GetAllCategoriesAsync();

            IEnumerable<SelectListItem> selectListItems = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
            ViewBag.categoryList = selectListItems;

            return View();
        }
    }
}
