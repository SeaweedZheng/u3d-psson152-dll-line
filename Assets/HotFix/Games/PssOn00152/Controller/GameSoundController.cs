using System.Collections.Generic;
using UnityEngine;
using GameMaker;
using SlotMaker;
using System.Collections;
using Random = UnityEngine.Random;

using _soundBB = PssOn00152.SoundBlackboard;
using _contentBB = PssOn00152.ContentBlackboard;
//ChinaTown100
//声音根路劲
namespace PssOn00152
{
    /// <summary>
    /// GameSoundController 只是挂在主游戏页面，其他页面要使用游戏声音时，可以创建soundHelper，或者指定主游戏页面来播放。
    /// </summary>
    public class GameSoundHelper : MonoSingleton<GameSoundHelper>
    {
        GameSoundController _controller = null;
        GameSoundController controller
        {
            get
            {
                if (_controller == null)
                    _controller = GameObject.FindObjectOfType<GameSoundController>();
                return _controller;
            }
        }

        public void PlaySound(SoundKey name, bool loop = false)
        {
            if (controller == null)
                return;
            controller.soundHelper.PlaySoundEff(name, loop);
        }

        public void PlaySoundSingle(SoundKey name, bool loop = false)
        {
            if (controller == null)
                return;
            controller.soundHelper.PlaySoundSingle(name, loop);
        }
        public void StopSound(SoundKey name)
        {
            if (controller == null)
                return;
            controller.soundHelper.StopSound(name);
        }
        public void StopMusic()
        {
            if (controller == null)
                return;
            controller.soundHelper.StopMusic();
        }


        public void PlayMusicSingle(SoundKey name, bool loop = false)
        {
            if (controller == null)
                return;
            controller.soundHelper.PlayMusicSingle(name, loop);
        }
        public bool IsPlaySound(SoundKey name)
        {
            if (controller == null)
                return false;

            bool isPlay = controller.soundHelper.IsPlaySound(name);
            return isPlay;
        }
    }


    /// <summary>
    /// GameSoundController 监听事件，并发做声音控制
    /// </summary>
    public partial class GameSoundController : CorBehaviour
    {

        SoundHelper _soundHelper;

        public SoundHelper soundHelper
        {
            get => _soundHelper;
        }

        //MessageDelegates onSlotDetailEventDelegates;
        //MessageDelegates onWinEventDelegates;

        MessageDelegates onPropertyChangedEventDelegates;
        MessageDelegates onContentEventDelegates;
    
        //FreeSpinAdd
        protected void Awake()  //这里必须是Awake 不能是Start
        {

            /*onSlotDetailEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                                 {SlotMachineEvent.PrepareStoppedReel, OnEventPrepareStoppedReel},
                 }
             );

            onSlotEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                     {SlotMachineEvent.StoppedSlotMachine, OnEventStoppedSlotMachine},
                 }
             );*/


            _soundHelper = new SoundHelper((enumObj) => _soundBB.Instance.soundRelativePath[(PssOn00152.SoundKey)enumObj]);

            onPropertyChangedEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                    { "./gameState", OnPropertyChangeGameState},
                 }
             );

