using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;

public class PlayerInteractionController : Singleton<PlayerInteractionController>
{
    private Camera cameraInstance;
    public bool inPlanet { get; set; }
    private bool homeShipOnPlanet = true;
    private float offset = 2f;

    public PlanetData CurrentPlanet { get; private set; }
    public GridHex CurrentGridHex { get; private set; }
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
            GridHex gh = GameManager.Instance.MapGrid.Grid.GetGridObject(hit.point);
            if (gh == null) return;
            CurrentGridHex = gh;
            GridHex playerGrid = GameManager.Instance.Player.CurrentHex;
            if (!inPlanet)
            {
                UINavigationManager.Instance.SetHomeShipButton(true);
                UINavigationManager.Instance.MoveHomeShipButton(gh.WorldPosition);
            }
            print($"In planet {inPlanet}");
            if (gh.IsOccupied)
            {
                PlanetData pd = (PlanetData)gh.Occupant;
                CurrentPlanet = pd;
                if (pd != null && !inPlanet)
                {
                    CameraController.Instance.Disable();
                    cameraInstance.transform.position = new Vector3(gh.WorldPosition.x, 55, gh.WorldPosition.z);
                    if (GameManager.Instance.Player.DiscoveredPlanets.Contains(pd) && playerGrid==gh)
                    {
                        if (pd.FactionType == FactionType.Human)
                        {
                            UINavigationManager.Instance.ShowFriendlyPlanetSheet(pd);
                        } else
                        {
                            UINavigationManager.Instance.ShowEnemyPlanetSheet(pd);    
                        }
                        inPlanet = true;
                    } 
                    else if(!GameManager.Instance.Player.DiscoveredPlanets.Contains(pd))
                    {
                        UINavigationManager.Instance.ShowUnknownPlanetSheet(pd);
                        inPlanet = true;
                    }
                    
                }
            } else
            {
                PointerEventData pointerData = new(EventSystem.current);
                pointerData.position = touchInfo.ScreenPos;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);
                if (results.Count > 0)
                {
                    foreach (RaycastResult r in results)
                    {
                        print(r.gameObject.layer);
                        if (r.gameObject.layer != LayerMask.NameToLayer("UI"))
                        {
                            CameraController.Instance.Enable();
                            UINavigationManager.Instance.DismissAllSheets();
                            inPlanet = false;
                        }
                    }
                } else
                {
                    CameraController.Instance.Enable();
                    UINavigationManager.Instance.DismissAllSheets();
                    inPlanet = false;
                }
                
            }
        } 
        else
        {
            PointerEventData pointerData = new(EventSystem.current);
            pointerData.position = touchInfo.ScreenPos;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            if (results.Count > 0)
            {
                foreach (RaycastResult r in results)
                {
                    print(r.gameObject.layer);
                    if (r.gameObject.layer != LayerMask.NameToLayer("UI"))
                    {
                        CameraController.Instance.Enable();
                        UINavigationManager.Instance.DismissAllSheets();
                        inPlanet = false;
                    }
                }
            }
            else
            {
                CameraController.Instance.Enable();
                UINavigationManager.Instance.DismissAllSheets();
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
