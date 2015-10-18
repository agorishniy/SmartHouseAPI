namespace SmartHouse.Models
{
    public class Lamp : Device
    {
        public Lamp()
        {
        }
        public Lamp(string nameDev, bool stateDev)
            : base(nameDev, stateDev)
        {
        }
    }
}