            onContentEventDelegates = new MessageDelegates
             (
                 new Dictionary<string, EventDelegate>
                 {
                     {SlotMachineEvent.BeginBonus, OnBeginBonus},
                 }
             );
        }

        // OnEnable 比 Start 快！
        private void OnEnable()
        {
            //ON_SLOT_EVENT
            //EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_SLOT_DETAIL_EVENT, onSlotDetailEventDelegates.Delegate);
            
            //EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, OnWinEvent);
            EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_SLOT_EVENT, OnSlotEvent);
            EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_SLOT_DETAIL_EVENT, OnSlotDetailEvent);
            EventCenter.Instance.AddEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);

            EventCenter.Instance.AddEventListener<EventData>(SlotMachineEvent.ON_CONTENT_EVEN, onContentEventDelegates.Delegate);
            InitParam();
        }


        private void OnDisable()
        {
            //EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_SLOT_DETAIL_EVENT, onSlotDetailEventDelegates.Delegate);
            //EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_WIN_EVENT, OnWinEvent);
            EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_SLOT_EVENT, OnSlotEvent);
            EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_SLOT_DETAIL_EVENT, OnSlotDetailEvent);
            EventCenter.Instance.RemoveEventListener<EventData>(ObservableObject.ON_PROPERTY_CHANGED_EVENT, onPropertyChangedEventDelegates.Delegate);

            EventCenter.Instance.RemoveEventListener<EventData>(SlotMachineEvent.ON_CONTENT_EVEN, onContentEventDelegates.Delegate);
        }


        void InitParam()
        {
            OnPropertyChangeGameState();
        }



        private void OnBeginBonus(EventData receivedEvent = null)
        {
            if ((string)receivedEvent.value == "FreeSpinAdd") //免费游戏添加7局
            {
                _soundHelper.PlayMusicSingle(SoundKey.FreeSpinAdd, false);
            }
        }


        const string COR_GOD_IDLE = "COR_GOD_IDLE";
        private void OnPropertyChangeGameState(EventData receivedEvent = null)
        {
            string gameState = receivedEvent != null? (string)receivedEvent.value : null;
            if (gameState == null)
                gameState = _contentBB.Instance.gameState; // BlackboardUtils.GetValue<string>("./gameState");
            
            if (gameState == "Idle")
                DoCor(COR_GOD_IDLE, _GodIdle());
            else
                ClearCor(COR_GOD_IDLE); 
        }

        IEnumerator _GodIdle()
        {
            List<SoundKey> godIdle = new List<SoundKey>() { 
                SoundKey.GodIdle1,
                SoundKey.GodIdle2,
                SoundKey.GodIdle3,
                SoundKey.GodIdle4,
                SoundKey.GodIdle5,
                SoundKey.GodIdle6,
            };

            while (true)
            {
                yield return new WaitForSeconds(Random.Range(3f,12f));

                int idx = Random.Range(0, godIdle.Count);
                _soundHelper.PlaySoundEff(godIdle[idx]);
            }
        }

        /*
        private void OnWinEvent(EventData receivedEvent)
        {
            switch ((string)receivedEvent.name)
            {
                case SlotMachineEvent.TotalWinCredit:
                    _soundHelper.PlaySoundEff(SoundKey.TotalWinLine);
                    break;
            }
        }*/

        private void OnSlotEvent(EventData receivedEvent)
        {
            switch ((string)receivedEvent.name)
            {
                case SlotMachineEvent.SpinSlotMachine:

                    if (_contentBB.Instance.isFreeSpin)
                    {
                        //if (soundHelper.IsPlaySound(SoundKey.RegularBG)) soundHelper.StopMusic();
                        if (!_soundHelper.IsPlaySound(SoundKey.FreeSpinBG))
                            _soundHelper.PlayMusicSingle(SoundKey.FreeSpinBG, false);
                    }
                    else
                    {
                        //if (soundHelper.IsPlaySound(SoundKey.FreeSpinBG))/soundHelper.StopMusic();
                        if (!_soundHelper.IsPlaySound(SoundKey.RegularBG))
                            _soundHelper.PlayMusicSingle(SoundKey.RegularBG, false);
                    }
                    break;
            }
        }

        private void OnSlotDetailEvent(EventData receivedEvent)
        {
            switch ((string)receivedEvent.name)
            {
                case SlotMachineEvent.PrepareStoppedReel:
                    {
                        _soundHelper.PlaySoundEff(SoundKey.ReelStop1);

                        int reelIdx = (int)receivedEvent.value;
                        if (_contentBB.Instance.isReelsSlowMotion && (reelIdx == 0 || reelIdx ==1))
                        {
                            _soundHelper.PlaySoundEff(SoundKey.SlowMotionReal123MeetGod);
                        }else if (_contentBB.Instance.isFreeSpinTrigger && reelIdx == 2)
                        {
                            _soundHelper.PlaySoundEff(SoundKey.SlowMotionReal123MeetGod);
                        }

                    }
                    break;
            }
        }

    }

}


