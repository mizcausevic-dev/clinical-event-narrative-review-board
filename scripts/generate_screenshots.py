from pathlib import Path
import textwrap


ROOT = Path(__file__).resolve().parents[1]
SHOT_DIR = ROOT / "screenshots"
SHOT_DIR.mkdir(exist_ok=True)


def wrap(text: str, width: int):
    return textwrap.wrap(text, width=width) or [text]


def draw_lines(lines, x, y, font_size=22, color="#e9f3ff", weight="400", family="Inter,Segoe UI,Arial"):
    parts = [f'<text x="{x}" y="{y}" fill="{color}" font-size="{font_size}" font-weight="{weight}" font-family="{family}">']
    dy = 0
    for line in lines:
        parts.append(f'<tspan x="{x}" dy="{dy}">{line}</tspan>')
        dy = int(font_size * 1.25)
    parts.append("</text>")
    return "\n".join(parts)


def stat_card(x, y, w, h, label, value, detail, accent):
    return f"""
    <rect x="{x}" y="{y}" width="{w}" height="{h}" rx="20" fill="#111827" stroke="rgba(120,255,170,.14)" />
    {draw_lines([label.upper()], x + 24, y + 34, 11, "#8fdcff", "700", "Consolas, monospace")}
    {draw_lines([value], x + 24, y + 86, 34, accent, "700", "Segoe UI, Arial")}
    {draw_lines(wrap(detail, 28), x + 24, y + 124, 15, "#b8c6db", "400")}
    """


def panel(x, y, w, h, kicker, title, body, accent="#19c7ff"):
    return f"""
    <rect x="{x}" y="{y}" width="{w}" height="{h}" rx="22" fill="#0b1220" stroke="rgba(120,255,170,.18)" />
    {draw_lines([kicker.upper()], x + 26, y + 34, 11, accent, "700", "Consolas, monospace")}
    {draw_lines(wrap(title, 30), x + 26, y + 84, 28, "#f5f7ff", "700", "Georgia, serif")}
    {draw_lines(wrap(body, 50), x + 26, y + 136, 16, "#b8c6db", "400")}
    """


def bullet_list(items, x, y, accent="#37ff8b"):
    rows = []
    offset = 0
    for item in items:
        rows.append(f'<circle cx="{x}" cy="{y + offset - 6}" r="5" fill="{accent}" />')
        rows.append(draw_lines(wrap(item, 70), x + 16, y + offset, 16, "#e9f3ff", "400"))
        offset += 54
    return "\n".join(rows)


def shell(eyebrow, title, subtitle, inner):
    return f"""<svg xmlns="http://www.w3.org/2000/svg" width="1400" height="860" viewBox="0 0 1400 860">
    <rect width="1400" height="860" fill="#070a0f"/>
    <rect x="24" y="24" width="1352" height="812" rx="30" fill="#0a1426" stroke="rgba(120,255,170,.18)"/>
    <rect x="58" y="58" width="1284" height="152" rx="26" fill="#0b1220" stroke="rgba(120,255,170,.12)"/>
    {draw_lines([eyebrow.upper()], 94, 96, 13, "#37ff8b", "700", "Consolas, monospace")}
    {draw_lines(wrap(title, 36), 94, 146, 34, "#f5f7ff", "700", "Georgia, serif")}
    {draw_lines(wrap(subtitle, 100), 94, 194, 18, "#b8c6db", "400")}
    {inner}
    </svg>"""


