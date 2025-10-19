using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherAnkiTool.Core.API
{
    public class ChainedWaveSource : IWaveSource
    {
        private readonly List<IWaveSource> _sources;
        private int _currentIndex;
        private IWaveSource _current;

        public ChainedWaveSource(IEnumerable<IWaveSource> sources)
        {
            _sources = sources.ToList();
            _currentIndex = 0;
            _current = _sources[_currentIndex];
            WaveFormat = _current.WaveFormat;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int read = _current.Read(buffer, offset, count);
            while (read == 0 && _currentIndex < _sources.Count - 1)
            {
                _currentIndex++;
                _current = _sources[_currentIndex];
                read = _current.Read(buffer, offset, count);
            }
            return read;
        }

        public bool CanSeek => _sources.All(s => s.CanSeek);
        public WaveFormat WaveFormat { get; }

        public long Position
        {
            get
            {
                long position = 0;
                for (int i = 0; i < _currentIndex; i++)
                    position += _sources[i].Length;

                position += _current.Position;
                return position;
            }
            set
            {
                if (!CanSeek)
                    throw new NotSupportedException();

                long remaining = value;
                for (int i = 0; i < _sources.Count; i++)
                {
                    if (remaining < _sources[i].Length)
                    {
                        _currentIndex = i;
                        _current = _sources[i];
                        _current.Position = remaining;
                        return;
                    }
                    remaining -= _sources[i].Length;
                }

                // If value exceeds total length, seek to end
                _currentIndex = _sources.Count - 1;
                _current = _sources[_currentIndex];
                _current.Position = _current.Length;
            }
        }

        public long Length => _sources.Sum(s => s.Length);

        public void Dispose()
        {
            foreach (var source in _sources)
                source.Dispose();
        }
    }

}
