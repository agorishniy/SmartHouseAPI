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
        public Param(int valueCurrent, int minVal, int maxVal)
        {
            Min = minVal;
            Max = maxVal;
            Value = valueCurrent;
        }

        public int Id { set; get; }
        public int Min { get; set; }

        public int Max { get; set; }

        public int Value  { get; set; }
 
        // -1 - error
        public int Up()
        {
            if (Value < Max)
            {
                Value++;
                return Value;
            }
            else
            {
                return -1;
            }
        }

        // -1 - error
        public int Down()
        {
            if (Value > Min)
            {
                Value--;
                return Value;
            }
            else
            {
                return -1;
            }
        }
    }
}