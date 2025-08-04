using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels.Exam;
using AtiExamSite.Services.Contracts;

namespace AtiExamSite.Services.Implementations
{
    public class ExamSessionService : IExamSessionService
    {
        private readonly IExamSessionRepository _examSessionRepository;

        #region [- Ctor() -]
        public ExamSessionService(IExamSessionRepository examSessionRepository)
        {
            _examSessionRepository = examSessionRepository;
        }
        #endregion

        #region [- GetSessionAsync() -]
        public async Task<ExamSession> GetSessionAsync(Guid examId)
            => await _examSessionRepository.GetByUserAndExamAsync(examId);
        #endregion

        #region [- CreateAsync() -]
        public async Task<bool> CreateAsync(ExamSession session)
        {
            await _examSessionRepository.AddAsync(session);
            return true;
        }
        #endregion

        #region [- UpdateAsync() -]
        public async Task<bool> UpdateAsync(ExamSession session)
        {
            await _examSessionRepository.UpdateAsync(session);
            return true;
        } 
        #endregion
    }

}
