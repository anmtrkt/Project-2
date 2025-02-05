using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Project_2.HostHandler
{
    public class DataHandler
    {

        private IPAddress[] _ipAddresses;
        private short[] _ports;
        private short _timeout;
        private short _threads;
        
        public DataHandler()
        {
            
        }

        public void setIPAddresses(IPAddress[] ipAddresses) { _ipAddresses = ipAddresses;}
        public void setPorts(short[] ports) { _ports = ports;}
        public void setTimeout(short timeout) { _timeout = timeout;}
        public void setThreads(short threads) { _threads = threads;}

        public IPAddress[] getIPAddresses() { return _ipAddresses;}
        public short[] getPorts() { return _ports;}
        public short getTimeout() { return _timeout;}
        public short getThreads() { return _threads;}

    }
    public class ipReader
    {
        private DataHandler _handler;

        public ipReader(DataHandler handler)
        {
            _handler = handler;
        }
        public string ReadFile(string filePath)
        {
            string[] ipAdressesString = File.ReadAllLines(filePath);
            _handler.setIPAddresses(ParseIPAddresses(ipAdressesString));
            string fileName = Path.GetFileName(filePath);
            return fileName;

        }

        public void ReadLine(string line)
        {
            _handler.setIPAddresses(ParseIPAddresses(line.Split()));
        }


        private IPAddress[] ParseIPAddresses(string[] ipStrings)
        {
            try
            {
                List<IPAddress> iPAddresses = new List<IPAddress>();
                foreach (string str in ipStrings)
                {
                    if (str.Contains("-") == true)
                    {
                        string[] str2 = str.Split('-');


                        IPAddress from = IPAddress.Parse(str2[0]);
                        IPAddress to = IPAddress.Parse(str2[1]);
                        var buffer = from.GetAddressBytes();
                        do
                        {
                            from = new IPAddress(buffer);
                            iPAddresses.Add(from);
                            int i = buffer.Length - 1;
                            while (i >= 0 && ++buffer[i] == 0) i--;
                        } while (!from.Equals(to));


                    }
                    else
                    {
                        
                        iPAddresses.Add(IPAddress.Parse(str));
                    }

                }
                
                return iPAddresses.ToArray();
            }
            catch ( Exception e)
            {
                return null;
            }

            
    
        }


    

    }
    public class portReader
    {
        private DataHandler _handler;
        public portReader(DataHandler handler)
        {
            _handler = handler;
        }
        private short[] ParsePorts(string rawPortsString)
        {
            string[] rawPorts = rawPortsString.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<short> result = new List<short>();
            switch (rawPortsString.Contains("-"))
            {
                case true:

                    foreach (string part in rawPorts)
                    {
                        if (part.Contains("-"))
                        {
                            Console.WriteLine(rawPortsString);
                            string[] range = part.Split('-');
                            if (range.Length == 2 && short.TryParse(range[0], out short start) && short.TryParse(range[1], out short end))
                            {
                                for (short i = start; i <= end; i++)
                                {
                                    result.Add(i);
                                }
                            }
                        }
                        else
                        {
                            result.Add(short.Parse(part));
                        }
                    }
                    
                    return result.ToArray();
                case false:
                    foreach (string part in rawPorts)
                    {
                        result.Add(short.Parse(part.Trim()));
                    }
                    return result.ToArray();
            }
            
        }
        public void ReadPorts(string rawPortsString)
        {
            _handler.setPorts(ParsePorts(rawPortsString));
        }

    }
}
