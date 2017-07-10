using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITI_System.Models;


namespace ITI_System.Controllers
{
    public class CourseController : Controller
    {
        
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Course
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Courses);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Course crs)
        {
            if(ModelState.IsValid)
            {
                db.Courses.Add(crs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            return View(crs);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Course crs = db.Courses.FirstOrDefault(i => i.Crs_Id == id);
            return View(crs);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(Course crs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crs).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(crs);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Course crs = db.Courses.FirstOrDefault(s => s.Crs_Id == id);
            return View(crs);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(Course crs)
        {
            var course = db.Student_Course_Instructor.FirstOrDefault(c => c.Crs_Id == crs.Crs_Id);
            if (course == null)
            {
                db.Courses.Remove(db.Courses.FirstOrDefault(c => c.Crs_Id == crs.Crs_Id));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {

                return View("Error", crs);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult assignCourse()
        {
          
            ViewBag.deps=new SelectList(db.Departments,"Dept_Id","Dept_name");

            return View(db.Courses);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult assignCourse(int deps, int[] courses)
        {
           
            Course crs;
            Department dep = db.Departments.FirstOrDefault(l => l.Dept_Id == deps);
            foreach (int item in courses)
            {
                crs = db.Courses.FirstOrDefault(c => c.Crs_Id == item);
                if (!dep.Crs_List.Contains(crs))
                {
                    dep.Crs_List.Add(crs);
                    crs.Dept_List.Add(dep);
                    db.SaveChanges();
                }

            }
   
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult assignInstructor()
        {
            ViewBag.inst = new SelectList(db.Instructor, "Id", "FirstName");

            return View(db.Courses);

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult assignInstructor(int inst, int[] courses)
        {

            Course crs;
            Instructor ins = db.Instructor.FirstOrDefault(l => l.Id==inst.ToString());
            foreach (int item in courses)
            {
                crs = db.Courses.FirstOrDefault(c => c.Crs_Id == item);
                if (!ins.Crs_List.Contains(crs))
                {
                    ins.Crs_List.Add(crs);
                    crs.Ins_List.Add(ins);
                    db.SaveChanges();
                }

            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult assignDepCrsInsee()
        {
            ViewBag.deps = new SelectList(db.Departments, "Dept_Id", "Dept_name");

            return View();
           
        }
        [Authorize(Roles = "Admin")]
        public ActionResult assignDepCrsInst(int id)
        {
            Department dep = db.Departments.FirstOrDefault(l => l.Dept_Id == id);
            var crs= dep.Crs_List.ToList();
            ViewBag.crs = new SelectList(crs, "Crs_Id", "Crs_name");
            return PartialView("courses_Partial");

        }
        [Authorize(Roles = "Admin")]
        public ActionResult assignDepCrsIns2(int id)
        {
            Course crs = db.Courses.FirstOrDefault(l => l.Crs_Id == id);
            var ins = crs.Ins_List.ToList();
            ViewBag.ins = new SelectList(ins, "Id", "FirstName");
            return PartialView("instr_Partial");

        }
        [Authorize(Roles = "Admin")]
        public ActionResult assignDepCrsIns(int deps,int crs,int ins)
        {
            
            var dep = db.Departments.FirstOrDefault(d => d.Dept_Id == deps);
            var course = db.Courses.FirstOrDefault(d => d.Crs_Id == crs);
            var instr = db.Instructor.FirstOrDefault(d => d.Id == ins.ToString());
            dep.Ins_list.Add(instr);
            dep.Crs_List.Add(course);
            course.Dept_List.Add(dep);
            course.Ins_List.Add(instr);
            instr.Crs_List.Add(course);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}