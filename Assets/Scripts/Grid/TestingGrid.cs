using UnityEngine;

public class TestingGrid : MonoBehaviour
{
    private Camera cameraInstance;
    private float fingerRayMaxDistance = 1000f;
    Grid grid;
    BoxCollider col;

    void Start()
    {
        cameraInstance = Camera.main;
        TouchscreenHandler.Instance.FingerDownCallback += OnTouch;
        grid = new Grid(3, 3, 10f);
        col = GetComponent<BoxCollider>();
        col.size = new Vector3(grid.TotalWidth, grid.TotalHeight);
        col.center = new Vector3(grid.TotalWidth, grid.TotalHeight) / 2f;
    }

    void OnTouch(object sender, TouchInfo touchInfo)
    {
        if (touchInfo.Index != 0) return;
        Ray fingerRay = cameraInstance.ScreenPointToRay(touchInfo.ScreenPos);
        RaycastHit hit;

        if (Physics.Raycast(fingerRay, out hit, fingerRayMaxDistance))
        {
            Debug.Log(hit.point);
            Debug.Log(grid.GetPositionOnGrid(hit.point));
        }
    }


}
