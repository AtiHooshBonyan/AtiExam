using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace AtiExamSite.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IExamRepository _examRepository;

        #region [- Ctor() -]
        public QuestionsController(IQuestionRepository questionRepository, IExamRepository examRepository)
        {
            _questionRepository = questionRepository;
            _examRepository = examRepository;
        }
        #endregion

        #region [- Index() -]
        public async Task<IActionResult> Index()
        {
            var questions = await _questionRepository.GetQuestionsWithAnswersAsync();
            return View(questions);
        }
        #endregion

        #region [- Details() -]
        public async Task<IActionResult> Details(Guid id)
        {
            var question = await _questionRepository.GetQuestionWithAnswersAsync(id);
            return question == null ? NotFound() : View(question);
        }
        #endregion

        #region [- Create() -]
        public async Task<IActionResult> Create()
        {
            ViewBag.Exams = await _examRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question question)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Exams = await _examRepository.GetAllAsync();
                return View(question);
            }

            if (string.IsNullOrWhiteSpace(question.Text))
            {
                ModelState.AddModelError("Text", "Question text is required");
                ViewBag.Exams = await _examRepository.GetAllAsync();
                return View(question);
            }

            await _questionRepository.AddAsync(question);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region [- Edit() -]
        public async Task<IActionResult> Edit(Guid id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewBag.Exams = await _examRepository.GetAllAsync();
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _questionRepository.UpdateAsync(question);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Exams = await _examRepository.GetAllAsync();
            return View(question);
        }
        #endregion

        #region [- Delete() -] 
        public async Task<IActionResult> Delete(Guid id)
        {
            var question = await _questionRepository.GetQuestionWithAnswersAsync(id);
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
            await _questionRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion


    }
}