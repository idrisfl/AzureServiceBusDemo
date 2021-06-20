using AzureServiceBusDemo.API.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureServiceBusDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
                
        [HttpPost("publish/order")]
        public void PublishOrder([FromBody] CreateOrderRequest request)
        {

        }


        [HttpPost("publish/product")]
        public void PublishProduct([FromBody] CreateProductRequest request)
        {

        }

    }
}
