using Logic;
using Logic.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace LogicTests;

[TestFixture]
public class AiTests
{
    private ServiceProvider provider;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        ServiceCollection services = new();
        services.AddLogic();
        provider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        provider.Dispose();
    }

    //[CancelAfter(1000 * 15)]
    //[TestCaseSource(nameof(Source_AiGivesValidPrediction_When_HasRegionAndName))]
    //public void AiGivesValidPrediction_When_HasValidLot(
    //    (EstimationRequest lot,
    //    PriceFork expected,
    //    double delta) given,
    //    CancellationToken cancellationToken)
    //{
    //    // Arrange
    //    var predictor = provider.GetService<AiPricePredictorService>();
    //    double realDeltaLower, realDeltaUpper;

    //    // Act
    //    PriceFork reply = default;
    //    var request = predictor!.GetPriceRecommendationAsync(given.lot, cancellationToken: cancellationToken);

    //    // Assert
    //    request.ShouldSatisfyAllConditions(
    //        request => request.ShouldNotThrowAsync(),
    //        request => reply = request.GetAwaiter().GetResult(),
    //        request =>
    //        {
    //            realDeltaLower = Math.Abs(reply.LowerLimit - given.expected.LowerLimit);
    //            realDeltaLower.ShouldBeLessThanOrEqualTo(given.delta,
    //                "the real value should differ from the expected value by the set delta");
    //        },
    //        request =>
    //        {
    //            realDeltaUpper = Math.Abs(reply.UpperLimit - given.expected.UpperLimit);
    //            realDeltaUpper.ShouldBeLessThanOrEqualTo(given.delta,
    //                "the real value should differ from the expected value by the set delta");
    //        });

    //    TestContext.Out.WriteLine($"Lower limit: {reply.LowerLimit}");
    //    TestContext.Out.WriteLine($"Upper limit: {reply.UpperLimit}");
    //}

    public static IEnumerable<TestCaseData<EstimationRequest, PriceFork, double>>
        Source_AiGivesValidPrediction_When_HasRegionAndName()
    {
        yield return new(
            new EstimationRequest("Килограмм картофеля", null, "Вещь", "Еда"),
            new PriceFork(39, 76),
            10);
    }

    [Test]
    [CancelAfter(1000 * 15)]
    public async Task ConnectionToAiIsSuccessful(CancellationToken cancellationToken)
    {
        // Arrange
        var predictor = provider.GetService<AiPricePredictorService>();

        // Act
        string? reply = null;
        Task<string?> ping = predictor!.Ping(cancellationToken: cancellationToken);

        // Assert
        await ping.ShouldNotThrowAsync();
        reply.ShouldNotBeNull("null represents aborted request, any string is a response");

        TestContext.Out.WriteLine(reply);
    }
}
