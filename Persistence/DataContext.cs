﻿using System;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext: IdentityDbContext<AppUser>
    {
        //the :base(options) is basically calling super
        //give access to the base class options
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Value> Values { get; set; }
        public DbSet<Activity> Activities { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //allows us when creating our database to give a user a pk
            base.OnModelCreating(builder);
            //hasdata is used to create seed data
            builder.Entity<Value>().HasData(
                new Value {Id=1, Name="Value 101"},
                new Value {Id=2, Name="Value 102"},
                new Value {Id=3, Name="Value 103"}
            );
        }
    }
}
