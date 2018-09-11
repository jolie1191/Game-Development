using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//定义一个含有方向的类
public enum InputDirection
{
    NULL,
    Left,
    Right,
    Up,
    Down
}

public class PlayerController : MonoBehaviour {

    public float speed = 1;
    InputDirection inputDirection;
    Vector3 mousePos;
    bool activeInput;
    //小人当前所站位置：左中右
    Position standPosition;
    //小人从哪个位置跳过来的
    Position fromPosition;
    //此向量控制x轴上的移动
    Vector3 xDirection;
    //此向量控制y + z轴的移动
    Vector3 moveDirection;
    //设定一个上跳值
    float jumpValue = 7;
    //向上跳时，会有重力
    float gravity = 20;

    //如果拿到跑鞋，即有双连跳状态
    public bool canDoubleJump = true;
    //是否已经进入双连跳状态
    bool doubleJump = false;

    //判断是否在极速暴走中
    bool isQuickMoving = false;
    float saveSpeed;
    float quickMoveDuration = 3;
    public float quickMoveTimeLeft;
    public Text statusText;

    IEnumerator quickMoveCor;

    //当捡到吸铁石的时候，启用MagnetCollider（一般是不启用的）
    //吸铁石持续时间
    float magnetDuration = 15;
    public float magnetTimeLeft;
    IEnumerator magnetCor;
    public GameObject MagnetCollider;

    //超级跑鞋
    float shoeDuration = 10;
    public float shoeTimeLeft;
    IEnumerator shoeCor;

    //双倍积分
    float multiplyDuration = 10;
    public float multiplyTimeLeft;
    IEnumerator multiplyCor;

    //用于人碰到金币产生的特效
    public static PlayerController instance;

    //引用CharacterController组件
    CharacterController characterController;

	// Use this for initialization
	void Start () {
        instance = this;
        //获得characterController组件
        characterController = GetComponent<CharacterController>();
        //默认当前位置：中间
        standPosition = Position.Middle;
        StartCoroutine(UpdateAction()); 
	}
	
    IEnumerator UpdateAction()
    {
        while (true)
        {
            GetInputDirection(); 
            //PlayAnimation();
            MoveLeftRight();
            MoveForward();
            yield return 0;
        }
    }

