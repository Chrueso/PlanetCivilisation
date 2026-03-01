using System.Collections.Generic;
using System.Linq;

public class FactionManager : Singleton<FactionManager>
{

    public FactionDataSO[] FactionData;
    public List<Faction> FactionsInUse { get; private set; } = new List<Faction>();

    private void Awake()
    {
        base.Awake();

        foreach(FactionDataSO data in FactionData)
        {
            FactionsInUse.Add(new Faction(data));
        }

        FactionsInUse = FactionsInUse.OrderByDescending(f => f.IsPlayer).ToList();
    }
}
