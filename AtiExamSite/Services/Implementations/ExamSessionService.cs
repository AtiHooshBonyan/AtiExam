using AtiExamSite.Data.Repositories.Contracts;
using AtiExamSite.Models.DomainModels;
using AtiExamSite.Services.Contracts;

namespace AtiExamSite.Services.Implementations
{
    public class ExamSessionService : IExamSessionService
    {
        private readonly IExamSessionRepository _examSessionRepository;

        public ExamSessionService(IExamSessionRepository examSessionRepository)
        {
            _examSessionRepository = examSessionRepository;
        }

        public async Task<ExamSession> GetSessionAsync(Guid examId)
            => await _examSessionRepository.GetByUserAndExamAsync(examId);

        public async Task<bool> CreateAsync(ExamSession session)
        {
            await _examSessionRepository.AddAsync(session);
            return true;
        }

        public async Task<bool> UpdateAsync(ExamSession session)
        {
            await _examSessionRepository.UpdateAsync(session);
            return true;
        }
    }

}
