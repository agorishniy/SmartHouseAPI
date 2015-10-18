using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartHouseMVC4.Models
{
    public class DevParam:DevInfo
    {
        public string Param { set; get; }
        public string Cmd { set; get; }
    }
}