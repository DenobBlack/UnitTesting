using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingLib.Weather;
using TestingLib.Shop;
using TestingLib.Library;

namespace UnitTesting.DenobBlack
{
    public class LabWork7Tests
    {
        private readonly Mock<IWeatherForecastSource> mockWeatherForecastSource;
        private readonly Mock<ICustomerRepository> mockCustomerRepository;
        private readonly Mock<INotificationService> mockNotificationService;
        private readonly Mock<IOrderRepository> mockOrderRepository;

        public LabWork7Tests()
        {
            mockWeatherForecastSource = new Mock<IWeatherForecastSource>();
            mockCustomerRepository = new Mock<ICustomerRepository>();
            mockNotificationService = new Mock<INotificationService>();
            mockOrderRepository = new Mock<IOrderRepository>();
        }

        [Fact]
        public void GetWeatherForecast_ShouldReturnCorrectResult()
        {
            // Arrange
            var weatherForecast = new WeatherForecast { TemperatureC = 27, Summary = "Солнечно" };
            var currentDate = DateTime.Now; 

            mockWeatherForecastSource.Setup(repo => repo.GetForecast(currentDate)).Returns(weatherForecast);

            var service = new WeatherForecastService(mockWeatherForecastSource.Object);

            // Act
            var result = service.GetWeatherForecast(currentDate);

            // Assert
            Assert.NotNull(result);
            mockWeatherForecastSource.Verify(repo => repo.GetForecast(It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void CreateOrder_ShouldCreateNewOrder()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Максим", Email = "max@gmail.com" };
            var order = new Order { Id = 10, Date = DateTime.Now, Customer = customer, Amount = 10 };
            mockOrderRepository.Setup(repo => repo.GetOrderById(1)).Returns(order);

            var service = new ShopService(mockCustomerRepository.Object, mockOrderRepository.Object, mockNotificationService.Object);

            // Act
            service.CreateOrder(order);

            // Assert
            mockOrderRepository.Verify(repo => repo.GetOrderById(It.IsAny<int>()), Times.Once);
            mockOrderRepository.Verify(repo => repo.AddOrder(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public void CreateOrder_ShouldSendNotification()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Максим", Email = "max@gmail.com" };
            var order = new Order { Id = 10, Date = DateTime.Now, Customer = customer, Amount = 10 };
            mockOrderRepository.Setup(repo => repo.GetOrderById(1)).Returns(order);

            var service = new ShopService(mockCustomerRepository.Object, mockOrderRepository.Object, mockNotificationService.Object);

            // Act
            service.CreateOrder(order);

            // Assert
            mockNotificationService.Verify(repo => repo.SendNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetCustomerInfo_ShouldReturnCorrectResult()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Максим", Email = "max@gmail.com" };
            var order = new Order { Id = 1, Date = DateTime.Now, Customer = customer, Amount = 10 };
            mockOrderRepository.Setup(repo => repo.GetOrders()).Returns(new List<Order> { order });
            mockCustomerRepository.Setup(repo => repo.GetCustomerById(1)).Returns(customer);

            var service = new ShopService(mockCustomerRepository.Object, mockOrderRepository.Object, mockNotificationService.Object);
            // Act
            string result = service.GetCustomerInfo(1);

            // Assert
            Assert.Equal("Customer Максим has 1 orders", result);
            mockCustomerRepository.Verify(repo => repo.GetCustomerById(It.IsAny<int>()), Times.Once);
            mockOrderRepository.Verify(repo => repo.GetOrders(), Times.Once);
        }
    }
}
