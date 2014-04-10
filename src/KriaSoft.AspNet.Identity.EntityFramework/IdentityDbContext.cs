// Copyright (c) KriaSoft, LLC.  All rights reserved.  See LICENSE.txt in the project root for license information.

using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace KriaSoft.AspNet.Identity.EntityFramework
{
    public class IdentityDbContext<TKey, TUser, TLogin, TRole, TClaim> : DbContext
        where TUser : IdentityUser<TKey, TLogin, TRole, TClaim>
        where TLogin : IdentityLogin<TKey>
        where TRole : IdentityRole<TKey, IdentityUser<TKey, TLogin, TRole, TClaim>>
        where TClaim : IdentityClaim<TKey>
    {
        public IdentityDbContext()
            : base("name=DefaultConnection")
        {
        }

        public IdentityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public IdentityDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        public virtual DbSet<TUser> Users { get; set; }

        public virtual DbSet<TLogin> UserLogins { get; set; }

        public virtual DbSet<TRole> UserRoles { get; set; }

        public virtual DbSet<TClaim> UserClaims { get; set; }
    }
}
