using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos.Birthday;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Infrastructure.Services;
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
        private string containerName = "birthdays";

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
        [HttpPost]
        public async Task<ActionResult> CreateBirthday([FromBody] BirthdayCreateDto birthdayDto)
        {
            var birthday = _mapper.Map<Birthday>(birthdayDto);
           
            await _unitOfWork.BirthdayRepository.AddBirthday(birthday);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBirthday(int id, [FromBody] BirthdayEditDto birthdayDto)
        {
            var birthday = _mapper.Map<Birthday>(birthdayDto);

            if (id != birthday.Id) return BadRequest("Bad request!");

            await _unitOfWork.BirthdayRepository.UpdateBirthday(birthday);

            if (birthdayDto.OrderStatus1Id == 7)
            {
                // order.PaymentIntentId = null;

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
            
            var data = _mapper.Map<IEnumerable<BirthdayPackageDto>>(list);

            return Ok(new Pagination<BirthdayPackageDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

    
    
    }
}















