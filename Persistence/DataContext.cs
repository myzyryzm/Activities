using System;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Persistence
{
    public class DataContext: DbContext
    {
        //the :base(options) is basically calling super
        //give access to the base class options
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Value> Values { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //hasdata is used to create seed data
            builder.Entity<Value>().HasData(
                new Value {Id=1, Name="Value 101"},
                new Value {Id=2, Name="Value 102"},
                new Value {Id=3, Name="Value 103"}
            );
        }
    }
}
