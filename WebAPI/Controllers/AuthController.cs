using AutoMapper;
using BusinessObjectLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerRepo customerRepo;
        private readonly IMapper mapper;
        public AuthController(ICustomerRepo customerRepo, IMapper mapper)
        {
            this.customerRepo = customerRepo;
            this.mapper = mapper;
        }
        private string Email => FindClaim(ClaimTypes.Email);
        private string FindClaim(string claimName)
        {

            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

            var claim = claimsIdentity.FindFirst(claimName);

            if (claim == null)
            {
                return null;
            }

            return claim.Value;

        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]CreateCustomerRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var cus = mapper.Map<Customer>(request);
            await customerRepo.Register(cus);
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.Created.ToString(),
                Message = "Create customer succesfully",
                Data = null
            });
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var adminLog = await customerRepo.LoginAsAdmin(request.Email, request.Password);
            if (adminLog == true)
            {
                Customer customer = new Customer
                {
                    Email = request.Email
                };
                var accessToken  = await customerRepo.CreateToken(customer, "Admin");
                TokenResponse res = new TokenResponse
                {
                    AccessToken = accessToken
                };
                return Ok(new ResponseObject
                {
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Login successfully",
                    Data = res
                });
            }
            else
            {
                var customer = await customerRepo.Login(request.Email, request.Password);
                if (customer == null)
                {
                    return NotFound();
                }
                var accessToken = await customerRepo.CreateToken(customer, "Customer");
                TokenResponse res = new TokenResponse
                {
                    AccessToken = accessToken
                };
                return Ok(new ResponseObject
                {
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Login successfully",
                    Data = res
                });
            }
            
        }
        [HttpPost("getInfo")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetInfoCustomer()
        {
            var cus = await customerRepo.GetCustomerByEmail(Email);
            var res = mapper.Map<CustomerResponse>(cus);
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.OK.ToString(),
                Message = "Get information of customer successfully",
                Data = res
            });
        }
    }
}
