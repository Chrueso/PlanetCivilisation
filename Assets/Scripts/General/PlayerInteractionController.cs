using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private Camera cameraInstance;
    private bool inPlanet = false;
    private float offset = 2f;

    // hardcode
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

            PlanetData planet = GetPlanet(name);
            if (planet != null)
            {
                if (true)
                    UINavigationManager.Instance.ShowFriendlyPlanetSheet(new PlanetData("F", new List<ResourceType>(), FactionType.DemiHuman));
                else
                    UINavigationManager.Instance.ShowEnemyPlanetSheet(new PlanetData("F", new List<ResourceType>(), FactionType.DemiHuman));
                CameraController.main.Disable();
                Vector3 planetPos = hit.collider.gameObject.transform.position;
                cameraInstance.transform.position = new Vector3(planetPos.x, 30, planetPos.z);
                inPlanet = true;
                
            } else
            {   // YEAH THIS IS PRETTY FUCKING SHIT :P
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && inPlanet)
                {
                    UINavigationManager.Instance.DismissAllSheets();
                    CameraController.main.Enable();
                    cameraInstance.transform.position = CameraController.main.CurrPos;
                    inPlanet = false;
                }
            }
        } else
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && inPlanet)
            {
                CameraController.main.Enable();
                cameraInstance.transform.position = CameraController.main.CurrPos;
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
