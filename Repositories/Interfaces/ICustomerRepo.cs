using BusinessObjectLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ICustomerRepo
    {
        Task Register(Customer customer);
        Task<Customer> Login(string email, string password);
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetCustomerById(int customerId);
        Task<Customer> GetCustomerByEmail(string email);
        Task<Customer> UpdateCustomer(Customer customer);
        Task DeleteCustomerById(int customerId);
        Task<bool> LoginAsAdmin(string email, string password);
        Task<string> CreateToken(Customer customer, string role);
    }
}
