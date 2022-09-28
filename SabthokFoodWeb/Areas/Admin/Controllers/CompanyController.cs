
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using SabthokFoodModel.Utillity;
using SabthokFoodWeb.DataAccess;
using SabthokFoodWeb.DataAccess.Repository;
using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;
using SabthokFoodWeb.Models.ViewModel;

namespace SabthokFoodWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new();
            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                if(obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["Success"] = "Company created Successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["Success"] = "Company updated Successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companylist = _unitOfWork.Company.GetAll();
            return Json(new { data = companylist });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj =_unitOfWork.Company.GetFirstOrDefault(u => u.Id == id); 
            if(obj==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }


}
