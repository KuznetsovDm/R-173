using P2PMulticastNetwork.Interfaces;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace P2PMulticastNetwork.Model
{
	[Serializable]
    public class DataModel
    {
        public byte[] RawAudioSample { get; set; }

        public Guid Guid { get; set; }

        public SendableRadioModel RadioModel { get; set; }

		public NetworkTaskData NetworkTask { get; set; }
	}

	[Serializable]
	public class NetworkTaskData
	{
		public Guid Id { get; set; }
		public int Frequency { get; set; }
	}

	public class Converter<T> : IDataAsByteConverter<T>
	{
		private readonly BinaryFormatter _formatter;

		public Converter()
		{
			_formatter = new BinaryFormatter();
		}

		public T ConvertFrom(byte[] data)
		{
			using (var ms = new MemoryStream(data))
			{
				return (T)_formatter.Deserialize(ms);
			}
		}

		public byte[] ConvertToBytes(T data)
		{
			using (var ms = new MemoryStream())
			{
				_formatter.Serialize(ms, data);
				return ms.ToArray();
			}
		}
	}

    public class DataModelConverter : IDataAsByteConverter<DataModel>
    {
        private readonly BinaryFormatter _formatter;

        public DataModelConverter()
        {
            _formatter = new BinaryFormatter();
        }

        public DataModel ConvertFrom(byte[] data)
        {
            DataModel model;
            using(var ms = new MemoryStream(data))
            {
                model = (DataModel)_formatter.Deserialize(ms);
            }
            return model;
        }

        public byte[] ConvertToBytes(DataModel data)
        {
            byte[] bytes;
            using(var ms = new MemoryStream())
            {
                _formatter.Serialize(ms, data);
                bytes = ms.ToArray();
            }
            return bytes;
        }
    }
}
