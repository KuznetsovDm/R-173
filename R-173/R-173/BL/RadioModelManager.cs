using R_173.Interfaces;
using R_173.Models;
using System.Collections.Generic;

namespace R_173.BL
{
    public class RadioModelManager : IRadioModelManager
    {
        private readonly Dictionary<string, RadioModel> _storage = new Dictionary<string, RadioModel>();

        public RadioModel this[string page]
        {
            get
            {
	            _storage.TryGetValue(page, out var model);
                return model;
            }
            set => _storage[page] = value;
        }
    }
}
