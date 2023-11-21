using GIReporter.Models;

namespace GIReporter.States
{
    public class UserStateAttribute : Attribute
    {
        public State State { get; init; }

        public UserStateAttribute(State state)
        {
            State = state;
        }
    }
}
