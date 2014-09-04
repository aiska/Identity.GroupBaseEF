
namespace Identity.GroupBaseEF
{
    public class IdentityUserClaim<TKey>
    {
        public virtual int Id { get; set; }
        public virtual TKey UserId { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
    }
    public class IdentityUserClaim : IdentityUserClaim<string>
    {
    }
}
