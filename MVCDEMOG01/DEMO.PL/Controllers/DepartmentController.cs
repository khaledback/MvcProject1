using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DEMO.PL.Controllers
{
	public class DepartmentController : Controller
	{
		//private readonly IDepartmentRepository _departmentRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<DepartmentController> _logger;

		public DepartmentController(/*IDepartmentRepository departmentRepository,*/IUnitOfWork unitOfWork,ILogger<DepartmentController>logger) {
			//_departmentRepository = departmentRepository;
			_unitOfWork = unitOfWork;
			_logger = logger;
		}
		[HttpGet]
		public IActionResult Index()
		{
			var departments=_unitOfWork.DepartmentRepository.GetAll();
			//ViewData["Message"] = "Hello From View Data";
			//ViewBag.MessageBag = "Hello From View Bag";
			//TempData.Keep("Message");
			return View(departments);
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View(new Department());
		}
		[HttpPost]
		public IActionResult Create(Department department)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.DepartmentRepository.Add(department);
				TempData["Message"] = "Department Created Successfully";
				return RedirectToAction("Index");
			}
			return View(department);
		}
		public IActionResult Details(int ? id)
		{
			try
			{
				//if (id is null)
				//	return NotFound();
				var department = _unitOfWork.DepartmentRepository.GetById(id);
				if (department is null)
					return NotFound();
				return View(department);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return RedirectToAction("Error", "Home");
			}

		}
		public IActionResult Update(int? id)
		{

            if (id is null)
                return NotFound();
            var department = _unitOfWork.DepartmentRepository.GetById(id);
            if (department is null)
                return NotFound();
            return View(department);
        }
		[HttpPost]
		public IActionResult Update(int id ,Department department)
		{
			if (id != department.Id)
				return NotFound();
			try
			{
				if (ModelState.IsValid)
				{
					_unitOfWork.DepartmentRepository.Update(department);
					return RedirectToAction("Index");
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return View(department);
		}
		public IActionResult Delete(int? id)
		{
			 if (id is null)
					return NotFound();
			var department = _unitOfWork.DepartmentRepository.GetById(id);
			if (department is null)
				return NotFound();
			_unitOfWork.DepartmentRepository.Delete(department);
			return RedirectToAction("Index");
		}
	}
}
