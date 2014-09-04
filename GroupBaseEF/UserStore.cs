using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.GroupBaseEF
{
    public class UserStore<TUser, TRole, TKey, TUserLogin, TUserClaim, TGroup, TGroupRole> :
        IUserLoginStore<TUser, TKey>,
        IUserClaimStore<TUser, TKey>,
        IUserGroupStore<TUser, TKey>,
        IUserRoleStore<TUser, TKey>,
        IUserPasswordStore<TUser, TKey>,
        IUserSecurityStampStore<TUser, TKey>,
        IQueryableUserStore<TUser, TKey>,
        IUserEmailStore<TUser, TKey>,
        IUserPhoneNumberStore<TUser, TKey>,
        IUserTwoFactorStore<TUser, TKey>,
        IUserLockoutStore<TUser, TKey>,
        IUserStore<TUser, TKey>,
        IDisposable
        where TUser : IdentityUser<TKey, TUserLogin, TUserClaim, TGroup, TGroupRole>
        where TGroup : IdentityGroup<TKey, TGroupRole>
        where TRole : IdentityRole<TKey, TGroupRole>
        where TKey : IEquatable<TKey>
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TGroupRole : IdentityGroupRole<TKey>, new()
    {
        private readonly IDbSet<TUserLogin> _logins;
        private readonly EntityStore<TGroup> _groupStore;
        private readonly EntityStore<TRole> _roleStore;
        private readonly IDbSet<TUserClaim> _userClaims;
        private readonly IDbSet<TGroupRole> _groupRole;
        private bool _disposed;
        private EntityStore<TUser> _userStore;
        public DbContext Context { get; private set; }
        public bool DisposeContext { get; set; }
        public bool AutoSaveChanges { get; set; }
        public IQueryable<TUser> Users
        {
            get
            {
                return this._userStore.EntitySet;
            }
        }
        public UserStore(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.Context = context;
            this.AutoSaveChanges = true;
            this._userStore = new EntityStore<TUser>(context);
            this._roleStore = new EntityStore<TRole>(context);
            this._groupStore = new EntityStore<TGroup>(context);
            this._logins = this.Context.Set<TUserLogin>();
            this._userClaims = this.Context.Set<TUserClaim>();
            this._groupRole = this.Context.Set<TGroupRole>();
        }
        public virtual Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            IList<Claim> result = (
                from c in user.Claims
                select new Claim(c.ClaimType, c.ClaimValue)).ToList<Claim>();
            return Task.FromResult<IList<Claim>>(result);
        }
        public virtual Task AddClaimAsync(TUser user, Claim claim)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            ICollection<TUserClaim> arg_7B_0 = user.Claims;
            TUserClaim item = Activator.CreateInstance<TUserClaim>();
            item.UserId = user.Id;
            item.ClaimType = claim.Type;
            item.ClaimValue = claim.Value;
            arg_7B_0.Add(item);
            return Task.FromResult<int>(0);
        }
        public virtual Task RemoveClaimAsync(TUser user, Claim claim)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            List<TUserClaim> list = (
                from uc in user.Claims
                where uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type
                select uc).ToList<TUserClaim>();
            foreach (TUserClaim current in list)
            {
                user.Claims.Remove(current);
            }
            IQueryable<TUserClaim> queryable =
                from uc in this._userClaims
                where uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type
                select uc;
            foreach (TUserClaim current2 in queryable)
            {
                this._userClaims.Remove(current2);
            }
            return Task.FromResult<int>(0);
        }
        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<bool>(user.EmailConfirmed);
        }
        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.EmailConfirmed = confirmed;
            return Task.FromResult<int>(0);
        }
        public Task SetEmailAsync(TUser user, string email)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.Email = email;
            return Task.FromResult<int>(0);
        }
        public Task<string> GetEmailAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<string>(user.Email);
        }
        public Task<TUser> FindByEmailAsync(string email)
        {
            this.ThrowIfDisposed();
            return this.GetUserAggregateAsync((TUser u) => u.Email.ToUpper() == email.ToUpper());
        }
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<DateTimeOffset>(user.LockoutEndDateUtc.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc)) : default(DateTimeOffset));
        }
        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEndDateUtc = ((lockoutEnd == DateTimeOffset.MinValue) ? null : new DateTime?(lockoutEnd.UtcDateTime));
            return Task.FromResult<int>(0);
        }
        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount++;
            return Task.FromResult<int>(user.AccessFailedCount);
        }
        public Task ResetAccessFailedCountAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount = 0;
            return Task.FromResult<int>(0);
        }
        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<int>(user.AccessFailedCount);
        }
        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<bool>(user.LockoutEnabled);
        }
        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEnabled = enabled;
            return Task.FromResult<int>(0);
        }
        public virtual Task<TUser> FindByIdAsync(TKey userId)
        {
            this.ThrowIfDisposed();
            return this.GetUserAggregateAsync((TUser u) => u.Id.Equals(userId));
        }
        public virtual Task<TUser> FindByNameAsync(string userName)
        {
            this.ThrowIfDisposed();
            return this.GetUserAggregateAsync((TUser u) => u.UserName.ToUpper() == userName.ToUpper());
        }
        public virtual async Task CreateAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            this._userStore.Create(user);
            await SaveChanges();
        }
        public virtual async Task DeleteAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            this._userStore.Delete(user);
            await SaveChanges();
        }
        public virtual async Task UpdateAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            this._userStore.Update(user);
            await SaveChanges();
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual async Task<TUser> FindAsync(UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            var provider = login.LoginProvider;
            var key = login.ProviderKey;
            var userLogin =
                await _logins.FirstOrDefaultAsync(l => l.LoginProvider == provider && l.ProviderKey == key);
            if (userLogin != null)
            {
                var userId = userLogin.UserId;
                return await GetUserAggregateAsync(u => u.Id.Equals(userId));
            }
            return null;
        }
        public virtual Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            ICollection<TUserLogin> arg_7B_0 = user.Logins;
            TUserLogin item = Activator.CreateInstance<TUserLogin>();
            item.UserId = user.Id;
            item.ProviderKey = login.ProviderKey;
            item.LoginProvider = login.LoginProvider;
            arg_7B_0.Add(item);
            return Task.FromResult<int>(0);
        }
        public virtual Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            string provider = login.LoginProvider;
            string key = login.ProviderKey;
            TUserLogin tUserLogin = user.Logins.SingleOrDefault((TUserLogin l) => l.LoginProvider == provider && l.ProviderKey == key);
            if (tUserLogin != null)
            {
                user.Logins.Remove(tUserLogin);
            }
            return Task.FromResult<int>(0);
        }
        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            IList<UserLoginInfo> result = (
                from l in user.Logins
                select new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList<UserLoginInfo>();
            return Task.FromResult<IList<UserLoginInfo>>(result);
        }
        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PasswordHash = passwordHash;
            return Task.FromResult<int>(0);
        }
        public Task<string> GetPasswordHashAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<string>(user.PasswordHash);
        }
        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult<bool>(user.PasswordHash != null);
        }
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumber = phoneNumber;
            return Task.FromResult<int>(0);
        }
        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<string>(user.PhoneNumber);
        }
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<bool>(user.PhoneNumberConfirmed);
        }
        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult<int>(0);
        }
        public virtual async Task<string> GetGroupAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (user.Group != null)
            {
                TKey groupId = user.Group.Id;
                TGroup group = await _groupStore.DbEntitySet.FirstOrDefaultAsync(g => g.Id.Equals(groupId));
                if (group != null)
                {
                    return group.Name;
                }
            }
            return "";
        }

        public virtual async Task SetGroupAsync(TUser user, string groupName)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            IdentityResult result;
            TGroup group = await _groupStore.DbEntitySet.FirstOrDefaultAsync(t => t.Name.Equals(groupName));
            if (group == null)
            {
                result = IdentityResult.Failed("Group Not Found");
                throw new ArgumentNullException("group");
            }
            else
            {
                user.Group = group;
                await this.UpdateAsync(user).ConfigureAwait(false);
            }
        }

        public virtual Task<bool> IsInGroupAsync(TUser user, string groupName)
        {
            throw new NotImplementedException();
        }

        public virtual Task AddToRoleAsync(TUser user, string roleName)
        {
            throw new NotImplementedException("AddToRole is Not Implemented please use GroupManager.AddToRole");

            //this.ThrowIfDisposed();
            //if (user == null)
            //{
            //    throw new ArgumentNullException("user");
            //}
            //if (string.IsNullOrWhiteSpace(roleName))
            //{
            //    throw new ArgumentException(IdentityResources.ValueCannotBeNullOrEmpty, "roleName");
            //}
            //TRole tRole = this._roleStore.DbEntitySet.SingleOrDefault((TRole r) => r.Name.ToUpper() == roleName.ToUpper());
            //if (tRole == null)
            //{
            //    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, IdentityResources.RoleNotFound, new object[]
            //    {
            //        roleName
            //    }));
            //}
            //TUserRole tUserRole = Activator.CreateInstance<TUserRole>();
            //tUserRole.UserId = user.Id;
            //tUserRole.RoleId = tRole.Id;
            //TUserRole entity = tUserRole;
            //this._userRoles.Add(entity);
            //return Task.FromResult<int>(0);
        }
        public virtual Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            throw new NotImplementedException("RemoveFromRole is Not Implemented please use GroupManager.RemoveFromRole");

            //this.ThrowIfDisposed();
            //if (user == null)
            //{
            //    throw new ArgumentNullException("user");
            //}
            //if (string.IsNullOrWhiteSpace(roleName))
            //{
            //    throw new ArgumentException(IdentityResources.ValueCannotBeNullOrEmpty, "roleName");
            //}
            //TRole tRole = this._roleStore.DbEntitySet.SingleOrDefault((TRole r) => r.Name.ToUpper() == roleName.ToUpper());
            //if (tRole != null)
            //{
            //    TKey roleId = tRole.Id;
            //    TKey userId = user.Id;
            //    TUserRole tUserRole = this._userRoles.FirstOrDefault((TUserRole r) => roleId.Equals(r.RoleId) && r.UserId.Equals(userId));
            //    if (tUserRole != null)
            //    {
            //        this._userRoles.Remove(tUserRole);
            //    }
            //}
            //return Task.FromResult<int>(0);
        }
        public virtual Task<IList<string>> GetRolesAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (user.Group == null)
            {
                return Task.FromResult<IList<string>>(new List<string>());
            }
            IEnumerable<string> source =
                from groupRoles in _groupRole
                join roles in this._roleStore.DbEntitySet on groupRoles.RoleId equals roles.Id
                where groupRoles.GroupId.Equals(user.Group.Id)
                select roles.Name;
            return Task.FromResult<IList<string>>(source.ToList<string>());
        }
        public virtual Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (user.Group == null)
            {
                return Task.FromResult<bool>(false);
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException(IdentityResources.ValueCannotBeNullOrEmpty, "roleName");
            }
            bool result = false;
            TRole role = this._roleStore.DbEntitySet.SingleOrDefault((TRole r) => r.Name.ToUpper() == roleName.ToUpper());
            if (role != null)
            {
                result = role.Groups.Any(delegate(TGroupRole gr)
                {
                    TKey roleId = gr.RoleId;
                    if (roleId.Equals(role.Id))
                    {
                        TKey groupId = gr.GroupId;
                        return groupId.Equals(user.Group.Id);
                    }
                    return false;
                });
            }
            return Task.FromResult<bool>(result);
        }
        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.SecurityStamp = stamp;
            return Task.FromResult<int>(0);
        }
        public Task<string> GetSecurityStampAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<string>(user.SecurityStamp);
        }
        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.TwoFactorEnabled = enabled;
            return Task.FromResult<int>(0);
        }
        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            this.ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<bool>(user.TwoFactorEnabled);
        }
        private async Task SaveChanges()
        {
            if (this.AutoSaveChanges)
            {
                await this.Context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        protected virtual Task<TUser> GetUserAggregateAsync(Expression<Func<TUser, bool>> filter)
        {
            return this.Users.Include((TUser u) => u.Group).Include((TUser u) => u.Claims).Include((TUser u) => u.Logins).FirstOrDefaultAsync(filter);
        }
        private void ThrowIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (this.DisposeContext && disposing && this.Context != null)
            {
                this.Context.Dispose();
            }
            this._disposed = true;
            this.Context = null;
            this._userStore = null;
        }
    }
    public class UserStore<TUser> : UserStore<TUser, IdentityRole, string, IdentityUserLogin, IdentityUserClaim, IdentityGroup, IdentityGroupRole>,
        IUserStore<TUser>,
        IUserStore<TUser, string>,
        IDisposable
        where TUser : IdentityUser
    {
        public UserStore()
            : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }
        public UserStore(DbContext context)
            : base(context)
        {
        }
    }
}
