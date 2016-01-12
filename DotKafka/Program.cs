using DotKafka.Prototype.Common.Protocol;
using DotKafka.Prototype.Common.Protocol.Types;
using DotKafka.Prototype.Common.Requests;
using Helios.Concurrency;
using Helios.Exceptions;
using Helios.Net;
using Helios.Net.Bootstrap;
using Helios.Serialization;
using Helios.Topology;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KafkaTcpTest
{
    class Program
    {
        public static INode RemoteHost;
        public static IConnection kafkaClient;


        private static void ConnectionTerminatedCallback(HeliosConnectionException reason, IConnection closedChannel)
        {
            Console.WriteLine("Connection Terminated");
        }

        private static void ReceivedDataCallback(NetworkData incomingData, IConnection responseChannel)
        {
            Console.WriteLine("Incomming Data ");
            HexPrint(incomingData.Buffer);
        }

        private static void ConnectionEstablishedCallback(INode remoteAddress, IConnection responseChannel)
        {
            Console.WriteLine("Connection Stablished");
            kafkaClient.BeginReceive(ReceivedDataCallback);
        }

        static void Main(string[] args)
        {
            
            RemoteHost = NodeBuilder.BuildNode().Host("192.168.0.12").WithPort(9092).WithTransportType(TransportType.Tcp);

            kafkaClient =
                new ClientBootstrap()
                    .SetTransport(TransportType.Tcp)
                    .SetDecoder(new NoOpDecoder())
                    .SetEncoder(new NoOpEncoder())
                    .RemoteAddress(RemoteHost)
                    .OnConnect(ConnectionEstablishedCallback)                    
                    .OnDisconnect(ConnectionTerminatedCallback)
                    .Build().NewConnection(Node.Empty().WithTransportType(TransportType.Tcp), RemoteHost);

            kafkaClient.Open();

            
            //            TcpClient client = new TcpClient("192.168.0.12", 9092);

            Console.Title = string.Format("KafkaClient {0}", Process.GetCurrentProcess().Id);

            kafkaClient.OnError += KafkaClient_OnError;

            var request = new MetadataRequest(new List<string> { "test_topic" });
            var requestHeader = new RequestHeader((short)ApiKeys.Metadata, "Mr Flibble", 1234);

            var buffer = new MemoryStream();
            
            requestHeader.WriteTo(buffer);
            request.WriteTo(buffer);

            var bytes = buffer.ToArray();

            var lenght = KafkaTypesHelper.Int32;

            buffer = new MemoryStream();

            lenght.Write(buffer, bytes.Length);

            var writter = new BinaryWriter(buffer);

            writter.Write(bytes);

            Console.WriteLine("Bytes Lenght " + bytes.Length);

            bytes = buffer.ToArray();

            HexPrint(bytes);

            //kafkaClient.Receive += KafkaClient_Receive;
            //NetworkStream nwStream = client.GetStream();            
            //nwStream.Write(bytes, 0, bytes.Length);
            kafkaClient.Send(new NetworkData() { Buffer = bytes, Length = bytes.Length });

            Thread.Sleep(5000);

            kafkaClient.Send(new NetworkData() { Buffer = bytes, Length = bytes.Length });

            /*byte[] bytesToRead = new byte[client.ReceiveBufferSize];

            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

            bytesToRead = bytesToRead.Take(bytesRead).ToArray();

            HexPrint(bytesToRead);

            bytesToRead = bytesToRead.Skip(8).ToArray();
            
            var s = ProtoUtils.parseResponse((short)ApiKeys.Metadata, new MemoryStream(bytesToRead));

            var response = new MetadataResponse(s);*/

            Console.WriteLine("Press any key to exit.");

            Console.ReadLine();
        }

        private static void KafkaClient_Receive(NetworkData incomingData, IConnection responseChannel)
        {
            Console.WriteLine("Data Received ... ");
            HexPrint(incomingData.Buffer);
        }

        private static void KafkaClient_OnError(Exception ex, IConnection connection)
        {
            throw new NotImplementedException();
        }

        private static void HexPrint(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (i > 0 && i % 2 == 0)
                {
                    Console.Write(" ");
                }

                if( i % 16 == 0 )
                {
                    Console.WriteLine();
                }

                Console.Write(data[i].ToString("x2"));
                                
            }
        }
        
        static void LoopConnect()
        {
            var attempts = 0;
            while (!kafkaClient.IsOpen())
            {
                try
                {
                    attempts++;
                    kafkaClient.Open();
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Connection attempt {0}", attempts);
                    if (attempts > 5) throw;
                }
            }
            Console.Clear();
            Console.WriteLine("Connected");
        }
    }
}
