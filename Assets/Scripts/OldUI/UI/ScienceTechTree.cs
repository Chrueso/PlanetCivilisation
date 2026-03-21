using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScienceTechTree : MonoBehaviour
{
    public enum NodeState { Locked, Available, Researched }

    [System.Serializable]
    public class TechNodeData
    {
        public string id;
        public string displayName;
        public int row;
        public int col; // 0=left, 1=center, 2=right
        public string[] parentIds;
        public NodeState state;
    }

    [Header("Layout")]
    [SerializeField] private float nodeSize = 150f;
    [SerializeField] private float topPadding = 40f;
    [SerializeField] private float lineThickness = 5f;
    [SerializeField] private int numColumns = 3;

    private float contentWidth;
    private float rowSpacing;

    [Header("Colors")]
    [SerializeField] private Color lockedColor = new Color(0.149f, 0.149f, 0.239f, 1f);       // #26263D
    [SerializeField] private Color availableColor = new Color(0.522f, 0.522f, 0.627f, 1f);     // #8585A0
    [SerializeField] private Color researchedColor = new Color(0.75f, 0.75f, 0.82f, 1f);       // light node fill
    [SerializeField] private Color researchedOutline = new Color(0f, 0.8f, 0.9f, 1f);          // #00CCE6 cyan glow
    [SerializeField] private Color activeLineColor = new Color(0.2f, 0.5f, 0.95f, 1f);         // blue
    [SerializeField] private Color inactiveLineColor = new Color(0.35f, 0.35f, 0.3f, 1f);      // dark olive

    private Dictionary<string, RectTransform> nodeRects = new Dictionary<string, RectTransform>();
    private Dictionary<string, Image> nodeImages = new Dictionary<string, Image>();
    private Dictionary<string, TechNodeData> nodeDataMap = new Dictionary<string, TechNodeData>();
    private RectTransform contentRect;

    // 3-column layout matching reference image #14
    private static readonly TechNodeData[] defaultNodes = new TechNodeData[]
    {
        // Row 0 — single root, centered, researched (cyan glow)
        new TechNodeData { id = "core_tech", displayName = "", row = 0, col = 1,
            parentIds = new string[0], state = NodeState.Researched },

        // Row 1 — 3 nodes
        new TechNodeData { id = "mining", displayName = "", row = 1, col = 0,
            parentIds = new[] { "core_tech" }, state = NodeState.Locked },
        new TechNodeData { id = "engineering", displayName = "", row = 1, col = 1,
            parentIds = new[] { "core_tech" }, state = NodeState.Available },
        new TechNodeData { id = "biology", displayName = "", row = 1, col = 2,
            parentIds = new[] { "core_tech" }, state = NodeState.Available },

        // Row 2 — 2 nodes
        new TechNodeData { id = "chemistry", displayName = "", row = 2, col = 1,
            parentIds = new[] { "engineering" }, state = NodeState.Available },
        new TechNodeData { id = "genetics", displayName = "", row = 2, col = 2,
            parentIds = new[] { "biology" }, state = NodeState.Available },

        // Row 3 — 3 nodes
        new TechNodeData { id = "alloys", displayName = "", row = 3, col = 0,
            parentIds = new[] { "mining" }, state = NodeState.Locked },
        new TechNodeData { id = "propulsion", displayName = "", row = 3, col = 1,
            parentIds = new[] { "chemistry" }, state = NodeState.Locked },
        new TechNodeData { id = "terraforming", displayName = "", row = 3, col = 2,
            parentIds = new[] { "chemistry", "genetics" }, state = NodeState.Locked },

        // Row 4 — 2 nodes
        new TechNodeData { id = "weapons", displayName = "", row = 4, col = 0,
            parentIds = new[] { "alloys" }, state = NodeState.Locked },
        new TechNodeData { id = "shields", displayName = "", row = 4, col = 2,
            parentIds = new[] { "terraforming" }, state = NodeState.Locked },

        // Row 5 — 1 node (bottom)
        new TechNodeData { id = "starships", displayName = "", row = 5, col = 1,
            parentIds = new[] { "weapons", "propulsion" }, state = NodeState.Locked },
    };

    public void Init()
    {
        contentRect = GetComponent<RectTransform>();
        Canvas.ForceUpdateCanvases();

        contentWidth = contentRect.rect.width;
        if (contentWidth <= 0f)
            contentWidth = 1080f;

        // Get visible height from viewport parent
        float visibleHeight = 1670f; // fallback (1920 - ~250 header)
        if (contentRect.parent != null)
        {
            var parentRect = contentRect.parent as RectTransform;
            if (parentRect != null && parentRect.rect.height > 0f)
                visibleHeight = parentRect.rect.height;
        }

        BuildTree(defaultNodes, visibleHeight);
    }

    public void BuildTree(TechNodeData[] nodes, float visibleHeight)
    {
        ClearTree();

        foreach (var data in nodes)
            nodeDataMap[data.id] = data;

        int maxRow = 0;
        foreach (var data in nodes)
            if (data.row > maxRow) maxRow = data.row;

        // Compute row spacing to fill visible area with bottom padding
        float bottomPadding = 300f;
        if (maxRow > 0)
            rowSpacing = (visibleHeight - topPadding - nodeSize - bottomPadding) / maxRow;
        else
            rowSpacing = 240f;

        float contentHeight = maxRow * rowSpacing + topPadding + nodeSize + 60f;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);

        // Connection lines first (behind nodes)
        foreach (var data in nodes)
        {
            if (data.parentIds == null) continue;
            foreach (string parentId in data.parentIds)
            {
                if (!nodeDataMap.ContainsKey(parentId)) continue;
                CreateOrthogonalLine(nodeDataMap[parentId], data);
            }
        }

        // Nodes on top
        foreach (var data in nodes)
            CreateNode(data);
    }

    private Vector2 GetNodeCenter(TechNodeData data)
    {
        // Divide width into equal columns, place node at column center
        float colWidth = contentWidth / numColumns;
        float x = colWidth * data.col + colWidth * 0.5f;
        float y = -(data.row * rowSpacing + topPadding + nodeSize * 0.5f);
        return new Vector2(x, y);
    }

    private void CreateNode(TechNodeData data)
    {
        GameObject nodeGO = new GameObject(data.id, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        nodeGO.transform.SetParent(contentRect, false);

        RectTransform rt = nodeGO.GetComponent<RectTransform>();
        Vector2 center = GetNodeCenter(data);
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.anchoredPosition = center;
        rt.sizeDelta = new Vector2(nodeSize, nodeSize);

        Image img = nodeGO.GetComponent<Image>();
        img.color = GetNodeColor(data.state);

        Button btn = nodeGO.GetComponent<Button>();
        string nodeId = data.id;
        btn.onClick.AddListener(() => OnNodeClicked(nodeId));

        // Cyan outline for researched nodes
        if (data.state == NodeState.Researched)
        {
            var outline = nodeGO.AddComponent<Outline>();
            outline.effectColor = researchedOutline;
            outline.effectDistance = new Vector2(4, 4);
        }

        nodeRects[data.id] = rt;
        nodeImages[data.id] = img;
    }

    /// <summary>
    /// Draws an orthogonal (L-shaped) connection: vertical from parent down, then horizontal to child.
    /// </summary>
    private void CreateOrthogonalLine(TechNodeData from, TechNodeData to)
    {
        Vector2 start = GetNodeCenter(from);
        Vector2 end = GetNodeCenter(to);

        // Start at bottom edge of parent, end at top edge of child
        start.y -= nodeSize * 0.5f;
        end.y += nodeSize * 0.5f;

        bool isActive = from.state == NodeState.Researched;
        Color lineColor = isActive ? activeLineColor : inactiveLineColor;

        if (Mathf.Abs(start.x - end.x) < 1f)
        {
            // Straight vertical line
            CreateLineSegment(start, end, lineColor);
        }
        else
        {
            // L-shape: vertical to midpoint Y, then horizontal to end X, then vertical to end
            float midY = (start.y + end.y) * 0.5f;
            Vector2 corner1 = new Vector2(start.x, midY);
            Vector2 corner2 = new Vector2(end.x, midY);

            CreateLineSegment(start, corner1, lineColor);
            CreateLineSegment(corner1, corner2, lineColor);
            CreateLineSegment(corner2, end, lineColor);
        }
    }

    private void CreateLineSegment(Vector2 a, Vector2 b, Color color)
    {
        GameObject lineGO = new GameObject("line", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        lineGO.transform.SetParent(contentRect, false);
        lineGO.transform.SetAsFirstSibling();

        RectTransform rt = lineGO.GetComponent<RectTransform>();
        Image img = lineGO.GetComponent<Image>();

        Vector2 mid = (a + b) * 0.5f;
        float dist = Vector2.Distance(a, b);
        float angle = Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;

        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.anchoredPosition = mid;
        rt.sizeDelta = new Vector2(dist, lineThickness);
        rt.localRotation = Quaternion.Euler(0, 0, angle);

        img.color = color;
    }

    private Color GetNodeColor(NodeState state)
    {
        switch (state)
        {
            case NodeState.Locked: return lockedColor;
            case NodeState.Available: return availableColor;
            case NodeState.Researched: return researchedColor;
            default: return lockedColor;
        }
    }

    public void OnNodeClicked(string nodeId)
    {
        if (!nodeDataMap.ContainsKey(nodeId)) return;
        Debug.Log($"[ScienceTechTree] Clicked: {nodeId} ({nodeDataMap[nodeId].state})");
    }

    public void SetNodeState(string nodeId, NodeState state)
    {
        if (!nodeDataMap.ContainsKey(nodeId)) return;
        nodeDataMap[nodeId].state = state;
        if (nodeImages.ContainsKey(nodeId))
            nodeImages[nodeId].color = GetNodeColor(state);
    }

    private void ClearTree()
    {
        foreach (Transform child in contentRect)
            Destroy(child.gameObject);
        nodeRects.Clear();
        nodeImages.Clear();
        nodeDataMap.Clear();
    }
}
