using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ErrorHandling;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.Birthday;
using Core.Entities;
using Core.Entities.Blogs;
using Core.Interfaces;
using Core.Utilities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class BirthdaysController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly IPdfService _pdfService;
        private readonly IEmailService _emailService;
        private string containerName = "birthdaypackages";
        private string containerName1 = "activities";
        private string containerName2 = "blogs";

        public BirthdaysController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config,
            IFileStorageService fileStorageService, IPdfService pdfService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _pdfService = pdfService;
            _emailService = emailService;
        }

        // birthdays
        [HttpGet]
        public async Task<ActionResult<Pagination<BirthdayDto>>> GetAllBirtdays(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.BirthdayRepository.GetCountForBirthdays();
            
            var list = await _unitOfWork.BirthdayRepository.GetAllBirthdays(queryParameters);
            
            var data = _mapper.Map<IEnumerable<BirthdayDto>>(list);

            return Ok(new Pagination<BirthdayDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BirthdayDto>> GetBirthdayById(int id)
        {
            var birthday = await _unitOfWork.BirthdayRepository.FindBirthdayById(id);

            if (birthday == null) return NotFound(new ServerResponse(404));

            return _mapper.Map<BirthdayDto>(birthday);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBirthday([FromBody] BirthdayCreateDto birthdayDto)
        {
            var birthday = _mapper.Map<Birthday1>(birthdayDto);
           
            await _unitOfWork.BirthdayRepository.AddBirthday(birthday);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBirthday(int id, [FromBody] BirthdayEditDto birthdayDto)
        {
            var birthday = _mapper.Map<Birthday1>(birthdayDto);

            if (id != birthday.Id) return BadRequest("Bad request!");

            await _unitOfWork.BirthdayRepository.UpdateBirthday(birthday);

            if (birthdayDto.OrderStatus1Id == 7)
            {
                _emailService.GeneratePdf2(birthdayDto.Id, birthday.Price, birthday.ClientName);

                string url = $"{_config["AngularAppUrl"]}/orders/{birthday.Id}";

                await _emailService.SendEmailAsync2(birthday.ContactEmail, 
                "Reservation confirmation", $"<h1>Thank you for your reservation</h1>" +
                $"<p>We are glad to inform you that you reservation has been accepted.</p>" +
                $"<p>You will find attached email with payment details. You can view details of your reservation by <a href='{url}'>Clicking here</a></p>", birthday.Id);
            }
                        
            return NoContent();
        }

        // birthdaypackages
        [HttpGet("birthdaypackages")]
        public async Task<ActionResult<Pagination<BirthdayPackageDto>>> GetAllBirtdayPackages(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.BirthdayRepository.GetCountForBirthdayPackages();
            
            var list = await _unitOfWork.BirthdayRepository.GetAllBirthdayPackages(queryParameters);

            var listforreset = await _unitOfWork.BirthdayRepository.GetBirthdayPackages();

            await _unitOfWork.BirthdayRepository.ResetBirthdayPackageDiscountedPriceDueToDiscountExpiry(listforreset);
            
            var data = _mapper.Map<IEnumerable<BirthdayPackageDto>>(list);

            await _unitOfWork.BirthdayRepository.DiscountSum1(listforreset, data);

            return Ok(new Pagination<BirthdayPackageDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("birthdaypackages/{id}")]
        public async Task<ActionResult<BirthdayPackageDto>> GetBirthdayPackageById(int id)
        {
            var birthdayPackage = await _unitOfWork.BirthdayRepository.GetBirthdayPackageById(id);

            if (birthdayPackage == null) return NotFound(new ServerResponse(404));

            var birthdayPackageDto = _mapper.Map<BirthdayPackageDto>(birthdayPackage);

            birthdayPackageDto.DiscountSum = await _unitOfWork.BirthdayRepository.DiscountSum(birthdayPackage);

            return birthdayPackageDto;
        }

        [HttpPost("birthdaypackages")]
        public async Task<ActionResult> CreateBirthdayPackage(
                [FromForm] BirthdayPackageCreateEditDto birthdayDto)
        {
            var birthdayPackage = _mapper.Map<BirthdayPackage>(birthdayDto);

            if (birthdayDto.Picture != null)
            {
                birthdayPackage.Picture = await _fileStorageService.SaveFile(containerName, birthdayDto.Picture);
            }

            await _unitOfWork.BirthdayRepository.AddBirthdayPackage(birthdayPackage);
            await _unitOfWork.BirthdayRepository.UpdateBirthdayPackageWithDiscount1(birthdayPackage);

            return Ok();
        }

        [HttpGet("birthdaypackages/locations")]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
        {
            var list = await _unitOfWork.BirthdayRepository.GetLocations();

            var locations = _mapper.Map<IEnumerable<LocationDto>>(list);

            return Ok(locations);        
        }

        [HttpGet("birthdaypackages/list")]
        public async Task<ActionResult<IEnumerable<BirthdayPackageDto>>> GetBPackages()
        {
            var list = await _unitOfWork.BirthdayRepository.GetBirthdayPackages();

            var bpackage = _mapper.Map<IEnumerable<BirthdayPackageDto>>(list);

            return Ok(bpackage);        
        }

        [HttpGet("birthdaypackages/services")]
        public async Task<ActionResult<IEnumerable<ServiceIncludedDto>>> GetServices()
        {
            var list = await _unitOfWork.BirthdayRepository.GetServices();

            var service = _mapper.Map<IEnumerable<ServiceIncludedDto>>(list);

            return Ok(service);        
        }

        [HttpGet("birthdaypackages/putget/{id}")]
        public async Task<ActionResult<BirthdayPackagePutGetDto>> GetBirthdayPackageByIdForEditing1(int id)
        {
            var birthdayPackage = await _unitOfWork.BirthdayRepository.GetBirthdayPackageById(id);

            if (birthdayPackage == null) return NotFound(new ServerResponse(404));

            var birthdayPackageToReturn = _mapper.Map<BirthdayPackageDto>(birthdayPackage);

            var discountSelectedIds = birthdayPackageToReturn.Discounts.Select(x => x.Id).ToList();

            var nonSelectedDiscounts = await _unitOfWork.ItemRepository
                .GetNonSelectedDiscounts(discountSelectedIds);

            var servicesSelectedIds = birthdayPackageToReturn.ServicesIncluded.Select(x => x.Id).ToList();

            var nonSelectedServices = await _unitOfWork.BirthdayRepository
                .GetNonSelectedServices(servicesSelectedIds);

            var nonSelectedDiscountsDto = _mapper.Map<IEnumerable<DiscountDto>>
                (nonSelectedDiscounts).OrderBy(x => x.Name);

            var nonSelectedServicesDto = _mapper.Map<IEnumerable<ServiceIncludedDto>>
                (nonSelectedServices).OrderBy(x => x.Name);

            var response = new BirthdayPackagePutGetDto();

            response.BirthdayPackage = birthdayPackageToReturn;
            response.SelectedDiscounts = birthdayPackageToReturn.Discounts.OrderBy(x => x.Name);
            response.NonSelectedDiscounts = nonSelectedDiscountsDto;
            response.SelectedServices = birthdayPackageToReturn.ServicesIncluded.OrderBy(x => x.Name);
            response.NonSelectedServices = nonSelectedServicesDto;

            return response;
        }

        [HttpPut("birthdaypackages/{id}")]
        public async Task<ActionResult> UpdateBirthdayPackage(
                int id, [FromForm] BirthdayPackageCreateEditDto birthdayPackageDto)
        {
            var birthdayPackage = await _unitOfWork.BirthdayRepository.GetBirthdayPackageById(id);

            if (birthdayPackage == null) return NotFound(new ServerResponse(404));

            birthdayPackage = _mapper.Map(birthdayPackageDto, birthdayPackage);
            
            if (birthdayPackageDto.Picture != null)
            {
                birthdayPackage.Picture = await _fileStorageService
                    .EditFile(containerName, birthdayPackageDto.Picture, birthdayPackage.Picture);
            }
            await _unitOfWork.BirthdayRepository.ResetBirthdayPackageDiscountedPrice(birthdayPackage);

            await _unitOfWork.BirthdayRepository.UpdateBirthdayPackage(birthdayPackage);
            await _unitOfWork.BirthdayRepository.UpdateBirthdayPackageWithDiscount1(birthdayPackage);

            return NoContent();
        }

        // locations
        [HttpGet("locations/{id}")]
        public async Task<ActionResult<LocationDto>> GetLocationById(int id)
        {
            var location = await _unitOfWork.BirthdayRepository.FindLocationById(id);

            if (location == null) return NotFound(new ServerResponse(404));

            return _mapper.Map<LocationDto>(location);
        }

        [HttpGet("locations")]
        public async Task<ActionResult<Pagination<LocationDto>>> GetAllLocations(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.BirthdayRepository.GetCountForLocations();
            
            var list = await _unitOfWork.BirthdayRepository.GetAllLocations(queryParameters);
            
            var data = _mapper.Map<IEnumerable<LocationDto>>(list);

            return Ok(new Pagination<LocationDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpPost("locations")]
        public async Task<ActionResult> CreateLocation([FromBody] LocationCreateEditDto locationDto)
        {
            var location = _mapper.Map<Location1>(locationDto);
           
            await _unitOfWork.BirthdayRepository.CreateLocation(location);

            return Ok();
        }

        [HttpPut("locations/{id}")]
        public async Task<ActionResult> UpdateLocation(int id, [FromBody] LocationCreateEditDto locationDto)
        {
            var location = await _unitOfWork.BirthdayRepository.FindLocationById(id);

            if (location == null) return NotFound(new ServerResponse(404));   

            location = _mapper.Map(locationDto, location);

            await _unitOfWork.BirthdayRepository.UpdateLocation(location);

            return NoContent();
        }

        // messages
        [HttpPost("messages")]
        public async Task<ActionResult> CreateMessage([FromBody] MessageCreateDto messageDto)
        {
            var message = _mapper.Map<Message>(messageDto);

            var adminuser = await _unitOfWork.BirthdayRepository.GetAdmin();
            message.ApplicationUserId = adminuser.Id;
            message.SendingDate = DateTime.Now.ToLocalTime();
           
            await _unitOfWork.BirthdayRepository.CreateMessage(message);

            return Ok();
        }

        // servicesincluded
        [HttpPost("servicesincluded")]
        public async Task<ActionResult> CreateServiceIncluded([FromForm] ServiceIncludedCreateEditDto serviceDto)
        {
            var serviceincluded = _mapper.Map<ServiceIncluded>(serviceDto);

            if (serviceDto.Picture != null)
            {
                serviceincluded.Picture = await _fileStorageService.SaveFile(containerName1, serviceDto.Picture);
            }

            await _unitOfWork.BirthdayRepository.AddServiceIncluded(serviceincluded);

            return Ok();
        }

        [HttpPut("servicesincluded/{id}")]
        public async Task<ActionResult> UpdateServiceIncluded(
                int id, [FromForm] ServiceIncludedCreateEditDto serviceDto)
        {
            var serviceIncluded = await _unitOfWork.BirthdayRepository.GetServiceIncludedById(id);

            if (serviceIncluded == null) return NotFound(new ServerResponse(404));

            serviceIncluded = _mapper.Map(serviceDto, serviceIncluded);
            
            if (serviceDto.Picture != null)
            {
                serviceIncluded.Picture = await _fileStorageService
                    .EditFile(containerName1, serviceDto.Picture, serviceIncluded.Picture);
            }

            await _unitOfWork.BirthdayRepository.UpdateServiceIncluded(serviceIncluded);

            return NoContent();
        }

        [HttpGet("servicesincluded")]
        public async Task<ActionResult<Pagination<ServiceIncludedDto>>> GetAllServicesIncluded(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.BirthdayRepository.GetCountForServicesIncluded();
            
            var list = await _unitOfWork.BirthdayRepository.GetAllServicesIncluded(queryParameters);
   
            var data = _mapper.Map<IEnumerable<ServiceIncludedDto>>(list);

            return Ok(new Pagination<ServiceIncludedDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("servicesincluded/{id}")]
        public async Task<ActionResult<ServiceIncludedDto>> GetServiceIncludedById(int id)
        {
            var serviceIncluded = await _unitOfWork.BirthdayRepository.GetServiceIncludedById(id);

            if (serviceIncluded == null) return NotFound(new ServerResponse(404));

            return _mapper.Map<ServiceIncludedDto>(serviceIncluded);
        }

        // blogs
        [Authorize]
        [HttpPost("blogs")]
        public async Task<ActionResult> CreateBlog([FromForm] BlogCreateEditDto blogDto)
        {
            var blog = _mapper.Map<Blog>(blogDto);

            var userId = User.GetUserId();

            if (blogDto.Picture != null)
            {
                blog.Picture = await _fileStorageService.SaveFile(containerName2, blogDto.Picture);
            }
            
            blog.ApplicationUserId = userId;
            blog.PublishedOn = DateTime.Now.ToLocalTime();

            await _unitOfWork.BirthdayRepository.AddBlog(blog);

            return Ok();
        }

        [Authorize]
        [HttpPut("blogs/{id}")]
        public async Task<ActionResult> UpdateBlog(
                int id, [FromForm] BlogCreateEditDto blogDto)
        {
            var blog = await _unitOfWork.BirthdayRepository.GetBlogById(id);

            if (blog == null) return NotFound(new ServerResponse(404));

            var userId = User.GetUserId();

            blog = _mapper.Map(blogDto, blog);
            
            if (blogDto.Picture != null)
            {
                blog.Picture = await _fileStorageService
                    .EditFile(containerName2, blogDto.Picture, blog.Picture);
            }

            blog.ApplicationUserId = userId;
            blog.UpdatedOn = DateTime.Now.ToLocalTime();

            await _unitOfWork.BirthdayRepository.UpdateBlog(blog);

            return NoContent();
        }

        [HttpGet("blogs/{id}")]
        public async Task<ActionResult<BlogDto>> GetBlogById(int id)
        {
            var blog = await _unitOfWork.BirthdayRepository.GetBlogById(id);

            if (blog == null) return NotFound(new ServerResponse(404));

            return _mapper.Map<BlogDto>(blog);
        }

        [Authorize]
        [HttpGet("blogs")]
        public async Task<ActionResult<Pagination<BlogDto>>> GetAllBlogsForUser(
            [FromQuery] QueryParameters queryParameters)
        {
            var userId = User.GetUserId();

            var count = await _unitOfWork.BirthdayRepository.GetCountForBlogsForUser(userId);
            
            var list = await _unitOfWork.BirthdayRepository.GetAllBlogsForUser(userId, queryParameters);
   
            var data = _mapper.Map<IEnumerable<BlogDto>>(list);

            return Ok(new Pagination<BlogDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("blogslist")]
        public async Task<ActionResult<Pagination<BlogDto>>> GetAllBlogs([FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.BirthdayRepository.GetCountForBlogs();
            
            var list = await _unitOfWork.BirthdayRepository.GetAllBlogs(queryParameters);
   
            var data = _mapper.Map<IEnumerable<BlogDto>>(list);

            return Ok(new Pagination<BlogDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        // blogcomments
        [Authorize]
        [HttpPost("blogcomments")]
        public async Task<ActionResult<BlogCommentDto>> UpsertBlogComment([FromBody] BlogCommentCreateEditDto blogCommentDto)
        {
            var blogComment = _mapper.Map<BlogComment>(blogCommentDto);

            var userId = User.GetUserId();

            if (blogCommentDto.Id == -1)
            {
                blogComment.ApplicationUserId = userId;
                blogComment.PublishedOn = DateTime.Now.ToLocalTime();
                blogComment.Id = 0;

                await _unitOfWork.BirthdayRepository.AddBlogComment(blogComment);
            }
            else
            {
                blogComment.ApplicationUserId = userId;
                blogComment.UpdatedOn = DateTime.Now.ToLocalTime();

                 await _unitOfWork.BirthdayRepository.UpdateBlogComment(blogComment);
            }
            var commentToReturn = _mapper.Map<BlogCommentDto>(blogComment);

            var comment1 = await _unitOfWork.BirthdayRepository.GetBlogCommentById(commentToReturn.Id);

            commentToReturn.Username = comment1.ApplicationUser.UserName;

            return Ok(commentToReturn);
        }

        [HttpGet("blogcomments/{blogId}")]
        public async Task<ActionResult<IEnumerable<BlogCommentDto>>> GetAllBlogComments(int blogId)
        {
            var blogComments = await _unitOfWork.BirthdayRepository.GetAllBlogComments(blogId);

            var data = _mapper.Map<IEnumerable<BlogCommentDto>>(blogComments);

            return Ok(data);
        }

        [HttpDelete("blogcomments/{id}")]
        public async Task<ActionResult<int>> DeleteBlogComment(int id)
        {
            var userId = User.GetUserId();

            var blogComment = await _unitOfWork.BirthdayRepository.GetBlogCommentById(id);

            if (blogComment == null) return NotFound(new ServerResponse(404));

            if (blogComment.ApplicationUserId == userId)
            {
                _unitOfWork.BirthdayRepository.DeleteBlogComment(blogComment);

                var affectedRows = await _unitOfWork.BirthdayRepository.Complete();

                return Ok(affectedRows);
            }
            else
            {
                return BadRequest("This comment was not created by the current user.");
            }
        }
    }
}















