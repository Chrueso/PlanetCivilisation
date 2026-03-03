using System;
using UnityEngine;

public class PlayerInteractionController : Singleton<PlayerInteractionController>
{
    private Camera cameraInstance;
    private bool inPlanet = false;
    private bool homeShipOnPlanet = true;
    private float offset = 2f;

    public PlanetData CurrentPlanet { get; private set; }
    public Vector3 currGrid;

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
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            //UINavigationManager.Instance.SetHomeShipButton(true);
            GridHex gh = GameManager.Instance.MapGrid.Grid.GetGridObject(hit.point);
            UINavigationManager.Instance.SetHomeShipButton(true);
            UINavigationManager.Instance.MoveHomeShipButton(gh.GridHexVisual.transform.position);
            if (gh.IsOccupied && !inPlanet)
            {
                PlanetData planet = (PlanetData)gh.Occupant;
                CurrentPlanet = planet;
                if (planet != null)
                {
                    if (planet.FactionType == FactionType.Human)
                        UINavigationManager.Instance.ShowFriendlyPlanetSheet(planet);
                    else
                        UINavigationManager.Instance.ShowEnemyPlanetSheet(planet);
                    CameraController.Instance.Disable();
                    cameraInstance.transform.position = new Vector3(gh.GridHexVisual.gameObject.transform.position.x, 35f, gh.GridHexVisual.gameObject.transform.position.z - offset);
                    inPlanet = true;
                }
            }
            else
            {   // YEAH THIS IS PRETTY FUCKING SHIT :P
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && inPlanet)
                {

                    CurrentPlanet = null;
                    UINavigationManager.Instance.DismissAllSheets();
                    CameraController.Instance.Enable();
                    cameraInstance.transform.position = CameraController.Instance.CurrPos;
                    inPlanet = false;
                }
            }
        }
        else
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && inPlanet)
            {
                CurrentPlanet = null;
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
