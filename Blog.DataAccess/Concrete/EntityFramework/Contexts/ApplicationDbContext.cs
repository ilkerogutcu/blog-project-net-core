using Blog.Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess.Concrete.EntityFramework.Contexts
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder
				.UseSnakeCaseNamingConvention()
				.UseSqlServer("Server=DESKTOP-9JI7HVR;Database=blog;Trusted_Connection=True;");
		}
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<Post> Posts { get; set; }	
		public DbSet<Image> Images { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Tag> Tags { get; set; }	
		public DbSet<Category> Categories { get; set; }
	}
}