overview = shell(
    "Clinical Event Narrative Review Board",
    "Control-plane summary for clinical narratives, triage gaps, and physician-safe review posture.",
    "Narrative chronology, seriousness framing, causality language, follow-up pressure, physician addenda, and submission readiness stay visible together before medical review hardens around weak packets.",
    f"""
    {stat_card(58, 238, 288, 150, "event clusters", "3", "Synthetic clinical-event narrative clusters across active intake, follow-up, and submission lanes.", "#19c7ff")}
    {stat_card(364, 238, 288, 150, "open gaps", "6", "Narrative intake, seriousness, causality, follow-up, medical review, and submission gaps still open.", "#ffcc66")}
    {stat_card(670, 238, 288, 150, "review blocks", "4", "Four issues still block clean medical review or outbound packet closure.", "#ff5c7a")}
    {stat_card(976, 238, 366, 150, "lead recommendation", "Repair intake and physician addenda", "Do not clear the neurologic packet until chronology, seriousness framing, and physician review posture recover.", "#37ff8b")}

    {panel(58, 420, 614, 356, "Triage gaps", "The riskiest narrative gaps stay visible first.", "Case chronology holes, seriousness drift, causality wording, and missing physician evidence stay tied to their owners so the queue is readable before review closure.", "#19c7ff")}
    <rect x="708" y="420" width="634" height="356" rx="22" fill="#0b1220" stroke="rgba(120,255,170,.18)" />
    {draw_lines(["REVIEW POSTURE"], 734, 454, 11, "#37ff8b", "700", "Consolas, monospace")}
    {draw_lines(["What must close before", "the next physician review"], 734, 506, 28, "#f5f7ff", "700", "Georgia, serif")}
    {bullet_list([
      "The neurologic intake packet still lacks a final onset chronology and aligned seriousness summary.",
      "The hepatic packet still depends on a missing literature reconciliation and final narrative revision.",
      "Two pediatric follow-up narratives still need outcome confirmation before the next review cycle."
    ], 744, 580)}
    """,
)

lane = shell(
    "Narrative Lane",
    "Each lane keeps owner, clinical focus, and next action visible.",
    "Narrative intake, medical review, follow-up, and submission lanes stay separated cleanly so clinical review does not collapse into one noisy queue.",
    f"""
    {panel(58, 238, 620, 250, "Narrative intake lane", "Chronology and reporter coherence", "Reporter framing, event timing, suspect product attribution, and readable chronology stay grouped under Case Processing ownership.", "#19c7ff")}
    {panel(708, 238, 634, 250, "Medical review lane", "Seriousness and physician addenda", "Seriousness summaries, medical-significance rationale, and physician narrative quality stay visible before another review cycle.", "#ffcc66")}
    {panel(58, 520, 620, 256, "Follow-up lane", "Outcome closure and affiliate handoff timing", "Open follow-up questions stay tied to Patient Safety review pressure before the next escalation window.", "#b88cff")}
    {panel(708, 520, 634, 256, "Submission lane", "Outbound packet quality and reviewer signoff", "Narrative revisions, literature linkage, and named approval ownership provide the final outbound-safe gate.", "#37ff8b")}
    """,
)

posture = shell(
    "Review Posture",
    "Packet readiness, missing evidence, and physician-review timing stay readable for biotech operators.",
    "The board keeps final clinical narrative posture explicit instead of hiding it behind generic safety status metrics.",
    f"""
    {panel(58, 238, 402, 244, "Packet CENB-12", "Neurologic intake packet", "56 percent ready. Case chronology is still missing the final onset clarification and seriousness alignment.", "#ff5c7a")}
    {panel(498, 238, 402, 244, "Packet CENB-18", "Hepatic submission packet", "61 percent ready. The packet still lacks the reconciled literature citation and final narrative revision.", "#ffcc66")}
    {panel(938, 238, 404, 244, "Packet CENB-31", "Safety physician packet", "72 percent ready. The physician addendum still needs the explicit seizure chronology and escalation rationale.", "#37ff8b")}
    {panel(58, 514, 1284, 262, "Why this monetizes cleanly", "Hosted preview planned, paid template pack later, embedded by engagement.", "This is a strong biotech review wedge because it lives where teams actually feel pressure: chronology quality, seriousness framing, follow-up closure, physician addenda, and the named narrative action needed before outbound review.", "#19c7ff")}
    """,
)

(SHOT_DIR / "01-overview.svg").write_text(overview, encoding="utf-8")
(SHOT_DIR / "02-narrative-lane.svg").write_text(lane, encoding="utf-8")
(SHOT_DIR / "03-review-posture.svg").write_text(posture, encoding="utf-8")
