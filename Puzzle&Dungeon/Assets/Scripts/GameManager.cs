using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

/// <summary>ゲームを動かす</summary>
public class GameManager : MonoBehaviour 
{
    [Tooltip("デバック確認フラグ"), SerializeField]
    private bool isDebug;

    //定数
    [Tooltip("マップの管理オブジェクト"),SerializeField]
    private MapManager cMm;
    [Tooltip("プレイヤ管理オブジェクト"), SerializeField]
    private Player cPlayer;
    [Tooltip("敵管理オブジェクト"), SerializeField]
    private EnemyManager cEm;
    

    //メンバ変数
    /// <summary>ステータス変更時の処理用</summary>
    private bool isStatusChange;
    /// <summary>ステータス</summary>
    private GameStatus mStatus;
    /// <summary>ターン終了用タイマー</summary>
    private float mEndTimer;
    

    //メンバ関数
    //private

    /// <summary>ステータス毎の分岐</summary>
    private void StatusBranch()
    {
        switch (mStatus) 
        {
            case GameStatus.STAY:
                BranchStay();
                break;
            case GameStatus.ACTION:
                BranchAction();
                break;
            case GameStatus.COUNT:
                BranchCount();
                break;
        }
    }
    /// <summary>ステイ時</summary>
    private void BranchStay()
    {
        if(isStatusChange)
        {
            if (isDebug) 
            {
                print("ゲームモード：待機");
            }
            isStatusChange = false;
        }

    }
    /// <summary>アクション時</summary>
    private void BranchAction()
    {
        if (isStatusChange)
        {
            if (isDebug) 
            {
                print("ゲームモード：アクション");
            }
            isStatusChange = false;
        }

        if (TurnCheck())
        {
            SetStatus(GameStatus.COUNT);
        }
    }
    /// <summary>カウント時</summary>
    private void BranchCount()
    {
        if (isStatusChange)
        {
            if (isDebug) 
            {
                print("ゲームモード：カウント");
            }
            mEndTimer = 0;
            isStatusChange= false;
        }
        mEndTimer += Time.deltaTime;
        if(0.1f < mEndTimer)
        {
            TurnEnd();
        }
    }

    /// <summary>ターンの終わりを確認</summary>
    private bool TurnCheck()
    {
        if(cPlayer.GetAction() && cEm.GetAction())
        {
            return true;
        }

        return false;
    }
    /// <summary>ターンの終了</summary>
    private void TurnEnd()
    {
        cPlayer.TurnEnd();

        SetStatus(GameStatus.STAY);
    }

    //public
   public void PlayerActionEnd()
    {
        SetStatus(GameStatus.ACTION);
        cEm.EnemyActionSelect();
    }
    public void RoomOpen(int num)
    {
        cEm.Spawn(num);
    }

    //Set関数
    /// <summary>ステータスチェンジ</summary>
    private void SetStatus(GameStatus tstatus)
    {
        mStatus = tstatus;
        isStatusChange = true;
    }

    //Get関数
    public bool GetDebug()
    {
        return isDebug;
    }
    public Point GetPlayer()
    {
        return cPlayer.GetPos();
    }

    /// <summary>起動時に呼ぶ</summary>
    private void Start()
    {

        mStatus = new GameStatus();
        mStatus = GameStatus.STAY;
       isStatusChange = false;
        mEndTimer = 0;

        //マップの生成
        cMm.Init(this);

        //プレイヤの生成
        //生成位置の決定
        PosId posId = cMm.GetRandomPlayer();
        cPlayer.Init(this, cMm,posId,1,"p");


        //敵管理初期化
        cEm.Init(this,cMm);



    }

    //
    private void Update()
    {
        StatusBranch();   

    }
}
