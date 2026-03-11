using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractionController : Singleton<PlayerInteractionController>
{
    [SerializeField] UINavigationManager uINavigationManager;
    [SerializeField] private CameraController cameraController;

    private Camera cameraInstance;
    public bool inPlanet { get; set; }
    private bool homeShipOnPlanet = true;
    private float offset = 2f;

    public PlanetData CurrentPlanet { get; private set; }
    public GridHex CurrentGridHex { get; private set; }

    private void Start()
    {
        cameraInstance = Camera.main;
        TouchscreenHandler.FingerDownCallback += FindPlanet;
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
                uINavigationManager.SetHomeShipButton(true);
                uINavigationManager.MoveHomeShipButton(gh.WorldPosition);
            }
            print($"In planet {inPlanet}");
            if (gh.IsOccupied)
            {
                PlanetData pd = (PlanetData)gh.Occupant;
                CurrentPlanet = pd;
                if (pd != null && !inPlanet)
                {
                    cameraController.Disable();
                    cameraInstance.transform.position = new Vector3(gh.WorldPosition.x, 55, gh.WorldPosition.z);
                    if (GameManager.Instance.Player.DiscoveredPlanets.Contains(pd) && playerGrid==gh)
                    {
                        if (pd.FactionType == FactionType.Human)
                        {
                            uINavigationManager.ShowFriendlyPlanetSheet(pd);
                        } else
                        {
                            uINavigationManager.ShowEnemyPlanetSheet(pd);    
                        }
                        inPlanet = true;
                    } 
                    else if(!GameManager.Instance.Player.DiscoveredPlanets.Contains(pd))
                    {
                        uINavigationManager.ShowUnknownPlanetSheet(pd);
                        inPlanet = true;
                    }
                    
                }



                //NOT OCCUPIED
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
                            cameraController.Enable();
                            uINavigationManager.DismissAllSheets();
                            inPlanet = false;
                        }
                    }
                } else
                {
                    cameraController.Enable();
                    uINavigationManager.DismissAllSheets();
                    inPlanet = false;
                }
                
            }
        } 



        //IF NO RAYCAAST
        //UI ???
        //CLOSE UI
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
                        cameraController.Enable();
                        uINavigationManager.DismissAllSheets(); //CLOSE UI
                        inPlanet = false;
                    }
                }
            }
            else
            {
                cameraController.Enable();
                uINavigationManager.DismissAllSheets();
                inPlanet = false;
            }
        }
        
    }

    //private bool CheckPlanet(string planetName)
    //{
    //    return PlanetMapGenerator.Instance.PlanetDict.ContainsKey(planetName);
    //}

    //private PlanetData GetPlanet(string planetName)
    //{
    //    PlanetMapGenerator.Instance.PlanetDict.TryGetValue(planetName, out var result);
    //    return result;
    //}

}
