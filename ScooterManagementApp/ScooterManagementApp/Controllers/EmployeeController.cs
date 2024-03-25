using Microsoft.AspNetCore.Mvc;
using ScooterManagementApp.DAL.Models;
using ScooterManagementApp.DAL.Repositories;

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
        public async Task<IActionResult> Create(Employee emp)
        {
            try
            {
                int id = await _employeeRepository.Insert(emp);
                // return RedirectToAction("Details", new { id = id});
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(emp);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetById(id);
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee emp)
        {
            try
            {
                await _employeeRepository.Update(emp);
                // return RedirectToAction("Details", new { id = emp.EmployeeId });
                return RedirectToAction("Index");
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
    }
}
