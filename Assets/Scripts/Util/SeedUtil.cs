using System.Text.RegularExpressions;

public static class SeedUtil
{
    // Changes strings into a consistent hash code integer
    public static int StringToHashCode(string seedStr)
    {
        unchecked
        {
            long hash = 5381L; //initial prime (Bernstein's djb2)
            foreach (char c in seedStr)
                hash = ((hash << 5) + hash) + c; //Equivalent to 33 * hash + c
            return (int)(hash & 0x7FFFFFFF);
        }
    }

    //public static bool IsValidSeed(string seed)
    //{
    //    return Regex.IsMatch(seed, @"^[A-Za-z]{6}\d{4}$");

    //}
}
