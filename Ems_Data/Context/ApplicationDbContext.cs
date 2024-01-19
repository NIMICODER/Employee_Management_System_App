
using Ems_Models.Entities;
using Ems_Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ems_Data.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
       public DbSet<Employee> Employees { get; set; }
        public DbSet<GeneralDepartment> GeneralDepartments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<EmsRoles> SystemRoles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<RefreshTokenInfo> RefreshTokenInfos { get; set; }
    }
}
