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