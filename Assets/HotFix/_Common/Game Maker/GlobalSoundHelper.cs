using GameMaker;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMaker
{
    public class GlobalSoundHelper : MonoSingleton<GlobalSoundHelper>
    {
        SoundHelper m_SoundHelper;
        SoundHelper soundHelper
        {
            get
            {
                if (m_SoundHelper == null)
                    m_SoundHelper = new SoundHelper();

                return m_SoundHelper;
            }
        }


        public void PlaySound(object enumObj, bool loop = false) => soundHelper?.PlaySoundEff(enumObj, loop);

        public void PlaySoundSingle(object enumObj, bool loop = false) => soundHelper?.PlaySoundEff(enumObj, loop);

        public bool IsPlaySound(object enumObj) => soundHelper.IsPlaySound(enumObj);
    }
}


