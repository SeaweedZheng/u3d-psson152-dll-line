using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Animation = UnityEngine.Animation;

/*
* == 动画：
* Spine
* Animation
* Animator
* Tween
* Timer
* Cor
* GameObject
*/


/// <summary>
/// # 动画ui组件
/// * 在父类写好通用方法
/// * 在子类写事件监听
/// * 父级的方法或属性是virtual类型，供子级复写
/// </summary>
public partial class AnimBaseUI : MonoBehaviour
{

    public GameObject goAnim;

    SkeletonGraphic skelGrap => goAnim ==null ? null: goAnim.GetComponent<SkeletonGraphic>();

    Animator animator => goAnim == null ? null : goAnim.GetComponent<Animator>();

    SkeletonAnimation skelAnim => goAnim == null ? null : goAnim.GetComponent<SkeletonAnimation>();



    //SkeletonMecanim skelMec => goAnim ==null ? null: goAnim.GetComponent<SkeletonMecanim>();


    Animation anim => goAnim == null ? null : goAnim.GetComponent<Animation>();


    string animName = null;

    private void Awake()
    {
        if (goAnim == null)
            goAnim = gameObject;
    }
    [System.Serializable]
    public class AnimInfo
    {
        public string animName;
        public GameObject goAnim;
    }

    public List<AnimInfo> animList = new List<AnimInfo>();

    public virtual void Play(string animName, bool loop = false)
    {
        this.animName = animName;

        if (skelGrap != null)
        {
            //skel.AnimationState.SetAnimation(0, "ide", false);
            skelGrap.AnimationState.SetAnimation(0, animName, loop);
        }
        else if (skelAnim != null)
        {
            skelAnim.AnimationState.SetAnimation(0, animName, loop);
        }
        /*else if (skelMec != null)
        {

        }*/
        else if (animator != null) { 
            if (animator.HasState(0, Animator.StringToHash(animName)))
            {
                animator.Play(animName);
                //animator.speed = 1;
            }
        }
        else if (animList != null)
        {
            for (int i=0; i< animList.Count; i++)
            {
                animList[i].goAnim.SetActive(animList[i].animName == animName);
            }
        }
        else if (anim != null)
        {
            // 播放
            //animition.Play("ani_name");
            // 暂停
            //animition["ani_name"].speed = 0;
            // 继续播放
            //animition["ani_name"].speed = 1;

            anim.Play(animName);
            //anim[animName].speed = 1;
        }
    }


    string m_State = "";
    public virtual string state
    {
        get => m_State;
        set
        {
            if (m_State != value)
            {
                OnValueChagne(value);
            }
            m_State = value;
        }
    }

    public const string STOP = "Defalut";
    protected virtual void OnValueChagne(string state)
    {
        if (state == STOP)
        {
            _AnimStop();
        }
        else
        {
            Play(state, true);
        }
    }

    protected virtual void _AnimStop(string animName = STOP)
    {

        if (skelGrap != null)
        {
            skelGrap.AnimationState.SetAnimation(0, animName, false);
        }
        else if (skelAnim != null)
        {
            skelAnim.AnimationState.SetAnimation(0, animName, false);
        }
        /*else if (skelMec != null)
        {

        }*/
        else if (animator != null)
        {
            if (animator.HasState(0, Animator.StringToHash(animName)))
            {
                animator.Play(animName);
            }
        }
        else if (animList != null)
        {
            for (int i = 0; i < animList.Count; i++)
            {
                animList[i].goAnim.SetActive(false);
            }
        }
    }



    [Button]
    void TestRunAnim(string name, bool loop = false)
    {
        Play(name, loop);
    }

}


public partial class AnimBaseUI : MonoBehaviour
{

    public void Kill()
    {

    }
    public virtual void Play()
    {
        if (animator != null)
        {
            // 继续播放
            animator.speed = 1f;
        }
        /*else if (anim != null && !string.IsNullOrEmpty(animName))
        {
            // 继续播放
            //[Bug]
            anim[animName].speed = 1f;
        }*/
    }

    public void Pause(string name,float normalizedTime)
    {
        this.animName = name;
        if (animator != null)
        {
            //暂停
            animator.Play(animName, 0,normalizedTime);
            // 暂停
            animator.speed = 0f;
        }
        else if (anim != null)
        {
            // 播放
            anim.Play(animName);
            // 暂停
            anim[animName].speed = 0f;
        }
    }
    public void Pause()
    {
        if (animator != null)
        {
            //暂停
            animator.speed = 0f;
            //继续播放
            //animator.speed = 1f;
        }
       /*else if (anim != null && !string.IsNullOrEmpty(animName))
        {
            // 播放
            //anim.Play("ani_name");
            // 暂停
            //anim["ani_name"].speed = 0f;
            // 继续播放
            //anim["ani_name"].speed = 1f;


            //[Bug]
            anim.Play(animName);
            anim[animName].speed = 0; 
        }*/
    }

    public void PlayFrame(string name, int frame)
    {
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != null)
            return;

        //当前动画机播放时长
        float currentTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        //动画片段长度
        float length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        //获取动画片段帧频
        float frameRate = animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
        //计算动画片段总帧数
        float totalFrame = length / (1 / frameRate);
        //计算当前播放的动画片段运行至哪一帧
        int currentFrame = (int)(Mathf.Floor(totalFrame * currentTime) % totalFrame);
        DebugUtils.Log(" Frame: " + currentFrame + "/" +totalFrame);

       // while (frame > totalFrame)
            //frame = frame - totalFrame;

        float normalizedFrameTime = frame / totalFrame;
        animator.Play(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, normalizedFrameTime);
        animator.speed = 1;

    }

    public void PlayPre(string name, float pre)
    {
        if (pre < 0 || pre > 1)
            DebugUtils.LogError("pre must between 0 - 1");

        if (animator != null)
        {
            animator.Play(name,0,pre);
        }
    }

    public void PlayReverse(string name, bool isReverse)
    {
        if (animator != null)
        {
            //Speed-Multiplier添加SpeedOpen
            if (isReverse)
            {
                //正播
                animator.SetFloat("SpeedOpen" , 1f);
                animator.Play(name, -1, 0f);
            }
            else
            {
                // 倒播
                animator.SetFloat("SpeedOpen", -1f);
                animator.Play(name, -1, 1f);
            }
        }
    }
}