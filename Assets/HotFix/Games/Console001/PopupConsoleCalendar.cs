using Game;
using GameMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Console001
{
    public class PopupConsoleCalendar : PageCorBase
    {
        public Button btnClose,btnCancel,btnSave;

        CalendarController ctrCalendar;

        private void Awake()
        {
            btnClose.onClick.AddListener(OnClickClose);
            btnCancel.onClick.AddListener(OnClickCancel);
            btnSave.onClick.AddListener(OnClickSave);
            ctrCalendar = transform.GetComponent<CalendarController>();
        }

        private void OnEnable()
        {
            //ctrCalendar.InitTime();
        }

        void OnClickClose()=>PageManager.Instance.ClosePage(this,new EventData<string>("",null));

        void OnClickCancel() => OnClickClose();

        void OnClickSave()
        {
            PageManager.Instance.ClosePage(this, new EventData<Dictionary<string, object>>("",

                    new Dictionary<string, object>()
                    {
                        ["date"] = ctrCalendar.GetLastSelectDate(),
                        ["timestamp"] = ctrCalendar.GetLastSelectTimestamp(),
                    }
                ));
        }

    }
}