    //控制y轴和z轴的移动
    void MoveForward() 
    {
        //用户向下滑动-->roll
        if(inputDirection == InputDirection.Down)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayRoll;
        }
        //如果小人在地面上的话， 有四种动画：滚动，slide left, slide right, and Run
        if(characterController.isGrounded)
        {
            //将y轴清零，之后会在update中设置
            moveDirection = Vector3.zero;
            if(AnimationManager.instance.animationHandler != AnimationManager.instance.PlayRoll 
               && AnimationManager.instance.animationHandler != AnimationManager.instance.PlayTurnLeft
               && AnimationManager.instance.animationHandler != AnimationManager.instance.PlayTurnRight)
            {
                AnimationManager.instance.animationHandler = AnimationManager.instance.PlayRun;            
            }
            if(inputDirection == InputDirection.Up)
            {
                JumpUp();
                if (canDoubleJump)
                    doubleJump = true;
            }
        }
        else
        {   
            //在空中，如果想要迅速下降到地面，进入以下statement
            if(inputDirection == InputDirection.Down)
            {
                QuickGround();
            }
            if(inputDirection == InputDirection.Up)
            {
                if (doubleJump)
                {
                    JumpDouble();
                    doubleJump = false;
                }
            }
            //播放在空中跳跃动画
            if(AnimationManager.instance.animationHandler != AnimationManager.instance.PlayJumpUp
               && AnimationManager.instance.animationHandler != AnimationManager.instance.PlayDoubleJump
               && AnimationManager.instance.animationHandler != AnimationManager.instance.PlayRoll)
            {
                AnimationManager.instance.animationHandler = AnimationManager.instance.PlayJumpLoop;
            }
        }
        
    }

    //在空中，想实现迅速下降到地面
    void QuickGround()
    {
        moveDirection.y -= jumpValue * 3;
    }

    //实现双连跳功能
    void JumpDouble()
    {
        AnimationManager.instance.animationHandler = AnimationManager.instance.PlayDoubleJump;
        moveDirection.y += jumpValue * 1.3f;
    }


    void JumpUp()
    {
        AnimationManager.instance.animationHandler = AnimationManager.instance.PlayJumpUp;
        moveDirection.y += jumpValue;

    }


    void MoveLeft()
    {
        //小人已经在道路最左侧，以下不会执行
        if(standPosition != Position.Left)
        {
            //停止播放当前动画， 因为要播放新的动画了
            GetComponent<Animation>().Stop();
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnLeft;

            xDirection = Vector3.left;

            //更新小人左移之后的位置状态
            if(standPosition == Position.Middle)
            {
                standPosition = Position.Left;
                fromPosition = Position.Middle;
            }
            else if (standPosition == Position.Right)
            {
                standPosition = Position.Middle;
                fromPosition = Position.Right;
            }

        }
    }

    void MoveRight()
    {
        //小人已经在道路最左侧，以下不会执行
        if (standPosition != Position.Right)
        {
            //停止播放当前动画， 因为要播放新的动画了
            GetComponent<Animation>().Stop();
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnRight;

            xDirection = Vector3.right;

            //更新小人左移之后的位置状态
            if (standPosition == Position.Middle)
            {
                standPosition = Position.Right;
                fromPosition = Position.Middle;
            }
            else if (standPosition == Position.Left)
            {
                standPosition = Position.Middle;
                fromPosition = Position.Left;
            }

        }
    }

    //根据输入方向，调用MoveRight或MoveLeft
    void MoveLeftRight()
    {
        if (inputDirection == InputDirection.Left)
        {
            MoveLeft();
        }
        else if(inputDirection == InputDirection.Right)
        {
            MoveRight();
        }

        //判断如果到了最左侧或最右侧， 就不能再左移或右移
        //把xDirection改成0， 那么就不存在左移了
        //refer参考: characterController.Move((xDirection * 5 + moveDirection) * Time.deltaTime);
        //移到左侧，最左侧就不能移动了
        if(standPosition == Position.Left)
        {
            if(transform.position.x <= -1.7f)
            {
                xDirection = Vector3.zero;
                transform.position = new Vector3(-1.7f, transform.position.y, transform.position.z);
            }
        }
        //从左或右移到中间时：限定每次只能移动一条道
        if(standPosition == Position.Middle)
        {
            if(fromPosition == Position.Left)
            {
                if(transform.position.x > 0)
                {
                    xDirection = Vector3.zero;
                }
            }
            else if(fromPosition == Position.Right)
            {
                if(transform.position.x < 0)
                {
                    xDirection = Vector3.zero;
                }
            }
            transform.position = new Vector3(0, transform.position.y, transform.position.z);


        }
        //移到右侧时，若最右，就不能再移动
        if (standPosition == Position.Right)
        {
            if (transform.position.x >= 1.7f)
            {
                xDirection = Vector3.zero;
                transform.position = new Vector3(1.7f, transform.position.y, transform.position.z);
            }
        }


    }



    void PlayAnimation()
    {
        if (inputDirection == InputDirection.Left)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnLeft;
        }
        else if (inputDirection == InputDirection.Right)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayTurnRight;
        }
        else if(inputDirection == InputDirection.Up)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayJumpUp;
        }
        else if(inputDirection == InputDirection.Down)
        {
            AnimationManager.instance.animationHandler = AnimationManager.instance.PlayRoll;
        }


    }

    public void QuickMove()
    {
        if (quickMoveCor != null)
            StopCoroutine(quickMoveCor);
        //speed = 20;
        quickMoveCor = QuickMoveCoroutine();
        StartCoroutine(quickMoveCor);
    }

    //在“极速暴走”中有一段时间的持续等待
    //不可在主线程中等待，会卡死
    //这里用到携程
    IEnumerator QuickMoveCoroutine()
    {
        quickMoveTimeLeft = quickMoveDuration;
        if(!isQuickMoving)
            saveSpeed = speed;
        speed = 20;
        isQuickMoving = true;
        //yield return new WaitForSeconds(quickMoveDuration);
        while(quickMoveTimeLeft >= 0)
        {
            quickMoveTimeLeft -= Time.deltaTime;
            yield return null;
        }
        speed = saveSpeed;
        isQuickMoving = false;
    }



    public void UseMagnet()
    {
        //如果正在用磁铁中，停止磁铁；
        //不在用磁铁中，启用磁铁
        if (magnetCor != null)
            StopCoroutine(magnetCor);
        magnetCor = MagnetCoroutine();
        StartCoroutine(magnetCor);
    }

    IEnumerator MagnetCoroutine()
    {
        magnetTimeLeft = magnetDuration;
        //启用磁铁碰撞机（平时是不启用的）
        MagnetCollider.SetActive(true);
        while(magnetTimeLeft >= 0)
        {
            magnetTimeLeft -= Time.deltaTime;
            yield return null;
        }
        //当磁铁没有剩余时间时，停止磁铁碰撞机
        MagnetCollider.SetActive(false);

    }

    public void UseShoe()
    {
        if (shoeCor != null)
            StopCoroutine(shoeCor);
        shoeCor = ShoeCoroutine();
        StartCoroutine(shoeCor);

    }
    
    

    IEnumerator ShoeCoroutine()
    {
        shoeTimeLeft = shoeDuration;
        PlayerController.instance.canDoubleJump = true;
        while(shoeTimeLeft >= 0)
        {
            shoeTimeLeft -= Time.deltaTime;
            yield return null;
        }
        PlayerController.instance.canDoubleJump = false;
        
    }

    //处理双倍积分与携程相关的东西
    public void Multiply()
    {
        if (multiplyCor != null)
            StopCoroutine(multiplyCor);
        multiplyCor = MultiplyCoroutine();
        StartCoroutine(multiplyCor);
    }

    //处理双倍积分在携程上的时间和积分倍数问题
    IEnumerator MultiplyCoroutine()
    {
        multiplyTimeLeft = multiplyDuration;
        GameAttribute.instance.multiply = 2;
        while(multiplyTimeLeft >= 0)
        {
            multiplyTimeLeft -= Time.deltaTime;
            yield return null;
        }
        GameAttribute.instance.multiply = 1;
    }

    void GetInputDirection()
    {
        inputDirection = InputDirection.NULL;
        if(Input.GetMouseButtonDown(0))
        {
            activeInput = true;
            mousePos = Input.mousePosition;
        }

        if(Input.GetMouseButton(0) && activeInput == true)
        {
            Vector3 vec = Input.mousePosition - mousePos;
            if (vec.magnitude > 20)
            {
                var angleY = Mathf.Acos(Vector3.Dot(vec.normalized, Vector2.up)) * Mathf.Rad2Deg;
                var angleX = Mathf.Acos(Vector3.Dot(vec.normalized, Vector2.right)) * Mathf.Rad2Deg;
                if(angleY <= 45)
                {
                    inputDirection = InputDirection.Up;
                }
                else if(angleY >= 135)
                {
                    inputDirection = InputDirection.Down;
                }
                else if(angleX <= 45)
                {
                    inputDirection = InputDirection.Right;
                }
                else if(angleX >= 135)
                {
                    inputDirection = InputDirection.Left;
                }
                activeInput = false;
                //Debug.Log(inputDirection);
            }


           // Vector2.up
        }

    }
	// Update is called once per frame
	void Update () {
        //this.transform.Translate(new Vector3(0, 0, speed*Time.deltaTime));
        //代替以上改变z轴位置移动->改用用characterController这个component实现含有更多功能，动画的移动位置关系
        moveDirection.z = speed;
        moveDirection.y -= gravity * Time.deltaTime;
        //(xDirection + moveDirection) * Time.deltaTime
        characterController.Move((xDirection * 5 + moveDirection) * Time.deltaTime);
        statusText.text = GetTime(multiplyTimeLeft);


    }	

    private string GetTime(float time)
    {
        if (time <= 0)
            return "0";
        //return Mathf.RoundToInt(time).ToString();
        return ((int)time + 1).ToString();
    }
}

//定义一个小人所处位置的类
public enum Position
{
    Left,
    Middle,
    Right
}





