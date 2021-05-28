using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlcApi.Entities.Elements;

namespace PlcApi.Entities
{
    public class PlcDbContext:DbContext
    {
        public DbSet<PlcEntity> PLCs { get; set; } 
        public DbSet<PlcModel> Models { get; set; } 
        public DbSet<InputOutput> InputsOutputs { get; set; }
        public DbSet<Diode> Diodes { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Conveyor> Conveyors { get; set; }
        public DbSet<ConveyorPoint> ConveyorPoints { get; set; }

        string _connectionString = "Server=DESKTOP-R8L9JN2\\LEARNINGSQL;Database=PlcDb;Trusted_Connection=True;";
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlcEntity>()
                .Property(p => p.ModelId)
                .IsRequired();
            modelBuilder.Entity<PlcEntity>()
                .Property(p => p.Ip)
                .IsRequired();
        
            modelBuilder.Entity<InputOutput>()
                .Property(i => i.PlcId)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
