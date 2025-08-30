using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Optimization_Example_2_solution.Exceptions
{
    public class InvalidProductException : Exception
    {
        public InvalidProductException(string message)
            : base(message)
        {

        }
    }
}
