using HisoBOT.DB;
using HisoBOT.Models;
using HisoBOT.Results;

namespace HisoBOT.Services
{
    public class ProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context) 
        {
            _context = context;
        }

        public Result CreateProject(string chatId, string projectName)
        {
            if (_context.Projects.Any(x => x.ChatId == chatId))
            {
                return new Result(false, "Проект с таким chat id уже существует! Введите другую...");
            }

            _context.Projects.Add(new Project()
            {
                Id = Guid.NewGuid(),
                ChatId = chatId,
                Name = projectName
            });

            _context.SaveChanges();
            return new Result(true, "Проект создан");
        }
    }
}
