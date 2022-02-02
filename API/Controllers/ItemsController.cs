using System.Collections.Generic;
using System.Threading.Tasks;
using API.ErrorHandling;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ItemsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private string containerName = "items";

        public ItemsController(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ItemDto>>> GetAllItems(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.ItemRepository.GetCountForItems();
            
            var list = await _unitOfWork.ItemRepository.GetAllItems(queryParameters);

            var data = _mapper.Map<IEnumerable<ItemDto>>(list);

            return Ok(new Pagination<ItemDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemById(int id)
        {
            var item = await _unitOfWork.ItemRepository.GetItemById(id);

            if (item == null) return NotFound(new ServerResponse(404));

            return _mapper.Map<ItemDto>(item);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem([FromForm] ItemCreateEditDto itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);

            if (itemDto.Picture != null)
            {
                item.Picture = await _fileStorageService.SaveFile(containerName, itemDto.Picture);
            }

            _unitOfWork.ItemRepository.AddItem(item);

            if (await _unitOfWork.SaveAsync()) return Ok();

            return BadRequest("Failed to save item");        
        }
    }
}













