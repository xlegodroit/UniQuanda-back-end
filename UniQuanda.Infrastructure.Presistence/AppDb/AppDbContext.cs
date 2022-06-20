﻿using Microsoft.EntityFrameworkCore;
using UniQuanda.Infrastructure.Presistence.AppDb.EfConfigurations;
using UniQuanda.Infrastructure.Presistence.AppDb.Models;

namespace UniQuanda.Infrastructure.Presistence.AppDb
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppUserEfConfiguration());
        }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}
