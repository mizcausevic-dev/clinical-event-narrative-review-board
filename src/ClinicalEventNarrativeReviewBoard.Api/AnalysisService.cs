namespace ClinicalEventNarrativeReviewBoard.Api;

public static class AnalysisService
{
    public static ClinicalEventNarrativeReport Analyze(ClinicalEventNarrativeExport payload)
    {
        var findings = payload.Gaps
            .Select(gap => new ClinicalEventNarrativeFinding(
                GapCode(gap.ControlFamily),
                gap.Severity,
                gap.Subject,
                gap.ObservedState,
                OwnerForGap(gap.ControlFamily)))
            .ToList();

        var snapshots = payload.Snapshots.Count;
        var currentPackets = payload.Snapshots.Count(snapshot => snapshot.PacketStatus == "CURRENT");
        var gaps = payload.Gaps.Count;
        var blockingNarratives = payload.Gaps.Count(gap => gap.BlocksMedicalReview);
        var evidenceRisks = payload.Gaps.Count(gap => gap.ControlFamily is "Narrative Intake" or "Seriousness Summary" or "Causality Framing");
        var reviewRisks = payload.Gaps.Count(gap => gap.ControlFamily is "Follow-Up" or "Medical Review" or "Submission Packet");
        var ok = blockingNarratives == 0;

        return new ClinicalEventNarrativeReport(
            snapshots,
            currentPackets,
            gaps,
            blockingNarratives,
            evidenceRisks,
            reviewRisks,
            findings,
            ok
        );
    }

    public static object Summary()
    {
        var report = Analyze(SampleData.Payload);
        return new
        {
            snapshots = report.Snapshots,
            currentPackets = report.CurrentPackets,
            gaps = report.Gaps,
            blockingNarratives = report.BlockingNarratives,
            evidenceRisks = report.EvidenceRisks,
            reviewRisks = report.ReviewRisks,
            ok = report.Ok
        };
    }

    private static string GapCode(string family) => family switch
    {
        "Narrative Intake" => "narrative-intake-gap",
        "Seriousness Summary" => "seriousness-summary-gap",
        "Causality Framing" => "causality-framing-gap",
        "Follow-Up" => "follow-up-gap",
        "Medical Review" => "medical-review-gap",
        "Submission Packet" => "submission-packet-gap",
        _ => "clinical-event-narrative-gap"
    };

    private static string OwnerForGap(string family) => family switch
    {
        "Narrative Intake" => "Case Processing",
        "Seriousness Summary" => "Medical Review",
        "Causality Framing" => "Drug Safety Science",
        "Follow-Up" => "Patient Safety",
        "Medical Review" => "Safety Physician",
        "Submission Packet" => "Regulatory Affairs",
        _ => "Case Processing"
    };
}
