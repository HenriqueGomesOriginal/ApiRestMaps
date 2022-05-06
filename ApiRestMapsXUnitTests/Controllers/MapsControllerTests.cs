using Xunit;
using ApiRestMaps.Controllers;
using Microsoft.Extensions.Logging;
using ApiRestMaps.Data.VO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiRestMaps.Services.Implementation;
using ApiRestMaps.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestMapsXUnitTests.ServicesTests
{
    public class MapsControllerTest
    {
        // Fake logger
        ILogger<MapsController> logger { get; set; }

        [Fact]
        public async Task Calculate_Distance_Controller_Reponse_Test()
        {
            // Arrange

            // Mocked data to send
            var _requestData = new MapsRequest();
            var _addresses = new List<AddressObj>();
            var _results = new List<MapsResult>();

            // Config API
            var _googleConfig = new GoogleCloud();
            _googleConfig.ApiKey = "YOUR-KEY-GOOGLE-CLOUD";

            IOptions<GoogleCloud> appSettingsOptions = Options.Create(_googleConfig);

            var _service = new MapsService(appSettingsOptions);

            _addresses.Add(new AddressObj
            {
                Address = "Av Afonso Arinos de Melo Franco 191, Barra da Tijuca, Rio de Janeiro"
            });
            _addresses.Add(new AddressObj
            {
                Address = "Av das Americas 500, Barra da Tijuca, Rio de Janeiro"
            });
            _addresses.Add(new AddressObj
            {
                Address = "Av Afranio de Melo Franco 290, Leblon, Rio de Janeiro"
            });
            _requestData = new MapsRequest { AddressArray = _addresses };

            // Proper result of this mocked data
            var mockedResult = new List<MapsResult>();

            var addressCompOne = new List<AddressObj>();
            var addressCompTwo = new List<AddressObj>();
            var addressCompThree = new List<AddressObj>();

            addressCompOne.Add(new AddressObj
            {
                Address = "Av. Afonso Arinos de Melo Franco, 191 - Barra da Tijuca, Rio de Janeiro - RJ, 22631-455, Brazil"
            });
            addressCompOne.Add(new AddressObj
            {
                Address = "Av. das Américas, 500 - Barra da Tijuca, Rio de Janeiro - RJ, 22640-100, Brazil"
            });

            addressCompTwo.Add(new AddressObj
            {
                Address = "Av. Afonso Arinos de Melo Franco, 191 - Barra da Tijuca, Rio de Janeiro - RJ, 22631-455, Brazil"
            });
            addressCompTwo.Add(new AddressObj
            {
                Address = "3 - Av. Afrânio de Melo Franco, 290 - Leblon, Rio de Janeiro - RJ, 22430-060, Brazil"
            });

            addressCompThree.Add(new AddressObj
            {
                Address = "Av. das Américas, 500 - Barra da Tijuca, Rio de Janeiro - RJ, 22640-100, Brazil"
            });
            addressCompThree.Add(new AddressObj
            {
                Address = "3 - Av. Afrânio de Melo Franco, 290 - Leblon, Rio de Janeiro - RJ, 22430-060, Brazil"
            });

            mockedResult.Add(new MapsResult
            {
                AddressArray = addressCompOne,
                Closeset = true,
                Distance = 0.6389984104562298,
                Further = false
            });
            mockedResult.Add(new MapsResult
            {
                AddressArray = addressCompTwo,
                Closeset = false,
                Distance = 12.310573645357106,
                Further = true
            });
            mockedResult.Add(new MapsResult
            {
                AddressArray = addressCompTwo,
                Closeset = false,
                Distance = 11.675396261362843,
                Further = false
            });


            var controller = new MapsController(logger, _service);

            // Act
            var actionResult = await controller.CalculateDistance(_requestData);

            // Assert
            var result = (OkObjectResult)actionResult;
            Assert.Equal(result.Value.ToString(), mockedResult.ToString());
        }
    }
}
