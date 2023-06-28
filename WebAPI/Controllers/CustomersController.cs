using AutoMapper;
using BusinessObjectLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo customerRepo;
        private readonly IMapper mapper;
        public CustomersController(ICustomerRepo customerRepo, IMapper mapper)
        {
            this.customerRepo = customerRepo;
            this.mapper = mapper;
        }
        [HttpGet("getAllCustomers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var cus = await customerRepo.GetAll();
            if (cus.Count() == 0)
            {
                return NotFound(new ResponseObject
                {
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "List null",
                    Data = null
                });
            }
            var res = mapper.Map<List<CustomerResponse>>(cus);
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.OK.ToString(),
                Message = "Get list of customers successfully",
                Data = res
            });
        }
        [HttpPut("updateCustomer")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateCustomer([FromBody]UpdateCustomerRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cus = mapper.Map<Customer>(request);
            var cusUpdate = await customerRepo.UpdateCustomer(cus);
            var res = mapper.Map<CustomerResponse>(cusUpdate);
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.OK.ToString(),
                Message = "Update info of customers successfully",
                Data = res
            });
        }
        [HttpDelete("deleteCustomer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            if(customerId == null)
            {
                return BadRequest();
            }
            await customerRepo.DeleteCustomerById(customerId);
           
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.OK.ToString(),
                Message = "Delete customer successfully",
                Data = null
            });
        }
    }
}
