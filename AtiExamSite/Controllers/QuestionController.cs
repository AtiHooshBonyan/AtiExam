using AtiExamSite.Models.DomainModels;
using AtiExamSite.Models.ViewModels.Question;
using AtiExamSite.Services.Contracts;
using AtiExamSite.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AtiExamSite.Web.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IOptionService _optionService;
        private readonly IQuestionOptionService _questionOptionService;


        #region [- Ctor() -]
        public QuestionController(
            IQuestionService questionService,
            IOptionService optionService,
            IQuestionOptionService questionOptionService)
        {
            _questionService = questionService;
            _optionService = optionService;
            _questionOptionService = questionOptionService;
        }
        #endregion

        #region [- Index() -]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var questions = await _questionService.GetAllQuestionsAsync();
                return View(questions);
            }
            catch (Exception ex)
            {
                // Log error
                return View(new List<Question>());
            }
        }
        #endregion

        #region [- Details() -]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var question = await _questionService.GetWithOptionsAsync(id);
            if (question == null) return NotFound();

            return View(question);
        }
        #endregion

        #region [- GetByDifficulty() -]
        public async Task<IActionResult> GetByDifficulty(string difficultyLevel)
        {
            if (string.IsNullOrWhiteSpace(difficultyLevel))
            {
                return BadRequest("Difficulty level must be specified");
            }

            var questions = await _questionService.GetByDifficultyAsync(difficultyLevel);
            return View(questions);
        }
        #endregion

        #region [- DifficultyCount() -]
        public async Task<IActionResult> DifficultyCount(string difficultyLevel)
        {
            var count = await _questionService.CountByDifficultyAsync(difficultyLevel);
            return View(count);
        }
        #endregion

        #region [- VerifyHasCorrectOption() -]
        public async Task<IActionResult> VerifyHasCorrectOption(Guid questionId)
        {
            var hasCorrectOption = await _questionService.HasCorrectOptionAsync(questionId);
            return View(hasCorrectOption);
        }
        #endregion

        #region [- Create() -]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var options = await _optionService.GetAllOptionsAsync(); 
            ViewBag.Options = options;
            return View(new Question());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question model)
        {

            ViewBag.Options = await _optionService.GetAllOptionsAsync();


            model.Id = Guid.NewGuid();
            await _questionService.CreateQuestionAsync(model);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region [- CreateBulk() -]
        [HttpGet]
        public async Task<IActionResult> CreateBulk()
        {
            var options = await _optionService.GetAllOptionsAsync();
            ViewBag.Options = options;
            return View(new List<Question>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBulk(List<Question> model)
        {
            ViewBag.Options = await _optionService.GetAllOptionsAsync();

            if (model == null || !model.Any())
            {
                ModelState.AddModelError("", "Please provide at least one question.");
                return View(model);
            }

            // Assign new GUIDs for all questions
            foreach (var question in model)
            {
                question.Id = Guid.NewGuid();
            }

            var result = await _questionService.CreateQuestionsAsync(model);

            if (!result)
            {
                ModelState.AddModelError("", "Failed to add questions.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        } 
        #endregion

        #region [- Edit() -]

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Question model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var updateSuccess = await _questionService.UpdateQuestionAsync(model);

            if (!updateSuccess)
            {
                TempData["ErrorMessage"] = "Failed to update the question.";
                return View(model);
            }

            TempData["SuccessMessage"] = "Question updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region [- Delete() -]

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var deleteSuccess = await _questionService.DeleteQuestionAsync(id);

            if (!deleteSuccess)
            {
                TempData["ErrorMessage"] = "Failed to delete the question. It may be linked to other records.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["SuccessMessage"] = "Question deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region [- AddOptions() -]

        [HttpGet]
        public async Task<IActionResult> AddOptions(Guid questionId)
        {
            var question = await _questionService.GetWithOptionsAsync(questionId);
            if (question == null)
            {
                return NotFound();
            }

            var allOptions = await _optionService.GetAllOptionsAsync();
            var assignedOptionIds = (await _questionOptionService.GetByQuestionIdAsync(questionId))
                .Select(qo => qo.OptionId)
                .ToList();

            var availableOptions = allOptions
                .Where(o => !assignedOptionIds.Contains(o.Id))
                .ToList();

            ViewBag.QuestionId = questionId;
            ViewBag.QuestionTitle = question.Title;
            ViewBag.AvailableOptions = availableOptions;
            return View(questionId); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOptions(Guid questionId, IEnumerable<Guid> optionIds, Guid? CorrectOptionId)
        {
            if (optionIds == null || !optionIds.Any())
            {
                TempData["ErrorMessage"] = "No options selected.";
                return RedirectToAction(nameof(AddOptions), new { questionId });
            }

            if (CorrectOptionId.HasValue && !optionIds.Contains(CorrectOptionId.Value))
            {
                TempData["ErrorMessage"] = "Correct option must be among the selected options.";
                return RedirectToAction(nameof(AddOptions), new { questionId });
            }

            var addSuccess = await _questionOptionService.AddOptionsToQuestionAsync(questionId, optionIds);
            if (!addSuccess)
            {
                TempData["ErrorMessage"] = "Failed to add options to the question.";
                return RedirectToAction(nameof(AddOptions), new { questionId });
            }

            if (CorrectOptionId.HasValue)
            {
                var setCorrectSuccess = await _questionService.SetCorrectOptionAsync(questionId, CorrectOptionId.Value);
                if (!setCorrectSuccess)
                {
                    TempData["WarningMessage"] = "Options added, but setting the correct answer failed.";
                }
            }

            TempData["SuccessMessage"] = "Options successfully added to question.";
            return RedirectToAction(nameof(Details), new { id = questionId });
        }


        #endregion

        #region [- RemoveOptions() -]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveOptions(Guid questionId, IEnumerable<Guid> optionIds)
        {
            if (optionIds == null || !optionIds.Any())
            {
                TempData["ErrorMessage"] = "No options selected for removal";
                return RedirectToAction(nameof(Details), new { id = questionId });
            }

            var success = await _questionOptionService.RemoveOptionsFromQuestionAsync(questionId, optionIds);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to remove options from question";
            }

            return RedirectToAction(nameof(Details), new { id = questionId });
        }
        #endregion
    }
}