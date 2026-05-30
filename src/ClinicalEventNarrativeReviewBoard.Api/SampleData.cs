namespace ClinicalEventNarrativeReviewBoard.Api;

public static class SampleData
{
    public static readonly ClinicalEventNarrativeExport Payload = new(
        Snapshots:
        [
            new(
                "evt-neuro",
                "Neurologic event narrative cluster",
                "Narrative intake and medical review lane",
                "Global Case Vault",
                "WATCH",
                "CURRENT",
                "Case Processing",
                8,
                3,
                DateTimeOffset.Parse("2026-05-30T15:10:00Z")
            ),
            new(
                "evt-hepatic",
                "Hepatic event signal packet",
                "Causality and submission lane",
                "E2B + Literature feed",
                "CRITICAL",
                "STALE",
                "Regulatory Affairs",
                5,
                2,
                DateTimeOffset.Parse("2026-05-29T10:45:00Z")
            ),
            new(
                "evt-peds",
                "Pediatric rash follow-up packet",
                "Follow-up and review lane",
                "Affiliate inbox",
                "WATCH",
                "CURRENT",
                "Patient Safety",
                4,
                1,
                DateTimeOffset.Parse("2026-05-30T08:55:00Z")
            )
        ],
        Gaps:
        [
            new(
                "gap-intake",
                "evt-neuro",
                "Narrative Intake",
                "high",
                "Initial case narrative completeness",
                "Every serious event narrative should include onset, suspect product timing, reporter framing, and a medically readable summary before physician review.",
                "Two neurologic cases still lack a coherent onset timeline and one reporter summary contradicts the structured event date.",
                13,
                true
            ),
            new(
                "gap-seriousness",
                "evt-neuro",
                "Seriousness Summary",
                "high",
                "Seriousness and hospitalization framing",
                "The narrative packet should make hospitalization, medical significance, and outcome posture explicit before safety physician review.",
                "One packet still buries the overnight hospitalization detail in free text and the seriousness summary is not aligned.",
                18,
                true
            ),
            new(
                "gap-causality",
                "evt-hepatic",
                "Causality Framing",
                "medium",
                "Narrative causality explanation",
                "Signal packets should keep dechallenge, rechallenge, and concomitant-med framing readable before committee review.",
                "The hepatic narrative packet still lacks a clean causality summary tying lab drift to product exposure timing.",
                12,
                false
            ),
            new(
                "gap-followup",
                "evt-peds",
                "Follow-Up",
                "medium",
                "Reporter follow-up narrative closure",
                "Follow-up packets should keep symptom resolution, intervention details, and missing outcomes visible before the next review cycle.",
                "Two pediatric narratives still need outcome follow-up to close the open rash progression questions.",
                15,
                false
            ),
            new(
                "gap-medreview",
                "evt-neuro",
                "Medical Review",
                "high",
                "Safety physician packet",
                "The physician packet should keep narrative chronology, seriousness rationale, and next action explicit before signoff.",
                "The current neurologic packet still lacks the physician addendum explaining seizure chronology and escalation rationale.",
                21,
                true
            ),
            new(
                "gap-submission",
                "evt-hepatic",
                "Submission Packet",
                "high",
                "Submission-ready narrative packet",
                "Submission packets should keep clean narratives, literature references, and reviewer signoff together before outbound regulatory use.",
                "The hepatic packet still needs the reconciled literature citation and the final submission-ready narrative revision.",
                24,
                true
            )
        ]
    );

    public static readonly IReadOnlyList<NarrativeLanePacket> NarrativeLane =
    [
        new(
            "intake-lane",
            "Narrative intake lane",
            "Case Processing",
            "red",
            "Readable case chronology, reporter framing, and event-sequence integrity",
            "Close the missing onset details before another serious packet reaches medical review with a broken chronology.",
            "Intake quality is currently the biggest narrative-risk pressure point."
        ),
        new(
            "medreview-lane",
            "Medical review lane",
            "Medical Review",
            "red",
            "Seriousness framing, physician addendum quality, and medically readable summaries",
            "Close the seriousness alignment and physician addendum before the next safety review pass.",
            "Medical review posture is not signoff-safe yet."
        ),
        new(
            "followup-lane",
            "Follow-up narrative lane",
            "Patient Safety",
            "yellow",
            "Outcome closure, symptom progression details, and affiliate handoff timing",
            "Close the overdue follow-up narratives before the next review cycle hardens the open unknowns.",
            "Follow-up posture is recoverable if the next outreach lands on time."
        ),
        new(
            "submission-lane",
            "Submission packet lane",
            "Regulatory Affairs",
            "red",
            "Submission-ready narrative quality, literature linkage, and named signoff ownership",
            "Close the missing narrative revision and literature reconciliation before another hepatic packet reaches outbound review.",
            "Submission posture is not yet outbound-safe."
        )
    ];

    public static readonly IReadOnlyList<ReviewPacket> ReviewPosture =
    [
        new(
            "CENB-12",
            "Neurologic intake packet",
            "Case Processing",
            "red",
            56,
            "Case chronology is still missing the final onset clarification and seriousness alignment.",
            "Do not let a partially coherent serious-event narrative move downstream without a defensible intake packet.",
            7
        ),
        new(
            "CENB-18",
            "Hepatic submission packet",
            "Regulatory Affairs",
            "red",
            61,
            "The packet still lacks the reconciled literature citation and final narrative revision.",
            "Do not let the outbound packet clear while the narrative still needs evidence reconciliation.",
            11
        ),
        new(
            "CENB-24",
            "Pediatric follow-up packet",
            "Patient Safety",
            "yellow",
            77,
            "Outcome follow-up is still missing for two open rash progression narratives.",
            "The lane can recover if the follow-up details land before the next review window.",
            16
        ),
        new(
            "CENB-31",
            "Safety physician packet",
            "Medical Review",
            "yellow",
            72,
            "The physician addendum still needs the explicit seizure chronology and escalation rationale.",
            "Review posture can recover if the physician addendum closes in the next cycle.",
            9
        )
    ];
}
