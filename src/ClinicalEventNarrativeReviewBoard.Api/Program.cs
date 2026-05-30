using System.Text.Json;
using ClinicalEventNarrativeReviewBoard.Api;

var app = ClinicalEventNarrativeReviewBoardApplication.BuildApp(args);

if (args.Contains("--prerender"))
{
    await SiteBuilder.WriteAsync();
    return;
}

if (args.Contains("--demo"))
{
    Console.WriteLine(JsonSerializer.Serialize(AnalysisService.Summary(), new JsonSerializerOptions { WriteIndented = true }));
    Console.WriteLine(JsonSerializer.Serialize(SampleData.NarrativeLane, new JsonSerializerOptions { WriteIndented = true }));
    return;
}

app.Run();

public partial class Program;

public static class ClinicalEventNarrativeReviewBoardApplication
{
    public static WebApplication BuildApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => Results.Content(RenderService.Overview(), "text/html"));
        app.MapGet("/narrative-lane", () => Results.Content(RenderService.NarrativeLane(), "text/html"));
        app.MapGet("/triage-gaps", () => Results.Content(RenderService.TriageGaps(), "text/html"));
        app.MapGet("/review-posture", () => Results.Content(RenderService.ReviewPosture(), "text/html"));
        app.MapGet("/verification", () => Results.Content(RenderService.Verification(), "text/html"));
        app.MapGet("/docs", () => Results.Content(RenderService.Docs(), "text/html"));

        app.MapGet("/api/dashboard/summary", () => Results.Json(AnalysisService.Summary()));
        app.MapGet("/api/narrative-lane", () => Results.Json(SampleData.NarrativeLane));
        app.MapGet("/api/triage-gaps", () => Results.Json(SampleData.Payload.Gaps));
        app.MapGet("/api/review-posture", () => Results.Json(SampleData.ReviewPosture));
        app.MapGet("/api/verification", () => Results.Json(new[]
        {
            "Synthetic clinical narrative evidence only; no patient-identifiable, tenant, or sponsor-confidential event narratives are published.",
            "Narrative intake, seriousness framing, causality language, follow-up closure, medical review, and submission posture are modeled as operator surfaces.",
            "This repo demonstrates biotech narrative-review workflow depth without claiming GVP, FDA, EMA, or pharmacovigilance compliance."
        }));
        app.MapGet("/api/sample", () => Results.Text(RenderService.Sample(), "application/json"));

        return app;
    }
}

public static class SiteBuilder
{
    public static async Task WriteAsync()
    {
        var root = FindRepoRoot();
        var siteDir = Path.Combine(root, "site");
        Directory.CreateDirectory(siteDir);

        var pages = new Dictionary<string, string>
        {
            ["index.html"] = RenderService.Overview(),
            [Path.Combine("narrative-lane", "index.html")] = RenderService.NarrativeLane(),
            [Path.Combine("triage-gaps", "index.html")] = RenderService.TriageGaps(),
            [Path.Combine("review-posture", "index.html")] = RenderService.ReviewPosture(),
            [Path.Combine("verification", "index.html")] = RenderService.Verification(),
            [Path.Combine("docs", "index.html")] = RenderService.Docs()
        };

        foreach (var (relative, html) in pages)
        {
            var target = Path.Combine(siteDir, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            await File.WriteAllTextAsync(target, html);
        }

        var apiDir = Path.Combine(siteDir, "api");
        Directory.CreateDirectory(Path.Combine(apiDir, "dashboard"));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "dashboard", "summary.json"), JsonSerializer.Serialize(AnalysisService.Summary(), new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "narrative-lane.json"), JsonSerializer.Serialize(SampleData.NarrativeLane, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "triage-gaps.json"), JsonSerializer.Serialize(SampleData.Payload.Gaps, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "review-posture.json"), JsonSerializer.Serialize(SampleData.ReviewPosture, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "verification.json"), JsonSerializer.Serialize(new[]
        {
            "Synthetic clinical narrative evidence only; no patient-identifiable, tenant, or sponsor-confidential event narratives are published.",
            "Narrative intake, seriousness framing, causality language, follow-up closure, medical review, and submission posture are modeled as operator surfaces.",
            "This repo demonstrates biotech narrative-review workflow depth without claiming GVP, FDA, EMA, or pharmacovigilance compliance."
        }, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync(Path.Combine(apiDir, "sample.json"), RenderService.Sample());

        const string domain = "narrative.kineticgain.com";
        await File.WriteAllTextAsync(
            Path.Combine(siteDir, "robots.txt"),
            $"User-agent: *{Environment.NewLine}Allow: /{Environment.NewLine}Sitemap: https://{domain}/sitemap.xml{Environment.NewLine}");
        await File.WriteAllTextAsync(Path.Combine(siteDir, "sitemap.xml"), """
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url><loc>https://narrative.kineticgain.com/</loc></url>
  <url><loc>https://narrative.kineticgain.com/narrative-lane/</loc></url>
  <url><loc>https://narrative.kineticgain.com/triage-gaps/</loc></url>
  <url><loc>https://narrative.kineticgain.com/review-posture/</loc></url>
  <url><loc>https://narrative.kineticgain.com/verification/</loc></url>
  <url><loc>https://narrative.kineticgain.com/docs/</loc></url>
</urlset>
""");
        await File.WriteAllTextAsync(Path.Combine(siteDir, "CNAME"), domain + Environment.NewLine);
    }

    private static string FindRepoRoot()
    {
        var current = AppContext.BaseDirectory;
        for (var i = 0; i < 8; i++)
        {
            if (File.Exists(Path.Combine(current, "clinical-event-narrative-review-board.sln")))
            {
                return current;
            }

            current = Directory.GetParent(current)?.FullName
                ?? throw new DirectoryNotFoundException("Unable to resolve repo root.");
        }

        throw new DirectoryNotFoundException("Unable to resolve repo root.");
    }
}
