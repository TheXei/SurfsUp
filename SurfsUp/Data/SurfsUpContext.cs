using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SurfsUp.Controllers;
using SurfsUp.Migrations;
using SurfsUp.Models;

namespace SurfsUp.Data
{
    public class SurfsUpContext : DbContext
    {
        public SurfsUpContext (DbContextOptions<SurfsUpContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Rent>()
                .HasOne(b => b.Board)
                .WithOne(i => i.Rent)
                .HasForeignKey<Models.Rent>(b => b.BoardId);
        }

        public DbSet<SurfsUp.Models.Board> Board { get; set; } = default!;
        public DbSet<SurfsUp.Models.Rent> Rent { get; set; } = default!;
    }
}
