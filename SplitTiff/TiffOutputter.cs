using System.IO;
using Microsoft.Analytics.Interfaces;

namespace SplitTiff
{
    [SqlUserDefinedOutputter(AtomicFileProcessing = true)]
    public class TiffOutputter : IOutputter
    {
        public override void Output(IRow input, IUnstructuredWriter output)
        {
            var tiff = input.Get<byte[]>("tiff");
            using (var writer = new BinaryWriter(output.BaseStream))
            {
                writer.Write(tiff);
            }
        }
    }
}
