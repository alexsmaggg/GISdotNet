using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;

namespace GISdotNet.Core.Channels
{

    public delegate Task MapWork<T>(T val);

    public class  Producer<T>
    {
        private readonly ChannelWriter<T> _writer;        

        public Producer(ChannelWriter<T> writer)
        {
            _writer = writer;           
        }

        public async Task BeginProducing(T data)
        {            
            await _writer.WriteAsync(data);
        }
    }


    public class Consumer<T>
    {
        private readonly ChannelReader<T> _reader;
        private readonly MapWork<T> _work;

        public Consumer(ChannelReader<T> reader, MapWork<T> work)
        {
            _reader = reader;
            _work = work;
        }

        public async Task ConsumeData()
        {
            while (await _reader.WaitToReadAsync())
            {
                while (_reader.TryRead(out T data))
                {
                    await _work(data);     // some map work on data              
                }
            }
        }
    }
}
