using BusinessObjectLayer.Models;
using DataAccessLayer;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        public async Task<string> CreateToken(Customer customer, string role)
        {
            return await CustomerDAO.CreateAccessToken(customer, role);
        }
        public async Task DeleteCustomerById(int customerId)
        {
            await CustomerDAO.DeleteCustomer(customerId);
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await CustomerDAO.GetAllCustomer();
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            return await CustomerDAO.GetCustomerByEmail(email);
        }

        public async Task<Customer> GetCustomerById(int customerId)
        {
            return await CustomerDAO.GetCustomerById(customerId);
        }

        public async Task<Customer> Login(string email, string password)
        {
            return await CustomerDAO.Login(email, password);
        }

        public async Task<bool> LoginAsAdmin(string email, string password)
        {
             return await CustomerDAO.LoginAsAdmin(email, password);
        }

        public async Task Register(Customer customer)
        {
            await CustomerDAO.Register(customer);
        }

        public Task<Customer> UpdateCustomer(Customer customer)
        {
            return CustomerDAO.UpdateCustomer(customer);
        }
    }
}
