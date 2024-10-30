using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace UsenetProgram.Services
{
    public class NetworkManager 
    {
        private static NetworkManager? instance;
        private static readonly object instanceLock = new object();
        private TcpClient? client;
        private NetworkStream? stream;

        private NetworkManager() { }

        public static NetworkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                            instance = new NetworkManager();
                    }
                }
                return instance;
            }
        }

        public async Task<bool> MakeConnectionAsync(string host, int port)
        {
            try
            {
                this.client = new TcpClient();
                await this.client.ConnectAsync(host, port);
                this.stream = this.client.GetStream();
                string initResponse = await ReadFromStreamAsync(false);
                if (initResponse.Equals("200 news.sunsite.dk NNRP Service Ready - staff@sunsite.dk (posting ok).\r\n"))
                    return true;
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"SocketException: {ex.Message}");
                return false;
            }
            return false;
        }

        public void Reset()
        {
            this.stream?.Close();
            this.client?.Close();
            this.stream = null;
            this.client = null;
        }

        public async Task<string> ReadFromStreamAsync(bool isExtendedResponse)
        {
            if (stream != null)
            {
                try
                {
                    StringBuilder responseBuilder = new StringBuilder();
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while (true)
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                        string part = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        responseBuilder.Append(part);

                        if (part.EndsWith(isExtendedResponse ? ".\r\n" : "\r\n"))
                        {
                            break;
                        }
                    }

                    return responseBuilder.ToString();
                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine($"Stream closed: {ex.Message}");
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public async Task<bool> WriteToStreamAsync(string message)
        {
            if (stream != null)
            {
                try
                {
                    byte[] messageBytes = Encoding.ASCII.GetBytes(message + "\r\n");
                    await this.stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                    return true;
                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine($"Stream closed: {ex.Message}");
                    return false;
                }
            }
            return false;
        }
    }
}
