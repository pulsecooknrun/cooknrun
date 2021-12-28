using AmoClient;
using CookRun.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RoistatApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<AmoController> _logger;
        private readonly ApplicationContext _applicationContext;
        private readonly AmoProxyService _amoProxyService;

        public TestController(ILogger<AmoController> logger, ApplicationContext applicationContext, AmoProxyService amoProxyService)
        {
            _logger = logger;
            _applicationContext = applicationContext;
            _amoProxyService = amoProxyService;
        }

        [AllowAnonymous]
        [HttpGet("GetUsers")]
        public string GetUsers()
        {
            _amoProxyService.GetUsers();
            return "";
        }
    }
}