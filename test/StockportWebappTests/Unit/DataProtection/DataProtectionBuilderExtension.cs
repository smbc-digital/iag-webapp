using System;
using System.Linq;
using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using StockportWebapp.DataProtection;
using Xunit;

namespace StockportWebappTests_Unit.Unit.DataProtection
{
    public class DataProtectionBuilderExtension
    {
        [Fact]
        public void PersistKeysToRedis_EmptyConnectionString()
        {
            var builder = new DataProtectionBuilder(new ServiceCollection());
            Assert.Throws<ArgumentException>(() => DataProtectionBuilderExtensions.PersistKeysToRedis(builder, ""));
        }

        [Fact]
        public void PersistKeysToRedis_NullBuilder()
        {
            Assert.Throws<ArgumentNullException>(() => DataProtectionBuilderExtensions.PersistKeysToRedis(null, "connection"));
        }

        [Fact]
        public void PersistKeysToRedis_NullConnectionString()
        {
            var builder = new DataProtectionBuilder(new ServiceCollection());
            Assert.Throws<ArgumentNullException>(() => DataProtectionBuilderExtensions.PersistKeysToRedis(builder, null));
        }

        [Fact]
        public void PersistKeysToRedis_RegistersServices()
        {
            var builder = new DataProtectionBuilder(new ServiceCollection());
            builder.PersistKeysToRedis("connection");

            // A lambda factory gets registered for the repo so we can't test the type without actually
            // trying to connect to Redis.
            Assert.Single(builder.Services.Where(s => s.ServiceType == typeof(IXmlRepository)));
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
            var builder = new DataProtectionBuilder(new ServiceCollection());
            Assert.Throws<ArgumentNullException>(() => DataProtectionBuilderExtensions.Use(builder, null));
        }

        [Fact]
        public void Use_ReplacesAllServicesMatchingType()
        {
            var descriptor = new ServiceDescriptor(typeof(string), "c");
            IServiceCollection services = new ServiceCollection();
            services.Add(new ServiceDescriptor(typeof(string), "a"));
            services.Add(new ServiceDescriptor(typeof(string), "b"));
            var builder = new DataProtectionBuilder(services);
            builder.Use(descriptor);
            Assert.Single(services.Where(s => s.ServiceType == typeof(string)));
            Assert.Equal("c", services[0].ImplementationInstance);
        }
    }
}
