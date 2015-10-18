using SmartHouse.Models;
using SmartHouseMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartHouseMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(GetListDevice(new ModelContext()));
        }

        public PartialViewResult AjaxOnOff(int id, string sType)
        {
            ModelContext db = new ModelContext();
            IDevice dev = new Device();

            switch (sType)
            {
                case "Lamp":
                    db.Lamps.Find(id).OnOff();
                    dev = db.Lamps.Find(id);
                    break;
                case "Fan":
                    db.Fans.Find(id).OnOff();
                    dev = db.Fans.Include("Speed").FirstOrDefault(p => p.Id == id);
                    break;
                case "Louvers":
                    db.LouversSet.Find(id).OnOff();
                    dev = db.LouversSet.Include("Open").FirstOrDefault(p => p.Id == id);
                    break;
                case "Tv":
                    db.TvSet.Find(id).OnOff();
                    dev = db.TvSet.Include("Volume").FirstOrDefault(p => p.Id == id);
                    break;
            }
            db.SaveChanges();

            return PartialView("DivDevice", dev);
        }


        public PartialViewResult AjaxDown(int id, string sType, string sParam)
        {
            ModelContext db = new ModelContext();
            IDevice dev = new Device();

            switch (sType)
            {
                case "Fan":
                    dev = db.Fans.Include("Speed").FirstOrDefault(p => p.Id == id);
                    ((Fan)dev).Speed.Down();
                    break;
                case "Louvers":
                    dev = db.LouversSet.Include("Open").FirstOrDefault(p => p.Id == id);
                    ((Louvers)dev).Open.Down();
                    break;
                case "Tv":
                    dev = db.TvSet.Include("Volume").FirstOrDefault(p => p.Id == id);
                    switch (sParam)
                    {
                        case "Volume":
                            ((Tv)dev).Volume.Down();
                            break;
                        case "Program":
                            ((Tv)dev).PreviousChannel();
                            break;
                    }
                    break;
            }
            db.Entry(dev).State = EntityState.Modified;
            db.SaveChanges();

            return PartialView("DivDevice", dev);
        }


        public PartialViewResult AjaxUp(int id, string sType, string sParam)
        {
            ModelContext db = new ModelContext();
            IDevice dev = new Device();

            switch (sType)
            {
                case "Fan":
                    dev = db.Fans.Include("Speed").FirstOrDefault(p => p.Id == id);
                    ((Fan)dev).Speed.Up();
                    break;
                case "Louvers":
                    dev = db.LouversSet.Include("Open").FirstOrDefault(p => p.Id == id);
                    ((Louvers)dev).Open.Up();
                    break;
                case "Tv":
                    dev = db.TvSet.Include("Volume").FirstOrDefault(p => p.Id == id);
                    switch (sParam)
                    {
                        case "Volume":
                            ((Tv)dev).Volume.Up();
                            break;
                        case "Program":
                            ((Tv)dev).NextChannel();
                            break;
                    }
                    break;
            }
            db.Entry(dev).State = EntityState.Modified;
            db.SaveChanges();

            return PartialView("DivDevice", dev);
        }


        
        public ActionResult Add(string sName, string sType)
        {
            ModelContext db = new ModelContext();

            switch (sType)
            {
                case "Lamp":
                    db.Lamps.Add(new Lamp(sName, false));
                    break;
                case "Fan":
                    db.Fans.Add(new Fan(sName, false, 1));
                    break;
                case "Louvers":
                    db.LouversSet.Add(new Louvers(sName, false, 1));
                    break;
                case "Tv":
                    db.TvSet.Add(new Tv(sName, false, Channels.football, 2));
                    break;
            }
            db.SaveChanges();

            return View("Index", GetListDevice(db));
        }

        public ActionResult Del(int id, string sType)
        {
            ModelContext db = new ModelContext();

            switch (sType)
            {
                case "Lamp":
                    db.Lamps.Remove((Lamp)db.Lamps.Find(id));
                    break;
                case "Fan":
                    db.Fans.Remove((Fan)db.Fans.Find(id));
                    break;
                case "Louvers":
                    db.LouversSet.Remove((Louvers)db.LouversSet.Find(id));
                    break;
                case "Tv":
                    db.TvSet.Remove((Tv)db.TvSet.Find(id));
                    break;
            }
            db.SaveChanges();

            return View("Index", GetListDevice(db));
        }


        public static List<IDevice> GetListDevice(ModelContext db)    
        {
            List<IDevice> dev = new List<IDevice>();

            foreach (var item in db.Lamps.ToList())
            {
                dev.Add(item);
            }
            foreach (var item in db.LouversSet.Include("Open").ToList())
            {
                dev.Add(item);
            }
            foreach (var item in db.Fans.Include("Speed").ToList())
            {
                dev.Add(item);
            }
            foreach (var item in db.TvSet.Include("Volume").ToList())
            {
                dev.Add(item);
            }

            return dev;
        }
    }
}