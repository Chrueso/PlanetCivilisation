using System;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private Camera cameraInstance;
    private bool inPlanet = false;
    private float offset = 2f;
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
        
        //When tap
        if (Physics.Raycast(fingerRay, out hit, 1000f))
        {
            string name = hit.collider.gameObject.name;
            if (CheckPlanet(name))
            print(hit.collider.gameObject.name);

            // i changed so it detect isit planet instead of check name of object (eexuan
            //PlanetData planet = hit.collider.GetComponent<PlanetData>();

            if (GetPlanet(name) != null)
            {
                //PlanetManagementInterface.main.ShowInterface(GetPlanet(name));
                CameraController.main.Disable();
                cameraInstance.transform.position = hit.collider.gameObject.transform.position - (cameraInstance.transform.forward * 4f) - new Vector3(0,offset,0);
                inPlanet = true;
                
            } else
            {   // YEAH THIS IS PRETTY FUCKING SHIT :P
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && inPlanet)
                {
                    //PlanetManagementInterface.main.HideInterface();
                    CameraController.main.Enable();
                    cameraInstance.transform.position = CameraController.main.CurrPos;
                    inPlanet = false;
                }
            }
        } else
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && inPlanet)
            {
                //PlanetManagementInterface.main.HideInterface();
                CameraController.main.Enable();
                cameraInstance.transform.position = CameraController.main.CurrPos;
                inPlanet = false;
            }
        }
    }

    private bool CheckPlanet(string planetName)
    {
        return PlanetManager.main.Planets.ContainsKey(planetName);
    }

    private PlanetData GetPlanet(string planetName)
    {
        PlanetManager.main.Planets.TryGetValue(planetName, out var result);
        return result;
    }

}
