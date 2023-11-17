namespace GIReporter.Models
{
    public class User
    {
        public long Id { get; set; }
        public State UserState { get; set; } = State.All;
    }
}
