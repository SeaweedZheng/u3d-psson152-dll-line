#if UNITY_ANDROID
namespace CryPrinter
{
    [System.Serializable]
    public class ImagingException : System.Exception
    {
        public ImagingException(string message)
            : base(message)
        { }
    }
}
#endif
