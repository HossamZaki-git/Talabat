using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Talabat.Core.Domain_Models;
using Talabat.Core.GenericRepository;
using Talabat.Repository.Repositories;
using Talabat.WebAPI.DTOs;
using Talabat.WebAPI.Errors;

namespace Talabat.WebAPI.Controllers
{
    public class BasketController : BaseAPIController
    {
        private readonly IBasketsRepository basketRepository;
        private readonly IMapper mapper;

        public BasketController(IBasketsRepository basketRepository, IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }

        [ProducesResponseType(typeof(Basket), 200)]
        [ProducesResponseType(typeof(ErrorReturn), 404)]
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasketAsync([FromQuery]string ID)
        {
            var basket = await basketRepository.GetBasketAsync(ID);
            return basket is not null ? Ok(mapper.Map<Basket, BasketDTO>(basket)) : NotFound(new ErrorReturn(404));
        }

        [ProducesResponseType(typeof(Basket), 200)]
        [ProducesResponseType(typeof(ErrorReturn), 400)]
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateBasket_UpdateBasket(BasketDTO basketDTO)
        {
            var basket = mapper.Map<BasketDTO, Basket>(basketDTO);
            var resultBasket = await basketRepository.Create_UpdateAsync(basket);
            return resultBasket is not null ? Ok(mapper.Map<Basket, BasketDTO>(resultBasket)) : BadRequest(new ErrorReturn(400));
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket([FromQuery]string ID)
            => await basketRepository.DeleteBasketAsync(ID);
    }
}
