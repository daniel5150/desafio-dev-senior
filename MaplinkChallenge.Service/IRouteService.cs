using System.Collections.Generic;
using System.ServiceModel;
using MaplinkChallenge.Domain;

namespace MaplinkChallenge.Service
{
    [ServiceContract]
    public interface IRouteService
    {
        [OperationContract]
        RouteValues GetRouteDetails(List<Address> addresses, Enums.RouteType routeType);

        //[OperationContract]
        //List<AddressLocation> GetAddressesDetails(MaplinkServices.AddressFinder.Address address);
    }
}
