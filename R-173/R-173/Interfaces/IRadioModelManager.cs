using R_173.Models;

namespace R_173.Interfaces
{
    public interface IRadioModelManager
    {
        RadioModel this[string page] { get; set; }
    }
}
