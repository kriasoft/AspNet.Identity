// Copyright (c) KriaSoft, LLC.  All rights reserved.  See LICENSE.txt in the project root for license information.

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace KriaSoft.AspNet.Identity.EntityFramework
{
    public class RoleStore : IQueryableRoleStore<UserRole, int>
    {
        private readonly ApplicationDbContext db;

        public RoleStore(ApplicationDbContext db)
        {
            this.db = db;
        }

        //// IQueryableRoleStore<UserRole, TKey>

        public IQueryable<UserRole> Roles
        {
            get { return this.db.UserRoles; }
        }

        //// IRoleStore<UserRole, TKey>

        public virtual Task CreateAsync(UserRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.UserRoles.Add(role);
            return this.db.SaveChangesAsync();
        }

        public Task DeleteAsync(UserRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            this.db.UserRoles.Remove(role);
            return this.db.SaveChangesAsync();
        }

        public Task<UserRole> FindByIdAsync(int roleId)
        {
            return this.db.UserRoles.FindAsync(new[] { roleId });
        }

        public Task<UserRole> FindByNameAsync(string roleName)
        {
            return this.db.UserRoles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public Task UpdateAsync(UserRole role)
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
