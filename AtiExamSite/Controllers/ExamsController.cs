using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace AtiExamSite.Controllers
{
    public class ExamsController : Controller
    {

        private readonly IExamRepository _examRepository;

        #region [- Ctor() -]
        public ExamsController(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }
        #endregion

        #region [- Index() -]
        public async Task<IActionResult> Index()
        {
            var exams = await _examRepository.GetAllWithQuestionsAsync();
            return View(exams);
        }
        #endregion

        #region [- Details() -]
        public async Task<IActionResult> Details(Guid id)
        {
            var exam = await _examRepository.GetWithQuestionsAsync(id);
            return exam == null ? NotFound() : View(exam);
        }
        #endregion

        #region [- Create() -]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exam exam)
        {
            if (!ModelState.IsValid) return View(exam);

            if (exam.QuestionsToShow <= 0 || exam.PassingScore <= 0)
            {
                ModelState.AddModelError("", "QuestionsToShow and PassingScore must be greater than 0");
                return View(exam);
            }

            await _examRepository.AddAsync(exam);
            return RedirectToAction(nameof(Index));
        } 
        #endregion

        #region [- Edit() -]
        public async Task<IActionResult> Edit(Guid id)
        {
            var exam = await _examRepository.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Exam exam)
        {
            if (id != exam.ExamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _examRepository.UpdateAsync(exam);
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }
        #endregion

        #region [- Delete() -]
        public async Task<IActionResult> Delete(Guid id)
        {
            var exam = await _examRepository.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _examRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        } 
        #endregion
    }
}