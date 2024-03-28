using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using ScooterManagementApp.DAL.Models;
using ScooterManagementApp.DAL.Repositories;
using ScooterManagementApp.Models;
using System;
using System.Text;
using X.PagedList;

namespace ScooterManagementApp.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _employeeRepository.GetAllAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel emp)
        {
            try
            {
                byte[] photoBytes = null;
                if (emp.ProfilePicture != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        emp.ProfilePicture.CopyTo(stream);
                        photoBytes = stream.ToArray();
                    }
                }
                var employee = new Employee()
                {
                    EmployeeId = emp.EmployeeId,
                    DateEmployed = emp.DateEmployed,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    ProfilePicture = photoBytes,
                    Position = emp.Position,
                    Salary = emp.Salary,
                    IsActive = emp.IsActive,
                    StationId = emp.StationId
                };
                int id = await _employeeRepository.Insert(employee);
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(emp);
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            var emp = await _employeeRepository.GetById(id);
            if (emp != null)
            {
                var e = new EmployeeViewModel()
                {
                    EmployeeId = emp.EmployeeId,
                    DateEmployed = emp.DateEmployed,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    ProfilePicture = null,
                    Position = emp.Position,
                    Salary = emp.Salary,
                    IsActive = emp.IsActive,
                    StationId = emp.StationId
                };
                return View(e);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel emp)
        {
            try
            {
                byte[]? photoBytes = null;
                if (emp.ProfilePicture != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        emp.ProfilePicture.CopyTo(stream);
                        photoBytes = stream.ToArray();
                    }
                }
                else
                {
                    var e = await _employeeRepository.GetById(emp.EmployeeId);
                    photoBytes = e?.ProfilePicture;
                }
                var employee = new Employee()
                {
                    EmployeeId = emp.EmployeeId,
                    DateEmployed = emp.DateEmployed,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    ProfilePicture = photoBytes,
                    Position = emp.Position,
                    Salary = emp.Salary,
                    IsActive = emp.IsActive,
                    StationId = emp.StationId
                };

                await _employeeRepository.Update(employee);
                return RedirectToAction("Details", new { id = emp.EmployeeId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(emp);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _employeeRepository.GetById(id));
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _employeeRepository.Delete(id);
            }
            catch (Exception ex)
            {
                TempData["DeleteErrors"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Filter(EmployeeFilterViewModel empFilterModel)
        {
            int totalCount;
            var employees = await _employeeRepository.Filter(
                empFilterModel.Date,
                empFilterModel.Position,
                empFilterModel.StationId,
                empFilterModel.SortField,
                empFilterModel.SortDesc,
                empFilterModel.Page,
                empFilterModel.PageSize);
            totalCount = employees.Item2;

            empFilterModel.Employees = new StaticPagedList<Employee>(
                employees.Item1,
                empFilterModel.Page,
                empFilterModel.PageSize,
                totalCount);

            empFilterModel.TotalCount = totalCount;
            return View(empFilterModel);
        }
        public IActionResult ExportToJson()
        {
            string json = _employeeRepository.ExportToJson(null, null, null);
            return File(
                Encoding.UTF8.GetBytes(json),
                "application/json",
                $"ScooterDb_Employees_{DateTime.Now}.json"
                );
        }

        public IActionResult ExportToXml()
        {
            string xml = _employeeRepository.ExportToXml(null, null, null);

            return File(
                Encoding.UTF8.GetBytes(xml),
                "text/xml",
                $"ScooterDb_Employees_{DateTime.Now}.xml"
                );
        }
        public async Task<FileResult?> ShowImage(int id)
        {
            var emp = await _employeeRepository.GetById(id);
            if (emp?.ProfilePicture != null)
            {
                return File(emp.ProfilePicture, "image/jpeg", $"employee{id}.jpg");
            }
            return null;
        }
    }
}
