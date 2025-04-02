using EFCoreBased.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreBased
{
    public class EventContext : DbContext
    {
        public DbSet<Models.FirstAidEvent> Events { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                "AccountEndpoint=https://js-cosmos-dotned-saturday.documents.azure.com:443/;AccountKey=5SolPM9sl3K1H4WiETE0kq1VWpxBIAQwyItSddHhKNrTDt6YN883xbXHnxEdPYG715DJKaQ4q5WEACDbvpCQmA==;",
                "eventplanning");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("events");
            modelBuilder.Entity<FirstAidEvent>().HasPartitionKey<Models.FirstAidEvent>("id");
            // Support for only one type.
            modelBuilder.Entity<FirstAidEvent>().HasNoDiscriminator();

            modelBuilder.Entity<Models.FirstAidEvent>().OwnsMany(e => e.Shifts);
            modelBuilder.Entity<FirstAidEvent>().Property(b => b.id).HasValueGenerator<GuidValueGenerator>();
        }
    }
}
