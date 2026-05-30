using ClinicalEventNarrativeReviewBoard.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ClinicalEventNarrativeReviewBoard.Tests;

public sealed class ClinicalEventNarrativeReviewBoardTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ClinicalEventNarrativeReviewBoardTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Overview_route_renders_narrative_review_shell()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("Clinical Event Narrative Review Board", html);
        Assert.Contains("narrative-review", html);
    }

    [Fact]
    public async Task Api_summary_returns_expected_counts()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/dashboard/summary");
        var json = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("\"snapshots\":3", json);
        Assert.Contains("\"currentPackets\":2", json);
        Assert.Contains("\"blockingNarratives\":4", json);
    }

    [Fact]
    public void Analysis_flags_high_risk_narrative_gaps()
    {
        var report = AnalysisService.Analyze(SampleData.Payload);

        Assert.Equal(3, report.Snapshots);
        Assert.Equal(6, report.Gaps);
        Assert.Contains(report.Findings, finding => finding.Code == "seriousness-summary-gap");
        Assert.Contains(report.Findings, finding => finding.Code == "submission-packet-gap");
    }
}
