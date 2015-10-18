using SmartHouse.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SmartHouseMVC.Models
{
    public class ModelInit : CreateDatabaseIfNotExists<ModelContext>
    {
        protected override void Seed(ModelContext db)
        {
            db.Lamps.Add(new Lamp("Living room", false));
            db.Lamps.Add(new Lamp("Kitchen", true));
            db.Lamps.Add(new Lamp("Outdoors", false));

            db.LouversSet.Add(new Louvers("Room 1", false, 1));
            db.LouversSet.Add(new Louvers("Room 2", true, 1));

            db.Fans.Add(new Fan("First floor", true, 5));
            db.Fans.Add(new Fan("Kitchen", false, 2));

            db.TvSet.Add(new Tv("Kitchen", true, Channels.football, 1));
            db.TvSet.Add(new Tv("Living room", false, Channels.m1, 2));

            db.SaveChanges();
        }
    }
}