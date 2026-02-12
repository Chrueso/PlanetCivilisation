using System;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private Camera cameraInstance;
    private bool inPlanet = false;
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
            string name = hit.collider.gameObject.name;
            if (CheckPlanet(name))
            print(hit.collider.gameObject.name);

            // i changed so it detect isit planet instead of check name of object (eexuan
            PlanetData planet = hit.collider.GetComponent<PlanetData>();

            if (planet != null)
            {
                PlanetManagementInterface.main.ShowInterface(planet);
                CameraController.main.Disable();
                cameraInstance.transform.position = hit.collider.gameObject.transform.position - (cameraInstance.transform.forward * 2f);
                inPlanet = true;
                
            } else
            {   // YEAH THIS IS PRETTY FUCKING SHIT :P
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && inPlanet)
                {
                    PlanetManagementInterface.main.HideInterface();
                    CameraController.main.Enable();
                    inPlanet = false;
                }
            }
        } else
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && inPlanet)
            {
                PlanetManagementInterface.main.HideInterface();
                CameraController.main.Enable();
                inPlanet = false;
            }
        }
    }

    private bool CheckPlanet(string planetName)
    {
        return PlanetMasterScript.main.Planets.ContainsKey(planetName);
    }

    private PlanetData GetPlanet(string planetName)
    {
        PlanetMasterScript.main.Planets.TryGetValue(planetName, out var result);
        return result;
    }

}
