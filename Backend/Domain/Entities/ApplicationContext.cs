using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CookRun.Domain.Entities
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{
		public ApplicationContext(DbContextOptions options) : base(options) { }

		public DbSet<Log> Logs { get; set; }
		public DbSet<JobReport> JobReports { get; set; }
		public DbSet<RoistatMeanReport> RoistatMeanReports { get; set; }
		public DbSet<AmoLeadReport> AmoLeadReports { get; set; }
		public DbSet<AmoLeadReportTag> AmoLeadReportTags { get; set; }
		public DbSet<AmoDayReport> AmoDayReports { get; set; }
		public DbSet<AmoMonthReport> AmoMonthReports { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<UsersProjects> UsersProjects { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}

		public async void EnsureSeedData(UserManager<ApplicationUser> userManager)
		{
			if (!this.Users.Any(u => u.UserName == "admin@cookrun.com"))
			{
				var adminUser = new ApplicationUser
				{
					UserName = "admin@cookrun.com",
					Email = "admin@cookrun.com"
				};
				await userManager.CreateAsync(adminUser, "MYP@55word");
				await userManager.SetLockoutEnabledAsync(adminUser, false);
			}
		}
	}
}