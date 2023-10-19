using HisoBOT.DB;
using HisoBOT.Models;

namespace HisoBOT.Services
{
    public class ProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context) 
        {
            _context = context;
        }

        public void CreateProject(string chatId, string projectName)
        {
            _context.Projects.Add(new Project()
            {
                Id = Guid.NewGuid(),
                ChatId = chatId,
                Name = projectName
            });

            _context.SaveChanges();
        }
    }
}
