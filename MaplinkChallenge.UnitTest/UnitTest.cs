using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using MaplinkChallenge.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaplinkChallenge.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        readonly Service.RouteService _routeService = new Service.RouteService();

        [TestMethod]
        public void GettingBasicRoutDetails()
        {
            //input addresses (simulate user inputs)
            var addresses = new List<Address>
            {
                new Address {StreetName = "Avenida Paulista", Number = "100", City = "São Paulo", State = "SP"},
                //new Address {StreetName = "José", Number = "100", City = "Manaus", State = "AM"},
                new Address {StreetName = "Av Pres Juscelino Kubitschek", Number = "100", City = "São Paulo", State = "SP"},
                new Address {StreetName = "Av Nove de Julho", Number = "1500", City = "São Paulo", State = "SP"},
            };

            //call service
            var result = _routeService.GetRouteDetails(addresses, Enums.RouteType.Fastest);

            //asserts
            Assert.IsNotNull(result);
            Assert.AreEqual(new TimeSpan(0,19,20),result.TotalTime);
            Assert.AreEqual(10.43, result.TotalDistance);
            Assert.AreEqual(3.48, result.FuelCost);
            Assert.AreEqual(3.48, result.TotalCostIncludingToll);

        }
    }
}
