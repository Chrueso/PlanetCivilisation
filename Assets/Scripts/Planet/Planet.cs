using UnityEngine;

public class Planet : MonoBehaviour
{

    [Header("Planet Resources")]
    [SerializeField] private string resourceName = "Metal";
    [SerializeField] private int resource = 1;
    [SerializeField] private int structures = 1;

    [Header("Resource Generation")]
    [SerializeField] private float baseResourcePerSecond = 1f;
    [SerializeField] private float resourcePerStructure = 0.5f;
    [SerializeField] private bool autoGenerateResources = true;

    private float resourceAccumulator = 0f;
    public string ResourceName { get => resourceName; }
    public int Resource { get => resource; }
    public int Structures { get => structures; }
    public float ResourcePerTurn
    {
        get => baseResourcePerSecond + (structures * resourcePerStructure);
    }

    private void Update()
    {
        // Making it time based for now as a placeholder
        if (autoGenerateResources)
        {
            GenerateResources();
        }
    }

    private void GenerateResources()
    {
        // Making it time based for now as a placeholder
        resourceAccumulator += ResourcePerTurn * Time.deltaTime;

        while (resourceAccumulator >= 1f)
        {
            resource++;
            resourceAccumulator -= 1f;
        }
    }

    private void IncreaseResource(int amount = 1)
    {
        resource += amount;
        resource = Mathf.Max(0, resource);
    }

    private void DecreaseResource(int amount)
    {
        resource -= amount;
        resource = Mathf.Max(0, resource);
    }

    public void AddStructure()
    {
        structures++;
    }

    public void RemoveStructure()
    {
        structures--;
        structures = Mathf.Max(0, structures);
    }

    public void SetResources(int amount)
    {
        // idk debug or upgrade
        resource = Mathf.Max(0, amount);
    }

    public void SetStructures(int amount)
    {
        // idk debug or upgrade
        structures = Mathf.Max(0, amount);
    }
}
