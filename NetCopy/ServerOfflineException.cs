using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCopy {
    public class ServerOfflineException : Exception {
        public ServerOfflineException() 
            : base("Server is offline or not available.") {
        }
    }
}
