// Copyright (c) KriaSoft, LLC.  All rights reserved.  See LICENSE.txt in the project root for license information.

using System.Collections.Generic;

using Microsoft.AspNet.Identity;

namespace KriaSoft.AspNet.Identity.EntityFramework
{
    public class IdentityRole<TKey, TUser> : IRole<TKey>
    {
        public IdentityRole()
        {
            this.Users = new HashSet<TUser>();
        }

        public virtual TKey Id { get; set; }

        public virtual string Name { get; set; }

        public virtual ICollection<TUser> Users { get; private set; }
    }
}
