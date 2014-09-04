using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.GroupBaseEF
{
    /// <summary>
    ///     Validates groups before they are saved
    /// </summary>
    /// <typeparam name="TGroup"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class GroupValidator<TGroup, TKey> : IIdentityValidator<TGroup>
        where TGroup : class, IGroup<TKey>
        where TKey : IEquatable<TKey>
    {
        private GroupManager<TGroup, TKey> Manager { get; set; }
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="manager"></param>
        public GroupValidator(GroupManager<TGroup, TKey> manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            this.Manager = manager;
        }
        /// <summary>
        ///     Validates a group before saving
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> ValidateAsync(TGroup item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            List<string> list = new List<string>();
            await this.ValidateGroupName(item, list);
            IdentityResult result;
            if (list.Count > 0)
            {
                result = IdentityResult.Failed(list.ToArray());
            }
            else
            {
                result = IdentityResult.Success;
            }
            return result;
        }
        private async Task ValidateGroupName(TGroup group, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(group.Name))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, IdentityResources.PropertyTooShort, new object[]
				{
					"Name"
				}));
            }
            else
            {
                TGroup tGroup = await this.Manager.FindByNameAsync(group.Name);
                if (tGroup != null && !EqualityComparer<TKey>.Default.Equals(tGroup.Id, group.Id))
                {
                    errors.Add(string.Format(CultureInfo.CurrentCulture, IdentityResources.DuplicateName, new object[]
					{
						group.Name
					}));
                }
            }
        }
    }
    /// <summary>
    ///     Validates groups before they are saved
    /// </summary>
    /// <typeparam name="TGroup"></typeparam>
    public class GroupValidator<TGroup> : GroupValidator<TGroup, string> where TGroup : class, IGroup<string>
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="manager"></param>
        public GroupValidator(GroupManager<TGroup, string> manager)
            : base(manager)
        {
        }
    }
}
