using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Bends a TextMeshPro object's mesh so its characters flow along an arbitrary
/// <see cref="SplineContainer"/>. Because the shape is a spline (not a function),
/// it supports loops, spirals and any self-overlapping path.
///
/// Each glyph is placed by arc-length distance along the spline, so spacing stays
/// even regardless of how the knots are distributed, and rotated to match the tangent.
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(TMP_Text))]
public class TextAlongSpline : MonoBehaviour
{
    [Tooltip("The spline the text should follow. Draw it via GameObject > Spline > Draw Spline.")]
    [SerializeField] private SplineContainer spline;

    [Tooltip("Distance (in text-local units) along the path at which the first character starts.")]
    [SerializeField] private float startOffset = 0f;

    [Tooltip("Multiplier on each glyph's advance. 1 keeps native spacing; >1 spreads letters out.")]
    [SerializeField] private float spacing = 1f;

    [Tooltip("Offset applied perpendicular to the path, letting the baseline sit above/below the spline.")]
    [SerializeField] private float verticalOffset = 0f;

    [Tooltip("If the text is longer than the spline, it wraps onto extra lines that follow the same spline, stacked below.")]
    [SerializeField] private bool wrap = true;

    [Tooltip("Multiplier on the gap between wrapped lines (1 = the font's natural line height).")]
    [SerializeField] private float lineSpacing = 1f;

    [Tooltip("Recompute every frame. Enable while authoring; disable and call Refresh() manually in play mode for performance.")]
    [SerializeField] private bool updateEveryFrame = true;

    private TMP_Text tmp;

    private void OnEnable()
    {
        tmp = GetComponent<TMP_Text>();
        Refresh();
    }

    private void OnValidate()
    {
        if (spacing <= 0f) spacing = 0.0001f;
    }

    private void Update()
    {
        if (updateEveryFrame) Refresh();
    }

    // Which wrapped line a given path distance falls on. With wrap off, everything stays on
    // the first line (and the tail clamps at the end of the spline).
    private int LineOf(float distance, float splineLength)
    {
        if (!wrap || distance <= 0f) return 0;
        return Mathf.FloorToInt(distance / splineLength);
    }

