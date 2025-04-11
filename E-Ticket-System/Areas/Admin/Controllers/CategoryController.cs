using E_Ticket_System.Models;
using E_Ticket_System.Repositries.Irepostries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticket_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]    
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var data=categoryRepository.Get();
            return View(data);
        }
        [HttpGet]
        public IActionResult Create() { 
        return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category) {
            if (ModelState.IsValid)
            {
                categoryRepository.Create(category);
                categoryRepository.comit();

                return RedirectToAction(nameof(Index)); 
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id) {
            var category = categoryRepository.GetOne(e => e.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid) {
                var data = categoryRepository.GetOne(e => e.Id == category.Id);
                data.Name=category.Name;
                categoryRepository.Edit(data);
                categoryRepository.comit();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Edit));
        }

        public IActionResult Delete(int id)
        {
            var data = categoryRepository.GetOne(e => e.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            categoryRepository.Delete(data);
            categoryRepository.comit();
            return RedirectToAction(nameof(Index));
        }

    }
}
