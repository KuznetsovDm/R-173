using System;
using NAudio.Wave;
using P2PMulticastNetwork.Model;
using P2PMulticastNetwork.Network;
using R_173.BL;

namespace NetworkTaskTest
{
	public class Player
	{
		private readonly UdpMulticastConnection _transmitter;
		private readonly Guid _guid;
		private readonly Converter<DataModel> _converter;
		private readonly WaveIn _waveIn;

		private readonly DataCompressor _compressor;

		public Player()
		{
			var options = MulticastConnectionOptions.Create();

			_guid = Guid.NewGuid();
			_converter = new Converter<DataModel>();
			_transmitter = new UdpMulticastConnection(options);

			_waveIn = new WaveIn { WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2) };
			_compressor = new DataCompressor();
		}

		public NetworkTaskData NetworkTaskData { get; set; }
		public int Frequency { get; set; }

		public void Play(string[] args)
		{
			_waveIn.StartRecording();
			_waveIn.DataAvailable += WaveIn_DataAvailable;
		}

		private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
		{
			var data = new DataModel
			{
				Guid = _guid,
				RawAudioSample = e.Buffer,
				RadioModel = new SendableRadioModel { Frequency = Frequency },
				NetworkTask = NetworkTaskData
			};
			
			var bytes = _converter.ConvertToBytes(data);

			var compressed = _compressor.Compress(bytes);
			_transmitter.Write(compressed);
		}
	}
}