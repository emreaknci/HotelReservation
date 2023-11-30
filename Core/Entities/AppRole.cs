namespace Core.Entities
{
    public class AppRole : BaseEntity
    {
        public AppRole()
        {
            Users = new HashSet<AppUser>();
        }
        public string? Name { get; set; }
        public virtual ICollection<AppUser>? Users { get; set; }

    }
}
