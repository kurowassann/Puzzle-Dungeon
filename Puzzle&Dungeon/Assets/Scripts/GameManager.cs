using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>ゲームを動かす</summary>
public class GameManager : MonoBehaviour 
{
    //デバック用
    [Tooltip("デバック確認フラグ"), SerializeField]
    private bool isDebug;
    [Tooltip("プレイヤ確認フラグ"), SerializeField]
    private bool isPlayer;
    [Tooltip("敵確認フラグ"), SerializeField]
    private bool isEnemy;
    [Tooltip("アイテム確認用フラグ"), SerializeField]
    private bool isItem;

    //定数
    [Tooltip("マップの管理オブジェクト"),SerializeField]
    private MapManager cMm;
    [Tooltip("プレイヤ管理オブジェクト"), SerializeField]
    private Player cPlayer;
    [Tooltip("敵管理オブジェクト"), SerializeField]
    private EnemyManager cEm;
    [Tooltip("アイテム管理オブジェクト"), SerializeField]
    private BonusManager cBm;
    [Tooltip("アイテムオブジェクト"), SerializeField]
    private GameObject oItem;
    

    //メンバ変数
    /// <summary>ステータス変更時の処理用</summary>
    private bool isStatusChange;
    /// <summary>ステータス</summary>
    private GameStatus mStatus;
    /// <summary>ターン終了用タイマー</summary>
    private float mEndTimer;
    //
    private GameObject mItem;
    /// <summary>アイテムの座標リスト</summary>
    private Point mItemPos;

    

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

    /// <summary>アイテムの生成</summary>
    private void ItemSpawn(int num)
    {
        mItem.SetActive(true);

        Point pos = cMm.GetPosId(num).GetPos();

        mItemPos = pos;

        Vector3 vec = cMm.GetTilePos(pos);

        mItem.transform.position = vec;
    }


    /// <summary>ターンの終わりを確認</summary>
    private bool TurnCheck()
    {
        if(!isEnemy && cPlayer.GetAction())
        {
            return true;
        }
        else if(cPlayer.GetAction() && cEm.GetAction())
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
    /// <summary>プレイヤの攻撃</summary>
    public void PlayerAttack(Point tpos)
    {
        cEm.Damege(tpos);
    }
    //
    public void EnemyAttack()
    {
        cPlayer.Damage();
    }
    /// <summary>プレイヤの行動の完了を受け取る</summary>
   public void PlayerActionEnd()
    {
        SetStatus(GameStatus.ACTION);
        if (isEnemy)
        {
            cEm.EnemyActionSelect();
        }

    }
    /// <summary>部屋が生成される</summary>
    public void RoomOpen(int num)
    {
        if(isEnemy)
        {
            cEm.Spawn(num);
        }

        if(isItem)
        {
            ItemSpawn(num);
        }
    }
    //
    public void ItemCheck(Point tpos)
    {
        if (mItemPos == tpos)//移動場所にアイテムがあった
        {
            switch( cBm.LotNextBonus1())
            {
                case AllBonus.HEELUP:
                    cPlayer.HpUp();
                    break;
            }



            //アイテム消去
                mItemPos = new Point(0, 0);
            mItem.SetActive(false);
        }
        
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
        //変数の初期化
        mStatus = new GameStatus();
        SetStatus(GameStatus.STAY);
        mEndTimer = 0;

        //アイテムを生成しておく
        var clone = Instantiate(oItem, this.transform);
        mItem = clone;
        mItem.SetActive(false);
        mItemPos = new Point(0,0);

        //マップの生成
        cMm.Init(this);

        //プレイヤの生成
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
