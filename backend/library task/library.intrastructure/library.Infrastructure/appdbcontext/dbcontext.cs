using library.DOMAIN.library.domain.entities;
using library_mangment.library.domain.entities;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace library_mangment.library.Infrastructure.appdbcontext
{
    public class Dbcontext : DbContext
    {
        public Dbcontext(DbContextOptions<Dbcontext> options) : base(options)
        {

        }
        public DbSet<book> books {  get; set; }
        public DbSet<member> members { get; set; }
        public DbSet<BorrowRecord> borrowRecords { get; set; }
        public DbSet<requests> Requests { get; set; }

        public DbSet<rate> BookRates { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<rate>()
            .HasIndex(r => new { r.memberId, r.bookId })
            .IsUnique();

            modelBuilder.Entity<member>().HasData(new member
            {
            Id = 1,
            Name = "Admin",
            Email = "admin",
            passwordHas = "AQAAAAIAAYagAAAAEAUoBi0U2jjozI6ABQdVjD91aSw0mDfe1aE+JrnTAkF+ClBZj2uM/aM9AqqU9J8PIA==",
            role = "Admin"
             });


            modelBuilder.Entity<BorrowRecord>()
                    .HasOne(p => p.member)
                    .WithMany(pc => pc.BorrowRecords)
                    .HasForeignKey(p => p.memberId);


            modelBuilder.Entity<BorrowRecord>()
                    .HasOne(p => p.book)
                    .WithMany(pc => pc.BorrowRecords)
                    .HasForeignKey(p => p.bookId);

        }

    }
}