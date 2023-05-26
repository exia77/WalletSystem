namespace BusinessObjects
{
    public class UsersModelObject
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime RegisterDate { get; set; }

        public AccountsModelObject? Accounts { get; set; }
    }
}