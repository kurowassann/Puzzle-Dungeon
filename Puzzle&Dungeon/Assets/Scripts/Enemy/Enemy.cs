using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Common;

public class Enemy : CharacterBace
{
    
    //オブジェクト
    protected EnemyManager cEm;
    protected Animator animator;

    //メンバ変数
    [SerializeField]
    string eName; 
    /// <summary>カメラ内に存在しているか</summary>
    protected bool mVisible;
    /// <summary>次に行う行動</summary>
    protected Status mNextAction;
    /// <summary>移動方向</summary>
    protected Vector mVec;
    /// <summary>今向いている方向</summary>
    private Vector mDir;
    /// <summary>プレイヤーまでの経路格納配列</summary>
    protected Common.Vector[] mRoute;
    /// <summary>経路のどこにいるか</summary>
    protected int mRouteNum;
    /// <summary>リスポーンまでのカウント</summary>
    protected int mRespawnCount;

    //メンバ関数
    //初期化処理
    virtual public void Init(EnemyManager tem, GameManager tgm, MapManager tmm, PosId tposId, int thp, string tstr)
    {
        base.Init(tgm,  tmm,  tposId,  thp,  tstr);

        print("エネミーの初期化");
        cEm = tem;
        mRoute = new Common.Vector[0];
        mRouteNum = 0;
        mNextAction = Status.STAY;
        mRespawnCount = 0;

        animator = GetComponent<Animator>();
        mVec = Vector.NONE;
        mDir = Vector.DOWN;

        //animator.SetTrigger("Reset");

    }      
    /// <summary>このターンの行動を決定する</summary>
    public bool Tracking()
    {
        if (mRoute.Length == mRouteNum)
        {
            print("ルートの末端です");
            return false;
        }
        if (mNextAction != Status.STAY)
        {
            print("すでに行動が決まっています。");
            return false;
        }
        Point point = mPosId.GetPos();

        //次の移動先へ
        switch(mRoute[mRouteNum])
        {
            case Common.Vector.UP:
                point.Y--;
                mVec = Vector.UP;
                break;
            case Common.Vector.DOWN:
                point.Y++;
                mVec = Vector.DOWN;
                break;
            case Common.Vector.LEFT:
                point.X--;
                mVec = Vector.LEFT;
                break;
            case Common.Vector.RIGHT:
                point.X++;
                mVec = Vector.RIGHT;
                break;
        }

        if (mDir != mVec && eName == "Ant")
        {
            mNextAction = Status.TRUN;
            mDir = mVec;
            //mGoalPos = transform.position;
            return true;
        }

        if (cMm.CheckTile(point) == Common.TileInfo.ROUTE)
        {
            SetPos(point);
            mNextAction = Status.MOVE;
            mRouteNum++;
            return true;
        }
        if (cMm.CheckTile(point) == Common.TileInfo.PLAYER)
        {
            Debug.Log("移動の方向はプレイヤーです");
            mNextAction = Status.ATTACK;
            //master.EnemyAttack();

            return true;
        }
        return false;
    }
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

        triggerName = SwitchMoveEnd(animationName);

        animator.SetTrigger(triggerName);
    }
    /// <summary>移動終了アニメーション分岐</summary>
    virtual protected string SwitchMoveEnd(string _animName)
    {
        //print(_animName);
        string triggerName = "";

        if (_animName == "MoveRight@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];

        }
        else if (_animName == "MoveLeft@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];

        }
        else if (_animName == "MoveUp@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];

        }
        else if (_animName == "MoveDown@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];

        }

        //switch (_animName)
        //{
        //    case "MoveRight@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];
        //        break;
        //    case "MoveLeft@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];
        //        break;
        //    case "MoveUp@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];
        //        break;
        //    case "MoveDown@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];
        //        break;
        //}

        //print("Enemy移動終了時のアニメーション:" + triggerName);

        return triggerName;
    }
    /// <summary>攻撃終了処理</summary>
    protected override void AttackEnd()
    {
        base.AttackEnd();
        //cEm.EnemyAttack();
        
        var animationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string triggerName = "";
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_MOVE_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_END_DIR);
        ResetTrigger(animator, Common.Common.CHARA_ANIMS_ATTACK_DIR);
        print("aaa" + animationName);
        triggerName = SwitchAttackEnd(animationName);

        print("Enemy攻撃終了時のアニメーション:" + triggerName);

        animator.SetTrigger(triggerName);
    }   
    /// <summary>攻撃終了アニメーション分岐</summary>
    virtual protected string SwitchAttackEnd(string _animName)
    {
        string triggerName = "";

        if (_animName == "AttackRight@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];

        }
        else if( _animName == "AttackLeft@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];

        }
        else if (_animName == "AttackUp@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];

        }
        else if (_animName == "AttackDown@" + eName)
        {
            triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];

        }

        //switch (_animName)
        //{
        //    case "AttackRight@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.RIGHT];
        //        break;
        //    case "AttackLeft@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.LEFT];
        //        break;
        //    case "AttackUp@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.UP];
        //        break;
        //    case "AttackDown@Enemy":
        //        triggerName = Common.Common.CHARA_ANIMS_END_DIR[(int)Vector.DOWN];
        //        break;
        //}

        print("Enemy攻撃終了時のアニメーション:" + triggerName);

        return triggerName;
    }   
    /// <summary>リスポーンまでのカウントを行い条件達成でリスポーンさせる</summary>
    public void RespawnCount()
    {
        //mRespawnCount++;
        if (5 < mRespawnCount)
        {
            Debug.Log("リスポーンします");
            this.gameObject.SetActive(true);
            isActive = true;
            mRespawnCount = 0;
            cEm.Generate("e", this);
        }
    }  
    //リスポーン
    public void Respawn()
    {

    }
    /// <summary>経路のリセット</summary>
    public void ResetRoute()
    {
        Array.Resize(ref mRoute, 0);
        mRouteNum = 0;
    }
    /// <summary>経路記録追加</summary>
    public void AddRoute(Common.Vector tvec)
    {
        Array.Resize(ref mRoute, mRoute.Length + 1);
        mRoute[mRoute.Length - 1] = tvec;
    }
    //記録されているルートの表示
    public void ShowRoute()
    {
        for (int i = 0; i < mRoute.Length; i++)
        {
            Debug.Log(mRoute[i]);
        }
    }

    //Set関数
    /// <summary>決定した行動を予約</summary>
    public void SetAction(Status tstatus)
    {
        mNextAction = tstatus;
    }
    /// <summary>行動の着火</summary>
    public override void SetStatus(Status tstatus)
    {
        base.SetStatus(tstatus);
        string triggerName = "";
        switch (tstatus)
        {
            case Status.ATTACK:
                triggerName = Common.Common.CHARA_ANIMS_ATTACK_DIR[(int)mVec];
                break;
            case Status.MOVE:
            case Status.TRUN:
                triggerName = Common.Common.CHARA_ANIMS_MOVE_DIR[(int)mVec];
                break;
			default:
				return;
        }
        //print("敵の行動開始:" + triggerName);
        animator?.SetTrigger(triggerName);
    }
    
    //Get関数
    /// <summary>今が経路の何手目にいるか</summary>
    public int GetNum()
    {
        return mRouteNum;
    }
/// <summary>経路が何手か</summary>
    public int GetRoute()
    {
        return mRoute.Length;
    }
    /// <summary>このターンどの行動を取るか</summary>
    public Status GetNextAction()
    {
        return mNextAction;
    }
    
}
