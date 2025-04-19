using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Main(string Name)
        {
            var items = _context.Item;

            if (Name is not null)
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Append("Name", Name, options);
            }
            else
            {
                Name = Request.Cookies["Name"];
            }

            ViewBag.Name = Name;
            return View(items.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Item item)
        {
            _context.Item.Add(item);
            _context.SaveChanges();

            TempData["SuccessMessage"] = item.Name;
            return RedirectToAction("Main");
        }

        public IActionResult Edit(int Id)
        {
            var item = _context.Item.Find(Id);
            if (item is not null)
            {
                return View(item);
            }

            return RedirectToAction("NotFound", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Item item)
        {
            var itemInDb = _context.Item.AsNoTracking().FirstOrDefault(i => i.Id == item.Id);
            if (itemInDb is not null)
            {
                _context.Item.Update(item);
                _context.SaveChanges();
                return RedirectToAction("Main");
            }

            return RedirectToAction("NotFound", "Home");
        }

        public IActionResult Delete(int Id)
        {
            var item = _context.Item.Find(Id);
            if (item is not null)
            {
                _context.Remove(item);
                _context.SaveChanges();
                return RedirectToAction("Main");
            }

            return RedirectToAction("NotFound","Home");
        }
    }
}
