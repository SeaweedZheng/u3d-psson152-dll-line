#if UNITY_ANDROID

namespace CryPrinter
{
    public enum FontWidthScalar
    {
        /// <summary>
        /// Keeps current width scalar
        /// </summary>
        NOP = 0xFF,
        w1 = 0,
        w2 = 16,
        w3 = 32,
        w4 = 48,
        w5 = 64,
        w6 = 80,
        w7 = 96,
        w8 = 112,
    }

    public enum FontHeighScalar
    {        
        /// <summary>
        /// Keeps current height scalar
        /// </summary>
        NOP = 0xFF,
        h1 = 0,
        h2 = 1,
        h3 = 2,
        h4 = 3,
        h5 = 4,
        h6 = 5,
        h7 = 6,
        h8 = 7,
    }
}
#endif
