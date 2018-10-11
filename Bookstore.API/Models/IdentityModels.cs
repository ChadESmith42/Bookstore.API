using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using Bookstore.API.Models;

namespace Bookstore.API.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //Setup DB reference for Models
        /// <summary>
        /// Table of Books
        /// </summary>
        public DbSet<Book> Books { get; set; }
        /// <summary>
        /// Table of Authors
        /// </summary>
        public DbSet<Author> Authors { get; set; }
        /// <summary>
        /// Table of Categories
        /// </summary>
        public DbSet<Category> Categories { get; set; }
        /// <summary>
        /// Table of Reviews
        /// </summary>
        public DbSet<Review> Reviews { get; set; }
        /// <summary>
        /// Reference Table of Books and Authors
        /// </summary>
        public DbSet<BookAuthor> BooksAuthors { get; set; }
        /// <summary>
        /// Reference Table of Books and Reviews
        /// </summary>
        public DbSet<BookReview> BooksReviews { get; set; }
        /// <summary>
        /// Reference Table of Authors Ranked by Review Ratings.
        /// </summary>
        public DbSet<AuthorRanking> AuthorRankings { get; set; }
        /// <summary>
        /// Reference Table of Books and Categories Ranked by Review Ratings.
        /// </summary>
        public DbSet<BookCategoryRanking> BookCategoryRankings { get; set; }

    }
}