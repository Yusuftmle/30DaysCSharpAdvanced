using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Optimization_Example_1_Solution
{
    public class DiscountCalculator
    {
        public static decimal Calculate(Customer customer)
        {
            return customer.IsActive ? 0.1M : 0;
        }
    }
}
