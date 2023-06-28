using BusinessObjectLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CustomerDAO
    {
        public static async Task Register(Customer customer)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    if (EmailExists(customer.Email))
                    {
                        throw new Exception("Mail existed");
                    }
                    while(context.Customers.Any(c => c.CustomerId == customer.CustomerId) == true)
                    {
                        customer.CustomerId += 1;
                    }
                    await context.Customers.AddAsync(customer);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<string> CreateAccessToken(Customer user, string role)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,  user.Email),
                    new Claim(ClaimTypes.Role, role)
                };
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    configuration.GetSection("JWT:Key").Value));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var token = new JwtSecurityToken(configuration["JWT:Issuer"],
                    configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: cred);
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<Customer> Login(string email, string password)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    if (EmailExists(email))
                    {
                        var cus = await context.Customers.SingleOrDefaultAsync(c => c.Email == email && c.Password == password);
                        if (cus == null)
                        {
                            throw new Exception("Password is invalid");
                        }
                        return cus;
                    }
                    else
                    {
                        throw new Exception("Email is invalid");
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<Customer> GetCustomerById(int userId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var cus = await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == userId);
                    if (cus == null)
                    {
                        throw new Exception("Customer not found");
                    }
                    return cus;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<Customer> GetCustomerByEmail(string email)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var cus = await context.Customers.FirstOrDefaultAsync(c => c.Email == email);
                    if (cus == null)
                    {
                        throw new Exception("Customer not found");
                    }
                    return cus;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<Customer> UpdateCustomer(Customer customer)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var cusUpdate = await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
                    if (cusUpdate == null)
                    {
                        throw new Exception("Customer not found");
                    }
                    if (cusUpdate.Email != customer.Email)
                    {
                        if (EmailExists(customer.Email))
                        {
                            throw new Exception("Email is existed");
                        }
                        cusUpdate.Email = customer.Email;
                    }
                    cusUpdate.CustomerName = customer.CustomerName;
                    cusUpdate.Birthday = customer.Birthday;
                    cusUpdate.City = customer.City;
                    cusUpdate.Country = customer.Country;
                    await context.SaveChangesAsync();
                    return cusUpdate;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task DeleteCustomer(int customerId)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var cus = await context.Customers.SingleOrDefaultAsync(c => c.CustomerId == customerId);
                    if (cus == null)
                    {
                        throw new Exception("Customer not found");
                    }
                    context.Customers.Remove(cus);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<Customer>> GetAllCustomer()
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    return await context.Customers.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //=================================
        private static bool EmailExists(string email)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    return context.Customers.Any(u => u.Email == email);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async static Task<bool> LoginAsAdmin(string email, string password)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string adminEmail = configuration["Account:Email"];
            string adminPassword = configuration["Account:Password"];

            return  (email == adminEmail && password == adminPassword);
        }
    }
}
