using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using Project_2.HostHandler;

namespace Project_2.Connector
{

    public class Connector
    {
        private  CancellationToken CT;
        private DataHandler _handler;
        public event Action<string> ConnectionStatusChanged;

        //private async 

        public Connector(DataHandler handler)
        {
            _handler = handler;
        }
         public void Connect(IPAddress ipAddress, short[] ports, short timeout)
        {
            Parallel.For(0, ports.Length, async port =>
            {
                using (TcpClient client = new TcpClient())
                {
                    try
                    {
                        Task connection = client.ConnectAsync(ipAddress, ports[port]);
                        if (await Task.WhenAny(connection, Task.Delay(timeout, CT)) == connection)
                        {
                            ConnectionStatusChanged?.Invoke($"{ipAddress}:{ports[port]} - подключение успешно");
                        }
                        else
                        {
                            //ConnectionStatusChanged?.Invoke($"{ipAddress}:{ports[port]} - подключение НЕуспешно");

                        }
                    }
                    catch (SocketException ignorable)
                    {
                        ConnectionStatusChanged?.Invoke($"{ipAddress}:{ports[port]} - подключение НЕ УДАЛОСЬ");

                    }
                
                    client.Close();
                    client.Dispose();
                }
            });

        }

         public void FindMeSomething()
        {
            foreach (IPAddress address in _handler.getIPAddresses())
            {
                Connect(address, _handler.getPorts(), _handler.getTimeout());
            }
        }
    }
}

