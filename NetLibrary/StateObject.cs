using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetLibrary {
    public class StateObject {
        public Socket workSocket = null;

        public const int BufferSize = 256;

        public byte[] buffer = new byte[BufferSize];

        //TODO: Generalize this
        //Recieved data
        public StringBuilder sb = new StringBuilder();
    }
}
