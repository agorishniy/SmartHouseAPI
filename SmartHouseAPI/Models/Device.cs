namespace SmartHouse.Models
{
    public class Device : IDevice
    {
        public Device()
        {
        }
        public Device(string nameDev, bool stateDev)
        {
            Name = nameDev;
            State = stateDev;
        }

        public int Id { set; get; }

        public string Name { get; set; }

        public bool State { get; set; }

        public void OnOff()
        {
            if (State)
            {
                State = false;
            }
            else
            {
                State = true;
            }
        }
    }
}
