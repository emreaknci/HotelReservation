namespace Core.Entities
{
    public class AppUser : BaseEntity
    {
        public AppUser()
        {
            //Roles = new HashSet<AppRole>();
        }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? ResetPasswordToken { get; set; }
        //public virtual ICollection<AppRole>? Roles { get; set; }
        public UserType UserType { get; set; }
    }
    public enum UserType
    {
        Admin,
        Customer
    }
}
