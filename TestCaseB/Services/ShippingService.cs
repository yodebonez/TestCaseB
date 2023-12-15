using GoogleMaps.LocationServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using TestCaseB.ApiRequestResponse;
using TestCaseB.Models;
using TestCaseB.Services.Interfaces;
using TestCaseB.Utility;

namespace TestCaseB.Services
{
    public class ShippingService : IShipping
    {
       // Google Maps API key
        private readonly string _apiKey;
        private readonly DataContext _dataContext;

         public ShippingService(DataContext dataContext)
         {
            // Replace with your actual Google Maps API key
            _apiKey = "YOUR_GOOGLE_MAPS_API_KEY"; 
            _dataContext = dataContext;
         }




        public async Task<Response<ShipProductResponse>> ShipGoods(ShipProductsRequest model)
        {
            try
            {

                if (model.CustomerEmail == null)
                    return await Task.FromResult(new Response<ShipProductResponse>
                    {
                        Message = "User email must be provided"
                    });
                var user = await _dataContext.users.FirstOrDefaultAsync(x => x.Email == model.CustomerEmail);
                if (user == null)
                    return await Task.FromResult(new Response<ShipProductResponse>
                    {
                        Message = "User must be a registered user"
                    });
                // cost is calculated by multiple distance in km and size of goods in kg

                double Distance = await GetDistance(model.ToAddress);
                decimal DistanceInKm = Convert.ToDecimal(await GetDistance(model.ToAddress));
                decimal sizeinKg = Convert.ToDecimal(model.GoodSizeinKg);
                decimal price = DistanceInKm * sizeinKg * 100;
                Shipping shipping = new Shipping
                {
                    City = model.City,
                    Country = model.Country,
                    CustomerNumber = user.CustomerNumber,
                    CustomerEmail = user.Email,
                    DateShipped = DateTime.Now,
                    DeliveryAddress = model.ToAddress,
                    Distance = Distance,
                    GoodsZise = model.GoodSizeinKg,
                    ProductName = model.ProductName,
                    ShippingStatus = "IN_TRANSIT",
                    ShippingCost = price,
                    State = model.Status,
                    ShipingIdentity = CodeGenerator.GenerateNumber(7, Utility.Constants.PREFIX_CUSTOMER_CODE),

                };


                await _dataContext.shippings.AddAsync(shipping);
                await _dataContext.SaveChangesAsync();

                ShipProductResponse response = new ShipProductResponse();
                response.Address = model.ToAddress;
                response.Distance = Distance;
                response.SizeinKg = model.GoodSizeinKg;
                response.CustomerEmail = user.Email;
                response.ProductName = model.ProductName;

                return await Task.FromResult(new Response<ShipProductResponse>
                {
                    Success = true,
                    Message = "Product shipped sucesfully",
                    Data = response
                });
            }catch(Exception ex)
            {
                return await Task.FromResult(new Response<ShipProductResponse>
                {
                    Message = ex.Message
                });

            }

        }

        public async Task<Response<UpdateShipingResponse>> UpdateShiping(UpdateShipingRequest model)
        {

            try
            {
                if (model.CustomerEmail == null && model.ShipingId == null)
                    return await Task.FromResult(new Response<UpdateShipingResponse>
                    {
                        Message = "User email and shipping Id must be provide must be provided"
                    });

                var ProductShiped = await _dataContext.shippings.
                    FirstOrDefaultAsync(x => x.CustomerEmail == model.CustomerEmail && x.ShipingIdentity == model.ShipingId);
                if (ProductShiped == null)
                    return await Task.FromResult(new Response<UpdateShipingResponse>
                    {
                        Message = "There is no product shipped with this shiping identity"
                    });

                var CurrentStatus = await _dataContext.shipingUpdates.FirstOrDefaultAsync(x => x.ShipingIdentity.Equals(model.ShipingId));
                if (CurrentStatus != null)
                {
                    if (CurrentStatus.Status == "PICKED_UP" || CurrentStatus.Status == "DELIVERED")
                    {
                        return await Task.FromResult(new Response<UpdateShipingResponse>
                        {
                            Message = "The product has been sucessfully delivered to the customer"
                        });

                    }

                }

                if (model.ShippingStatus != "IN_TRANSIT" || model.ShippingStatus != "WAREHOUSE" || model.ShippingStatus != "PICKED_UP" || model.ShippingStatus != "DELIVERED")
                    return await Task.FromResult(new Response<UpdateShipingResponse>
                    {
                        Message = "Make can only indicate shiping status in IN_TRANSIT WAREHOUSE PICKED_UP DELIVERED"
                    });


                ShippingUpdate shippingUpdate = new ShippingUpdate
                {
                    CurrentLocation = model.CurrentLocation,
                    DateTime = DateTime.UtcNow,
                    Status = model.ShippingStatus.ToUpper(),
                    ShipingIdentity = model.ShipingId

                };

                await _dataContext.shipingUpdates.AddAsync(shippingUpdate);
                await _dataContext.SaveChangesAsync();

                UpdateShipingResponse response = new UpdateShipingResponse
                {
                    CurrentLocation = model.CurrentLocation,
                    ShipingId = model.ShipingId,
                    ShippingStatus = model.ShippingStatus,

                };

                return await Task.FromResult(new Response<UpdateShipingResponse>
                {
                    Success = true,
                    Message = "Shipping status Updated sucessfully",
                    Data = response
                });


            }
            catch (Exception ex)
            {


                return await Task.FromResult(new Response<UpdateShipingResponse>
                {
                    Message =  ex.Message
                });

            }
           
        }


