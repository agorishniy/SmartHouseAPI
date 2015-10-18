namespace SmartHouse.Models
{
    public class Louvers : Device, IDevOpen
    {
        public Louvers()
        {
        }
        public Louvers(string nameDev, bool stateDev, byte openPercent)
            : base(nameDev, stateDev)
        {
            Open = new Param(openPercent, 0, 2);
        }

        public Param Open { get; set; }
    }
}