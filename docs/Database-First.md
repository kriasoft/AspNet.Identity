ASP.NET Identity Provider with Database-First
---------------------------------------------

### 1. Publish ASP.NET Identity Database Project

> See: `./src/KriaSoft.AspNet.Identity.Database`

### 2. Generate Entity Framework model using EF Designer

> See: `./src/examples/KriaSoft.AspNet.Identity.DbFirst/Data/Model.edmx`

### 3. Update EF T4 templates to make the generated model match the provider classes

After you update .tt templates, the generated EF model classes should look like this:

#### Model.Context.cs (before):

```
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ApplicationDbContext : DbContext
    {
        ...
        public virtual DbSet<User> Users { get; set; }
        ...
    }
```

#### Model.Context.cs (after):

```
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    using KriaSoft.AspNet.Identity.EntityFramework;
    
    public partial class ApplicationDbContext : IdentityDbContext<int, User, Login, Role, Claim>
    {
        ...
    }
```

#### User.cs (before):

```
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            ...
        }
    
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        ...
    }
```

#### User.cs (after):

```
    using System;
    using System.Collections.Generic;
    
    using KriaSoft.AspNet.Identity.EntityFramework;
    
    public partial class User : IdentityUser<int, Login, Role, Claim>
    {
        ...
    }
```

**Note**: You can find modified .tt templates here:

> `./examples/KriaSoft.AspNet.Identity.DbFirst/Data/*.tt`