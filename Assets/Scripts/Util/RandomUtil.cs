using UnityEngine;

public static class RandomUtil
{
    public static float NextRangef(System.Random rng, float min, float max)
    {
        return min + (float)rng.NextDouble() * (max - min);
    }
}
