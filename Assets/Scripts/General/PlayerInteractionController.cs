using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : Singleton<PlayerInteractionController>
{
    private Camera cameraInstance;
    private bool inPlanet = false;
    private float offset = 2f;
    public PlanetData CurrentPlanet { get; private set; }
    public GameObject PlanetObject { get; private set; }

    // hardcode
    private FactionType playerFaction = FactionType.Human;
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
                PlanetObject = hit.collider.gameObject;
                CurrentPlanet = planet;
                if (planet.FactionType == playerFaction)
                    UINavigationManager.Instance.ShowFriendlyPlanetSheet(planet);
                else
                    UINavigationManager.Instance.ShowEnemyPlanetSheet(planet);
                CameraController.Instance.Disable();
                Vector3 planetPos = hit.collider.gameObject.transform.position;
                cameraInstance.transform.position = new Vector3(planetPos.x, 30, planetPos.z - offset);
                inPlanet = true;
                print($"Planet Name: {planet.PlanetName} | Structure: {planet.Structures} | Faction: {planet.FactionType}");
                
            } else
            {   // YEAH THIS IS PRETTY FUCKING SHIT :P
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && inPlanet)
                {
                    PlanetObject = null;
                    CurrentPlanet = planet;
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
                PlanetObject = null;
                CurrentPlanet = null;
                UINavigationManager.Instance.DismissAllSheets();
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
