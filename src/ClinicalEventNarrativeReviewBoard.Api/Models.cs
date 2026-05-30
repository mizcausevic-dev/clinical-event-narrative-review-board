namespace ClinicalEventNarrativeReviewBoard.Api;

public sealed record ClinicalSnapshot(
    string Id,
    string Name,
    string NarrativeLane,
    string SourceSystem,
    string Status,
    string PacketStatus,
    string Owner,
    int OpenNarratives,
    int EscalatedNarratives,
    DateTimeOffset CollectedAt
);

public sealed record NarrativeGap(
    string Id,
    string SnapshotId,
    string ControlFamily,
    string Severity,
    string Subject,
    string ExpectedState,
    string ObservedState,
    int HoursOpen,
    bool BlocksMedicalReview
);

public sealed record NarrativeLanePacket(
    string Id,
    string Lane,
    string Owner,
    string Status,
    string Focus,
    string NextAction,
    string Note
);

public sealed record ReviewPacket(
    string PacketId,
    string Lane,
    string Owner,
    string Status,
    int ReadinessScore,
    string Blocker,
    string DecisionNote,
    int ReviewWindowHours
);

public sealed record ClinicalEventNarrativeExport(
    IReadOnlyList<ClinicalSnapshot> Snapshots,
    IReadOnlyList<NarrativeGap> Gaps
);

public sealed record ClinicalEventNarrativeFinding(
    string Code,
    string Severity,
    string Subject,
    string Message,
    string Owner
);

public sealed record ClinicalEventNarrativeReport(
    int Snapshots,
    int CurrentPackets,
    int Gaps,
    int BlockingNarratives,
    int EvidenceRisks,
    int ReviewRisks,
    IReadOnlyList<ClinicalEventNarrativeFinding> Findings,
    bool Ok
);
