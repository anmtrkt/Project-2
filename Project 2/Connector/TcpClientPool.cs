using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Project_2.Connector
{
    public class TcpClientPool
    {
        private readonly ConcurrentBag<TcpClient> _clients;
        private readonly short _maxClients;

        public TcpClientPool(short maxClients)
        {
            _clients = new ConcurrentBag<TcpClient>();
            _maxClients = maxClients;
            CreatePool(_maxClients);
        }
        /// <summary>
        /// Заполняет пул _count кол-вом TcpClientов.
        /// </summary>
        /// <param name="count"></param>
        /// <returns>true</returns>
        private bool CreatePool(short count)     
        {
           for(int i = 0; i < count; i++)
           {
               _clients.Add(new TcpClient());
           }
           return true;
        }
        public TcpClient GetClient()
        {
            if (_clients.TryTake(out TcpClient client))
            {
                return client;
            }
            else
            {
                return new TcpClient();
            }
        }

        public void ReturnClient(TcpClient client)
        {
            _clients.Add(client);
        }
    }
}
