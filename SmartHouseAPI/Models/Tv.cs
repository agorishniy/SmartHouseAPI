namespace SmartHouse.Models
{
    public class Tv : Device, IDevVolume
    {
        public Tv()
        {
        }

        public Tv(string nameTv, bool stateTv, Channels channelCur, byte volumeCur)
            : base(nameTv, stateTv)
        {
            Channel = channelCur;
            Volume = new Param(volumeCur, 1, 5);
        }

        public Channels Channel { get; set; }

        public int VolumeId { get; set; }

        public Param Volume { get; set; }
 
        public void NextChannel()
        {
            if ((int)Channel < System.Enum.GetValues(typeof(Channels)).Length - 1)
            {
                Channel++;
            }
            else
            {
                Channel = 0;
            }
        }

        public void PreviousChannel()
        {
            if ((int)Channel > 0)
            {
                Channel--;
            }
            else
            {
                Channel = (Channels)(System.Enum.GetValues(typeof(Channels)).Length - 1);
            }
        }
    }
}