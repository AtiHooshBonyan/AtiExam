using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace AtiExamSite.Controllers
{
    public class ExamSessionsController : Controller
    {
        private readonly IExamSessionRepository _examSessionRepository;
        private readonly IExamRepository _examRepository;
        private readonly IUserResponseRepository _userResponseRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerOptionRepository _answerOptionRepository;

        #region [- Ctor() -]
        public ExamSessionsController(
            IExamSessionRepository examSessionRepository,
            IExamRepository examRepository,
            IUserResponseRepository userResponseRepository,
            IQuestionRepository questionRepository,
            IAnswerOptionRepository answerOptionRepository)
        {
            _examSessionRepository = examSessionRepository;
            _examRepository = examRepository;
            _userResponseRepository = userResponseRepository;
            _questionRepository = questionRepository;
            _answerOptionRepository = answerOptionRepository;
        }
        #endregion

        #region [- Index() -]
        public async Task<IActionResult> Index()
        {
            var sessions = await _examSessionRepository.GetAllAsync();
            return View(sessions);
        }
        #endregion

        #region [- Details() -]
        public async Task<IActionResult> Details(Guid id)
        {
            var session = await _examSessionRepository.GetSessionWithDetailsAsync(id);
            return session == null ? NotFound() : View(session);
        }
        public async Task<IActionResult> DetailsByUser(string userId)
        {
            var sessions = await _examSessionRepository.GetSessionsByUserAsync(userId);
            return View(sessions);
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
        public async Task<IActionResult> Create(ExamSession examSession)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Exams = await _examRepository.GetAllAsync();
                return View(examSession);
            }

            examSession.StartTime = DateTime.Now;
            examSession.Status = "In Progress";
            await _examSessionRepository.AddAsync(examSession);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region [- Edit() -]
        public async Task<IActionResult> Edit(Guid id)
        {
            var session = await _examSessionRepository.GetByIdAsync(id);
            if (session == null) return NotFound();

            ViewBag.Exams = await _examRepository.GetAllAsync();
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ExamSession examSession)
        {
            if (id != examSession.SessionId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Exams = await _examRepository.GetAllAsync();
                return View(examSession);
            }

            await _examSessionRepository.UpdateAsync(examSession);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region [- Delete() -]
        public async Task<IActionResult> Delete(Guid id)
        {
            var session = await _examSessionRepository.GetByIdAsync(id);
            return session == null ? NotFound() : View(session);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _examSessionRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region [- ExamHandler() -]

        #region [- SessionComplete() -]
        public async Task<IActionResult> SessionComplete(Guid id)
        {
            var session = await _examSessionRepository.GetByIdAsync(id);
            if (session == null) return NotFound();

            session.EndTime = DateTime.Now;
            session.Status = "Completed";
            await _examSessionRepository.UpdateAsync(session);

            return RedirectToAction("Details", new { id });
        }
        #endregion

        #region [- StartExam() -]
        public async Task<IActionResult> StartExam(Guid examId)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (await _examSessionRepository.HasUserTakenExamAsync(userId, examId))
            {
                return RedirectToAction("AlreadyTaken");
            }

            var session = await _examSessionRepository.StartNewSessionAsync(examId, userId);
            return RedirectToAction("TakeExam", new { sessionId = session.SessionId });
        }
        #endregion

        #region [- TakeExam() -]
        public async Task<IActionResult> TakeExam(Guid sessionId)
        {
            var session = await _examSessionRepository.GetSessionWithDetailsAsync(sessionId);
            if (session == null || !await _examSessionRepository.IsSessionActiveAsync(sessionId))
            {
                return RedirectToAction("ExamExpired", new { sessionId });
            }

            var answeredQuestionIds = session.UserResponses.Select(r => r.QuestionId).ToList();
            var exam = await _examRepository.GetByIdAsync(session.ExamId);
            var remainingQuestions = (await _questionRepository.GetQuestionsByExamIdAsync(session.ExamId))
                .Where(q => !answeredQuestionIds.Contains(q.QuestionId))
                .OrderBy(q => Guid.NewGuid())
                .Take(exam.QuestionsToShow - answeredQuestionIds.Count)
                .ToList();

            if (!remainingQuestions.Any())
            {
                await _examSessionRepository.CompleteSessionAsync(sessionId);
                return RedirectToAction("ExamComplete", new { sessionId });
            }

            ViewBag.Session = session;
            ViewBag.TimeRemaining = exam.TimeLimitMinutes.HasValue
                ? session.StartTime.AddMinutes(exam.TimeLimitMinutes.Value) - DateTime.UtcNow
                : TimeSpan.MaxValue;

            return View(remainingQuestions.First());
        }
        #endregion

        #region [- SubmitAnswer() -]
        [HttpPost]
        public async Task<IActionResult> SubmitAnswer(Guid sessionId, Guid questionId, Guid? answerId)
        {
            if (!await _examSessionRepository.IsSessionActiveAsync(sessionId))
            {
                return Json(new { expired = true });
            }

            var existingResponse = (await _userResponseRepository.FindAsync(r =>
                r.SessionId == sessionId && r.QuestionId == questionId)).FirstOrDefault();

            var response = existingResponse ?? new UserResponse
            {
                ResponseId = Guid.NewGuid(),
                SessionId = sessionId,
                QuestionId = questionId,
                ResponseTime = DateTime.UtcNow
            };

            response.AnswerId = answerId;
            if (answerId.HasValue)
            {
                var answer = await _answerOptionRepository.GetByIdAsync(answerId.Value);
                response.IsCorrect = answer?.IsCorrect;
            }

            if (existingResponse != null)
            {
                await _userResponseRepository.UpdateAsync(response);
            }
            else
            {
                await _userResponseRepository.AddAsync(response);
            }

            return Json(new { success = true });
        }
        #endregion

        #region [- ExamComplete() -]
        public async Task<IActionResult> ExamComplete(Guid sessionId)
        {
            var session = await _examSessionRepository.GetSessionWithDetailsAsync(sessionId);
            if (session == null) return NotFound();

            ViewBag.Score = await _userResponseRepository.CalculateSessionScoreAsync(sessionId);
            return View(session);
        }
        #endregion

        #region [- ExamExpired() -]
        public async Task<IActionResult> ExamExpired(Guid sessionId)
        {
            var session = await _examSessionRepository.GetSessionWithDetailsAsync(sessionId);
            if (session == null) return NotFound();

            ViewBag.Score = await _userResponseRepository.CalculateSessionScoreAsync(sessionId);
            return View(session);
        }
        #endregion

        #region [- AlreadyTaken() -]
        public IActionResult AlreadyTaken()
        {
            return View();
        }
        #endregion 

        #endregion
    }
}