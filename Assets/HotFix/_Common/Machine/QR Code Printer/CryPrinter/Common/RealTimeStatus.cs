#if UNITY_ANDROID

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace CryPrinter
{
    #region BaseStatus for JSONify
   //[JsonConverter(typeof(ToStringJsonConverter))]  
    public class BaseVal
    {
        protected bool m_value;
        
        public override string ToString()
        {
            return m_value.ToString();
        }
    }
    #endregion

    /// <summary>
    /// Printer is reporting online if value is true
    /// </summary>
    public class IsOnlineVal : BaseVal
    {
        public static implicit operator bool(IsOnlineVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsOnlineVal(bool val)
        {
            return new IsOnlineVal { m_value = val };
        }
    }

    /// <summary>
    /// Printer head (cover) is closed if value is true
    /// </summary>  
    public class IsCoverClosedVal : BaseVal
    {
        public static implicit operator bool(IsCoverClosedVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsCoverClosedVal(bool val)
        {
            return new IsCoverClosedVal { m_value = val };
        }
    }

    /// <summary>
    /// Last paper feed was NOT due to diag push button if value is true
    /// </summary>    
    public class IsNormalFeedVal : BaseVal
    {
        public static implicit operator bool(IsNormalFeedVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsNormalFeedVal(bool val)
        {
            return new IsNormalFeedVal { m_value = val };
        }
    }

    /// <summary>
    /// Paper level is at or above the low paper threshold if value is true
    /// </summary>
    public class IsPaperLevelOkayVal : BaseVal
    {
        public static implicit operator bool(IsPaperLevelOkayVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsPaperLevelOkayVal(bool val)
        {
            return new IsPaperLevelOkayVal { m_value = val };
        }
    }

    /// <summary>
    /// There is some paper present if this value is true. Note, the paper level 
    /// may be low but is still conidered present.
    /// </summary>
    public class IsPaperPresentVal : BaseVal
    {
        public static implicit operator bool(IsPaperPresentVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsPaperPresentVal(bool val)
        {
            return new IsPaperPresentVal { m_value = val };
        }
    }

    /// <summary>
    /// If the printer is reporting any error type, this value is true
    /// </summary>     
    public class HasErrorVal : BaseVal
    {
        public static implicit operator bool(HasErrorVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator HasErrorVal(bool val)
        {
            return new HasErrorVal { m_value = val };
        }
    }

    /// <summary>
    /// The cutter is okay if this value is true
    /// </summary>
    public class IsCutterOkayVal : BaseVal
    {
        public static implicit operator bool(IsCutterOkayVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsCutterOkayVal(bool val)
        {
            return new IsCutterOkayVal { m_value = val };
        }
    }

    /// <summary>
    /// There is a non-recoverable error state if this value is true
    /// </summary>
    public class HasFatalErrorVal : BaseVal
    {
        public static implicit operator bool(HasFatalErrorVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator HasFatalErrorVal(bool val)
        {
            return new HasFatalErrorVal { m_value = val };
        }
    }

    /// <summary>
    /// There is a recoverable error state if this value is true
    /// </summary>
    public class HasRecoverableErrorVal : BaseVal
    {
        public static implicit operator bool(HasRecoverableErrorVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator HasRecoverableErrorVal(bool val)
        {
            return new HasRecoverableErrorVal { m_value = val };
        }
    }

    /// <summary>
    /// The paper motor is currently off if this value is true
    /// </summary>
    public class IsPaperMotorOffVal : BaseVal
    {
        public static implicit operator bool(IsPaperMotorOffVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsPaperMotorOffVal(bool val)
        {
            return new IsPaperMotorOffVal { m_value = val };
        }
    }

    /// <summary>
    /// Paper is in the present position if this value is true
    /// </summary>
    public class IsTicketPresentAtOutputVal : BaseVal
    {
        public static implicit operator bool(IsTicketPresentAtOutputVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsTicketPresentAtOutputVal(bool val)
        {
            return new IsTicketPresentAtOutputVal { m_value = val };
        }
    }

    /// <summary>
    /// The diagnostic button is NOT being pushed if this value is true
    /// </summary>
    public class IsDiagButtonReleasedVal : BaseVal
    {
        public static implicit operator bool(IsDiagButtonReleasedVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsDiagButtonReleasedVal(bool val)
        {
            return new IsDiagButtonReleasedVal { m_value = val };
        }
    }

    /// <summary>
    /// The head temperature is okay if this value is true
    /// </summary>
    public class IsHeadTemperatureOkayVal : BaseVal
    {
        public static implicit operator bool(IsHeadTemperatureOkayVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsHeadTemperatureOkayVal(bool val)
        {
            return new IsHeadTemperatureOkayVal { m_value = val };
        }
    }

    /// <summary>
    /// Comms are okay, no errors, if this value is true
    /// </summary>
    public class IsCommsOkayVal : BaseVal
    {
        public static implicit operator bool(IsCommsOkayVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsCommsOkayVal(bool val)
        {
            return new IsCommsOkayVal { m_value = val };
        }
    }

    /// <summary>
    /// Power supply voltage is within tolerance if this value is true
    /// </summary>
    public class IsPowerSupplyVoltageOkayVal : BaseVal
    {
        public static implicit operator bool(IsPowerSupplyVoltageOkayVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsPowerSupplyVoltageOkayVal(bool val)
        {
            return new IsPowerSupplyVoltageOkayVal { m_value = val };
        }
    }

    /// <summary>
    /// Power supply voltage is within tolerance if this value is true
    /// </summary>
    public class IsPaperPathClearVal : BaseVal
    {
        public static implicit operator bool(IsPaperPathClearVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsPaperPathClearVal(bool val)
        {
            return new IsPaperPathClearVal { m_value = val };
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IsBusyVal : BaseVal
    {
        public static implicit operator bool(IsBusyVal o)
        {
            return ((o == null) ? false : o.m_value);
        }

        public static implicit operator IsBusyVal(bool val)
        {
            return new IsBusyVal { m_value = val };
        }
    }

}
#endif
