using TestCaseB.ApiRequestResponse;
using TestCaseB.Models;
using TestCaseB.Utility;

namespace TestCaseB.Services.Interfaces
{
    public interface IShipping
    {
        Task<Response<ShipProductResponse>> ShipGoods(ShipProductsRequest model);

        Task<Response<UpdateShipingResponse>> UpdateShiping(UpdateShipingRequest model);

        Task<Response<List<Shipping>>> GetAllProductShipedByCusomer(string CustomerEmail);

        Task<Response<List<ShippingUpdate>>> TrackProductShipped(string ShippingId);


        Task<Response<PagedResponse<List<Shipping>>>> GetAllProductShipped(int PageZise, int PageNumber);


    }
}
