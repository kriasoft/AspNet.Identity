// Copyright (c) KriaSoft, LLC.  All rights reserved.  See LICENSE.txt in the project root for license information.

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace KriaSoft.AspNet.Identity.EntityFramework
{
    public partial class RoleStore<TKey, TRole, TUser> : IQueryableRoleStore<TRole, TKey>
        where TRole : IdentityRole<TKey, TUser>
    {
        private readonly DbContext db;

        public RoleStore(DbContext db)
        {
            this.db = db;
        }

        //// IQueryableRoleStore<TRole, TKey>

        public IQueryable<TRole> Roles
        {
            get { return this.db.Set<TRole>(); }
        }

        //// IRoleStore<TRole, TKey>

        public virtual Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.Set<TRole>().Add(role);
            return this.db.SaveChangesAsync();
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.Set<TRole>().Remove(role);
            return this.db.SaveChangesAsync();
        }

        public Task<TRole> FindByIdAsync(TKey roleId)
        {
            return this.db.Set<TRole>().FindAsync(new[] { roleId });
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            return this.db.Set<TRole>().FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.Entry(role).State = EntityState.Modified;
            return this.db.SaveChangesAsync();
        }

        //// IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.db != null)
            {
                this.db.Dispose();
            }
        }
    }
}
