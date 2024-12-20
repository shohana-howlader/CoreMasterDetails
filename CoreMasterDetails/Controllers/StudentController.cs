using CoreMasterDetails.Models;
using CoreMasterDetails.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreMasterDetails.Controllers
{
    public class StudentController : Controller
    {
        private readonly CoreDbContext _db;
        private readonly IWebHostEnvironment _webHost;

        public StudentController(CoreDbContext db, IWebHostEnvironment webHost)
        {
            _db = db;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            var applicants = _db.Employees.Include(i => i.Departments).ToList();
            applicants = _db.Employees.Include(a => a.PerformanceReview).ToList();
            return View(applicants);
        }
        public JsonResult GetPerformanceReviews()
        {
            List<SelectListItem> cors = (from cor in _db.PerformanceReviews
                                         select new SelectListItem
                                         {
                                             Value = cor.ReviewId.ToString(),
                                             Text = cor.ReviewNotes
                                         }).ToList();
            return Json(cors);
        }
        public IActionResult Create()
        {
            EmployeeViewModel employee = new EmployeeViewModel();
            employee.PerformanceReviews = _db.PerformanceReviews.ToList();
            employee.Departments.Add(new Department() { DepartmentId = 1 });

            return View(employee);
        }
        [HttpPost]

        public IActionResult Create(EmployeeViewModel employee)
        {
            string uniqueFileName = GetUploadedFileName(employee);
            employee.ImageUrl = uniqueFileName;
            Employee obj = new Employee();
            obj.EmployeeName = employee.EmployeeName;
            obj.ReviewId = employee.ReviewId;
            obj.MobileNo = employee.MobileNo;
            obj.IsDeleted = employee.IsDeleted;
            obj.JoiningDate = employee.JoiningDate;
            obj.ImageUrl = employee.ImageUrl;
            _db.Add(obj);
            _db.SaveChanges();
            var user = _db.Employees.FirstOrDefault(x => x.MobileNo == employee.MobileNo);
            if (user != null)
            {
                if (employee.Departments.Count > 0)
                {
                    foreach (var item in employee.Departments)
                    {
                        Department objM = new Department();
                        objM.EmployeeId = user.EmployeeId;
                        objM.Budget = item.Budget;
                        objM.DepartmentName = item.DepartmentName;
                        _db.Add(objM);
                    }
                }
            }
            _db.SaveChanges();
            return RedirectToAction("index");
        }


        public ActionResult Delete(int? id)
        {
            var app = _db.Employees.Find(id);
            var existsDepartments = _db.Departments.Where(e => e.EmployeeId == id).ToList();
            foreach (var exp in existsDepartments)
            {
                _db.Departments.Remove(exp);
            }
            _db.Entry(app).State = EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }



        private string GetUploadedFileName(EmployeeViewModel employee)
        {
            string uniqueFileName = null;

            if (employee.ProfileFile != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + employee.ProfileFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    employee.ProfileFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Employee app = _db.Employees.Include(a => a.Departments).FirstOrDefault(x => x.EmployeeId == id);

            if (app != null)
            {
                EmployeeViewModel aps = new EmployeeViewModel()
                {
                    EmployeeId = app.EmployeeId,
                    EmployeeName = app.EmployeeName,
                    MobileNo = app.MobileNo,
                    JoiningDate = app.JoiningDate,
                    ImageUrl = app.ImageUrl,
                    ReviewId = app.ReviewId,
                    IsDeleted = app.IsDeleted,
                    //PerformanceReview = _db.PerformanceReviews.ToList(),
                    Departments = app.Departments.ToList()
                };

                return View("Edit", aps);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeViewModel employee)
        {
            try
            {
                Employee existingStudent = _db.Employees
                    .Include(a => a.Departments)
                    .FirstOrDefault(x => x.EmployeeId == employee.EmployeeId);

                if (existingStudent != null)
                {
                    existingStudent.EmployeeName = employee.EmployeeName;
                    existingStudent.ReviewId = employee.ReviewId;
                    existingStudent.MobileNo = employee.MobileNo;
                    existingStudent.IsDeleted = employee.IsDeleted;
                    existingStudent.JoiningDate = employee.JoiningDate;

                    if (employee.ProfileFile != null)
                    {
                        string uniqueFileName = GetUploadedFileName(employee);
                        existingStudent.ImageUrl = uniqueFileName;
                    }

                    existingStudent.Departments.Clear();
                    foreach (var item in employee.Departments)
                    {   
                        var newModule = new Department
                        {
                            EmployeeId = existingStudent.EmployeeId,
                            DepartmentName = item.DepartmentName,
                            Budget = item.Budget
                        };

                        existingStudent.Departments.Add(newModule);
                    }

                    _db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return NotFound();
            }
            catch (DbUpdateConcurrencyException ex)
            {

                var entry = ex.Entries.FirstOrDefault();
                if (entry != null)
                {
                    var databaseValues = entry.GetDatabaseValues();
                    if (databaseValues != null)
                    {
                        var databaseEmployee = (Employee)databaseValues.ToObject();

                        ModelState.AddModelError("", "The entity you are trying to edit has been modified by another user. Please refresh the page and try again.");


                        entry.OriginalValues.SetValues(databaseValues);


                        employee.PerformanceReviews = _db.PerformanceReviews.ToList();
                        employee.Departments = databaseEmployee.Departments.ToList();

                        return View("Edit", employee);
                    }
                }

                ModelState.AddModelError("", "The entity you are trying to edit has been deleted by another user.");

            }

            return RedirectToAction("Index");
        }
    }
}
