using AtiExamSite.Models.DomainModels.Exam;
using AtiExamSite.Services.Contracts;
using AtiExamSite.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AtiExamSite.Controllers
{
    public class UserResponseController : Controller
    {
        private readonly IUserResponseService _userResponseService;
        private readonly IQuestionService _questionService;
        private readonly IExamService _examService;
        private readonly IExamQuestionService _examQuestionService;
        private readonly IExamSessionService _examSessionService;


        #region [- Ctor() -]
        public UserResponseController(
            IUserResponseService userResponseService,
            IQuestionService questionService,
            IExamService examService,
            IExamQuestionService examQuestionService,
            IExamSessionService examSessionService)
        {
            _userResponseService = userResponseService ?? throw new ArgumentNullException(nameof(userResponseService));
            _questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
            _examService = examService ?? throw new ArgumentNullException(nameof(examService));
            _examQuestionService=examQuestionService;
            _examSessionService = examSessionService;
        }
        #endregion

        #region [- Index() -]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = UserIdHelper.GetCurrentUserId(HttpContext) ?? Guid.Empty;

            var exams = await _examService.GetAllAsync() ?? new List<Exam>();

            return View(exams);
        }
        #endregion

        #region [- GetRandomQuestions() -]
        public async Task<IActionResult> GetRandomQuestions(int count)
        {
            try
            {
                var questions = await _examService.GetRandomQuestionsAsync(count);
                return View(questions);
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region [- GetByExam() -]
        [HttpGet]
        public async Task<IActionResult> GetByExam(string examId)
        {
            if (examId == string.Empty)
                return BadRequest("Invalid exam ID.");

            var responses = await _userResponseService.GetByExamAsync(examId) ?? new List<UserResponse>();

            return View(responses);
        }
        #endregion

        #region [- GetByUser() -]
        //[HttpGet]
        //public async Task<IActionResult> GetByUser(Guid? userId)
        //{
        //    if (userId == null || userId == Guid.Empty)
        //        return BadRequest("Valid User ID is required.");

        //    var responses = await _userResponseService.GetByUserAsync(userId.Value) ?? new List<UserResponse>();

        //    return View(responses);
        //}
        #endregion

        #region [- GetScore() -]
        [HttpGet]
        public async Task<IActionResult> GetScore(string examId)
        {
            if (string.IsNullOrWhiteSpace(examId))
                return BadRequest("Invalid exam ID.");

            var (score, passed) = await _userResponseService.CalculateExamScoreAsync(examId);
            ViewBag.Score = score;
            ViewBag.Passed = passed;

            var exam = await _examService.GetByIdAsync(examId);
            if (exam == null)
                return NotFound("Exam not found.");

            return View(exam);
        }

        #endregion

        #region [- Create() -]
        [HttpGet]
        public async Task<IActionResult> Create(string examId)
        {
            //var userId = UserIdHelper.GetCurrentUserId(HttpContext) ?? string.Empty;

            var exam = await _examService.GetByIdAsync(examId);
            if (exam == null)
                return NotFound("Exam not found.");

            var session = await _examSessionService.GetSessionAsync(examId);
            if (session == null)
            {
                // No active session, create new one
                session = new ExamSession
                {
                    Id = Guid.NewGuid().ToString(),
                    ExamId = examId,
                    StartTime = DateTime.UtcNow
                };
                await _examSessionService.CreateAsync(session);
            }
            else if (session.IsCompleted)
            {
                return RedirectToAction(nameof(GetScore), new { examId });
            }

            // Time check for ongoing session
            var timeElapsed = DateTime.UtcNow - session.StartTime;
            var allowedTime = TimeSpan.FromMinutes(exam.TimeLimitMinutes ?? 0);

            if (timeElapsed > allowedTime)
            {
                TempData["Error"] = "Time expired.";
                return RedirectToAction(nameof(GetScore), new { examId });
            }

            var questions = await _examQuestionService.GetExamQuestionsAsync(examId);
            questions = questions.OrderBy(_ => Guid.NewGuid()).Take(exam.RequiredQuestion).ToList();

            ViewBag.ExamId = examId;
            ViewBag.RemainingSeconds = (int)(allowedTime - timeElapsed).TotalSeconds;

            return View(questions);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string examId, List<UserResponse> responses)
        {
            if (responses == null || !responses.Any())
            {
                ModelState.AddModelError("", "Please submit at least one response.");
                var questions = await _questionService.GetWithOptionsAsync(examId);
                ViewBag.ExamId = examId;
                return View(questions);
            }

            // Get existing session
            var session = await _examSessionService.GetSessionAsync(examId);
            if (session == null)
            {
                return BadRequest("Session not found.");
            }

            if (session.IsCompleted)
            {
                TempData["Error"] = "This exam session has already been completed.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var response in responses)
            {
                response.ExamId = examId;
            }

            var success = await _userResponseService.SubmitResponsesAsync(responses);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to submit responses.");
                var questions = await _questionService.GetWithOptionsAsync(examId);
                ViewBag.ExamId = examId;
                return View(questions);
            }

            // **End the session immediately**
            session.EndTime = DateTime.UtcNow;
            await _examSessionService.UpdateAsync(session);

            return RedirectToAction(nameof(GetScore), new { examId });
        }

        #endregion
    }
}
