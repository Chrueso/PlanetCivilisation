using UnityEngine;

/// <summary>
/// Utility class which tracks the minimum and maximum values from a sequence of float inputs
/// </summary>
[System.Serializable]
public class MinMaxf
{
    public float Min;
    public float Max;

    public MinMaxf()
    {
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public void AddValue(float v)
    {
        if (v > Max) Max = v;

        if (v < Min) Min = v;
    }

    public float Average()
    {
        return (Min + Max) / 2f;
    }
}

/// <summary>
/// Utility class which tracks the minimum and maximum values from a sequence of integer inputs
/// </summary>
[System.Serializable]
public class MinMax
{
    public int Min;
    public int Max;

    public MinMax()
    {
        Min = int.MaxValue;
        Max = int.MinValue;
    }

    public void AddValue(int v)
    {
        if (v > Max) Max = v;

        if (v < Min) Min = v;
    }

    public int Average()
    {
        return (Min + Max) / 2;
    }
}