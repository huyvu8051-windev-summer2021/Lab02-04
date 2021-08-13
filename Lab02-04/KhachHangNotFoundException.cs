using System;
using System.Collections.Generic;
using System.Text;

namespace Lab02_04
{
    class KhachHangNotFoundException : Exception
    {
        public KhachHangNotFoundException(string message) : base(message)
        {
            
        }
    }
}
