using System;
using UnityEngine;
using UnityEngine.Pool;

public class EntityScoutShipPool : MonoBehaviour
{
    [SerializeField] private int MaxScoutShips = 5;
    [SerializeField] private EntityScoutShipView scoutShipPrefab;
    private ObjectPool<EntityScoutShipView> scoutShipViewPool;

    private void Awake()
    {
        scoutShipViewPool = new ObjectPool<EntityScoutShipView>(
            CreateScoutShip,
            OnScoutShipRequested,
            OnScoutShipReturned,
            OnScoutShipDestroyed,
            true,
            3,
            MaxScoutShips
            );
    }

    private void OnScoutShipDestroyed(EntityScoutShipView view)
    {
        Destroy(view.gameObject);
    }

    private void OnScoutShipReturned(EntityScoutShipView view)
    {
        view.gameObject.SetActive(false);
    }

    private void OnScoutShipRequested(EntityScoutShipView view)
    {
        view.gameObject.SetActive(true);
    }

    private EntityScoutShipView CreateScoutShip()
    {
        EntityScoutShipView scoutShipInstance = Instantiate(scoutShipPrefab);
        scoutShipInstance.SetPool(scoutShipViewPool);
        return scoutShipInstance;
        
    }

    public EntityScoutShipView GetScoutShipInstance()
    {
        return scoutShipViewPool.Get();
    }
}
