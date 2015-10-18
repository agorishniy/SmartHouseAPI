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

        [Route("api/values/onoff/{id}")]
        public Dictionary<string, string> PostOnOff(int id, [FromBody]string sType)
        {
            ModelContext db = new ModelContext();
            Device dev = new Device();
            Dictionary<string, string> paramList = new Dictionary<string, string>();

            switch (sType)
            {
                case "Lamp":
                    dev = db.Lamps.Find(id);
                    break;
                case "Fan":
                    dev = db.Fans.Include("Speed").FirstOrDefault(p => p.Id == id);
                    paramList.Add("Speed", ((Fan)dev).Speed.Value.ToString());
                    break;
                case "Louvers":
                    dev = db.LouversSet.Include("Open").FirstOrDefault(p => p.Id == id);
                    paramList.Add("Open", ((Louvers)dev).Open.Value.ToString());
                    break;
                case "Tv":
                    dev = db.TvSet.Include("Volume").FirstOrDefault(p => p.Id == id);
                    paramList.Add("Volume", ((Tv)dev).Volume.Value.ToString());
                    paramList.Add("Channel", ((Tv)dev).Channel.ToString());
                    break;
            }

            if (dev != null) 
            {
                dev.OnOff();
                paramList.Add("State", dev.State.ToString());
                db.SaveChanges();
                return paramList;
            }
            else
            {
                return null;
            }
        }


        [Route("api/values/param/{id}/{type}/{cmd}")]
        public Dictionary<string, string> PostDown(int id, string type, string cmd, [FromBody]string param)
        {
            ModelContext db = new ModelContext();
            IDevice dev = new Device();
            Dictionary<string, string> paramList = new Dictionary<string, string>();

            switch (type)
            {
                case "Fan":
                    dev = db.Fans.Include("Speed").FirstOrDefault(p => p.Id == id);
                    if (cmd == "down")
                        ((Fan)dev).Speed.Down();
                    else
                        ((Fan)dev).Speed.Up();
                    paramList.Add("Speed", ((Fan)dev).Speed.Value.ToString());
                    break;
                case "Louvers":
                    dev = db.LouversSet.Include("Open").FirstOrDefault(p => p.Id == id);
                    if (cmd == "down")
                        ((Louvers)dev).Open.Down();
                    else
                        ((Louvers)dev).Open.Up();
                    paramList.Add("Open", ((Louvers)dev).Open.Value.ToString());
                    break;
                case "Tv":
                    switch (param)
                    {
                        case "Volume":
                            dev = db.TvSet.Include("Volume").FirstOrDefault(p => p.Id == id);
                            if (cmd == "down")
                                ((Tv)dev).Volume.Down();
                            else
                                ((Tv)dev).Volume.Up();
                            paramList.Add("Volume", ((Tv)dev).Volume.Value.ToString());
                            break;    
                        case "Program":
                            dev = db.TvSet.Find(id);
                            if (cmd == "down")
                                ((Tv)dev).PreviousChannel();
                            else
                                ((Tv)dev).NextChannel();
                            paramList.Add("Channel", ((Tv)dev).Channel.ToString());
                            break;
                    }
                    break;
            }

            if (dev != null)
            {
                db.Entry(dev).State = EntityState.Modified;
                db.SaveChanges();

                return paramList;
            }
            else
            {
                return null;
            }
        }


        [Route("api/values/del/{id}/{type}")]
        public int GetDel(int id, string type)
        {
            ModelContext db = new ModelContext();
 
            switch (type)
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

            return 0;
        }


        [Route("api/values/add/{type}")]
        public Dictionary<string, string> PostAdd(string type, [FromBody]string name)
        {
            ModelContext db = new ModelContext();
            IDevice dev = new Device();
            Dictionary<string, string> paramList = new Dictionary<string, string>();

            switch (type)
            {
                case "Lamp":
                    dev = db.Lamps.Add(new Lamp(name, true));
                    break;
                case "Fan":
                    dev = db.Fans.Add(new Fan(name, true, 1));
                    paramList.Add("Speed", "1");
                    break;
                case "Louvers":
                    dev = db.LouversSet.Add(new Louvers(name, true, 1));
                    paramList.Add("Open", "1");
                    break;
                case "Tv":
                    dev = db.TvSet.Add(new Tv(name, true, Channels.football, 2));
                    paramList.Add("Volume", "2");
                    paramList.Add("Program", ((Tv)dev).Channel.ToString());
                    break;
            }

            db.SaveChanges();

            paramList.Add("State", dev.State.ToString());
            paramList.Add("Id", dev.Id.ToString());

            return paramList;
        }


    }
}