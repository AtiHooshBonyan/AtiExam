using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace AtiExamSite.Controllers
{
    public class UserResponsesController : Controller
    {
        private readonly IUserResponseRepository _userResponseRepository;
        private readonly IExamSessionRepository _examSessionRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerOptionRepository _answerOptionRepository;

        #region [- Ctor() -]
        public UserResponsesController(
            IUserResponseRepository userResponseRepository,
            IExamSessionRepository examSessionRepository,
            IQuestionRepository questionRepository,
            IAnswerOptionRepository answerOptionRepository)
        {
            _userResponseRepository = userResponseRepository;
            _examSessionRepository = examSessionRepository;
            _questionRepository = questionRepository;
            _answerOptionRepository = answerOptionRepository;
        }
        #endregion

        #region [- Index() -]
        public async Task<IActionResult> Index()
        {
            var responses = await _userResponseRepository.GetAllAsync();
            return View(responses);
        }
        #endregion

        #region [- Details() -]
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _userResponseRepository.GetByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }
        public async Task<IActionResult> DetailsBySession(Guid sessionId)
        {
            var responses = await _userResponseRepository.GetResponsesBySessionAsync(sessionId);
            ViewBag.Session = await _examSessionRepository.GetByIdAsync(sessionId);
            return View(responses);
        }
        #endregion

        #region [- Create() -]
        public async Task<IActionResult> Create(Guid sessionId, Guid questionId)
        {
            var session = await _examSessionRepository.GetByIdAsync(sessionId);
            var question = await _questionRepository.GetQuestionWithAnswersAsync(questionId);

            if (session == null || question == null)
            {
                return NotFound();
            }

            ViewBag.Session = session;
            ViewBag.Question = question;
            ViewBag.AnswerOptions = await _answerOptionRepository.GetAnswerOptionsByQuestionAsync(questionId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserResponse userResponse)
        {
            if (!ModelState.IsValid)
            {
                var session = await _examSessionRepository.GetByIdAsync(userResponse.SessionId);
                var question = await _questionRepository.GetByIdAsync(userResponse.QuestionId);
                ViewBag.Session = session;
                ViewBag.Question = question;
                ViewBag.AnswerOptions = await _answerOptionRepository.GetAnswerOptionsByQuestionAsync(userResponse.QuestionId);
                return View(userResponse);
            }

            // Check if session is still active
            if (!await _examSessionRepository.IsSessionActiveAsync(userResponse.SessionId))
            {
                return RedirectToAction("ExamExpired", new { sessionId = userResponse.SessionId });
            }

            // Check if answer is correct
            if (userResponse.AnswerId.HasValue)
            {
                var answer = await _answerOptionRepository.GetByIdAsync(userResponse.AnswerId.Value);
                userResponse.IsCorrect = answer?.IsCorrect;
            }

            userResponse.ResponseTime = DateTime.UtcNow;
            await _userResponseRepository.AddAsync(userResponse);
            return RedirectToAction("Details", "ExamSessions", new { id = userResponse.SessionId });
        }
        #endregion

        #region [- Edit() -]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _userResponseRepository.GetByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }

            var session = await _examSessionRepository.GetByIdAsync(response.SessionId);
            var question = await _questionRepository.GetByIdAsync(response.QuestionId);

            ViewBag.Session = session;
            ViewBag.Question = question;
            ViewBag.AnswerOptions = await _answerOptionRepository.GetAnswerOptionsByQuestionAsync(response.QuestionId);

            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserResponse userResponse)
        {
            if (id != userResponse.ResponseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if the answer is correct
                if (userResponse.AnswerId.HasValue)
                {
                    var answer = await _answerOptionRepository.GetByIdAsync(userResponse.AnswerId.Value);
                    userResponse.IsCorrect = answer?.IsCorrect;
                }

                await _userResponseRepository.UpdateAsync(userResponse);
                return RedirectToAction("Details", "ExamSessions", new { id = userResponse.SessionId });
            }

            var session = await _examSessionRepository.GetByIdAsync(userResponse.SessionId);
            var question = await _questionRepository.GetByIdAsync(userResponse.QuestionId);

            ViewBag.Session = session;
            ViewBag.Question = question;
            ViewBag.AnswerOptions = await _answerOptionRepository.GetAnswerOptionsByQuestionAsync(userResponse.QuestionId);

            return View(userResponse);
        }
        #endregion

        #region [- Delete() -]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _userResponseRepository.GetByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var response = await _userResponseRepository.GetByIdAsync(id);
            await _userResponseRepository.DeleteAsync(id);
            return RedirectToAction("Details", "ExamSessions", new { id = response.SessionId });
        }
        #endregion

        #region [- CalculateScore() -]
        public async Task<IActionResult> CalculateScore(Guid sessionId)
        {
            var score = await _userResponseRepository.CalculateSessionScoreAsync(sessionId);
            ViewBag.Session = await _examSessionRepository.GetByIdAsync(sessionId);
            return View(score);
        } 
        #endregion

    }
}