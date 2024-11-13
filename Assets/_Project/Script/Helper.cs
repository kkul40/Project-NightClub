public static class Helper
{
    /// <summary>
    /// Returns True When It Reaches To 100
    ///  and Iterates Itself
    /// </summary>
    /// <param name="iterationCount"></param>
    /// <returns></returns>
    public static bool IterateTo100(ref int iterationCount)
    {
        if (iterationCount++ < 100) return false;
        return true;
    }
    
    /// <summary>
    /// Returns True When It Reaches To 50
    ///  and Iterates Itself
    /// </summary>
    /// <param name="iterationCount"></param>
    /// <returns></returns>
    public static bool IterateTo50(ref int iterationCount)
    {
        if (iterationCount++ < 50) return false;
        return true;
    }
}