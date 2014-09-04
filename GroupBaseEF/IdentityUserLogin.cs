
namespace Identity.GroupBaseEF
{
    public class IdentityUserLogin<TKey>
    {
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual TKey UserId { get; set; }
    }
    public class IdentityUserLogin : IdentityUserLogin<string>
    {
    }
}
