using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Let_sTalk.Models;

namespace Let_sTalk.Data.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        public DbSet<User> users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => entity.HasIndex(u => u.Email).IsUnique()); //here we're making sure email is unique amoung all users in the users db table

            //modelBuilder.Entity<UserPreference>()
            //    .HasOne(x => x.Preference)
            //    .WithMany(x => x.UserPreferences)
            //    .HasForeignKey(x => x.PreferenceId);
            //modelBuilder.Entity<UserPreference>()
            //    .HasOne(x => x.User)
            //    .WithMany(x => x.UserPreferences)
            //    .HasForeignKey(x => x.UserId);

        }
        public DbSet<Reservation> reservations { get; set; }
        public DbSet<Preference> preferences { get; set; }
        public DbSet<Location> locations { get; set; }
        public DbSet<Match> matches { get; set; }

        public DbSet<UserPreference> userPreferences { get; set; }
    }
}
