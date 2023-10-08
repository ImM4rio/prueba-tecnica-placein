using Microsoft.Extensions.Configuration;
using PedidosAPI.Controllers;
using PedidosAPI.Models;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Xunit;
using Moq.Protected;
using System.Net;

namespace TestAPI
{
    public class PedidosControllerTest
    {
        private IConfiguration configuration;

        [SetUp]
        public void Setup()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c.GetSection("ConnectionStrings")["CadenaSQLProd"]).Returns("Data Source=MARIO\\SQLEXPRESS;Initial Catalog=ProdPedidos;Integrated Security=SSPI");
            configuration.Setup(c => c.GetSection("ConnectionStrings")["CadenaSQL"]).Returns("Data Source=MARIO\\SQLEXPRESS;Initial Catalog=Pedidos;Integrated Security=SSPI");
            configuration.Setup(c => c.GetSection("ConnectionStrings")["WeatherURL"]).Returns("http://api.weatherstack.com/current?access_key=c190f8aeb7838b5dc98e1e2cb169471e&query=");

            var controller = new PedidosController(configuration.Object);

        }

        [Test]
        public void Lista_ReturnsOkResult_WhenDataIsAvailable()
        {
            // Arrange
            PedidosController controller = new(configuration);

            // Act
            var result = controller.lista() as List<Pedido>;

            // Assert
            NUnit.Framework.Assert.That(result?.Count, Is.Not.EqualTo(0));


        }

        [Test]
        public void Lista_ReturnsInternakServerErrorOnError()
        {
            // Arrange
            PedidosController controller = new(configuration);

            // Act
            var result = controller.lista() as List<Pedido>;

            // Assert
            NUnit.Framework.Assert.That(result, Is.EqualTo(null));

        }

        [Test]
        public async Task Regitro_ReturnsOkWhenSucces()
        {
            // Arrange
            Pedido pedido = new()
            {
                IdPedido = "Id de prueba",
                Nombre = "Prueba",
                Descripcion = "Descripción de prueba",
                Ubicacion = "Ciudad de Prueba",
                Temperatura = 20,
                Humedad = 60
            };
            var sandbox = "true";


            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory()) 
               .AddJsonFile("appsettings.json")
               .Build();

            PedidosController controller = new(configuration);
            var result = await controller.Registro(pedido, sandbox);

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            NUnit.Framework.Assert.That(okResult, Is.Not.Null);
            NUnit.Framework.Assert.That(okResult.StatusCode, Is.EqualTo(200));

        }

        [Test]
        public async Task Registro_ReturnsBadRequest()
        {
            // Arrange
            Pedido pedido = new()
            {
                IdPedido = "Id de prueba",
                Nombre = "Prueba",
                Descripcion = "Descripción de prueba",
                Ubicacion = "Ciudad de Prueba",
                Temperatura = 20,
                Humedad = 60
            };
            var sandbox = "invalid";
            

            // Act
            PedidosController controller = new(configuration);
            var result = await controller.Registro(pedido, sandbox);


            // Assert
            var resultadoBadRequest = Xunit.Assert.IsType<BadRequestObjectResult>(result);
            NUnit.Framework.Assert.That(resultadoBadRequest.Value, Is.EqualTo("El valor de 'sandbox' no es válido."));
        }


        [Test]

        public async Task CheckWeatherAsync_WhenDataIsAvailable()
        {
            //Arrange
            string city = "Santiago de Compostela";
            string weatherURL = "http://api.weatherstack.com/current?access_key=c190f8aeb7838b5dc98e1e2cb169471e&query=";

            //Act
            (int temperatura, int humedad) = await WeatherUtility.CheckWeatherAsync(city, weatherURL);

            // Assert
            NUnit.Framework.Assert.That(temperatura, Is.Not.EqualTo(0));
            NUnit.Framework.Assert.That(humedad, Is.Not.EqualTo(0));

        }

        [Test]

        public async Task Weatherstack_ReturnsOkResult_WhenDataIsAvailable()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
            {"WeatherURL", "http://api.weatherstack.com/current?access_key=c190f8aeb7838b5dc98e1e2cb169471e&query="} 
                })
                .Build();

            WeatherstackController controller = new(configuration);
            string city = "Santiago de Compostela";

            // Act
            var result = await controller.GetWeatherData(city);

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            Xunit.Assert.NotNull(okResult);
            Xunit.Assert.Equal(200, okResult.StatusCode);
            
                }

        [Test]
        public async Task GetWeatherData_ReturnsInternalServerErrorOnError()
        {

            
            // Arrange
            WeatherstackController controller = new(configuration);

            // Act
            var result = await controller.GetWeatherData("");

            // Assert
            NUnit.Framework.Assert.NotNull(result);
            var resultadoBadRequest = Xunit.Assert.IsType<ObjectResult>(result);
            NUnit.Framework.Assert.That(resultadoBadRequest, Is.Not.Null);
            NUnit.Framework.Assert.That(resultadoBadRequest.StatusCode, Is.EqualTo(500));


        }
    }
}