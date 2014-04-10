// Copyright (c) KriaSoft, LLC.  All rights reserved.  See LICENSE.txt in the project root for license information.

using System;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

namespace KriaSoft.AspNet.Identity.EntityFramework
{
    public class IdentityUser<TKey, TLogin, TRole, TClaim> : IUser<TKey>
        where TRole : IdentityRole<TKey, IdentityUser<TKey, TLogin, TRole, TClaim>>
        where TLogin : IdentityLogin<TKey>
        where TClaim : IdentityClaim<TKey>
    {
        public IdentityUser()
        {
            this.Logins = new HashSet<TLogin>();
            this.Roles = new HashSet<TRole>();
            this.Claims = new HashSet<TClaim>();
        }

        public virtual TKey Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Email { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual string SecurityStamp { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual DateTime? LockoutEndDateUtc { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public virtual ICollection<TLogin> Logins { get; private set; }

        public virtual ICollection<TRole> Roles { get; private set; }

        public virtual ICollection<TClaim> Claims { get; private set; }
    }
}
