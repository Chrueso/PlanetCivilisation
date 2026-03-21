public static class FactionDatabase
{
    public static readonly FactionType[] factions = (FactionType[])System.Enum.GetValues(typeof(FactionType));
}

public enum FactionType
{
    Nothing,
    Human,
    DemiHuman,
    IntelligentConstruct,
}