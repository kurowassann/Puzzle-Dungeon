using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;


public class Player : CharacterBace
{
    
    //定数
    [Tooltip("スティック"), SerializeField]
    private InputAction inputMover;
    [Tooltip("スティック"), SerializeField]
    private InputActionAsset inputActions;
    [Tooltip("アニメーターコンポーネント")]
    Animator animator;


    //メンバ変数
    /// <summary>攻撃位置</summary>
    private Point AttackPos;


    //メンバ関数 
    //private
    // オブジェクトがアクティブになったときに呼ばれるイベント
    private void OnDisable()
    {
        inputMover.Disable();
        Debug.Log("a");
    }
    // オブジェクトが非表示になったときに呼ばれるイベント
    private void OnEnable()
    {
        inputMover = inputActions.FindAction("Player/Move");

        inputMover.Enable();

    }    
    /// <summary>キー入力に対する反応</summary>
    private void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetStatus(Status.REAR_GAP);
        }

        //キー入力分岐
        if (inputMover.ReadValue<Vector2>() != new Vector2(0, 0))
        {
            //ステートが行動受付状態かどうか
            if (mStatus == Status.STAY)
            {
                TileInfo TI;
                Point point = mPosId.GetPos();

                // 呼び出すアニメーショントリガーの名前
                Vector vec;
                // アニメーションのリセット
                ResetTrigger(animator, Common.Common.CHARA_ANIMS_MOVE_DIR);
                ResetTrigger(animator, Common.Common.CHARA_ANIMS_END_DIR);
                ResetTrigger(animator, Common.Common.CHARA_ANIMS_ATTACK_DIR);



                if (inputMover.ReadValue<Vector2>().x == 1.0f)//→
                {
                    point.X += 1;
                    vec = Vector.RIGHT;

                }
                else if (inputMover.ReadValue<Vector2>().x == -1.0f)//左
                {
                    point.X -= 1;
                    vec = Vector.LEFT;
                }
                else if (inputMover.ReadValue<Vector2>().y == 1.0f)//↑
                {
                    point.Y -= 1;
                    vec = Vector.UP;

                }
                else if (inputMover.ReadValue<Vector2>().y == -1.0f)//下
                {
                    point.Y += 1;
                    vec = Vector.DOWN;

                }
                else { return; }
                TI = cMm.CheckTile(point);
                //Debug.Log(point);
                if (TI == TileInfo.ROUTE)
                {
                    Debug.Log($"{point}に移動を開始します。");
                    SetPos(point);
                    SetStatus(Status.MOVE);
                    // アニメーションの呼び出し
                    var triggerName = Common.Common.CHARA_ANIMS_MOVE_DIR[(int)vec];
                    animator.SetTrigger(triggerName);
                }
                else if (TI == TileInfo.ENEMY)
                {
                    //Debug.Log("攻撃");

                    SetStatus(Status.ATTACK);
                    AttackPos = point;
                    //master.PlayerAttack(point);

                    // アニメーション起動部
                    //
                    //("攻撃します:" + point);
                    var triggerName = Common.Common.CHARA_ANIMS_ATTACK_DIR[(int)vec];
                    animator.SetTrigger(triggerName);
                    um.AddLog("敵を攻撃！");
                }
            }

        }

    }

    //public
    /// <summary>初期化処理</summary>
    public override void Init(GameManager tgm, MapManager tmm, PosId tposId, int thp, string tstr)
    {
        base.Init(tgm, tmm, tposId, thp, tstr);


        //Debug.Log("プレイヤ初期化");
        //SetCam();

        // アニメーターコンポーネントの取得
        animator = GetComponent<Animator>();
    }
    /// <summary>ダメージ処理</summary>
    public override bool Damage()
	{
		bool ret = base.Damage();
        /*
		um.DisplayPlayerHP(mHp);
		um.AddLog("プレイヤにダメージ！！");
		if(ret == true) 
		{
			um.AddLog("やられました");
		}
        */
		return ret;
	}   
    /// <summary>体力回復</summary>
    public void Heal()
    {
        mHp++;
        if(mHp > 2)
        {
            mHp = 2;
        }
        /*
        um.DisplayPlayerHP(mHp);
        um.AddLog("プレイヤーのHPが回復");
        */
    }    
    //体力上限UP
    public void HpUp()
    {
        um.AddLog("HP上限があがった！");
        print("上限UP");
        mHp++;
        um.HpUp(1);
    }
    //体力上限Down
    public void HpDown()
    {
        mHp--;
        um.HpDown(1);
    }


    //protected
    /// <summary>移動終了処理</summary>
    protected override void MoveEnd()
    {
        base.MoveEnd();
        var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string triggerName = "";
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_MOVE_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_END_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_ATTACK_DIR);
        //print("aaa" + animationName);
        switch (animationName)
        {
            case "MoveRight@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];
                break;
            case "MoveLeft@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];
                break;
            case "MoveUp@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];
                break;
            case "MoveDown@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];
                break;
        }

        animator.SetTrigger(triggerName);
        cGm.PlayerActionEnd();

    }
    /// <summary>攻撃終了処理</summary>
    protected override void AttackEnd()
    {
        base.AttackEnd();
        cGm.PlayerAttack(AttackPos);
        var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string triggerName = "";
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_MOVE_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_END_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_ATTACK_DIR);
        //print("aaa" + animationName);
        switch (animationName)
        {
            case "AttackRight@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];
                break;
            case "AttackLeft@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];
                break;
            case "AttackUp@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];
                break;
            case "AttackDown@Player":
                triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];
                break;
        }

        animator.SetTrigger(triggerName);
        cGm.PlayerActionEnd();


        Heal();
    }



    //Set関数
    /// <summary>カメラを追尾</summary>
    private void SetCam()
    {
        /*
        float z = Camera.main.transform.position.z;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, z);
 */
        }

    // Update is called once per frame
    override protected void Update()
    {
        KeyInput();

        base.Update();


    }
    /// <summary>Updateのあと、最後に呼ばれる</summary>
    private void LateUpdate()
    {
        SetCam();
    }
}
