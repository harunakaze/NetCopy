using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCopy {
    static class NetworkClient {
        //NetCopy Port
        private const ushort WorkingPort = 32323;

        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent recieveDone = new ManualResetEvent(false);

        //TODO: Generalize this
        //Recieved data
        private static String responseMessage = String.Empty;
        
        public static void StartClient() {
            Console.WriteLine("Starting network client...");

            try {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint endPoint = new IPEndPoint(ipAddress, WorkingPort);

                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                client.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                Send(client, "Sended data<EOF>");
                sendDone.WaitOne();

                Receive(client);
                recieveDone.WaitOne();

                Console.WriteLine("Response Recieved {0}", responseMessage);

                client.Shutdown(SocketShutdown.Both);
                client.Close();

                Console.WriteLine("Network client stopped...");
            } catch(Exception e) {
                Console.WriteLine("Start Client Error " + e);
            }
        }

        private static void ConnectCallback(IAsyncResult ar) {
            try {
                Socket client = (Socket) ar.AsyncState;

                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
            }
            catch (Exception e) {
                Console.WriteLine("Connect Callback Error " + e);
            }
            finally {
                connectDone.Set();
            }
        }

        //TODO: Generalize this
        private static void Send(Socket client, String data) {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar) {
            try {
                Socket client = (Socket)ar.AsyncState;

                int byteSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} byte to server.", byteSent);

            }
            catch (Exception e) {
                Console.WriteLine("Send Callback Error " + e);
            }
            finally {
                sendDone.Set();
            }
        }

        private static void Receive(Socket client) {
            try {
                StateObject state = new StateObject();
                state.workSocket = client;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e) {
                Console.WriteLine("Recieve Error " + e);
            }
        }

        private static void ReceiveCallback(IAsyncResult ar) {
            try {
                StateObject stateObject = (StateObject)ar.AsyncState;
                Socket client = stateObject.workSocket;

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0) {
                    stateObject.sb.Append(Encoding.ASCII.GetString(stateObject.buffer, 0, bytesRead));

                    client.BeginReceive(stateObject.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), stateObject);
                }
                else {
                    if (stateObject.sb.Length > 1) {
                        responseMessage = stateObject.sb.ToString();
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("Receive Callback Error " + e);
            }
            finally {
                recieveDone.Set();
            }
        }
    }
}
