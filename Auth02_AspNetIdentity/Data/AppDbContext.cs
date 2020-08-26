using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth02_AspNetIdentity.Data
{
    // IdentityDbContext contain all the tables
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) 
            : base (dbContextOptions)
        {

        }
    }
}
