using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniQuanda.Core.Application.Repositories;
using UniQuanda.Core.Domain.Entities.App;
using UniQuanda.Infrastructure.Presistence.AppDb;

namespace UniQuanda.Infrastructure.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _context;
        public TestRepository(AppDbContext context)
        {
            _context = context;
        }

		public async Task<IEnumerable<AutomaticTestQuestionEntity>> GetAutomaticTestQuestionsAsync(IEnumerable<int> tagsIds, CancellationToken ct)
		{
			return await _context.TagsInQuestions
                .Where(tq => tagsIds.Contains(tq.TagId) 
                    || (tq.TagIdNavigation.ParentTagId != null && tagsIds.Contains(tq.TagIdNavigation.ParentTagId.Value)))
                .Where(tq => tq.QuestionIdNavigation.Answers.Any(a => a.IsCorrect))
                .OrderBy(tq => Guid.NewGuid())
                .Take(10)
                .Select(tq => new AutomaticTestQuestionEntity 
                {
                    Id = tq.QuestionId,
                    CreatedAt = tq.QuestionIdNavigation.CreatedAt,
                    Header = tq.QuestionIdNavigation.Header,
                    HTML = tq.QuestionIdNavigation.ContentIdNavigation.RawText,
                    Answer = tq.QuestionIdNavigation.Answers
                        .Where(a => a.IsCorrect)
                        .Select(a => new AutomaticTestAnswerEntity {
                            Id = a.Id,
                            HTML = a.ContentIdNavigation.RawText,
                            CreatedAt = a.CreatedAt,
                            CommentsCount = a.Comments.Count()
                        })
                        .SingleOrDefault()!
                })
                .ToListAsync(ct);
		}
	}
}