        public async Task<Response<List<Shipping>>> GetAllProductShipedByCusomer(string CustomerEmail)
        {
            var cusomer = await _dataContext.users.FirstOrDefaultAsync(x => x.Email == CustomerEmail);
            if (cusomer == null)
                return await Task.FromResult(new Response<List<Shipping>>
                {
                    Message = "User is not available"
                });

            var Productshipped = await _dataContext.shippings.Where(x => x.CustomerEmail == CustomerEmail).ToListAsync();

            return await Task.FromResult(new Response<List<Shipping>>
            {
                Success = true,
                Message = "Product shipped",
                Data = Productshipped
            });
        }

        public async Task<Response<List<ShippingUpdate>>> TrackProductShipped(string ShippingId)
        {

            var ShipingUpdate = await _dataContext.shipingUpdates.Where(x => x.ShipingIdentity == ShippingId).ToListAsync();
            return await Task.FromResult(new Response<List<ShippingUpdate>>
            {
                Success = true,
                Message = "Product shipped",
                Data = ShipingUpdate
            });
        }


        public async Task<Response<PagedResponse<List<Shipping>>>> GetAllProductShipped(int PageZise, int PageNumber)
        {

            var validFilter = new PaginationFilter(PageNumber, PageZise);
               var ProductShpped = await _dataContext.shippings.OrderByDescending(d => d.DateShipped)
                      .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();


            var totalRecords = await _dataContext.shippings.CountAsync();

            var pagedReponse = PaginationHelper.CreatePagedReponse<Shipping>(ProductShpped, validFilter, totalRecords);


            return await Task.FromResult(new Response<PagedResponse<List<Shipping>>>
            {
                Message = "Request Successful",
                Success = true,
                Data = pagedReponse
            });
        }




        public async Task<double> GetDistance(  string toAddress)
        {
            try
            {
                // ffrom latitude and I assumed i'm using DHL office address as my from
                var fromAddress = "3636 7th Ave. North\r\nBirmingham, Alabama, 35222";
                var fromLocation = new GoogleLocationService();
                var point = fromLocation.GetLatLongFromAddress(fromAddress);
                var fromlatitude = point.Latitude;
                var fromlongitude = point.Longitude;


                var toLocation = new GoogleLocationService();
                var topoint = toLocation.GetLatLongFromAddress(toAddress);
                var tolatitude = topoint.Latitude;
                var tolongitude = topoint.Longitude;


                if (fromLocation != null && toLocation != null)
                {
                    // Calculate distance in kilometers using Haversine formula
                    double distance = CalculateDistance(fromlatitude, fromlongitude, tolatitude, tolongitude);

                   return distance;
                }
                else
                {
                    return 0.0;
                }
            }
            catch (Exception ex)
            {
                return Convert.ToDouble (ex.StackTrace);
            }
        }


        // the method to call google api to get real location. But I will just asume location
        // for the purpose of test . I dont have api key
        private async Task<Location> GetLocation(string address)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";

                var response = await httpClient.GetStringAsync(url);

                var result = JsonConvert.DeserializeObject<GeocodingResult>(response);

                if (result.Status == "OK" && result.Results.Count > 0)
                {
                    return result.Results[0].Geometry.Location;
                }
                else
                {
                    throw new Exception($"Failed to get location for address: {address}");
                }
            }
        }


        private double ToRadians(double degree)
        {
            return degree * Math.PI / 180;
        }


        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Earth radius in kilometers
            const double R = 6371; 

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

     
    }
}
