﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PC_04
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PC_04Entities : DbContext
    {
        public PC_04Entities()
            : base("name=PC_04Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CleaningRoom> CleaningRooms { get; set; }
        public virtual DbSet<CleaningRoomDetail> CleaningRoomDetails { get; set; }
        public virtual DbSet<CleaningRoomItem> CleaningRoomItems { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemStatu> ItemStatus { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationCheckOut> ReservationCheckOuts { get; set; }
        public virtual DbSet<ReservationRequestItem> ReservationRequestItems { get; set; }
        public virtual DbSet<ReservationRoom> ReservationRooms { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
    }
}