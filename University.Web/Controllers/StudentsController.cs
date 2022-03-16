using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;

namespace University.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly DBContext context = new DBContext();

        private static List<StudentDTO> students = new List<StudentDTO>
        {
            new StudentDTO{ID = 1, FirstMidName = "Albert", LastName = "beto",EnrollmentDate=DateTime.UtcNow},
            new StudentDTO{ID = 2, FirstMidName = "alex", LastName = "casa",EnrollmentDate=DateTime.UtcNow},
            new StudentDTO{ID = 3, FirstMidName = "pablo", LastName = "el",EnrollmentDate=DateTime.UtcNow}
        };
        //Students/Index
        // GET: Students
        [HttpGet]
        public ActionResult Index(int? id)
        {
            #region Traspaso de  datos
            //1.View(model)
            //2.ViewBag
            //3.ViewData
            //4.session

            ViewBag.StudensCount = students.Count;
            ViewBag.StudentFirst = students.FirstOrDefault();

            ViewData["StudentCount"] = students.Count;
            ViewData["StudentCount"] = students.FirstOrDefault();

            Session["StudentsCount"] = students.Count;
            var count = (int)Session["StudentsCount"];
            Session.Remove("StudentsCount");
            Session.RemoveAll(); 
            #endregion

            var studentsModel = context.Students.ToList();
            var studentsDTO = studentsModel.Select(x => ConvertStudent(x)).ToList();

            if(id != null)
            {
                //SELET _ courses.*
                //* FROM Enrollment _enrolments
                // JOIN Courses _courses ON _enrollments.CourseID = _courses.CourseID
                //WHERE 
                var coursesModel = (from _enrollments in context.Enrollments
                               join _courses in context.Courses
                               on _enrollments.CourseID equals _courses.CourseID
                               where _enrollments.StudentID == id.Value
                               select _courses).ToList();

                var instructorsDTO = coursesModel.Select(x => ConvertCourse(x)).ToList();

                ViewBag.Courses = instructorsDTO;
            }

            return View(studentsDTO);
        }
        private StudentDTO ConvertStudent(Student student)
        {

            return new StudentDTO
            {
                ID = student.ID,
                FirstMidName = student.FirstMidName,
                LastName = student.LastName,
                EnrollmentDate = student.EnrollmentDate,
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

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(StudentDTO studentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Ef
                    //Insert into Student(firstMidNmae ......)
                    //Values(@firsMidName
                    var student = new Student
                    {
                        FirstMidName = studentDTO.FirstMidName,
                        LastName = studentDTO.LastName,
                        EnrollmentDate = studentDTO.EnrollmentDate,
                    };
                    context.Students.Add(student);
                    context.SaveChanges();
                    

                  
                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                
            }

            return View(studentDTO);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var studentModel = context.Students.Find(id);
            var studentDTO = ConvertStudent(studentModel);

            return View(studentDTO);
        }

        [HttpPost]
        public ActionResult Edit(StudentDTO studentDTO)
        {
            try

            {
                if (ModelState.IsValid)
                {
                    var studentModel = context.Students.Find(studentDTO.ID);
                    //campos a modificar   // UPDATE student SET = FirstMidName = @FirstMidName
                    //where ID = @ID
                    studentModel.FirstMidName = studentDTO.FirstMidName;
                    studentModel.LastName = studentDTO.LastName;
                    studentModel.EnrollmentDate = studentDTO.EnrollmentDate;

                    context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }

            return View(studentDTO);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                //Dependencias
                var enrollments = context.Enrollments.Where(x => x.StudentID == id).ToList();
                if (!enrollments.Any())
                {
                    var studentModel = context.Students.Find(id);
                    context.Students.Remove(studentModel);
                    context.SaveChanges();
                }
                else
                    throw new Exception("Tiene cursos maytriculados");

                               
            }
            catch (Exception ex)
            {
                var message = ex.Message;
               
            }
            return RedirectToAction(nameof(Index));

        }
    }
}