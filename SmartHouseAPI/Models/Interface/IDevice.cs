namespace SmartHouse.Models
{
    public interface IDevice
    {
        int Id { get; set; }
        string Name { get; set; }

        bool State { get; set; }

        void OnOff();
    }

}
