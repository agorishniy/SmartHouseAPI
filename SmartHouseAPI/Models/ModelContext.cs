using SmartHouse.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SmartHouseMVC.Models
{
    public class ModelContext: DbContext
    {
        public ModelContext()
            : base("name=ModelContext")
        {
            Database.SetInitializer<ModelContext>(new ModelInit());
        }

        public DbSet<Param> Params { get; set; }
        public DbSet<Lamp> Lamps { get; set; }
        public DbSet<Louvers> LouversSet { get; set; }
        public DbSet<Fan> Fans { get; set; }
        public DbSet<Tv> TvSet { get; set; }
    }
}