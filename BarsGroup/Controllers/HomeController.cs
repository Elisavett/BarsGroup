using BarsGroup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarsGroup.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        public HomeController(AppDbContext context)
        {
            db = context;
        }
        //Главная страница - список групп
        public IActionResult Index()
        {
            //Выбор полей группы для отображения
            var groups = db.Groups.Include(g=>g.Teacher).Include(g=>g.Employees)
                .Select(g => new GroupExtended {
                    Id = g.Id,
                    Name = g.Name, 
                    Teacher = g.Teacher.FullName, 
                    StudentsCount = g.Employees.Count 
                }).ToList();
            return View(groups);
        }
        //Получить форму для создания группы
        public IActionResult CreateGroup()
        {
            //Выпадающие списки для преподавателей и курсов
            ViewBag.TeacherId = new SelectList(db.Teachers.ToList(), "Id", "FullName");
            ViewBag.CourseId = new SelectList(db.Courses.ToList(), "Id", "Name");
            return View();
        }
        //Создать группу
        [HttpPost]
        public ActionResult CreateGroup([Bind("Name,TeacherId,CourseId")] Group group)
        {
            if (ModelState.IsValid)
            {
                //Добавление группы
                db.Add(group);
                db.SaveChanges();
                return RedirectToAction("EditGroup", new { id = group.Id });
            }
            //Выпадающий список преаодавателями
            ViewBag.TeacherId = new SelectList(db.Teachers.ToList(), "Id", "FullName");
            return View(group);
        }
        //Получение формы для редактирования группы
        public IActionResult EditGroup(int id)
        {
            //Группа по id, включая преподавателя, курс, студентов и организаций, в которых студены являются сотрудниками
            Group group = db.Groups
                .Include(g=>g.Teacher)
                .Include(g=>g.Course)
                .Include(g => g.Employees).ThenInclude(s=>s.Organization)
                .Where(g=>g.Id == id)
                .FirstOrDefault();
            return View(group);
        }
        //Редактирование группы
        [HttpPost]
        public ActionResult EditGroup([Bind("Id,Name,TeacherId,CourseId")] Group group)
        {
            if (ModelState.IsValid)
            {
                //Редактировать группу
                db.Update(group);
                db.SaveChanges();
                return RedirectToAction("index");
            }
            //Выпадающий список преаодавателями
            ViewBag.TeacherId = new SelectList(db.Teachers.ToList(), "Id", "FullName");
            return View(group);
        }
        //Отображения формы добавления студента в группу
        public IActionResult AddStudentToGroup(int id)
        {
            //Группа по id, включая преподавателя и организации преподавателя
            Group group = db.Groups
                .Include(g => g.Teacher).ThenInclude(t => t.Organizations)
                .Where(g => g.Id == id)
                .FirstOrDefault();
            List<Organization> teacherOrganizations = group.Teacher.Organizations.ToList();
            //Выпадающий список с организациями преподавателя
            ViewBag.OrganizationId = new SelectList(teacherOrganizations, "Id", "Name", teacherOrganizations[0]);
            return View(group);
        }
        //Добавление студента в группу
        [HttpPost]
        public ActionResult AddStudentToGroup(int StudentId, int GroupId)
        {
            Employee employee = db.Employees.Find(StudentId);
            db.Groups.Find(GroupId).Employees.Add(employee);
            db.SaveChanges();
            return RedirectToAction("EditGroup", new { id = GroupId });
        }
        //Удаление студента из группы
        [HttpPost]
        public ActionResult DeleteStudentFromGroup(int StudentId, int GroupId)
        {
            Employee employee = db.Employees.Find(StudentId);
            db.Groups.Include(s=>s.Employees).SingleOrDefault(g=>g.Id == GroupId).Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("EditGroup", new { id = GroupId });
        }
        public IActionResult GetStudForOrg(int id, int groupId)
        {
            //Список студентов в группе
            List<Employee> studentsInGroup = db.Groups.Include(g => g.Employees).Where(g => g.Id == groupId).SingleOrDefault().Employees;
            //Список сотрудников организации
            List<Employee> employees = db.Organizations.Include(o => o.Employees).Where(g => g.Id == id).FirstOrDefault()?.Employees;
            //Список сотрудкинов организации за исключением уже добавленных в группу студентов
            var employeesNotInGroup = employees.Except(studentsInGroup);
            //Выпадающий список с сотрудниками
            ViewBag.StudentId = new SelectList(employeesNotInGroup, "Id", "FullName");
            return PartialView("selectStudentsPartial");
        }
    }
}
