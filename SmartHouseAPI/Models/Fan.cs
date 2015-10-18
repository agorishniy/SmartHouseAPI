namespace SmartHouse.Models
{
    public class Fan : Device, IDevSpeed
    {
        public Fan()
        {

        }

        public Fan(string nameDev, bool stateDev, byte speedFan)
            : base(nameDev, stateDev)
        {
            Speed = new Param(speedFan, 1, 5);
        }

        public int SpeedId { get; set; }
        public Param Speed { get; set; }
    }
}