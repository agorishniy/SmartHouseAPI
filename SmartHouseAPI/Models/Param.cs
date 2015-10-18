using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartHouse.Models
{
    public class Param
    {
        public Param()
        {
        }
        public Param(byte valueCurrent, byte minVal, byte maxVal)
        {
            Min = minVal;
            Max = maxVal;
            Value = valueCurrent;
        }

        public int Id { set; get; }
        public byte Min { get; set; }

        public byte Max { get; set; }

        public byte Value  { get; set; }
 
        public void Up()
        {
            if (Value < Max)
                Value++;
        }

        public void Down()
        {
            if (Value > Min)
                Value--;
        }
    }
}