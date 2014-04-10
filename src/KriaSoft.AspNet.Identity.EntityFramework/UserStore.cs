// Copyright (c) KriaSoft, LLC.  All rights reserved.  See LICENSE.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace KriaSoft.AspNet.Identity.EntityFramework
{
    public partial class UserStore<TKey, TUser, TLogin, TRole, TClaim> :
        IUserPasswordStore<TUser, TKey>, IUserLoginStore<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey, TLogin, TRole, TClaim>
        where TLogin : IdentityLogin<TKey>
        where TRole : IdentityRole<TKey, IdentityUser<TKey, TLogin, TRole, TClaim>>
        where TClaim : IdentityClaim<TKey>
    {
        private readonly DbContext db;

        public UserStore(DbContext db)
        {
            if (db == null)
            {
                throw new ArgumentNullException("db");
            }

            this.db = db;
        }

        #region IUserStore<TUser, Key>
        public Task CreateAsync(TUser user)
        {
            this.db.Set<TUser>().Add(user);
            return this.db.SaveChangesAsync();
        }

        public Task DeleteAsync(TUser user)
        {
            this.db.Set<TUser>().Remove(user);
            return this.db.SaveChangesAsync();
        }

        public Task<TUser> FindByIdAsync(TKey userId)
        {
            return this.db.Set<TUser>()
                .Include(u => u.Logins).Include(u => u.Roles).Include(u => u.Claims)
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            return this.db.Set<TUser>()
                .Include(u => u.Logins).Include(u => u.Roles).Include(u => u.Claims)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public Task UpdateAsync(TUser user)
        {
            this.db.Entry<TUser>(user).State = EntityState.Modified;
            return this.db.SaveChangesAsync();
        } 
        #endregion

        #region IUserPasswordStore<TUser, Key>
        public Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        } 
        #endregion

        #region IUserLoginStore<TUser, Key>
        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var userLogin = Activator.CreateInstance<TLogin>();
            userLogin.UserId = user.Id;
            userLogin.LoginProvider = login.ProviderKey;
            userLogin.ProviderKey = login.ProviderKey;
            user.Logins.Add(userLogin);
            return Task.FromResult(0);
        }

        public async Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var provider = login.LoginProvider;
            var key = login.ProviderKey;

            var userLogin = await this.db.Set<TLogin>().FirstOrDefaultAsync(l => l.LoginProvider == provider && l.ProviderKey == key);

            if (userLogin == null)
            {
                return default(TUser);
            }

            return await this.db.Set<TUser>()
                .Include(u => u.Logins).Include(u => u.Roles).Include(u => u.Claims)
                .FirstOrDefaultAsync(u => u.Id.Equals(userLogin.UserId));
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult<IList<UserLoginInfo>>(user.Logins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var provider = login.LoginProvider;
            var key = login.ProviderKey;

            var item = user.Logins.SingleOrDefault(l => l.LoginProvider == provider && l.ProviderKey == key);

            if (item != null)
            {
                user.Logins.Remove(item);
            }

            return Task.FromResult(0);
        } 
        #endregion

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
