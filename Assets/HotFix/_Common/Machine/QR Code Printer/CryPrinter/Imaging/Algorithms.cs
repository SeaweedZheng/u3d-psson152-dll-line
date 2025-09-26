#if UNITY_ANDROID
namespace CryPrinter
{
    /// <summary>
    /// List of available dithering algorithms
    /// </summary>
    public enum Algorithms
    {
        JarvisJudiceNinke,
        FloydSteinberg,
        Atkinson,
        Stucki,
        None,
        FloydSteinbergFalse,
        Sierra,
        Sierra2,
        SierraLite,
        Burkes,
    }
}
#endif
