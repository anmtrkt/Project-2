using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using Project_2.HostHandler;
using System.Collections.Concurrent;

namespace Project_2.Connector
{

    public class Connector
    {
        private  CancellationToken CT;
        private DataHandler _handler;
        public event Action<string> ConnectionStatusChanged;
        private TcpClientPool _clientPool;


        //private async 

        public Connector(DataHandler handler, TcpClientPool pool)
        {
            _handler = handler;
            _clientPool = pool;

        }




         public void Connect(IPAddress ipAddress, short[] ports, short timeout)
        {
            Parallel.ForEach(ports, port =>
            {
                TcpClient client = null;
                try
                {
                    client = _clientPool.GetClient();
                    client.Connect(ipAddress, port);
                    ConnectionStatusChanged?.Invoke($"{ipAddress}:{port} - подключение успешно");
                }
                catch (SocketException)
                {
                    ConnectionStatusChanged?.Invoke($"{ipAddress}:{port} - подключение НЕ УДАЛОСЬ");
                }
                catch (Exception ex)
                {
                    ConnectionStatusChanged?.Invoke($"{ipAddress}:{port} - ошибка: {ex.Message}");
                    client.Dispose();
                    
                }
                finally
                {
                    if (client != null)
                    {
                        _clientPool.ReturnClient(client);
                    }
                }
            });





                    /*
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
                    */

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

