using System;
using System.Collections.Generic;
using System.Linq;
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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //        .Entity<Board>()
        //        .HasMany(p => p.Equipments)
        //        .WithMany(p => p.Boards)
        //        .UsingEntity(j => j.ToTable("BoardEquipment"));
        //}

        public DbSet<SurfsUp.Models.Board> Board { get; set; } = default!;

        public DbSet<SurfsUp.Models.Equipment> Equipment { get; set; }
    }
}
