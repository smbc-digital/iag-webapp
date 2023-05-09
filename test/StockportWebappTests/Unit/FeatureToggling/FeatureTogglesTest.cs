namespace StockportWebappTests_Unit.Unit.FeatureToggling;

public class FeatureTogglesReaderTest
{
    private FeatureTogglesReader _featureTogglesReader;
    private static readonly Mock<ILogger<FeatureTogglesReader>> Logger = new Mock<ILogger<FeatureTogglesReader>>();
    readonly string YamlFile = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
        "..", "..", "..", "Unit", "FeatureToggling", "featureToggles.yml"));

    readonly string invalidYamlFile = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
        "..", "..", "..", "Unit", "FeatureToggling", "yamlWithSyntaxError.yml"));

    [Fact]
    public void ShouldSetToggleValuesToTrueForGivenEnvironment()
    {
        const string appEnvironment = "prod";
        _featureTogglesReader = new FeatureTogglesReader(YamlFile, appEnvironment, Logger.Object);
        var featureTogglesReader = _featureTogglesReader;

        var featureToggles = featureTogglesReader.Build<FakeFeatureToggles>();

        featureToggles.SearchBar.Should().Be(true);
    }

    [Fact]
    public void ShouldSetToggleValuesToFalseForGivenEnvironment()
    {
        string appEnvironment = "prod";

        var featureTogglesReader = new FeatureTogglesReader(YamlFile, appEnvironment, Logger.Object);

        var featureToggles = featureTogglesReader.Build<FakeFeatureToggles>();

        featureToggles.AToZ.Should().Be(false);
    }

    [Fact]
    public void ShouldInheritToggleValuesFromProdEnvironment()
    {
        string appEnvironment = "preprod";

        var featureTogglesReader = new FeatureTogglesReader(YamlFile, appEnvironment, Logger.Object);

        var featureToggles = featureTogglesReader.Build<FakeFeatureToggles>();

        featureToggles.SearchBar.Should().Be(true);
        featureToggles.AToZ.Should().Be(false);
    }

    [Fact]
    public void ShouldOverrideToggleValuesThatAreSetInProdEnvironment()
    {
        string appEnvironment = "preprod";

        var featureTogglesReader = new FeatureTogglesReader(YamlFile, appEnvironment, Logger.Object);

        var featureToggles = featureTogglesReader.Build<FakeFeatureToggles>();

        featureToggles.OverriddenFeature.Should().Be(true);
    }

    [Fact]
    public void ShouldLogInformationTheLoadedFeatureToggles()
    {
        string appEnvironment = "preprod";

        var featureTogglesReader = new FeatureTogglesReader(YamlFile, appEnvironment, Logger.Object);

        var featureToggles = featureTogglesReader.Build<FakeFeatureToggles>();

        LogTesting.Assert(Logger, LogLevel.Information,
            $"Feature Toggles for: {appEnvironment}\n" +
            $"SearchBar: {featureToggles.SearchBar}, " +
            $"AToZ: {featureToggles.AToZ}, " +
            $"OverriddenFeature: {featureToggles.OverriddenFeature}, ");
    }

    [Fact]
    public void ShouldDefaultAllFeatureTogglesToFalseIfEnvironmentIsNotFound()
    {
        string appEnvironment = "notfound";

        var featureTogglesReader = new FeatureTogglesReader(YamlFile, appEnvironment, Logger.Object);

        var featureToggles = featureTogglesReader.Build<FakeFeatureToggles>();

        featureToggles.AToZ.Should().Be(false);
        featureToggles.SearchBar.Should().Be(false);
        featureToggles.OverriddenFeature.Should().Be(false);

        LogTesting.Assert(Logger, LogLevel.Warning,
            $"No feature toggle configuration found for environment: {appEnvironment}. Setting all features to false.");
    }

    [Fact]
    public void ShouldDefaultToFalseIfFileNotFound()
    {
        string appEnvironment = "prod";

        var nonExistentFile = "notfound";
        var featureTogglesReader = new FeatureTogglesReader(nonExistentFile, appEnvironment, Logger.Object);

        var featureToggles = featureTogglesReader.Build<FakeFeatureToggles>();

        featureToggles.AToZ.Should().Be(false);
        featureToggles.SearchBar.Should().Be(false);
        featureToggles.OverriddenFeature.Should().Be(false);

        LogTesting.Assert(Logger, LogLevel.Warning,
            $"No feature toggle configuration file found ({nonExistentFile}). Setting all features to false.");
    }

    [Fact]
    public void ShouldDefaultToFalseIfFileCannotBeParsed()
    {
        string appEnvironment = "prod";

        var featureTogglesReader = new FeatureTogglesReader(invalidYamlFile, appEnvironment, Logger.Object);

        var featureToggles = featureTogglesReader.Build<FakeFeatureToggles>();

        featureToggles.AToZ.Should().Be(false);
        featureToggles.SearchBar.Should().Be(false);
        featureToggles.OverriddenFeature.Should().Be(false);

        LogTesting.Assert(Logger, LogLevel.Warning,
            $"Cannot parse feature toggles in {invalidYamlFile}. Setting all features to false.");
    }
}
