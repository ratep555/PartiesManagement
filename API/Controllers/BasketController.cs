using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ClientBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasket(id);

            return Ok(basket ?? new ClientBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<ClientBasket>> EditBasket(ClientBasketDto basketDto)
        {
            var basket = _mapper.Map<ClientBasket>(basketDto);

            var editedBasket = await _basketRepository.EditBasket(basket);

            return Ok(editedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasket(id);
        }


    }
}













