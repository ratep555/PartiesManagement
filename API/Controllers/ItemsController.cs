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

            // ovo si stavio zato što pagination vraća samo 12 podataka sa klijenta
            var listforreset = await _unitOfWork.ItemRepository.GetAllItemsForItemWarehouses();

            await _unitOfWork.ItemRepository.ResetItemDiscountedPriceDueToDiscountExpiry(listforreset);
            await _unitOfWork.ItemRepository.ResetCategoryDiscountedPriceDueToDiscountExpiry(listforreset);
            await _unitOfWork.ItemRepository.ResetManufacturerDiscountedPriceDueToDiscountExpiry(listforreset);
            await _unitOfWork.ItemRepository.UpdatingItemStockQuantityBasedOnWarehousesQuantity(listforreset);
            
            var data = _mapper.Map<IEnumerable<ItemDto>>(listforreset);

            foreach (var item in data)
            {
                item.LikesCount = await _unitOfWork.ItemRepository.GetCountForLikes(item.Id);
            }

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
            itemToReturn.DiscountSum = await _unitOfWork.ItemRepository.DiscountSum(item);

            return Ok(itemToReturn);
        }

       // [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<ActionResult> CreateItem([FromForm] ItemCreateEditDto itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);

            if (itemDto.Picture != null)
            {
                item.Picture = await _fileStorageService.SaveFile(containerName, itemDto.Picture);
            }

            await _unitOfWork.ItemRepository.AddItem7(item);
            await _unitOfWork.ItemRepository.UpdateItemWithDiscount7(item);

            return Ok();
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
            await _unitOfWork.ItemRepository.ResetItemDiscountedPrice7(item);

            await _unitOfWork.ItemRepository.UpdateItem7(item);
            await _unitOfWork.ItemRepository.UpdateItemWithDiscount7(item);

            return NoContent();
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

            await _unitOfWork.SaveAsync();

            await _unitOfWork.ItemRepository.RemovingReservedQuantityFromItemWarehouses(id, quantity);

            await _unitOfWork.ItemRepository.IncreasingItemWarehousesQuantity(id, quantity);
            return NoContent();
        }

        [HttpPut("decrease1/{id}/{quantity}")]
        public async Task<ActionResult> DecreaseStockQuantity1(int id, int quantity)
        {
            var item = await _unitOfWork.ItemRepository.GetItemById(id);

            if (quantity > item.StockQuantity)
            {
                return BadRequest("There are only " + (item.StockQuantity) + " items on stock right now.");
            }

            if (item.StockQuantity > 0)
            {
                item.StockQuantity = item.StockQuantity - quantity;

                if (item.StockQuantity == 0)
                {
                    item.StockQuantity = 0;
                }
            }
            await _unitOfWork.SaveAsync();

            if (quantity <= 1)
            {
                await _unitOfWork.ItemRepository.DecreasingItemWarehousesQuantity(id, quantity);
            }

            if (quantity > 1)
            {
                await _unitOfWork.ItemRepository.DecreasingItemWarehousesQuantity1(id, quantity);
            } 


            return NoContent();               
        }


        // discounts
        [HttpGet("discount/{id}")]
        public async Task<ActionResult<DiscountDto>> GetDiscountById(int id)
        {
            var discount = await _unitOfWork.ItemRepository.FindDiscountById(id);

            if (discount == null) return NotFound(new ServerResponse(404));

            var discy = _mapper.Map<DiscountDto>(discount);

            return Ok(discy);
        }
        

        [HttpGet("putget1discount/{id}")]
        public async Task<ActionResult<DiscountPutGetDto>> GetDiscountByIdForEditing1(int id)
        {
            var discount = await _unitOfWork.ItemRepository.FindDiscountById(id);

            if (discount == null) return NotFound(new ServerResponse(404));

            var discountToReturn = _mapper.Map<DiscountDto>(discount);

            var itemsSelectedIds = discountToReturn.Items.Select(x => x.Id).ToList();

            var nonSelectedItems = await _unitOfWork.ItemRepository
                .GetNonSelectedItems(itemsSelectedIds);

            var nonSelectedItemsDto = _mapper.Map<IEnumerable<ItemDto>>
                (nonSelectedItems).OrderBy(x => x.Name);

            var response = new DiscountPutGetDto();

            response.Discount = discountToReturn;
            response.SelectedItems = discountToReturn.Items.OrderBy(x => x.Name);
            response.NonSelectedItems = nonSelectedItemsDto;

            return response;
        }

        [HttpGet("putget1discount1/{id}")]
        public async Task<ActionResult<DiscountPutGetDto>> GetDiscountByIdForEditing2(int id)
        {
            var discount = await _unitOfWork.ItemRepository.FindDiscountById(id);

            if (discount == null) return NotFound(new ServerResponse(404));

            var discountToReturn = _mapper.Map<DiscountDto>(discount);

            var itemsSelectedIds = discountToReturn.Items.Select(x => x.Id).ToList();

            var nonSelectedItems = await _unitOfWork.ItemRepository
                .GetNonSelectedItems(itemsSelectedIds);

            var categoriesSelectedIds = discountToReturn.Categories.Select(x => x.Id).ToList();

            var nonSelectedCategories = await _unitOfWork.ItemRepository
                .GetNonSelectedCategories(categoriesSelectedIds);
            
            var manufacturersSelectedIds = discountToReturn.Manufacturers.Select(x => x.Id).ToList();

            var nonSelectedManufacturers = await _unitOfWork.ItemRepository
                .GetNonSelectedManufacturers1(manufacturersSelectedIds);

            var nonSelectedItemsDto = _mapper.Map<IEnumerable<ItemDto>>
                (nonSelectedItems).OrderBy(x => x.Name);

            var nonSelectedCategoriesDto = _mapper.Map<IEnumerable<CategoryDto>>
                (nonSelectedCategories).OrderBy(x => x.Name);

            var nonSelectedManufacturersDto = _mapper.Map<IEnumerable<Manufacturer1Dto>>
                (nonSelectedManufacturers).OrderBy(x => x.Name);

            var response = new DiscountPutGetDto();

            response.Discount = discountToReturn;
            response.SelectedItems = discountToReturn.Items.OrderBy(x => x.Name);
            response.NonSelectedItems = nonSelectedItemsDto;
            response.SelectedCategories = discountToReturn.Categories.OrderBy(x => x.Name);
            response.NonSelectedCategories = nonSelectedCategoriesDto;
            response.SelectedManufacturers = discountToReturn.Manufacturers.OrderBy(x => x.Name);
            response.NonSelectedManufacturers = nonSelectedManufacturersDto;

            return response;
        }

        [HttpPost("discountpost")]
        public async Task<ActionResult> CreateDiscount([FromBody] DiscountCreateEditDto discountDto)
        {
            var discount = _mapper.Map<Discount>(discountDto);

            await _unitOfWork.ItemRepository.AddDiscount(discount);

            await _unitOfWork.ItemRepository.UpdateItemWithDiscount(discount);
            await _unitOfWork.ItemRepository.UpdateItemWithCategoryDiscount(discount);
            await _unitOfWork.ItemRepository.UpdateItemWithManufacturerDiscount(discount);

            return Ok();
        }

        [HttpPut("discountput/{id}")]
        public async Task<ActionResult> UpdateDiscount(int id, [FromBody] DiscountCreateEditDto discountDto)
        {
            var discount = await _unitOfWork.ItemRepository.FindDiscountById(id);

            if (discount == null) return NotFound(new ServerResponse(404));

            await _unitOfWork.ItemRepository.ResetItemDiscountedPrice(discount);

            discount = _mapper.Map(discountDto, discount);

            await _unitOfWork.ItemRepository.UpdateDiscount(discount);

            await _unitOfWork.ItemRepository.UpdateItemWithDiscount(discount);

            return NoContent();
        }
        // ovo koristiš!
        [HttpPut("discountput1/{id}")]
        public async Task<ActionResult> UpdateDiscount1(int id, [FromBody] DiscountCreateEditDto discountDto)
        {
            var discount = await _unitOfWork.ItemRepository.FindDiscountById(id);

            if (discount == null) return NotFound(new ServerResponse(404));

            await _unitOfWork.ItemRepository.ResetItemDiscountedPrice(discount);
            await _unitOfWork.ItemRepository.ResetCategoryDiscountedPrice(discount);
            await _unitOfWork.ItemRepository.ResetManufacturerDiscountedPrice(discount);

            discount = _mapper.Map(discountDto, discount);

            await _unitOfWork.ItemRepository.UpdateDiscount(discount);

            await _unitOfWork.ItemRepository.UpdateItemWithDiscount(discount);
            await _unitOfWork.ItemRepository.UpdateItemWithCategoryDiscount(discount);
            await _unitOfWork.ItemRepository.UpdateItemWithManufacturerDiscount(discount);

            return NoContent();
        }

        [HttpDelete("discountdelete/{id}")]
        public async Task<ActionResult> DeleteDiscount(int id)
        {
            var discount = await _unitOfWork.ItemRepository.FindDiscountById(id);

            if (discount == null) return NotFound(new ServerResponse(404));

            await _unitOfWork.ItemRepository.DeleteDiscount(discount);

            return NoContent();
        }

        [HttpGet("discountlist")]
        public async Task<ActionResult<Pagination<DiscountDto>>> GetAllDiscounts(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.ItemRepository.GetCountForDiscounts();
            
            var list = await _unitOfWork.ItemRepository.GetAllDiscounts(queryParameters);

            var data = _mapper.Map<IEnumerable<DiscountDto>>(list);

            return Ok(new Pagination<DiscountDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("discounts/items")]  // možeš u originalu staviti neki dto da ti ne vraća koješta itemdto
        public async Task<ActionResult<List<ItemDto>>> GetAllItemsForDiscounts()
        {
            var list = await _unitOfWork.ItemRepository.GetAllItemsForDiscounts();

            return _mapper.Map<List<ItemDto>>(list);
        }

        [HttpGet("discounts/categories")]  
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategoriesForDiscounts()
        {
            var list = await _unitOfWork.ItemRepository.GetAllCategoriesForDiscounts();

            return _mapper.Map<List<CategoryDto>>(list);
        }

        [HttpGet("discounts/manufacturers")]
        public async Task<ActionResult<IEnumerable<Manufacturer1Dto>>> GetManufacturers()
        {
            var list = await _unitOfWork.ItemRepository.GetManufacturers();

            var manufacturers = _mapper.Map<IEnumerable<Manufacturer1Dto>>(list);

            return Ok(manufacturers);        
        }

        [HttpGet("discounts/attributedtoitems")]
        public async Task<ActionResult<List<Manufacturer1Dto>>> GetManufacturersAttributetToItems()
        {
            var list = await _unitOfWork.ItemRepository.GetManufacturersAttributedToItems();

            return _mapper.Map<List<Manufacturer1Dto>>(list);
        }

        [HttpGet("discounts/tagsattributedtoitems")]
        public async Task<ActionResult<List<TagDto>>> GetTagsAttributetToItems()
        {
            var list = await _unitOfWork.ItemRepository.GetTagsAttributedToItems();

            return _mapper.Map<List<TagDto>>(list);
        }

        [HttpGet("discounts/categoriesattributedtoitems")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategoriesAttributetToItems()
        {
            var list = await _unitOfWork.ItemRepository.GetCategoriesAttributedToItems();

            return _mapper.Map<List<CategoryDto>>(list);
        }

        // likes
        [Authorize]
        [HttpPost("addlike/{id}")]
        public async Task<ActionResult> AddLike (int id)
        {
            var userId = User.GetUserId();

            var item = await _unitOfWork.ItemRepository.GetItemById(id);

            if (item == null) return NotFound(new ServerResponse(404));

            if (await _unitOfWork.ItemRepository.CheckIfUserHasAlreadyLikedThisProduct(userId, id))
            return BadRequest("You have already liked this product!");

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _unitOfWork.ItemRepository.AddLike(userId, id);

                return Ok();
            }

            return BadRequest("You must be registered/logged in order to like this product!");
        }


        // itemwarehouses

        [HttpGet("itemwarehouse")]
        public async Task<ActionResult<Pagination<ItemWarehouseDto>>> GetAllItemWarehouses(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.ItemRepository.GetCountForItemWarehouses();
            
            var list = await _unitOfWork.ItemRepository.GetAllItemWarehouses(queryParameters);

            var data = _mapper.Map<IEnumerable<ItemWarehouseDto>>(list);

            return Ok(new Pagination<ItemWarehouseDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        // ovdje možeš kasnije createdataction pa ćeš i vraćati nešto
        [HttpPost("itemwarehousecreate")]
        public async Task<ActionResult> CreateItemWarehouse([FromBody] ItemWarehouseDto itemWarehouseDto)
        {
            var itemWarehouse = _mapper.Map<ItemWarehouse>(itemWarehouseDto);

            await _unitOfWork.ItemRepository.AddItemWarehouse(itemWarehouse);

           /*  var item = await _unitOfWork.ItemRepository.GetItemById(itemWarehouseDto.ItemId);

            if (item.StockQuantity.HasValue)
            {
                item.StockQuantity = item.StockQuantity += itemWarehouseDto.StockQuantity;
            }
            else
            {
                item.StockQuantity = itemWarehouseDto.StockQuantity;
            }

            await _unitOfWork.SaveAsync(); */

            return Ok();
        }

        [HttpPut("itemwarehouseedit/{id}/{warehouseid}")]
        public async Task<ActionResult> EditItemWarehouse(
            int id, int warehouseid, [FromBody] ItemWarehouseCreateEditDto itemWarehouseDto)
        {
            var itemWarehouse = _mapper.Map<ItemWarehouse>(itemWarehouseDto);

            if (id != itemWarehouse.ItemId && warehouseid != itemWarehouse.WarehouseId) 
            return BadRequest("Bad request!");        

            await _unitOfWork.ItemRepository.UpdateItemWarehouse(itemWarehouse);

            var item = await _unitOfWork.ItemRepository.GetItemById(itemWarehouseDto.ItemId);

            await _unitOfWork.ItemRepository.AddingNewStockQuantityToItemAndRemovingOldOne(item);

            return Ok();
        }

        [HttpGet("itemwarehouse/{id}/{warehouseid}")]
        public async Task<ActionResult<ItemWarehouseDto>> GetItemWarehouseByItemIdAndItemWarehouseId(
            int id, int warehouseId)
        {
            var itemwarehouse = await _unitOfWork.ItemRepository
                .FindItemWarehouseByItemIdAndWarehouseId(id, warehouseId);

            if (itemwarehouse == null) return NotFound(new ServerResponse(404));

            return _mapper.Map<ItemWarehouseDto>(itemwarehouse);
        }
        
        [HttpGet("itemwarehouses/items")]
        public async Task<ActionResult<List<ItemDto>>> GetAllItemsForItemWarehouses()
        {
            var list = await _unitOfWork.ItemRepository.GetAllItemsForItemWarehouses();

            return _mapper.Map<List<ItemDto>>(list);
        }

        [HttpGet("itemwarehouses/warehouses")]
        public async Task<ActionResult<List<WarehouseDto>>> GetAllWarehousesForItemWarehouses()
        {
            var list = await _unitOfWork.ItemRepository.GetAllWarehousesForItemWarehouse();

            return _mapper.Map<List<WarehouseDto>>(list);
        }

    }
}