    /// <summary>Rebuilds the warped mesh. Call this whenever the text or spline changes.</summary>
    public void Refresh()
    {
        if (tmp == null) tmp = GetComponent<TMP_Text>();
        if (tmp == null || spline == null) return;

        // We lay the whole string out ourselves along the path, so TMP must keep it on a
        // single line — its own wrapping would otherwise break our horizontal mapping.
        if (tmp.textWrappingMode != TextWrappingModes.NoWrap)
            tmp.textWrappingMode = TextWrappingModes.NoWrap;

        tmp.ForceMeshUpdate();
        TMP_TextInfo textInfo = tmp.textInfo;
        int characterCount = textInfo.characterCount;
        if (characterCount == 0) return;

        float splineLength = spline.CalculateLength();
        if (splineLength <= 0f) return;

        // Find the left edge of the text so distances are measured from the start of the
        // string, independent of the TMP alignment/anchor. With startOffset = 0 this puts
        // the first character at the very beginning of the curve and lays the rest forward.
        float originX = float.MaxValue;
        for (int i = 0; i < characterCount; i++)
        {
            TMP_CharacterInfo c = textInfo.characterInfo[i];
            if (!c.isVisible) continue;
            float leftEdge = textInfo.meshInfo[c.materialReferenceIndex].vertices[c.vertexIndex].x;
            if (leftEdge < originX) originX = leftEdge;
        }
        if (originX == float.MaxValue) return;

        // Height of one line, used to stack wrapped lines below the first.
        float lineHeight = textInfo.lineCount > 0 ? textInfo.lineInfo[0].lineHeight : tmp.fontSize;

        // Precompute each glyph's distance along the path (relative to the text's left edge).
        float[] charDistance = new float[characterCount];
        for (int i = 0; i < characterCount; i++)
        {
            TMP_CharacterInfo c = textInfo.characterInfo[i];
            if (!c.isVisible) continue;
            Vector3[] v = textInfo.meshInfo[c.materialReferenceIndex].vertices;
            float mx = (v[c.vertexIndex].x + v[c.vertexIndex + 2].x) * 0.5f;
            charDistance[i] = (mx - originX) * spacing;
        }

        // Word-aware wrapping: shift whole words forward so a word never straddles the end
        // of the spline. A word that is itself longer than the spline is left to flow (it
        // can't fit on one line anyway). The accumulated pad rolls forward through the string.
        if (wrap)
        {
            float pad = 0f;
            int idx = 0;
            while (idx < characterCount)
            {
                if (char.IsWhiteSpace(textInfo.characterInfo[idx].character)) { idx++; continue; }

                int start = idx, end = idx, firstVisible = -1, lastVisible = -1;
                while (end < characterCount && !char.IsWhiteSpace(textInfo.characterInfo[end].character))
                {
                    if (textInfo.characterInfo[end].isVisible)
                    {
                        if (firstVisible < 0) firstVisible = end;
                        lastVisible = end;
                    }
                    end++;
                }

                if (firstVisible >= 0)
                {
                    float wordStart = charDistance[firstVisible] + pad;
                    float wordEnd = charDistance[lastVisible] + pad;
                    bool straddles = Mathf.Floor(wordStart / splineLength) != Mathf.Floor(wordEnd / splineLength);
                    if (straddles && (wordEnd - wordStart) < splineLength)
                    {
                        float nextLine = (Mathf.Floor(wordStart / splineLength) + 1f) * splineLength;
                        pad += nextLine - wordStart;
                    }
                }

                for (int k = start; k < end; k++) charDistance[k] += pad;
                idx = end;
            }
        }

        // Per-line horizontal alignment. Measure each line's content extent (from the left
        // edge of its first glyph to the right edge of its last), then shift the whole line
        // within [0, splineLength] so it flushes left, centers, or flushes right to match
        // the TMP_Text's own horizontal alignment.
        int lineCount = 1;
        for (int i = 0; i < characterCount; i++)
            if (textInfo.characterInfo[i].isVisible)
                lineCount = Mathf.Max(lineCount, LineOf(charDistance[i], splineLength) + 1);

        float[] lineMinEdge = new float[lineCount];
        float[] lineMaxEdge = new float[lineCount];
        for (int l = 0; l < lineCount; l++) { lineMinEdge[l] = float.MaxValue; lineMaxEdge[l] = float.MinValue; }

        for (int i = 0; i < characterCount; i++)
        {
            TMP_CharacterInfo c = textInfo.characterInfo[i];
            if (!c.isVisible) continue;
            Vector3[] v = textInfo.meshInfo[c.materialReferenceIndex].vertices;
            float halfWidth = (v[c.vertexIndex + 2].x - v[c.vertexIndex].x) * 0.5f * spacing;

            int line = LineOf(charDistance[i], splineLength);
            float inLine = charDistance[i] - line * splineLength;
            lineMinEdge[line] = Mathf.Min(lineMinEdge[line], inLine - halfWidth);
            lineMaxEdge[line] = Mathf.Max(lineMaxEdge[line], inLine + halfWidth);
        }

        HorizontalAlignmentOptions align = tmp.horizontalAlignment;
        float[] lineAlignShift = new float[lineCount];
        for (int l = 0; l < lineCount; l++)
        {
            if (lineMinEdge[l] > lineMaxEdge[l]) continue; // empty line
            float contentWidth = lineMaxEdge[l] - lineMinEdge[l];
            switch (align)
            {
                case HorizontalAlignmentOptions.Center:
                    lineAlignShift[l] = (splineLength - contentWidth) * 0.5f - lineMinEdge[l];
                    break;
                case HorizontalAlignmentOptions.Right:
                    lineAlignShift[l] = splineLength - lineMaxEdge[l];
                    break;
                default: // Left / Justified / Flush / Geometry -> flush to start
                    lineAlignShift[l] = -lineMinEdge[l];
                    break;
            }
        }

        for (int i = 0; i < characterCount; i++)
        {
            TMP_CharacterInfo ci = textInfo.characterInfo[i];
            if (!ci.isVisible) continue;

            int matIndex = ci.materialReferenceIndex;
            int vertexIndex = ci.vertexIndex;
            Vector3[] verts = textInfo.meshInfo[matIndex].vertices;

            // Pivot the glyph on the baseline, not on its own center: X is the glyph's
            // horizontal midpoint (drives its position along the path), Y is the shared
            // baseline so each glyph keeps its true height above/below it (e.g. a comma
            // hangs at the bottom, an apostrophe rides at the top).
            float midX = (verts[vertexIndex].x + verts[vertexIndex + 2].x) * 0.5f;
            Vector3 basePoint = new Vector3(midX, ci.baseLine, 0f);

            // Split into line + position-within-line, apply that line's alignment shift,
            // then the global startOffset nudge along the path.
            int lineIndex = LineOf(charDistance[i], splineLength);
            float distance = charDistance[i] - lineIndex * splineLength + lineAlignShift[lineIndex] + startOffset;
            float t = Mathf.Clamp01(distance / splineLength);

            // Evaluate the spline (world space), then bring it into this text's local space
            // so the modified vertices render correctly wherever the spline object sits.
            spline.Evaluate(t, out float3 worldPos, out float3 worldTangent, out float3 worldUp);

            Vector3 localPos = transform.InverseTransformPoint(worldPos);
            Vector3 localTangent = transform.InverseTransformDirection(worldTangent);

            float angle = Mathf.Atan2(localTangent.y, localTangent.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0f, 0f, angle);

            // Preserve each glyph's height above the baseline via (orig - charMid),
            // then apply the perpendicular verticalOffset in the rotated frame.
            // Push wrapped lines downward (perpendicular to the path), so they follow the spline too.
            float lineY = verticalOffset - lineIndex * lineHeight * lineSpacing;
            Vector3 anchor = localPos + rot * new Vector3(0f, lineY, 0f);
            for (int v = 0; v < 4; v++)
            {
                Vector3 offset = verts[vertexIndex + v] - basePoint;
                verts[vertexIndex + v] = anchor + rot * offset;
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            tmp.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
