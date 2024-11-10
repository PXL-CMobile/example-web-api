﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ExampleWebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExampleWebApi.Infrastructure
{
    public class ExampleDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<UserFavoriteActor> UserFavoriteActors { get; set; }


        public ExampleDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("ExternalLogins");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");


            builder.Entity<UserFavoriteActor>()
            .HasKey(ufa => new { ufa.UserId, ufa.ActorId });

            builder.Entity<UserFavoriteActor>()
                .HasOne(ufa => ufa.User)
                .WithMany(u => u.FavoriteActors)
                .HasForeignKey(ufa => ufa.UserId);

            builder.Entity<UserFavoriteActor>()
                .HasOne(ufa => ufa.Actor)
                .WithMany(a => a.UserFavorites)
                .HasForeignKey(ufa => ufa.ActorId);
        }
    }
}
