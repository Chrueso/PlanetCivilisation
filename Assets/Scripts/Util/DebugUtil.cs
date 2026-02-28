using TMPro;
using UnityEngine;

public static class DebugUtil 
{
    public const int sortingOrderDefault = 5000;

    // Create Text in the World Parameters Version so u can do like CreateWorldText("Hello");
    public static TextMeshPro CreateWorldText(
        string text,
        Transform parent = null,
        Vector3 localPosition = default,
        int fontSize = 36,
        Color? color = null,
        TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft,
        int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;

        return CreateWorldText(parent, text, localPosition, fontSize,
            (Color)color, alignment, sortingOrder);
    }

    // Create Text in the World
    public static TextMeshPro CreateWorldText(
        Transform parent,
        string text,
        Vector3 localPosition,
        int fontSize,
        Color color,
        TextAlignmentOptions alignment,
        int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMeshPro));
        Transform transform = gameObject.transform;

        transform.SetParent(parent, false);
        transform.localPosition = localPosition;

        TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
        textMeshPro.text = text;
        textMeshPro.fontSize = fontSize;
        textMeshPro.color = color;
        textMeshPro.alignment = alignment;

        textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        return textMeshPro;
    }
}
