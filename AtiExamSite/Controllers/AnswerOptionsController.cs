using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace AtiExamSite.Controllers
{
    public class AnswerOptionsController : Controller
    {
        private readonly IAnswerOptionRepository _answerOptionRepository;
        private readonly IQuestionRepository _questionRepository;

        #region [- Ctor() -]
        public AnswerOptionsController(IAnswerOptionRepository answerOptionRepository, IQuestionRepository questionRepository)
        {
            _answerOptionRepository = answerOptionRepository;
            _questionRepository = questionRepository;
        }
        #endregion

        #region [- Index() -]
        public async Task<IActionResult> Index()
        {
            var answerOptions = await _answerOptionRepository.GetAllAsync();
            return View(answerOptions);
        }
        #endregion

        #region [- Details() -]
        public async Task<IActionResult> Details(Guid id)
        {
            var answerOption = await _answerOptionRepository.GetByIdAsync(id);
            if (answerOption == null)
            {
                return NotFound();
            }
            return View(answerOption);
        }
        public async Task<IActionResult> DetailsByQuestion(Guid questionId)
        {
            var answerOptions = await _answerOptionRepository.GetAnswerOptionsByQuestionAsync(questionId);
            ViewBag.Question = await _questionRepository.GetByIdAsync(questionId);
            return View(answerOptions);
        }
        #endregion

        #region [- Create() -]
        public async Task<IActionResult> Create(Guid questionId)
        {
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null) return NotFound();

            ViewBag.Question = question;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnswerOption answerOption)
        {
            if (!ModelState.IsValid)
            {
                var question = await _questionRepository.GetByIdAsync(answerOption.QuestionId);
                ViewBag.Question = question;
                return View(answerOption);
            }

            if (string.IsNullOrWhiteSpace(answerOption.Text))
            {
                ModelState.AddModelError("Text", "Answer text is required");
                var question = await _questionRepository.GetByIdAsync(answerOption.QuestionId);
                ViewBag.Question = question;
                return View(answerOption);
            }

            await _answerOptionRepository.AddAsync(answerOption);
            return RedirectToAction("Details", "Questions", new { id = answerOption.QuestionId });
        }
        #endregion

        #region [- Edit() -]
        public async Task<IActionResult> Edit(Guid id)
        {
            var answerOption = await _answerOptionRepository.GetByIdAsync(id);
            if (answerOption == null)
            {
                return NotFound();
            }
            var question = await _questionRepository.GetByIdAsync(answerOption.QuestionId);
            ViewBag.Question = question;
            return View(answerOption);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AnswerOption answerOption)
        {
            if (id != answerOption.AnswerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _answerOptionRepository.UpdateAsync(answerOption);
                return RedirectToAction("Details", "Questions", new { id = answerOption.QuestionId });
            }
            var question = await _questionRepository.GetByIdAsync(answerOption.QuestionId);
            ViewBag.Question = question;
            return View(answerOption);
        }
        #endregion

        #region [- Delete() -]
        public async Task<IActionResult> Delete(Guid id)
        {
            var answerOption = await _answerOptionRepository.GetByIdAsync(id);
            if (answerOption == null)
            {
                return NotFound();
            }
            return View(answerOption);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var answerOption = await _answerOptionRepository.GetByIdAsync(id);
            await _answerOptionRepository.DeleteAsync(id);
            return RedirectToAction("Details", "Questions", new { id = answerOption.QuestionId });
        } 
        #endregion
    }
}