using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Moq;
using StockportWebapp.DataProtection;
using Xunit;
using DataProtectionBuilderExtensions = StockportWebapp.DataProtection.DataProtectionBuilderExtensions;

namespace StockportWebappTests_Unit.Unit.DataProtection
{
    public class DataProtectionBuilderExtension
    {
        private readonly Mock<IDataProtectionBuilder> _mockDataProtectionBuilder = new Mock<IDataProtectionBuilder>();

        [Fact]
        public void PersistKeysToRedis_EmptyConnectionString()
        {
            var builder = new Mock<IDataProtectionBuilder>();
            Assert.Throws<ArgumentException>(() => DataProtectionBuilderExtensions.PersistKeysToRedis(builder.Object, ""));
        }

        [Fact]
        public void PersistKeysToRedis_NullBuilder()
        {
            Assert.Throws<ArgumentNullException>(() => DataProtectionBuilderExtensions.PersistKeysToRedis(null, "connection"));
        }

        [Fact]
        public void PersistKeysToRedis_NullConnectionString()
        {
            var builder = new Mock<IDataProtectionBuilder>();
            Assert.Throws<ArgumentNullException>(() => DataProtectionBuilderExtensions.PersistKeysToRedis(builder.Object, null));
        }

        [Fact]
        public void PersistKeysToRedis_RegistersServices()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.Add(new ServiceDescriptor(typeof(IXmlRepository), "test"));
            _mockDataProtectionBuilder.Setup(x => x.Services).Returns(services);
            var builder = _mockDataProtectionBuilder.Object;

            // Act
            builder.PersistKeysToRedis("connection");

            // Assert
            // A lambda factory gets registered for the repo so we can't test the type without actually
            // trying to connect to Redis.
            Assert.Equal(1, builder.Services.Count(s => s.ServiceType.Equals(typeof(IXmlRepository))));
        }

        [Fact]
        public void Use_NullBuilder()
        {
            var descriptor = new ServiceDescriptor(typeof(string), "a");
            Assert.Throws<ArgumentNullException>(() => DataProtectionBuilderExtensions.Use(null, descriptor));
        }

        [Fact]
        public void Use_NullDescriptor()
        {
            var builder = new Mock<IDataProtectionBuilder>();
            Assert.Throws<ArgumentNullException>(() => DataProtectionBuilderExtensions.Use(builder.Object, null));
        }

        [Fact]
        public void Use_ReplacesAllServicesMatchingType()
        {
            var descriptor = new ServiceDescriptor(typeof(string), "c");
            IServiceCollection services = new ServiceCollection();
            services.Add(new ServiceDescriptor(typeof(string), "a"));
            services.Add(new ServiceDescriptor(typeof(string), "b"));
            _mockDataProtectionBuilder.Setup(x => x.Services).Returns(services);
            var builder = _mockDataProtectionBuilder.Object;

            builder.Use(descriptor);
            Assert.Single(services.Where(s => s.ServiceType == typeof(string)));
            Assert.Equal("c", services[0].ImplementationInstance);
        }
    }
}
