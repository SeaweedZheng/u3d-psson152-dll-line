using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace GameMaker
{
    //public class SoundHelper<T1, T2> where T1 : Enum where T2 : MonoBehaviour
    public class SoundHelper { 
        public SoundHelper(Func<object, string> getSoundPath)
        {
            this.getSoundPath = getSoundPath;
        }
        public SoundHelper()
        {
            this.getSoundPath = (enumObj) => null;
        }


        private Func<object, string> getSoundPath;

        private string GetAudioClipName(string path)
        {
            string[] res = path.Split('/');
            string name = res[res.Length - 1].Split('.')[0];
            return name;
        }

        public void PlaySoundEff(object enumObj, bool loop = false)
        {
            string path = "";
            if (enumObj is GameMaker.SoundKey)
                path = GlobalSoundBlackboard.Instance.soundRelativePath[(GameMaker.SoundKey)enumObj];
            else
                path = getSoundPath(enumObj);

            SoundManager.Instance.PlaySoundEffX(path, loop);

        }


        public void PlaySoundSingle(object enumObj, bool loop = false)
        {
            string path = "";
            if (enumObj is GameMaker.SoundKey)
                path = GlobalSoundBlackboard.Instance.soundRelativePath[(GameMaker.SoundKey)enumObj];
            else
                path = getSoundPath(enumObj);
            SoundManager.Instance.PlaySoundEffSingle(path, loop);
        }


        public void PlayMusicSingle(object enumObj, bool loop = false)
        {
            string path = "";
            if (enumObj is GameMaker.SoundKey)
                path = GlobalSoundBlackboard.Instance.soundRelativePath[(GameMaker.SoundKey)enumObj];
            else
                path = getSoundPath(enumObj);
            SoundManager.Instance.PlayMusicX(path, loop);
        }


        [Button]
        public bool IsPlaySound(object enumObj)
        {
            string path = "";
            if (enumObj is GameMaker.SoundKey)
                path = GlobalSoundBlackboard.Instance.soundRelativePath[(GameMaker.SoundKey)enumObj];
            else
                path = getSoundPath(enumObj);

            // bool isPlay = SoundManager.Instance.IsPlaySound(__SoundBB.Instance.soundName[name]);
            bool isPlay = SoundManager.Instance.IsPlaySound(path);

            DebugUtils.Log($"is play sound : {GetAudioClipName(path)} = {isPlay}");

            return isPlay;
        }


        public void StopSound(object enumObj)
        {
            string path = "";
            if (enumObj is GameMaker.SoundKey)
                path = GlobalSoundBlackboard.Instance.soundRelativePath[(GameMaker.SoundKey)enumObj];
            else
                path = getSoundPath(enumObj);
            SoundManager.Instance.StopSoundEff(path);  //这个不能用哩
        }

        public void StopMusic()
        {
            SoundManager.Instance.StopMusic();
        }
    }

}