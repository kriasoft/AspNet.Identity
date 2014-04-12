Getting Started with ASP.NET Identity and EF DbFirst
----------------------------------------------------

### 1. Publish ASP.NET Identity Database Project

SQL Database Project (SSDT):

> `./src/KriaSoft.AspNet.Identity.Database`

You can publish it by double-clicking on the `./Publish Profiles/Local.publish.xml` file in the Solution Explorer.

### 2. Generate Entity Framework model using EF Designer

See ASP.NET Identity Database-First model example:

> `./src/KriaSoft.AspNet.Identity.EntityFramework/Model.edmx`

### 3. Copy UserEntity.cs, UserRoleEntity.cs, RoleStore.cs, UserStore.cs and Resources into your project

> See: `./src/KriaSoft.AspNet.Identity.EntityFramework/`

And make sure property names in User, UserLogin, UserRole and UserClaim entities match the ones used in RoleStore and UserStore providers.

![ASP.NET Identity Model](http://i.imgur.com/KHDqq3B.png)

### 4. Done! You can use it like this:

```
var db = new ApplicationDbContext(); // your custom EF model
var userManager = new UserManager(new UserStore(db));

userManager.CreateUserAsync(new User { UserName = "demouser" });
```
