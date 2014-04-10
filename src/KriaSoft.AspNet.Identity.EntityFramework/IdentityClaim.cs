// Copyright (c) KriaSoft, LLC.  All rights reserved.  See LICENSE.txt in the project root for license information.

namespace KriaSoft.AspNet.Identity.EntityFramework
{
    public class IdentityClaim<TKey>
    {
        public virtual TKey Id { get; set; }

        public virtual TKey UserId { get; set; }

        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }
    }
}
