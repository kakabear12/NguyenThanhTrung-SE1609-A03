using AutoMapper;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo orderRepo;
        private readonly ICustomerRepo customerRepo;
        private readonly IMapper mapper;
        public OrderController(IOrderRepo orderRepo, IMapper mapper, ICustomerRepo customer)
        {
            this.orderRepo = orderRepo;
            this.mapper = mapper;
            customerRepo = customer;
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
        [HttpGet("getOrderHistory")]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> GetOrderHistory() {
            var cus = await customerRepo.GetCustomerByEmail(Email);
            var oder = await orderRepo.GetOrderHistory(cus.CustomerId);
            var res = mapper.Map<OrdResponse>(oder);
            return Ok(res);
        }
    }
}
