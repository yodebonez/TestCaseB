using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using TestCaseB.ApiRequestResponse;
using TestCaseB.Services.Interfaces;

namespace TestCaseB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {


        private readonly IShipping _service;
        public ShippingController(IShipping shipping)
        {
           _service = shipping;
        }




        [HttpPost]
        [Route("shipproducts")]
        public async Task<IActionResult> ShipNewProducts(ShipProductsRequest model)
        {
            Log.Information($"ship product request {JsonConvert.SerializeObject(model)}");
            var response = await _service.ShipGoods(model);
            Log.Information($"ship product response {JsonConvert.SerializeObject(response)}");
            return await Task.FromResult(new JsonResult(response));
        }


        [HttpPost]
        [Route("UpdateShippingStatus")]
        public async Task<IActionResult> UpdateShipingStatus(UpdateShipingRequest model)
        {
            Log.Information($"ConfirmEmailAsync request {JsonConvert.SerializeObject(model)}");
            var response = await _service.UpdateShiping(model);
            Log.Information($"ConfirmEmailAsync response {JsonConvert.SerializeObject(response)}");
            return await Task.FromResult(new JsonResult(response));
        }


        [HttpGet]
        [Route("getproductshipedbyUser")]
        public async Task<IActionResult> GetProductsShppedByUser( string email)
        {

            var response = await _service.GetAllProductShipedByCusomer(email);
            Log.Information($"Get products shipped {JsonConvert.SerializeObject(response)}");
            return await Task.FromResult(new JsonResult(response));
        }


        [HttpGet]
        [Route("getshiipingbyshippingId")]
        public async Task<IActionResult> GetShipingUpdates(string shippingId)
        {

            var response = await _service.TrackProductShipped(shippingId);
            Log.Information($"create post response {JsonConvert.SerializeObject(response)}");
            return await Task.FromResult(new JsonResult(response));
        }



        [HttpGet]
        [Route("getallproductshiped")]
        public async Task<IActionResult> GetListingbyCategory(int PageZise, int PageNumber)
        {

            var response = await _service.GetAllProductShipped( PageZise,  PageNumber);
            Log.Information($"create post response {JsonConvert.SerializeObject(response)}");
            return await Task.FromResult(new JsonResult(response));
        }








    }
}
