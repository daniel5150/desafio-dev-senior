using System.Collections.Generic;
using System.Linq;
using MaplinkChallenge.Domain;
using MaplinkChallenge.Service.MaplinkServices.AddressFinder;
using MaplinkChallenge.Service.MaplinkServices.Route;

namespace MaplinkChallenge.Service
{
    public class RouteService : IRouteService
    {
        //constant properties
        private readonly string _token = System.Configuration.ConfigurationManager.AppSettings["Maplink.API.Token"];
        private readonly string _language = System.Configuration.ConfigurationManager.AppSettings["Maplink.API.Language"];

        /// <summary>
        /// Main method responsible to find the addresses's latitude/longitude and find the route details
        /// </summary>
        /// <param name="addresses">list of addresses to calculate the route totals</param>
        /// <param name="routeType">the type of route: fastest or avoiding traffic</param>
        /// <returns>an object of type "RouteValues" containing all the route totals</returns>
        public RouteValues GetRouteDetails(List<Domain.Address> addresses, Enums.RouteType routeType)
        {
            //cast addresses to RouteStop type
            var routes = (from address in addresses
                let ad = GetAddressDetails(address)
                select new RouteStop
                {
                    description = address.StreetName + ", " + address.Number,
                    point = new MaplinkServices.Route.Point {x = ad.point.x, y = ad.point.y}
                }).ToArray();

            //define route options
            var routeOptions = new RouteOptions
            {
                language = _language,
                routeDetails = new RouteDetails {descriptionType = 0, routeType = (int) routeType, optimizeRoute = true},
                vehicle = new Vehicle
                {
                    tankCapacity = 20,
                    averageConsumption = 9,
                    fuelPrice = 3,
                    averageSpeed = 60,
                    tollFeeCat = 2
                }
            };

            //call Maplink WebServices
            RouteValues routeValues;
            using (var routeSoapClient = new RouteSoapClient())
            {
                routeValues = (RouteValues) routeSoapClient.getRouteTotals(routes, routeOptions, _token);
            }

            return routeValues;
        }

        private AddressLocation GetAddressDetails(Domain.Address address)
        {
            var findAddress = (MaplinkServices.AddressFinder.Address) address;

            var addressOptions = new AddressOptions
            {
                usePhonetic = true,
                searchType = 2,
                resultRange = new ResultRange {pageIndex = 1, recordsPerPage = 10}
            };

            List<AddressLocation> addressLocations;

            using (var addressFinderSoapClient = new AddressFinderSoapClient())
            {
                var findAddressResponse = addressFinderSoapClient.findAddress(findAddress, addressOptions, _token);
                addressLocations = findAddressResponse.addressLocation.ToList();
            }
            return addressLocations.FirstOrDefault();
        }
    }
}