using AutoMapper;
using BC_ContosoRecordsModule.Application.LoadOptions;
using BC_ContosoRecordsModule.Application.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace ContosoRetail.WebAPI.Controllers.BC_ContosoRecordsModule_Controllers
{
    [Route("api/[controller]")]
    public class CustomerOrdersController : Controller
    {
        private IGetCustomerOrdersDTOsWithOptionsQuery _getCustomerOrdersDTOsWithOptionsQuery;
        private IMapper _mapper;
        private ILogger<CustomerOrdersController> _logger;

        public CustomerOrdersController(
                IGetCustomerOrdersDTOsWithOptionsQuery getCustomerOrdersDTOsWithOptionsQuery,
                IMapper mapper,
                ILogger<CustomerOrdersController> logger)
        {
            _getCustomerOrdersDTOsWithOptionsQuery = getCustomerOrdersDTOsWithOptionsQuery;
            _mapper = mapper;
            _logger = logger;

        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions options)
        {
            try
            {
                var responseData = _getCustomerOrdersDTOsWithOptionsQuery.Execute(options);
                if (responseData == null) return NotFound("No customer orders were found");
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something bad happened in the server, here is the exception message: {ex.Message}");
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
