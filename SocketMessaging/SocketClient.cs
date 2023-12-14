using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketMessaging
{
    internal class SocketClient
    {
        private IPAddress _address;
        private IPEndPoint _remoteEP;
        private Socket _socket;
        private bool _loggedIn;
        public bool loggedIn {get { return _loggedIn; } set { _loggedIn = value; } }
        // Constructor for the socket client, requires string IP address and int port
        public SocketClient(string ipAddress, int port)
        {
            _address = IPAddress.Parse(ipAddress);
            _remoteEP = new IPEndPoint(_address, port);

            _socket = new Socket(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _socket.ReceiveTimeout = 1000;
            _loggedIn = false;
        }

        // Use the connect method to establish a connection with the remote device
        public List<string> Connect()
        {
            _socket.Connect(_remoteEP);

            //var temp = this.Receive();

            return this.Receive();
        }
        
        public List<string> Send(string msg)
        {
            //_socket.Blocking = false;

            // Send message to the remote device
            byte[] bMsg = Encoding.ASCII.GetBytes(msg + "\r\n");
            int bytesSent = _socket.Send(bMsg);

            return this.Receive();
        }

        // Release the socket
        public void Disconnect()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        private List<string> Receive()
        {
            // Receive the response from the remote device
            byte[] buffer = new byte[1024];
            int bytesRec = _socket.Receive(buffer);
            Thread.Sleep(100);

            // Create a list to store the received bytes
            List<byte> byteList = new List<byte>(buffer.Take(bytesRec));            

            // While there is more data available, receive it and add it to the list
            if (!loggedIn)
            {
                bytesRec = _socket.Receive(buffer);
                byteList.AddRange(buffer.Take(bytesRec));
            }

            // Convert the list of bytes to a string
            string response = Encoding.ASCII.GetString(byteList.ToArray());

            // Split the string into an array of messages using the <EOM> delimeter
            string[] messageArray = response.Split(new string[] { "<EOM>" }, StringSplitOptions.RemoveEmptyEntries);

            // Create a list to store the messages
            List<string> messages = new List<string>();

            // Add each message to the list
            messages.AddRange(messageArray);

            // Return the list of received strings
            return messages;
        }

    }
}
