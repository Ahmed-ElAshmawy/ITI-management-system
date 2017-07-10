using ITI_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace ITI_System.Controllers
{
    public class DepartmentController : Controller
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        SelectList manage_drop =
 new SelectList((from i in db.Instructor.ToList()
                 select new
                 {
                     Id = i.Id,
                     FullName = i.FirstName + " " + i.LastName
                 }),
     "Id",
     "FullName",
     null);
        SelectList Std_drop = new SelectList(db.Students, "Std_Id", "UserName");
        SelectList Dept_drop = new SelectList(db.Departments, "Dept_Id", "Dept_name");
        // GET: Department
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var DeptList = db.Departments.ToList();
            return View(DeptList);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult create()
        {
            ViewBag.manage_drop = manage_drop;
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> create(Department Dept)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(Dept);
                db.SaveChanges();
                if (Dept.Manager_Id != null)
                {
                    ApplicationUserManager Usermanager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    await Usermanager.AddToRoleAsync(Dept.Manager_Id, "Manager");
                }
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> delete(int id)
        {
            Department Dept = db.Departments.FirstOrDefault(a => a.Dept_Id == id);

            if (Dept.Manager_Id != null)
            {
                ApplicationUserManager Usermanager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                await Usermanager.RemoveFromRoleAsync(Dept.Manager_Id, "Manager");
            }

            db.Departments.Remove(Dept);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Department Dept = db.Departments.FirstOrDefault(a => a.Dept_Id == id);
            ViewBag.manage_drop = manage_drop;
            return View(Dept);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Edit(Department newDept)
        {
            Department Dept = db.Departments.FirstOrDefault(a => a.Dept_Id == newDept.Dept_Id);
            Dept.Dept_name = newDept.Dept_name;
            Dept.Dept_Capacity = newDept.Dept_Capacity;
            Dept.Manager_Id = newDept.Manager_Id;
            db.SaveChanges();

            ApplicationUserManager Usermanager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            await Usermanager.RemoveFromRoleAsync(Dept.Manager_Id, "Manager");

            if (newDept.Manager_Id != null)
            {
                await Usermanager.AddToRoleAsync(newDept.Manager_Id, "Manager");
            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult StdOfDept()
        {
            ViewBag.dept_drop = Dept_drop;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult StdOfDepartment(int id)
        {
            var dept = (from i in db.Departments
                        where i.Dept_Id == id
                        select i).First();
            TempData["deptartment"] = dept;
            var std = (from i in db.Students
                       join d in db.Departments on i.Dept_Id equals d.Dept_Id
                       where i.Dept_Id == id
                       select i).ToList();
            return PartialView(std);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult InstOfDept()
        {
            ViewBag.dept_drop = Dept_drop;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult InstOfDepartment(int id)
        {
            var dept = (from i in db.Departments
                        where i.Dept_Id == id
                        select i).First();
            var inst = (from i in db.Instructor
                        where i.Id == dept.Manager_Id
                        select i).First();
            TempData["instructor"] = inst;
            TempData["deptartment"] = dept;
            var ins = (from i in db.Instructor
                       join d in db.Departments on i.Dept_Id equals d.Dept_Id
                       where i.Dept_Id == id && i.Id != dept.Manager_Id
                       select i).ToList();
            return PartialView(ins);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CrsOfDept()
        {
            ViewBag.dept_drop = Dept_drop;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CrsOfDepartment(int id)
        {
            var crs = db.Departments.FirstOrDefault(a => a.Dept_Id == id).Crs_List.ToList();
            var dept = (from i in db.Departments
                        where i.Dept_Id == id
                        select i).First();
            TempData["deptartment"] = dept;

            return PartialView(crs);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeptListManager()
        {
            ViewBag.dept_drop = Dept_drop;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ChangeManger(int id)
        {
            var dept = (from i in db.Departments
                        where i.Dept_Id == id
                        select i).First();
            TempData["deptartment"] = dept;
            var ins = (from i in db.Instructor
                       join d in db.Departments on i.Dept_Id equals d.Dept_Id
                       where i.Dept_Id == id
                       select i).ToList();
            return PartialView(ins);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> SetManager(string deptOfMan, string RadioChoosen)
        {
            var dep = int.Parse(deptOfMan);
            var dept = (from i in db.Departments
                        where i.Dept_Id == dep
                        select i).First();

            ApplicationUserManager Usermanager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            await Usermanager.RemoveFromRoleAsync(dept.Manager_Id, "Manager");

            dept.Manager_Id = RadioChoosen;
            db.SaveChanges();

            await Usermanager.AddToRoleAsync(dept.Manager_Id, "Manager");
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                ViewBag.error = "You must enter excel File";
                return View();
            }
            else
            {
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/" + excelFile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelFile.SaveAs(path);
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;

                    for (int i = 2; i < range.Rows.Count + 1; i++)
                    {
                        Department dept = new Department();
                        dept.Dept_name = ((Excel.Range)range.Cells[i, 1]).Text;
                        dept.Dept_Capacity = int.Parse(((Excel.Range)range.Cells[i, 2]).Text);
                        string x = ((Excel.Range)range.Cells[i, 3]).Text;
                        if (x.Length < 1)
                            dept.Manager_Id = null;
                        else
                            dept.Manager_Id = x;
                        db.Departments.Add(dept);
                        db.SaveChanges();
                    }
                    var s = db.Departments.ToList();
                    return View("Index", s);
                }
                else
                {
                    ViewBag.error = "You must choose Excel File";
                    return PartialView();
                }

            }

        }

    }
}