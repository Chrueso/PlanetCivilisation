using UnityEngine;

public static class NoiseFilterFactory 
{
    public static INoiseFilter CreateNoiseFilter(PlanetNoiseSettings settings)
    {
        switch (settings.FilterType)
        {
            case PlanetNoiseSettings.NoiseFilterType.Simple:
                return new SimpleNoiseFilter(settings.SimpleNoiseSettings);
               
            case PlanetNoiseSettings.NoiseFilterType.Rigid:
                return new RigidNoiseFilter(settings.RigidNoiseSettings);
        }

        return null;
    }
    
}
