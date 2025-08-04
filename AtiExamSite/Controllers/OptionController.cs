using AtiExamSite.Models.DomainModels.Exam;
using AtiExamSite.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AtiExamSite.Controllers
{
    public class OptionController : Controller
    {
        private readonly IOptionService _optionService;

        #region [- Ctor() -]
        public OptionController(IOptionService optionService)
        {
            _optionService = optionService;
        }
        #endregion

        #region [- Index() -]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var options = await _optionService.GetAllOptionsAsync();
            return View(options);
        }
        #endregion

        #region [- Details() -]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var option = await _optionService.GetByIdAsync(id);
            if (option == null) return NotFound();

            return View(option);
        }
        #endregion

        #region [- GetByQuestion() -]
        public async Task<IActionResult> GetByQuestion(Guid questionId)
        {
            var options = await _optionService.GetOptionsByQuestionAsync(questionId);
            return View(options);
        }
        #endregion

        #region [- GetCorrectForQuestion() -]
        public async Task<IActionResult> GetCorrectForQuestion(Guid questionId)
        {
            var option = await _optionService.GetCorrectOptionForQuestionAsync(questionId);
            if (option == null)
            {
                return NotFound();
            }
            return View(option);
        }
        #endregion

        #region [- Create() -]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Option());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Option option)
        {

            var success = await _optionService.CreateOptionAsync(option);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to create option");
                return View(option);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region [- Edit() -]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var option = await _optionService.GetByIdAsync(id);
            if (option == null)
            {
                return NotFound();
            }
            return View(option);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Option option)
        {
            if (id != option.Id)
                return BadRequest();

            var success = await _optionService.UpdateOptionAsync(option);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to update option");
                return View(option);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region [- Delete() -]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var option = await _optionService.GetByIdAsync(id);
            if (option == null)
            {
                return NotFound();
            }
            return View(option);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var success = await _optionService.DeleteOptionAsync(id);
            if (!success)
            {
                // Optionally handle failure (e.g. add model error or redirect with error)
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region [- VerifyCorrectOption() -]
        public async Task<IActionResult> VerifyCorrectOption(Guid optionId)
        {
            var isCorrect = await _optionService.IsCorrectOptionAsync(optionId);
            return View(isCorrect);
        }
        #endregion
    }
}