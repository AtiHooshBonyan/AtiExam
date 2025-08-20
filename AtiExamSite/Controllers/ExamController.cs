using AtiExamSite.Models.DomainModels.Exam;
using AtiExamSite.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AtiExamSite.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        private readonly IQuestionService _questionService;
        private readonly IExamQuestionService _examQuestionService;

        #region [- Ctor() -]
        public ExamController(IExamService examService, IExamQuestionService examQuestionService, IQuestionService questionService)
        {
            _examService = examService;
            _examQuestionService = examQuestionService;
            _questionService = questionService;
        }
        #endregion

        #region [- Index() -]
        public async Task<IActionResult> Index()
        {
            var exams = await _examService.GetAllAsync();
            return View(exams);
        }
        #endregion

        #region [- Details() -]
        public async Task<IActionResult> Details(string id)
        {
            var exam = await _examService.GetExamWithQuestionsAsync(id);
            if (exam == null) return NotFound();

            return View(exam);
        }
        #endregion

        #region [- Create() -]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Exam { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exam exam)
        {

            var success = await _examService.AddAsync(exam);

            if (!success)
            {
                ModelState.AddModelError("", "Failed to create exam");
                return View(exam);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region [- Edit() -]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null) return NotFound();
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Exam exam)
        {
            if (id != exam.Id) return NotFound();

            var success = await _examService.UpdateAsync(exam);

            if (!success)
            {
                ModelState.AddModelError("", "Failed to update exam");
                return View(exam);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region [- Delete() -]
        public async Task<IActionResult> Delete(string id)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _examService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region [- AddQuestions() -]
        [HttpGet]
        public async Task<IActionResult> AddQuestions(string examId)
        {
            var exam = await _examService.GetExamWithQuestionsAsync(examId);
            if (exam == null)
            {
                return NotFound();
            }

            var allQuestions = await _questionService.GetAllQuestionsAsync();
            var assignedQuestionIds = exam.ExamQuestions.Select(eq => eq.QuestionId).ToHashSet();

            var availableQuestions = allQuestions
                .Where(q => !assignedQuestionIds.Contains(q.Id))
                .ToList();

            ViewBag.ExamId = exam.Id;
            ViewBag.ExamTitle = exam.Title;
            ViewBag.AvailableQuestions = availableQuestions;

            return View(exam);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestions(string examId, IEnumerable<string> questionIds)
        {
            var success = await _examQuestionService.AddQuestionsToExamAsync(examId, questionIds);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to add questions to exam";
            }

            return RedirectToAction(nameof(Details), new { id = examId });
        }
        #endregion

        #region [- QuestionCount() -]
        public async Task<IActionResult> QuestionCount(string examId)
        {
            var count = await _examQuestionService.CountQuestionsInExamAsync(examId);
            return View(count);
        }
        #endregion
    }
}