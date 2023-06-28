using AutoMapper;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICardRepo cartRepo;
        private readonly ICustomerRepo customerRepo;
        private readonly IMapper mapper;
        public CartController(ICardRepo cart, IMapper mapper, ICustomerRepo customer)
        {
            this.cartRepo = cart;
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
        [HttpPost("addToCart")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddToCartRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseObject
                {
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = ModelState.ToString(),
                    Data =null
                });
            }
            var cus = await customerRepo.GetCustomerByEmail(Email);
            await cartRepo.AddFlowerBouquetToCart(cus.CustomerId, request.FlowerBouquetId, request.Quantity);
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.OK.ToString(),
                Message = "Add to cart successfully",
                Data = null
            });
        }
        [HttpGet("getCartItems")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCartItems()
        {
            var cus = await customerRepo.GetCustomerByEmail(Email);
            var order = await cartRepo.GetCartItems(cus.CustomerId);
            var res = mapper.Map<OrderResponse>(order);
            return Ok(res);
        }
        [HttpPost("checkout")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CheckoutCart()
        {
            var cus = await customerRepo.GetCustomerByEmail(Email);
            var order = await cartRepo.CheckoutCart(cus.CustomerId);
            var res = mapper.Map<OrdResponse>(order);
            return Ok(res);
        }
        [HttpDelete("removeCartItem/{flowerId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteCartItem(int flowerId)
        {
            var cus = await customerRepo.GetCustomerByEmail(Email);
            await cartRepo.RemoveFlowerBouquetFromCart(cus.CustomerId, flowerId);
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.OK.ToString(),
                Message = "Delete cart item successfully",
                Data = null
            });
        }
    }
}
