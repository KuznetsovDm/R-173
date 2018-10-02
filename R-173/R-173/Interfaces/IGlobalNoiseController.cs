namespace R_173.Interfaces
{
    public interface IGlobalNoiseController
    {
        double Volume { get; set; }
        void Play();
        void Stop();
    }
}