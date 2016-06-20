using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetPaste {
    class NetworkServer {
        //NetCopy Port
        private const ushort WorkingPort = 32323;

        private static ManualResetEvent allDone = new ManualResetEvent(false);

        public static void StartListening() {
            Console.WriteLine("Server StartListening...");
            byte[] bytes = new byte[1024];

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, WorkingPort);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try {
                listener.Bind(endPoint);
                listener.Listen(100);

                while (true) {
                    allDone.Reset();

                    Console.WriteLine("Waiting for connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    allDone.WaitOne();
                }
            }
            catch (Exception e) {
                Console.WriteLine("Start Listening Error " + e);
            }

            Console.WriteLine("Server Terminated...");
        }

        private static void AcceptCallback(IAsyncResult ar) {
            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject stateObject = new StateObject();
            stateObject.workSocket = handler;

            handler.BeginReceive(stateObject.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), stateObject);
        }

        private static void ReadCallback(IAsyncResult ar) {
            //TODO: Generalize this
            String content = String.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int byteRead = handler.EndReceive(ar);

            if (byteRead > 0) {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, byteRead));

                content = state.sb.ToString();

                if (content.IndexOf("<EOF>") > -1) {
                    Console.WriteLine("Read {0} byte from socket. \n Data : {1}", content.Length, content);

                    Send(handler, content);
                }
                else {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data) {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar) {
            try {
                Socket handler = (Socket)ar.AsyncState;

                int byteSend = handler.EndSend(ar);
                Console.WriteLine("Sent {0} byte to client", byteSend);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e) {
                Console.WriteLine("Send Callback Error " + e);
            }
        }
    }
}
