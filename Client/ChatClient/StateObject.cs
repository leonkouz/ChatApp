using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public class StateObject
    {
        private Socket _workSocket = null;
        private byte[] _buffer = new byte[BufferSize];
        private StringBuilder _sb = new StringBuilder();

        public const int BufferSize = 1024;

        public Socket WorkSocket
        {
            get { return _workSocket; }
            set { _workSocket = value; }
        }

        public byte[] Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }
           
        public StringBuilder Sb
        {
            get { return _sb; }
        }

        public void DeleteData()
        {
            _sb = new StringBuilder();
        }

    }
}
