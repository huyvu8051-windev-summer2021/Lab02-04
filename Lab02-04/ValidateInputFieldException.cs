using System;
using System.Collections.Generic;
using System.Text;

namespace Lab02_04
{
    class ValidateInputFieldException : Exception
    {
        public ValidateInputFieldException(string message) : base(message)
        {
        }
    }
}
