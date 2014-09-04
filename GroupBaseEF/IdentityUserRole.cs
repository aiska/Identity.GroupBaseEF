
namespace Identity.GroupBaseEF
{
    public class IdentityUserRole<TKey>
    {
        public virtual TKey UserId { get; set; }
        public virtual TKey RoleId { get; set; }
    }
    public class IdentityUserRole : IdentityUserRole<string>
    {
    }
}
