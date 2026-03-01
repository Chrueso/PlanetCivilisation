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
        TouchscreenHandler.Instance.FingerDownCallback += FindPlanet;
    }

    private void FindPlanet(object sender, TouchInfo touchInfo)
    {
        if (touchInfo.Index != 0) return;
        Ray fingerRay = cameraInstance.ScreenPointToRay(touchInfo.ScreenPos);
        RaycastHit hit;
        
        //When tap
        if (Physics.Raycast(fingerRay, out hit, 1000f))
        {
            string name = hit.collider.gameObject.name;
            if (CheckPlanet(name))
            print(hit.collider.gameObject.name);

            // i changed so it detect isit planet instead of check name of object (eexuan
            //PlanetData planet = hit.collider.GetComponent<PlanetData>();

            PlanetData planet = GetPlanet(name);
            if (planet != null)
            {
                //PlanetManagementInterface.main.ShowInterface(GetPlanet(name));
                if (planet.FactionType == FactionType.Human)
                    UINavigationManager.Instance.ShowFriendlyPlanetSheet(planet);
                else
                    UINavigationManager.Instance.ShowEnemyPlanetSheet(planet);
                CameraController.Instance.Disable();
                cameraInstance.transform.position = hit.collider.gameObject.transform.position - (cameraInstance.transform.forward * 4f) - new Vector3(0,offset,0);
                inPlanet = true;
                
            } else
            {   // YEAH THIS IS PRETTY FUCKING SHIT :P
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && inPlanet)
                {
                    //PlanetManagementInterface.main.HideInterface();
                    UINavigationManager.Instance.DismissAllSheets();
                    CameraController.Instance.Enable();
                    cameraInstance.transform.position = CameraController.Instance.CurrPos;
                    inPlanet = false;
                }
            }
        } else
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && inPlanet)
            {
                //PlanetManagementInterface.main.HideInterface();
                CameraController.Instance.Enable();
                cameraInstance.transform.position = CameraController.Instance.CurrPos;
                inPlanet = false;
            }
        }
    }

    private bool CheckPlanet(string planetName)
    {
        return PlanetManager.Instance.PlanetDict.ContainsKey(planetName);
    }

    private PlanetData GetPlanet(string planetName)
    {
        PlanetManager.Instance.PlanetDict.TryGetValue(planetName, out var result);
        return result;
    }

}
