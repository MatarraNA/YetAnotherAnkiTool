using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherAnkiTool.Core.API
{
    public class SimpleWaveSource : IWaveSource
    {
        private readonly Stream _stream;
        public WaveFormat WaveFormat { get; }
        public long Length => _stream.Length;
        public long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        public SimpleWaveSource(Stream stream, WaveFormat format)
        {
            _stream = stream;
            WaveFormat = format;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public bool CanSeek => _stream.CanSeek;
        public void Dispose() => _stream.Dispose();
    }
}
