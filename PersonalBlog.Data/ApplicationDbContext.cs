﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data.Models;

namespace PersonalBlog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("BlogUsers");
            builder.Entity<RefreshToken>().ToTable("BlogRefreshTokens");
            builder.Entity<Role>().ToTable("BlogRoles");
            builder.Entity<Blog>().ToTable("Blogs");
            builder.Entity<Article>().ToTable("BlogArticles");
            builder.Entity<Tag>().ToTable("BlogTags");
            builder.Entity<Comment>().ToTable("BlogComments");
        }
    }
}
