using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ErrorHandling;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
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

            var averageVote = 0.0;
            var userVote = 0;

            if (await _unitOfWork.ItemRepository.ChechIfAny(id))
            {
                averageVote = await _unitOfWork.ItemRepository.AverageVote(id);
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();

                var ratingDb = await _unitOfWork.ItemRepository.FindCurrentRate(id, userId);

                if (ratingDb != null)
                {
                    userVote = ratingDb.Rate;
                }
            }

            var itemToReturn = _mapper.Map<ItemDto>(item);

            itemToReturn.AverageVote = averageVote;
            itemToReturn.UserVote = userVote;

            return Ok(itemToReturn);
        }

       // [Authorize(Policy = "RequireAdminRole")]
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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, [FromForm] ItemEditDto itemDto)
        {
            var item = await _unitOfWork.ItemRepository.GetItemById(id);

            if (item == null) return NotFound(new ServerResponse(404));

            item = _mapper.Map(itemDto, item);
            
            if (itemDto.Picture != null)
            {
                item.Picture = await _fileStorageService.EditFile(containerName, itemDto.Picture, item.Picture);
            }

          //  await _unitOfWork.ItemRepository.UpdateItemWithDiscount(item);

            await _unitOfWork.SaveAsync(); return NoContent();

        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            var list = await _unitOfWork.ItemRepository.GetAllCategories();

            return _mapper.Map<List<CategoryDto>>(list);
        }

        [HttpGet("discounts")]
        public async Task<ActionResult<List<DiscountDto>>> GetAllDiscounts()
        {
            var list = await _unitOfWork.ItemRepository.GetAllDiscounts();

            return _mapper.Map<List<DiscountDto>>(list);
        }

        [HttpGet("manufacturers")]
        public async Task<ActionResult<List<ManufacturerDto>>> GetAllManufacturers()
        {
            var list = await _unitOfWork.ItemRepository.GetAllManufacturers();

            return _mapper.Map<List<ManufacturerDto>>(list);
        }
        [HttpGet("tags")]
        public async Task<ActionResult<List<TagDto>>> GetAllTags()
        {
            var list = await _unitOfWork.ItemRepository.GetAllTags();

            return _mapper.Map<List<TagDto>>(list);
        }

        [HttpGet("putget/{id}")]
        public async Task<ActionResult<ItemPutGetDto>> GetItemByIdForEditing(int id)
        {
            var item = await _unitOfWork.ItemRepository.GetItemById(id);

            if (item == null) return NotFound(new ServerResponse(404));

            var itemToReturn = _mapper.Map<ItemDto>(item);

            var categoriesSelectedIds = _unitOfWork.ItemRepository.CategoryIds(id);

            var selectedCategories = await _unitOfWork.ItemRepository
                .GetSelectedCategories(categoriesSelectedIds);

            var nonSelectedCategories = await _unitOfWork.ItemRepository
                .GetNonSelectedCategories(categoriesSelectedIds);

            var manufacturersSelectedIds = _unitOfWork.ItemRepository.ManufacturerIds(id);

            var selectedManufacturers = await _unitOfWork.ItemRepository
                .GetSelectedManufacturers(manufacturersSelectedIds);

            var nonSelectedManufacturers = await _unitOfWork.ItemRepository
                .GetNonSelectedManufacturers(manufacturersSelectedIds);

            var tagsSelectedIds = _unitOfWork.ItemRepository.TagIds(id);

            var selectedTags = await _unitOfWork.ItemRepository
                .GetSelectedTags(tagsSelectedIds);

            var nonSelectedTags = await _unitOfWork.ItemRepository
                .GetNonSelectedTags(tagsSelectedIds);

            var selectedCategoriesDto = _mapper.Map<IEnumerable<CategoryDto>>
                (selectedCategories).OrderBy(x => x.Name);

            var nonSelectedCategoriesDto = _mapper.Map<IEnumerable<CategoryDto>>
                (nonSelectedCategories).OrderBy(x => x.Name);

            var selectedManufacturersDto = _mapper.Map<IEnumerable<ManufacturerDto>>
                (selectedManufacturers).OrderBy(x => x.Name);

            var nonSelectedManufacturersDto = _mapper.Map<IEnumerable<ManufacturerDto>>
                (nonSelectedManufacturers).OrderBy(x => x.Name);

            var selectedTagsDto = _mapper.Map<IEnumerable<TagDto>>
                (selectedTags).OrderBy(x => x.Name);

            var nonSelectedTagsDto = _mapper.Map<IEnumerable<TagDto>>
                (nonSelectedTags).OrderBy(x => x.Name);

            var response = new ItemPutGetDto();

            response.Item = itemToReturn;
            response.SelectedCategories = selectedCategoriesDto.OrderBy(x => x.Name);
            response.NonSelectedCategories = nonSelectedCategoriesDto;
            response.SelectedManufacturers = selectedManufacturersDto.OrderBy(x => x.Name);
            response.NonSelectedManufacturers = nonSelectedManufacturersDto;
            response.SelectedTags = selectedTagsDto.OrderBy(x => x.Name);
            response.NonSelectedTags = nonSelectedTagsDto;

            return response;
        }

        // ovo koristiš!
        [HttpGet("putget1/{id}")]
        public async Task<ActionResult<ItemPutGetDto>> GetItemByIdForEditing1(int id)
        {
            var item = await _unitOfWork.ItemRepository.GetItemById(id);

            if (item == null) return NotFound(new ServerResponse(404));

            var itemToReturn = _mapper.Map<ItemDto>(item);

            var categoriesSelectedIds = itemToReturn.Categories.Select(x => x.Id).ToList();

            var nonSelectedCategories = await _unitOfWork.ItemRepository
                .GetNonSelectedCategories(categoriesSelectedIds);

            var discountSelectedIds = itemToReturn.Discounts.Select(x => x.Id).ToList();

            var nonSelectedDiscounts = await _unitOfWork.ItemRepository
                .GetNonSelectedDiscounts(discountSelectedIds);

            var manufacturersSelectedIds = itemToReturn.Manufacturers.Select(x => x.Id).ToList();

            var nonSelectedManufacturers = await _unitOfWork.ItemRepository
                .GetNonSelectedManufacturers(manufacturersSelectedIds);

            var tagsSelectedIds = itemToReturn.Tags.Select(x => x.Id).ToList();

            var nonSelectedTags = await _unitOfWork.ItemRepository
                .GetNonSelectedTags(tagsSelectedIds);

            var nonSelectedCategoriesDto = _mapper.Map<IEnumerable<CategoryDto>>
                (nonSelectedCategories).OrderBy(x => x.Name);

            var nonSelectedDiscountsDto = _mapper.Map<IEnumerable<DiscountDto>>
                (nonSelectedDiscounts).OrderBy(x => x.Name);

            var nonSelectedManufacturersDto = _mapper.Map<IEnumerable<ManufacturerDto>>
                (nonSelectedManufacturers).OrderBy(x => x.Name);

            var nonSelectedTagsDto = _mapper.Map<IEnumerable<TagDto>>
                (nonSelectedTags).OrderBy(x => x.Name);

            var response = new ItemPutGetDto();

            response.Item = itemToReturn;
            response.SelectedCategories = itemToReturn.Categories.OrderBy(x => x.Name);
            response.NonSelectedCategories = nonSelectedCategoriesDto;
            response.SelectedDiscounts = itemToReturn.Discounts.OrderBy(x => x.Name);
            response.NonSelectedDiscounts = nonSelectedDiscountsDto;
            response.SelectedManufacturers = itemToReturn.Manufacturers.OrderBy(x => x.Name);
            response.NonSelectedManufacturers = nonSelectedManufacturersDto;
            response.SelectedTags = itemToReturn.Tags.OrderBy(x => x.Name);
            response.NonSelectedTags = nonSelectedTagsDto;

            return response;
        }

        [Authorize]
        // rating
        [HttpPost("ratings")]
        public async Task<ActionResult> CreateRate([FromBody] RatingDto ratingDto)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var userId = User.GetUserId();

            if (await _unitOfWork.ItemRepository.CheckIfCustomerHasOrderedThisItem(ratingDto.ItemId, email))
            {
                return BadRequest("You have not ordered this item yet!");
            } 

            var currentRate = await _unitOfWork.ItemRepository
                .FindCurrentRate(ratingDto.ItemId, userId);

            if (currentRate == null)
            {
                await _unitOfWork.ItemRepository.AddRating(ratingDto, userId);
            }
            else
            {
                currentRate.Rate = ratingDto.Rating;
            }

            if (await _unitOfWork.SaveAsync()) return Ok();

            return BadRequest("Failed to add rate");            
        }

        // attempt to decrease stock quantity
        [HttpPut("decrease/{id}")]
        public async Task<ActionResult> DecreaseStockQuantity(int id)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var item = await _unitOfWork.ItemRepository.GetItemById(id);

            if (item.StockQuantity > 0)
            {
                item.StockQuantity--;

                if (item.StockQuantity == 0)
                {
                    item.StockQuantity = 0;
                }
            }

            await _unitOfWork.SaveAsync();

            return NoContent();               
        }

        [HttpPut("increase/{id}")]
        public async Task<ActionResult> IncreaseStockQuantity(int id)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var item = await _unitOfWork.ItemRepository.GetItemById(id);
            item.StockQuantity++;

            if (await _unitOfWork.SaveAsync()) return NoContent();

            return BadRequest("Failed to decrease quantity");               
        }

        [HttpPut("increase1/{id}/{quantity}")]
        public async Task<ActionResult> IncreaseStockQuantity1(int id, int quantity)
        {
            var item = await _unitOfWork.ItemRepository.GetItemById(id);
            item.StockQuantity = item.StockQuantity + quantity;

            if (await _unitOfWork.SaveAsync()) return NoContent();

            return BadRequest("Failed to decrease quantity");               
        }

        [HttpPut("decrease1/{id}/{quantity}")]
        public async Task<ActionResult> DecreaseStockQuantity1(int id, int quantity)
        {
            var item = await _unitOfWork.ItemRepository.GetItemById(id);

            if (item.StockQuantity > 0)
            {
                item.StockQuantity = item.StockQuantity - quantity;

                if (item.StockQuantity == 0)
                {
                    item.StockQuantity = 0;
                }
            }

            await _unitOfWork.SaveAsync();

            return NoContent();               
        }

     
    }
}













