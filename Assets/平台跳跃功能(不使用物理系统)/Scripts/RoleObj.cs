using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace 平台跳跃功能
{
    public class RoleObj : MonoBehaviour
    {
        //移动速度
        public float moveSpeed = 5;

        //初始跳跃时的 竖直上抛速度
        public float initYSpeed = 10;

        //重力加速度
        public float G = 9.8f;

        //影子对象
        public GameObject shadowObj;

        //当人跳这么高时 影子缩放为0 消失
        public float jumpMaxH = 5;

        //影子的位置
        private Vector3 shadowPos;

        //跳跃当中 实时的速度是多少
        private float nowYSpeed;

        //当前平台的 Y 值
        private float nowPlatformY;

        //是否可以主动下落
        private bool canFall;

        //二段跳计数
        private int jumpIndex;


        // 动画控制相关组件
        private Animator roleAnimator;

        //输入移动相关的系数
        private float horizontalMove;

        //用于处理上下平台的逻辑对象
        PlatformLogic platformLogic;

        //得到当前玩家是否是跳跃状态
        public bool isJump => roleAnimator.GetBool("isJump");

        //得到当前玩家是否在下落状态
        public bool isFall => roleAnimator.GetBool("isFall");

        // Start is called before the first frame update
        void Start()
        {
            roleAnimator = this.GetComponent<Animator>();
            shadowPos = shadowObj.transform.position; //避免影子挡住脚

            platformLogic = new PlatformLogic(this);
        }

        // Update is called once per frame
        void Update()
        {
            #region 移动相关逻辑

            //得到水平方向的输入 一般就是左右键输入
            //-1  0   1 三个值
            horizontalMove = Input.GetAxisRaw("Horizontal");
            //面朝左
            if (horizontalMove < 0)
                this.transform.rotation = Quaternion.Euler(0, -90, 0);
            else if (horizontalMove > 0)
                this.transform.rotation = Quaternion.Euler(0, 90, 0);
            //当有输入时，直接朝面朝向移动即可
            if (horizontalMove != 0)
                this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            //跑步动画的切换
            roleAnimator.SetBool("isRun", horizontalMove != 0);

            #endregion

            #region 跳跃逻辑

            //主动下落按键检测
            //我们没有跳跃没有下落时 才应该响应这个组合键
            if (canFall && !isJump && !isFall &&
                Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
            {
                this.Fall();
            }
            //组合键的响应 应该和跳跃时互斥的 所以我们使用 if else
            else if (Input.GetKeyDown(KeyCode.Space) && jumpIndex != 2)
            {
                //跳跃
                roleAnimator.SetBool("isJump", true);
                //当前Y速度 = 竖直上抛的初始速度
                nowYSpeed = initYSpeed;
                //二段跳计数 只能连续跳两次
                ++jumpIndex;
            }

            //跳跃状态时 Y 坐标的变化
            //下落状态时 Y 坐标也需要变化
            if (isJump || isFall)
            {
                //当我们跳跃或者下落时
                //第一帧位移之前 就让当前Y的速度产生变化
                //收到重力加速度的影响 每一帧 都去改变当前的移动速度
                nowYSpeed -= G * Time.deltaTime;
                //位移逻辑
                this.transform.Translate(Vector3.up * nowYSpeed * Time.deltaTime);

                //当竖直方向的速度小于等于0 就应该播放 下落的动画
                roleAnimator.SetBool("isFall", nowYSpeed <= 0);

                //判断是否落在了对应平台上
                if (this.transform.position.y <= nowPlatformY)
                {
                    //停止跳跃动画
                    roleAnimator.SetBool("isJump", false);
                    roleAnimator.SetBool("isFall", false);
                    //避免下落时 落到"平台里" 把它拉回来
                    Vector3 pos = this.transform.position;
                    pos.y = nowPlatformY;
                    this.transform.position = pos;
                    //落地时才去清楚二段跳计数
                    jumpIndex = 0;
                }
            }

            #endregion

            #region 影子相关逻辑

            //移动相关
            shadowPos.x = this.transform.position.x; //跟随玩家动
            shadowPos.y = nowPlatformY; //影子的y的位置 一定是和平台一致的
            shadowObj.transform.position = shadowPos;

            //缩放相关
            shadowObj.transform.localScale = 1.5f * Vector3.one * Mathf.Max(0, (jumpMaxH - (this.transform.position.y - nowPlatformY))) / jumpMaxH;

            #endregion

            #region 平台切换相关逻辑

            platformLogic.UpdateCheck();

            #endregion
        }

        /// <summary>
        /// 去改变平台相关信息的
        /// </summary>
        /// <param name="y"></param>
        /// <param name="showShadow"></param>
        /// <param name="canFall"></param>
        public void ChangePlatformData(float y, bool showShadow, bool canFall)
        {
            //改变当前平台的Y
            this.nowPlatformY = y;
            //是否显示影子
            shadowObj.gameObject.SetActive(showShadow);
            //是否可以在该平台下落
            this.canFall = canFall;
        }


        /// <summary>
        /// 玩家下落方法
        /// </summary>
        public void Fall()
        {
            //动作的切换
            roleAnimator.SetBool("isFall", true);
            //相当于把平台设置为null的感觉
            //把它设置为一个非常小的数 那么这样 在更新跳跃下落时
            //就不会把它拉回之前平台的位置了
            nowPlatformY = -9999;
            //由于是自由落体 我们Y上的速度应该从0开始
            //避免之前有残留 所以我们将其清0
            nowYSpeed = 0;
            //由于在下落时只允许跳一次 所以我们将计数从1开始
            //这样再下落过程中就不会进行二段跳了
            jumpIndex = 1;
        }
    }
}