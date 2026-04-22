using UnityEngine;

public class ObjIconScreenshotter 
{
    private Camera screenshotCamera;
    private RenderTexture renderTexture;

    public ObjIconScreenshotter(Camera screenshotCamera, RenderTexture renderTexture)
    {
        this.screenshotCamera = screenshotCamera;
        this.renderTexture = renderTexture;
        screenshotCamera.targetTexture = renderTexture;
    }

    public Texture2D Screenshot(GameObject obj)
    {
        Vector3 oriPos = obj.transform.position;
        int oriLayer = obj.layer;
        int screenshotLayer = LayerMask.NameToLayer("Screenshot");

        foreach (Transform t in obj.GetComponentsInChildren<Transform>())
            t.gameObject.layer = screenshotLayer;
        obj.layer = screenshotLayer;
        obj.transform.position = new Vector3(screenshotCamera.transform.position.x, oriPos.y, screenshotCamera.transform.position.z);
        screenshotCamera.Render();

        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenshot.Apply();
        RenderTexture.active = null;

        foreach (Transform t in obj.GetComponentsInChildren<Transform>())
            t.gameObject.layer = oriLayer;
        obj.layer = oriLayer;
        obj.transform.position = oriPos;
        Debug.Log($"For some reason no image: {screenshot}");
        return screenshot;
    }
}
