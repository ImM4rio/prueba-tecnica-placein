using Microsoft.Extensions.Configuration;
using PedidosAPI.Controllers;
using PedidosAPI.Models;
using NUnit.Framework;
using Moq;

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

            PedidosController controller = new(configuration);
          

            var result = controller.lista() as List<Pedido>;
            Assert.That(result?.Count, Is.Not.EqualTo(0));


        }

        [Test]

        public async Task CheckWeatherAsync_WhenDataIsAvailable()
        {
            (int temperatura, int humedad) = await WeatherUtility.CheckWeatherAsync("Santiago de Compostela", "http://api.weatherstack.com/current?access_key=c190f8aeb7838b5dc98e1e2cb169471e&query=");
            Assert.That(temperatura, Is.Not.EqualTo(0));
            Assert.That(humedad, Is.Not.EqualTo(0));

        }  
    }
}