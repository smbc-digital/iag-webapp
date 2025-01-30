namespace StockportWebappTests_Unit.Unit.DataProtection;

public class DataProtectionBuilderExtension
{
    private readonly Mock<IDataProtectionBuilder> _mockDataProtectionBuilder = new Mock<IDataProtectionBuilder>();

    [Fact]
    public void PersistKeysToRedis_EmptyConnectionString()
    {
        // Arrange
        Mock<IDataProtectionBuilder> builder = new();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => DataProtectionBuilderExtensions.PersistKeysToRedis(builder.Object, string.Empty));
    }

    [Fact]
    public void PersistKeysToRedis_NullBuilder()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => DataProtectionBuilderExtensions.PersistKeysToRedis(null, "connection"));
    }

    [Fact]
    public void PersistKeysToRedis_NullConnectionString()
    {
        // Arrange
        Mock<IDataProtectionBuilder> builder = new();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => DataProtectionBuilderExtensions.PersistKeysToRedis(builder.Object, null));
    }

    [Fact]
    public void PersistKeysToRedis_RegistersServices()
    {
        // Arrange
        IServiceCollection services = new ServiceCollection();
        services.Add(new ServiceDescriptor(typeof(IXmlRepository), "test"));

        _mockDataProtectionBuilder
            .Setup(builder => builder.Services)
            .Returns(services);
        
        IDataProtectionBuilder builder = _mockDataProtectionBuilder.Object;

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
        // Arrange
        ServiceDescriptor descriptor = new(typeof(string), "a");
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => DataProtectionBuilderExtensions.Use(null, descriptor));
    }

    [Fact]
    public void Use_NullDescriptor()
    {
        // Arrange
        Mock<IDataProtectionBuilder> builder = new();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => DataProtectionBuilderExtensions.Use(builder.Object, null));
    }

    [Fact]
    public void Use_ReplacesAllServicesMatchingType()
    {
        // Arrange
        ServiceDescriptor descriptor = new(typeof(string), "c");
        IServiceCollection services = new ServiceCollection();
        services.Add(new ServiceDescriptor(typeof(string), "a"));
        services.Add(new ServiceDescriptor(typeof(string), "b"));

        _mockDataProtectionBuilder
            .Setup(builder => builder.Services)
            .Returns(services);

        // Act
        IDataProtectionBuilder builder = _mockDataProtectionBuilder.Object;

        // Assert
        builder.Use(descriptor);
        Assert.Single(services, s => s.ServiceType.Equals(typeof(string)));
        Assert.Equal("c", services[0].ImplementationInstance);
    }
}