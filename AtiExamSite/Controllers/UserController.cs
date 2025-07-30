using AtiExamSite.Models.DomainModels;
using AtiExamSite.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AtiExamSite.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        #region [- Ctor -]
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region [- Index() -]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        #endregion

        #region [- Details() -]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        } 
        #endregion

        #region [- Create() -]
       
        public IActionResult Create()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                await _userService.CreateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                return View(user);
            }
        }

        #endregion

        #region [- Edit() -]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, User user)
        {
            if (id != user.Id) return BadRequest();


            try
            {
                await _userService.UpdateAsync(user);
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(user);
            }
        } 
        #endregion

        #region [- Delete() -]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        } 
        #endregion
    }
}
