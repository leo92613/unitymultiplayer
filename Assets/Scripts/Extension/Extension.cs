/// <summary>
/// This class extends system class with some helper method
/// </summary>
public static class Extension
{
    /// <summary>
    /// Generate a simple int from a string
    /// to use in UWP environment
    /// </summary>
    /// <param name="str">The string to generate int</param>
    /// <returns></returns>
    public static int Hash(this string str)
    {
        int hash = 0;
        foreach (char c in str)
        {
            hash += (int)c;
        }
        return hash;
    }
}
