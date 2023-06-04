using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoNBD.Data;
using MongoNBD.Models;
using System.Diagnostics;

namespace MongoNBD.Controllers
{
    public class HomeController : Controller
	    {
	        private readonly ILogger<HomeController> _logger;
	        private readonly ComputerContext db;
	
	        public HomeController(ILogger<HomeController> logger, ComputerContext computerContext)
	        {
	            _logger = logger;
	            db = computerContext;
	        }
	
	        public async Task<IActionResult> Index(ComputerFilter filter)
	        {
	            var computers = await db.GetComputers(filter.Year, filter.ComputerName);
	            var model = new ComputerList { Computers = computers, Filter = filter };
	            return View(model);
	        }

         public IActionResult Create() => View();

         [HttpPost]
         public async Task<IActionResult> Create(Computers computer)
         {
             if (ModelState.IsValid)
             {
await db.Create(computer);
                 return RedirectToAction("Index");
             }
             return View(computer);
         }


        public async Task<IActionResult> Edit(string id)
        {
            Computers computer = await db.GetComputer(id);
            if (computer == null)
                return Error();
            else
                return View(computer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Computers computer)
        {
            if (ModelState.IsValid)
            {
                await db.Update(computer);
                return RedirectToAction("Index");
            }
            return View(computer);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await db.Remove(id);
            return RedirectToAction("Index");
        }
        [HttpGet]

        public async Task<IActionResult> AttachImage(string id)
        {
            Computers computer = await db.GetComputer(id);
            if (computer == null)
                return Error();
            else
                return View(computer);
        }

        [HttpPost]
        public async Task<IActionResult> AttachImage(string id, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
                await db.StoreImage(id, uploadedFile.OpenReadStream(), uploadedFile.FileName);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetImage(string ImageId)
        {
            var image = await db.GetImage(ImageId);
            if (image == null) return Error();
            return File(image, "image/png");
        }

        public IActionResult Privacy()
         {
             return View();
         }

         [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
         public IActionResult Error()
         {
             return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
         }
     }


}