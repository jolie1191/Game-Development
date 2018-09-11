using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {

    //1. 代理是用来封装函数的，之后可有把符合此代理模版的函数assign给代理的某个instance
    public delegate void AnimationHandler();

    Animation animation;

    //对错有animationClip 这些组件做一个全局的实列
    public static AnimationManager instance;

    public AnimationClip Dead;
    public AnimationClip JumpDown;
    public AnimationClip JumpLoop;
    public AnimationClip JumpUp;
    public AnimationClip Roll;
    public AnimationClip Run;
    public AnimationClip TurnLeft;
    public AnimationClip TurnRight;

    //1. 这是一个代理的实列
    public AnimationHandler animationHandler;

	// Use this for initialization
	void Start () {
        //把当前的AnimationClip x assign 给AnimationManager这个全局对象instance
        instance = this;
        animationHandler = PlayRun;
        animation = GetComponent<Animation>();
	}


    //3. 封装这些需要运行的动画函数，之后付给animationHandler这个代理进行处理。
    //之后在PlayerController的update中会对此代理进行调用
    public void PlayDead()
    {
        animation.Play(Dead.name);
    }

    public void PlayJumpDown()
    {
        animation.Play(JumpDown.name);
    }

    public void PlayJumpLoop()
    {
        animation.Play(JumpLoop.name);
    }

    public void PlayJumpUp()
    {
        animation.Play(JumpUp.name);
        if (animation[JumpUp.name].normalizedTime > 0.95f)
        {
            animationHandler = PlayRun;
        }
    }

    public void PlayRoll()
    {
        animation.Play(Roll.name);
        if (animation[Roll.name].normalizedTime > 0.95f)
        {
            animationHandler = PlayRun;
        }
    }

    public void PlayDoubleJump()
    {
        animation.Play(Roll.name);
        if(animation[Roll.name].normalizedTime > 0.95f)
        {
            animationHandler = PlayJumpLoop;
        }
    }

    public void PlayRun()
    {
        animation.Play(Run.name);
    }

    public void PlayTurnLeft()
    {
        animation.Play(TurnLeft.name);
        if (animation[TurnLeft.name].normalizedTime > 0.95f)
        {
            animationHandler = PlayRun;
        }
    }
    public void PlayTurnRight()
    {
        animation.Play(TurnRight.name);
        if (animation[TurnRight.name].normalizedTime > 0.95f)
        {
            animationHandler = PlayRun;
        }
    }



	
	// Update is called once per frame
	void Update () {
        //animation.Play(Run.name);
        if(animationHandler != null)
        {
            animationHandler();
        }

	}
}
