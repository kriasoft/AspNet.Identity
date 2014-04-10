// Copyright (c) KriaSoft, LLC.  All rights reserved.  See LICENSE.txt in the project root for license information.

namespace KriaSoft.AspNet.Identity.EntityFramework
{
    public class IdentityLogin<TKey>
    {
        public virtual TKey UserId { get; set; }

        public virtual string LoginProvider { get; set; }

        public virtual string ProviderKey { get; set; }
    }
}
