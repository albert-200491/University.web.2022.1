using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;

namespace University.Web.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly DBContext context = new DBContext();

        // GET: Departments
        public ActionResult Index()
        {

            var departmentsModel = context.Departments.Include(d => d.Instructor);
            var departmentsDTO = departmentsModel.Select(x => ConvertDepartment(x)).ToList();

            return View(departmentsDTO);

        }
        private DepartmentDTO ConvertDepartment(Department department)
        {
            return new DepartmentDTO
            {
                DepartmentID = department.DepartmentID,
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                InstructorID = department.InstructorID,

            };
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = context.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(context.Instructors, "ID", "LastName");
            return View();
        }
       
        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentID,Name,Budget,StartDate,InstructorID")] DepartmentDTO departmentDTO)
        {
            if (ModelState.IsValid)
            {
                Department department = new Department();

                department.InstructorID = departmentDTO.InstructorID;
                department.Budget = departmentDTO.Budget;
                department.Name = departmentDTO.Name;
                department.StartDate = departmentDTO.StartDate;
                department.DepartmentID = departmentDTO.DepartmentID;

                context.Departments.Add(department);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InstructorID = new SelectList(context.Instructors, "ID", "LastName", departmentDTO.InstructorID);
            return View(departmentDTO);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var departmentModel = context.Departments.Find(id);
            if (departmentModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorID = new SelectList(context.Instructors, "ID", "LastName", departmentDTO.InstructorID);
            var
            return View(departmentDTO);

            
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                context.Entry(department).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InstructorID = new SelectList(context.Instructors, "ID", "LastName", department.InstructorID);
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = context.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = context.Departments.Find(id);
            context.Departments.Remove(department);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
