using BarsGroup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BarsGroup.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        public HomeController(AppDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var groups = db.Groups.Include(g=>g.Teacher).Include(g=>g.Students)
                .Select(g => new GroupExtended {
                    Id = g.Id,
                    Name = g.Name, 
                    Teacher = g.Teacher.FullName, 
                    StudentsCount = g.Students.Count 
                }).ToList();
            return View(groups);
        }
        public IActionResult CreateGroup()
        {
            ViewBag.TeacherId = new SelectList(db.Teachers.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult CreateGroup([Bind("Name,TeacherId")] Group group)
        {
            if (ModelState.IsValid)
            {
                //Добавление заявки и сохранение изменений
                db.Add(group);
                db.SaveChanges();
                return RedirectToAction("RequestList");
            }
            //Выпадающий список с приложениями (отображается название, идентификатор - как передаваемое формой значение),
            ViewBag.TeacherId = new SelectList(db.Teachers.ToList(), "Id", "Name");
            return View(group);
        }
        public IActionResult EditGroup(int id)
        {
            var tt = db.Groups.Include(g=>g.Teacher).Include(g => g.Students).ThenInclude(s=>s.Organization).Where(g=>g.Id == id).FirstOrDefault();
            return View(tt);
        }
        [HttpPost]
        public ActionResult EditGroup([Bind("Name,TeacherId")] Group group)
        {
            if (ModelState.IsValid)
            {
                //Добавление заявки и сохранение изменений
                db.Add(group);
                db.SaveChanges();
                return RedirectToAction("RequestList");
            }
            //Выпадающий список с приложениями (отображается название, идентификатор - как передаваемое формой значение),
            ViewBag.TeacherId = new SelectList(db.Teachers.ToList(), "Id", "Name");
            return View(group);
        }
    }
}
