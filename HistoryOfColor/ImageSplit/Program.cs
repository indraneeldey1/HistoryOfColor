// See https://aka.ms/new-console-template for more information

using System.Drawing;
using Confluent.Kafka;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

class Program
{
    static void Main(string[] args)
    {
        //TODO: remove temp image path and swap to
        string imagePath = "image/young_woman_with_a_butterfly_1980.62.40.jpg";
        try
        {
            //Set up producer for Kafka
            ProducerConfig producerConfig = new()
            {
                BootstrapServers = Environment.GetEnvironmentVariable("HC_BOOTSTRAP") ?? "localhost:9092",
                Acks = Acks.All
            };

        #region Image Mat set up

            Mat imageMat = CvInvoke.Imread(imagePath);
            int squareWidth = imageMat.Width / 100;
            int squareHeight = imageMat.Height / 100;
            Mat square = new();

        #endregion

            using IProducer<string, byte[]>? producer = new ProducerBuilder<string, Byte[]>(producerConfig).Build();

            /* Parallel set up for 100 x 100
             * only goes parallel on the outer side to prevent any overflows or issues
             */
            Parallel.For(0, 100, x =>
            {
                for (int y = 0; y < 100; y++)
                {
                    int xPos = x * squareWidth;
                    int yPos = y * squareHeight;

                    square = new(imageMat, new Rectangle(xPos, yPos, squareWidth, squareHeight));
                    
                    // Creates basically an array of bytes
                    VectorOfByte buffer = new();
                    
                    //fills buffer as a jpg encoding
                    CvInvoke.Imencode(".jpg", square, buffer);
                    try
                    {
                        producer.ProduceAsync("Topic1", new Message<string, byte[]>
                        {
                            Key = $"x_{x}_y_{y}",
                            Value = buffer.ToArray()
                        });
                        producer.Flush();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}