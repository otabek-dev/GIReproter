namespace HisoBOT.Models
{
    public class User
    {
        public long Id { get; set; }
        public UserState UserState { get; set; } = UserState.None;
    }
}
