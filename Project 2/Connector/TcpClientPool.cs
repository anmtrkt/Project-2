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
        }

        public TcpClient GetClient()
        {
            if (_clients.TryTake(out TcpClient client))
            {
                return client;
            }
            else if (_clients.Count < _maxClients)
            {
                return new TcpClient();
            }
            else
            {
                _clients.
            }
        }

        public void ReturnClient(TcpClient client)
        {
            _clients.Add(client);
        }
    }
}
