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
    public class CoursesController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Courses
        public ActionResult Index(int? id)
        {
            
            var coursesModel = db.Courses.ToList();
            var coursesDTO = coursesModel.Select(x => ConvertCourse(x)).ToList();

            if (id != null)
            {
                
                var instructorsModel = (from _enrollments in db.Courses
                                    join _instructors in db.Instructors 
                                    on _enrollments.CourseInstructors equals _instructors.CourseInstructors                                   
                                    where _enrollments.CourseID == id.Value
                                    select _instructors).ToList();

                var instructorsDTO = instructorsModel.Select(x => ConvertInstructor(x)).ToList();

                ViewBag.Instructors = instructorsDTO;
            }    

            return View(coursesDTO);
        }

        private InstructorDTO ConvertInstructor(Instructor instructor)
        {
            return new InstructorDTO
            {
                ID = instructor.ID,
                FirstMidName = instructor.FirstMidName,
                LastName = instructor.LastName,
                HireDate = instructor.HireDate,
            };
        }


        private CourseDTO ConvertCourse(Course course)
        {

            return new CourseDTO
            {
                CourseID = course.CourseID,
                Title = course.Title,
                Credits = course.Credits,

            };
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseModel = db.Courses.Find(id);
            if (courseModel == null)
            {
                return HttpNotFound();
            }
            var courseDTO = ConvertCourse(courseModel);
            return View(courseDTO);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits")] CourseDTO courseDTO)
        {
            if (ModelState.IsValid)
            {
                var courseModel = new Course
                {
                    CourseID = courseDTO.CourseID,
                    Title = courseDTO.Title,
                    Credits = courseDTO.Credits
                };

                db.Courses.Add(courseModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(courseDTO);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseModel = db.Courses.Find(id);
            if (courseModel == null)
            {
                return HttpNotFound();
            }
            var courseDTO = ConvertCourse(courseModel);
            return View(courseDTO);
        }

        // POST: Courses/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits")] CourseDTO courseDTO)
        {
            if (ModelState.IsValid)
            {
                var courseModel = db.Courses.Find(courseDTO.CourseID);
                courseModel.Title = courseDTO.Title;
                courseModel.Credits = courseDTO.Credits;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(courseDTO);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            var courseModel = db.Courses.Find(id);

            if (courseModel == null)
            {
                return HttpNotFound();
            }
            var courseDTO = ConvertCourse(courseModel);
            return View(courseDTO);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
