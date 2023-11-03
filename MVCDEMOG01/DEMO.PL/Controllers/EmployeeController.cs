using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using DEMO.PL.Helper;
using DEMO.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DEMO.PL.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
        public IActionResult Index(string SearchValue="")
		{
			IEnumerable<Employee> employees;
			IEnumerable<EmployeeViewModel> mappedEmployees;
			if (string.IsNullOrEmpty(SearchValue))
			{
				 employees = _unitOfWork.EmployeeRepository.GetAll();
				mappedEmployees=_mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
			}
			else
			{
				employees=_unitOfWork.EmployeeRepository.Search(SearchValue);
				mappedEmployees = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

			}
			return View(mappedEmployees);
		}
		public IActionResult Create() {
			ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();

			return View(new EmployeeViewModel());
		}
		[HttpPost]
		public IActionResult Create(EmployeeViewModel employeeVM)
		{
			//employee.Department = _unitOfWork.DepartmentRepository.GetById(employee.DepartmentId);
			//Employee employee = new Employee()
			//{
			//	Name = employeeVM.Name,
			//	Address = employeeVM.Address,
			//	DepartmentId = employeeVM.DepartmentId,
			//	Email = employeeVM.Email,
			//	HireDate = employeeVM.HireDate,
			//	IsActive = employeeVM.IsActive,
			//	Salary = employeeVM.Salary
			//};

			////ModelState["Department"].ValidationState = ModelValidationState.Valid;
			if (ModelState.IsValid)
			{
				var employee = _mapper.Map<Employee>(employeeVM);
				employee.ImageUrl = DocumentSettings.UploadFile(employeeVM.Image, "Images");
				_unitOfWork.EmployeeRepository.Add(employee);
				return RedirectToAction(nameof(Index));
			}
			ViewBag.Department = _unitOfWork.DepartmentRepository.GetAll();
			return View(employeeVM);
		}
		//public IActionResult Update(int? id)
		//{ 

		//	if (id is null)
		//		return NotFound();
		//	var department = _unitOfWork.DepartmentRepository.GetById(id);
		//	if (department is null)
		//		return NotFound();
		//	return View(new EmployeeViewModel());
		//}
		//[HttpPost]
		//public IActionResult Update(int id, EmployeeViewModel employeeVM)
		//{
		//	if (id != employeeVM.Id)
		//		return NotFound();
		//	try
		//	{
		//		if (ModelState.IsValid)
		//		{
		//			var employee=_mapper.Map<Employee>(employeeVM);
		//			_unitOfWork.EmployeeRepository.Update(employee);
		//			return RedirectToAction("Index");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		throw new Exception(ex.Message);
		//	}
		//	return View(employeeVM);
		//}
	}
}
