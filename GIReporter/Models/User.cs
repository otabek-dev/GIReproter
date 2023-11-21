using GIReporter.Commands.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIReporter.Models
{
    public class User
    {
        public long Id { get; set; }
        public State UserState { get; set; } = State.Any;
        public string? InProcessCommandName { get; set; } = null;
    }
}
