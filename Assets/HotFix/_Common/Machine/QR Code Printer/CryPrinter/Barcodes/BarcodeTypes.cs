#if UNITY_ANDROID


namespace CryPrinter
{
    public enum BarcodeTypes
    {
        /// <summary>
        /// Supported since 1.18 firmware
        /// </summary>
        Code39,
        /// <summary>
        /// Supported since 1.18 firmware
        /// </summary>
        Code128,
        /// <summary>
        /// Supported since 1.21 firmware
        /// </summary>
        ITF,
        /// <summary>
        /// 2D barcode generator
        /// </summary>
        TwoD
    }
}
#endif
