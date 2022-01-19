using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WiiTrakApi.Controllers
{
    [Route("api/deliverytickets")]
    [ApiController]
    public class DeliveryTicketsController : ControllerBase
    {
        private readonly ILogger<DeliveryTicketsController> _logger;
        private readonly IMapper _mapper;

        public DeliveryTicketsController(ILogger<DeliveryTicketsController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
    }
}
