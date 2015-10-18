using SmartHouse.Models;
using SmartHouseMVC.Models;
using SmartHouseMVC4.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartHouseMVC.Controllers
{

    public class ValuesController : ApiController
    {
        ModelContext db = new ModelContext();
        Device dev = new Device();

        // Add device in database
        [HttpPost]
        [Route("api/values/add")]
        public Object AddDevice([FromBody]DevNew devNew)
        {
            switch (devNew.Type)
            {
                case "Lamp":
                    dev = db.Lamps.Add(new Lamp(devNew.Name, true));
                    break;
                case "Fan":
                    dev = db.Fans.Add(new Fan(devNew.Name, true, 1));
                    break;
                case "Louvers":
                    dev = db.LouversSet.Add(new Louvers(devNew.Name, true, 1));
                    break;
                case "Tv":
                    dev = db.TvSet.Add(new Tv(devNew.Name, true, Channels.football, 2));
                    break;
            }

            if (dev != null)
            {
                try
                {
                    db.SaveChanges();
                    return dev;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        // Change state of device (On/Off)
        [HttpPost]
        [Route("api/values/onoff")]
        public Object ChangeState([FromBody]DevInfo devInfo)
        {
            switch (devInfo.Type)
            {
                case "Lamp":
                    dev = db.Lamps.Find(devInfo.Id);
                    break;
                case "Fan":
                    dev = db.Fans.Include("Speed").FirstOrDefault(p => p.Id == devInfo.Id);
                    break;
                case "Louvers":
                    dev = db.LouversSet.Include("Open").FirstOrDefault(p => p.Id == devInfo.Id);
                    break;
                case "Tv":
                    dev = db.TvSet.Include("Volume").FirstOrDefault(p => p.Id == devInfo.Id);
                    break;
            }

            if (dev != null) 
            {
                try
                {
                    dev.OnOff();
                    db.SaveChanges();

                    return dev;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        
        // Change parameter of device 
        // devInfo.Cmd - type of change 
        //               "up" - increase parameter
        //               "down" - decrease parameter
        [HttpPost]
        [Route("api/values/param")]
        public int ChangeParam([FromBody]DevParam devInfo)
        {
            int newValue = -1;

            switch (devInfo.Type)
            {
                case "Fan":
                    dev = db.Fans.Include("Speed").FirstOrDefault(p => p.Id == devInfo.Id);
                    if (devInfo.Cmd == "down")
                        newValue = ((Fan)dev).Speed.Down();
                    else
                        newValue = ((Fan)dev).Speed.Up();
                    break;
                case "Louvers":
                    dev = db.LouversSet.Include("Open").FirstOrDefault(p => p.Id == devInfo.Id);
                    if (devInfo.Cmd == "down")
                        newValue = ((Louvers)dev).Open.Down();
                    else
                        newValue = ((Louvers)dev).Open.Up();
                    break;
                case "Tv":
                    switch (devInfo.Param)
                    {
                        case "Volume":
                            dev = db.TvSet.Include("Volume").FirstOrDefault(p => p.Id == devInfo.Id);
                            if (devInfo.Cmd == "down")
                                newValue = ((Tv)dev).Volume.Down();
                            else
                                newValue = ((Tv)dev).Volume.Up();
                            break;    
                        case "Program":
                            dev = db.TvSet.Find(devInfo.Id);
                            if (devInfo.Cmd == "down")
                                newValue = ((Tv)dev).PreviousChannel();
                            else
                                newValue = ((Tv)dev).NextChannel();
                            break;
                    }
                    break;
            }

            if (dev != null)
            {
                try
                {
                    db.Entry(dev).State = EntityState.Modified;
                    db.SaveChanges();

                    return newValue;
                }
                catch
                {
                    return -1;  
                }
            }
            else
            {
                return -1;  
            }
        }



        // Delete device from database
        public bool DeleteDevice([FromBody]DevInfo devInfo)
        {
            switch (devInfo.Type)
            {
                case "Lamp":
                    db.Lamps.Remove((Lamp)db.Lamps.Find(devInfo.Id));
                    break;
                case "Fan":
                    db.Fans.Remove((Fan)db.Fans.Find(devInfo.Id));
                    break;
                case "Louvers":
                    db.LouversSet.Remove((Louvers)db.LouversSet.Find(devInfo.Id));
                    break;
                case "Tv":
                    db.TvSet.Remove((Tv)db.TvSet.Find(devInfo.Id));
                    break;
            }

            try
            {
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}