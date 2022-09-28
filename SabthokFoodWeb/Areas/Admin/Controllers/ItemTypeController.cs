
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SabthokFoodModel.Utillity;
using SabthokFoodWeb.DataAccess;
using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;

namespace SabthokFoodWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ItemTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemTypeController(IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<ItemType> CategoryList = _unitOfWork.CoverType.GetAll();
            return View(CategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemType obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Item Created Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var index = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (index == null)
            {
                return NotFound();
            }

            return View(index);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ItemType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Item updated Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var index = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (index == null)
            {
                return NotFound();
            }

            return View(index);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ItemType obj)
        {
            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Item deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
