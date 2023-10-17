using HisoBOT.DB;

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
            Console.WriteLine($"{chatId} {projectName}");
        }
    }
}
