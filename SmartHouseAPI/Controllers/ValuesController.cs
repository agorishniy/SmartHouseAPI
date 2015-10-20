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

        // Get data of device
        public Object GetDevice(int id, string type)
        {
            switch (type)
            {
                case "Lamp":
                    dev = db.Lamps.Find(id);
                    break;
                case "Fan":
                    dev = db.Fans.Include("Speed").FirstOrDefault(p => p.Id == id);
                    break;
                case "Louvers":
                    dev = db.LouversSet.Include("Open").FirstOrDefault(p => p.Id == id);
                    break;
                case "Tv":
                    dev = db.TvSet.Include("Volume").FirstOrDefault(p => p.Id == id);
                    break;
            }

            if (dev != null)
            {
                return dev;
            }
            else
            {
                return null;
            }
        }

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
        
        // Change parameter of device 
        // devInfo.Cmd - type of change 
        //               "up" - increase parameter
        //               "down" - decrease parameter
        //               "onoff" - On/Off device
        // return  >=0 - new value of parameter
        //          -1 - error
        [HttpPut]
        [Route("api/values/param")]
        public sbyte ChangeParam([FromBody]DevParam devInfo)
        {
            sbyte newValue = -1;

            if (devInfo.Cmd == "onoff")
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
                dev.OnOff();
                if (dev.State)
                {
                    newValue = 1;
                }
                else
                {
                    newValue = 0;
                }
            }
            else
            {
                switch (devInfo.Type)
                {
                    case "Fan":
                        dev = db.Fans.Include("Speed").FirstOrDefault(p => p.Id == devInfo.Id);
                        if (devInfo.Cmd == "down")
                            newValue = (sbyte)((Fan)dev).Speed.Down();
                        else
                            newValue = (sbyte)((Fan)dev).Speed.Up();
                        break;
                    case "Louvers":
                        dev = db.LouversSet.Include("Open").FirstOrDefault(p => p.Id == devInfo.Id);
                        if (devInfo.Cmd == "down")
                            newValue = (sbyte)((Louvers)dev).Open.Down();
                        else
                            newValue = (sbyte)((Louvers)dev).Open.Up();
                        break;
                    case "Tv":
                        switch (devInfo.Param)
                        {
                            case "Volume":
                                dev = db.TvSet.Include("Volume").FirstOrDefault(p => p.Id == devInfo.Id);
                                if (devInfo.Cmd == "down")
                                    newValue = (sbyte)((Tv)dev).Volume.Down();
                                else
                                    newValue = (sbyte)((Tv)dev).Volume.Up();
                                break;
                            case "Program":
                                dev = db.TvSet.Find(devInfo.Id);
                                if (devInfo.Cmd == "down")
                                    newValue = (sbyte)((Tv)dev).PreviousChannel();
                                else
                                    newValue = (sbyte)((Tv)dev).NextChannel();
                                break;
                        }
                        break;
                }
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
        // return    0 - ok
        //          -1 - error
        public sbyte DeleteDevice([FromBody]DevInfo devInfo)
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
                return 0;   // ok
            }
            catch
            {
                return -1;  // error
            }
        }
    }
}