using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Optimization_Example_1_Solution
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        IEnumerable<Customer> GetAllActive();
    }
}
