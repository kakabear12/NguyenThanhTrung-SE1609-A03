using AutoMapper;
using BusinessObjectLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowerBouquetsController : ControllerBase
    {
        private readonly IFlowerBouquetRepo flowerBouquetRepo;
        private readonly IMapper mapper;
        public FlowerBouquetsController(IFlowerBouquetRepo flowerBouquetRepo, IMapper mapper)
        {
            this.flowerBouquetRepo = flowerBouquetRepo;
            this.mapper = mapper;
        }

        [HttpGet("getAllFlowerBouquets")]
        [Authorize]
        public async Task<IActionResult> GetAllCustomers()
        {
            var flowerBouquets = await flowerBouquetRepo.GetFlowerBouquets();
            if (flowerBouquets.Count() == 0)
            {
                return NotFound(new ResponseObject
                {
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "List null",
                    Data = null
                });
            }
            var res = mapper.Map<List<FlowerBouquetResponse>>(flowerBouquets);
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.OK.ToString(),
                Message = "Get list of customers successfully",
                Data = res
            });
        }
        [HttpPost("createFlowerBouquet")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateFlowerBouquet(CreateFlowerBouquetRequest request) {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseObject
                {
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = ModelState.ToString(),
                    Data = null
                });
            }
            var creatFlow = mapper.Map<FlowerBouquet>(request);
            await flowerBouquetRepo.CreateFlowerBouquet(creatFlow);
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.OK.ToString(),
                Message = "Create flower bouquet successfully",
                Data = null
            });
        }
        [HttpPut("updateFlowerBouquet")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateFlowerBouquet(UpdateFlowerBouquetRequest request) {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseObject
                {
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = ModelState.ToString(),
                    Data = null
                });
            }
            var flowerBouquet = await flowerBouquetRepo.GetFlowerBouquetById(request.FlowerBouquetId);

            if (flowerBouquet == null)
            {
                return NotFound(new ResponseObject
                {
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "List null",
                    Data = null
                });
            }
            var update = mapper.Map<FlowerBouquet>(request);
            var flow = await flowerBouquetRepo.UpdateFlowerBouquet(update);
            var res = mapper.Map<FlowerBouquetResponse>(flow);
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.OK.ToString(),
                Message = "Update flower bouquet successfully",
                Data = res
            });

        }
        [HttpDelete("deleteFlowerBouquet")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFlowerBouquet(int flowerId)
        {
            await flowerBouquetRepo.RemoveFlowerBouquet(flowerId);
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.OK.ToString(),
                Message = "Delete flower bouquet successfully",
                Data = null
            });
        }
        [HttpGet("getFlowerBouquet")]
        [Authorize]
        public async Task<IActionResult> SearchFlowerBouquet(string keyword)
        {
            if(keyword == null)
            {
                return BadRequest(new ResponseObject
                {
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Must enter keyword",
                    Data = null
                });
            }
            var list = await flowerBouquetRepo.SearchFlowerBouquetByName(keyword);
            if(list.Count() == 0)
            {
                return NotFound(new ResponseObject
                {
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "List Null",
                    Data = null
                });
            }
            var res = mapper.Map<IEnumerable<FlowerBouquetResponse>>(list);
            return Ok(new ResponseObject
            {
                Status = HttpStatusCode.OK.ToString(),
                Message = "Search flower bouquet by key successfully",
                Data = res
            });
        }

    }
}
