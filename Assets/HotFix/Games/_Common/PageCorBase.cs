using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Game
{
    public class PageCorBase : PageMachineButtonBase
    {
        /*
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }
        */

        private CorController _corCtrl;
        private CorController corCtrl
        {
            get
            {
                if (_corCtrl == null)
                {
                    _corCtrl = new CorController(this);
                }
                return _corCtrl;
            }
        }

        public void ClearCor(string name) => corCtrl.ClearCor(name);

        public void ClearAllCor() => corCtrl.ClearAllCor();

        public void DoCor(string name, IEnumerator routine) => corCtrl.DoCor(name, routine);

        public bool IsCor(string name) => corCtrl.IsCor(name);

        public IEnumerator DoTaskRepeat(Action cb, int ms) => corCtrl.DoTaskRepeat(cb, ms);

        public IEnumerator DoTask(Action cb, int ms) => corCtrl.DoTask(cb, ms);




        /// <summary>
        /// </summary>
        /// <param name="timeS"></param>
        /// <returns></returns>
        /// <remark>
        /// 这个受到Time.timeScale的影响，会变快变慢，停止。
        /// </remark>
        public IEnumerator DoWaitForSeconds(float timeS) => corCtrl.DoWaitForSeconds(timeS);

        /// <summary>
        /// </summary>
        /// <param name="timeS"></param>
        /// <returns></returns>
        /// <remark>
        /// 这个不受到Time.timeScale的影响。
        /// </remark>
        public IEnumerator DoWaitForSecondsRealtime(float timeS) => corCtrl.DoWaitForSecondsRealtime(timeS);
    }
}