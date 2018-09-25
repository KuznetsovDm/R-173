namespace R_173.Interfaces
{
    internal interface IGlobalNoiseController
    {
        double Volume { get; set; }
        void Play();
        void Stop();
    }
}