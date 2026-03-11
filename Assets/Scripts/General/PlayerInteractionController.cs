using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private CameraController PlayerCam;
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
        TouchscreenHandler.Instance.FingerUpCallback += SelectGrid;
    }

    private void SelectGrid(object sender, TouchInfo e)
    {
        if (PlayerCam.CameraMoving) return;
        if (CurrentGridHex != null)
        {
            CurrentGridHex.GridHexVisual.OnSelected();
            CurrentGridHex = null;
        }
        Ray ray = cameraInstance.ScreenPointToRay(e.ScreenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var grid = GameManager.Instance.MapGrid.Grid.GetGridObject(hit.point);
            if (grid != null)
            {
                grid.GridHexVisual.OnSelected();
                CurrentGridHex = grid;

                var planetData = (PlanetData)grid.Occupant;

                if (planetData != null)
                {
                    CurrentPlanet = planetData;

                    if (GameManager.Instance.Player.OwnedPlanets.Contains(planetData))
                    {
                        UINavigationManager.Instance.ShowFriendlyPlanetSheet(planetData);
                    } else if (GameManager.Instance.Player.DiscoveredPlanets.Contains(planetData))
                    {
                        UINavigationManager.Instance.ShowEnemyPlanetSheet(planetData);
                    } else
                    {
                        UINavigationManager.Instance.ShowUnknownPlanetSheet(planetData);
                    }

                    cameraInstance.transform.position = new(grid.WorldPosition.x, 55, grid.WorldPosition.z);
                    PlayerCam.Disable();
                } else
                {
                    PointerEventData pointerData = new(EventSystem.current);
                    pointerData.position = e.ScreenPos;
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerData, results);
                    if (results.Count <= 0)
                    {
                        UINavigationManager.Instance.DismissAllSheets();
                        PlayerCam.Enable();
                    }
                    
                }
            }
        }
    }

    private void FindPlanet(object sender, TouchInfo touchInfo)
    {
        
        
    }

}


/*
 
 */