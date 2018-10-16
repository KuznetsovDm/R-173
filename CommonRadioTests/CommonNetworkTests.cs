using NUnit.Framework;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Model;
using R_173.BL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonRadioTests
{
    [TestFixture]
    public class CommonNetworkTests
    {
        [Test]
        public void TestCompressor()
        {
            DataCompressor compressor = new DataCompressor();
            DataModelConverter converter = new DataModelConverter();

            DataModel model = new DataModel
            {
                Guid = Guid.NewGuid(),
                RadioModel = new SendableRadioModel
                {
                    Frequency = 10
                },
                RawAudioSample = Encoding.UTF32.GetBytes(String.Join("", Enumerable.Range(0, 100)))
            };
            var bytes = converter.ConvertToBytes(model);

            var r1 = compressor.Compress(bytes);
            var r2 = compressor.Decompress(r1);

            Assert.IsTrue(bytes.Except(r2).Count() == 0);
        }

    }
}
