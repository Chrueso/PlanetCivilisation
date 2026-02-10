using System;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private Camera cameraInstance;
    private void Start()
    {
        cameraInstance = Camera.main;
        TouchscreenHandler.main.FingerDownCallback += FindPlanet;
    }

    private void FindPlanet(object sender, TouchInfo e)
    {
        if (e.Index != 0) return;
        Ray fingerRay = cameraInstance.ScreenPointToRay(e.Pos);
        RaycastHit hit;
        if (Physics.Raycast(fingerRay, out hit, 1000f))
        {
            print(hit.collider.gameObject.name);
            if (hit.collider.gameObject.name == "Sphere")
            {
                PlanetManagementInterface.main.ShowInterface();
                CameraController.main.Disable();
                cameraInstance.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, cameraInstance.transform.position.y, hit.collider.gameObject.transform.position.z);
                
                
            } else
            {
                
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    PlanetManagementInterface.main.HideInterface();
                    CameraController.main.Enable();
                }
            }
        }
    }
}
