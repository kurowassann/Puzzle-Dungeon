using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>廊下をまとめて管理する</summary>
public class AisleManager
{
    //オブジェクト
    /// <summary>廊下クラス</summary>
    private List<Aisle> cAisles;
    /// <summary>接続部のリスト</summary>
    private List<RoomJoint> cJoints;


    //メンバ変数
    /// <summary>廊下の数、IDに使う</summary>
    private int mAisleCount;

    //メンバ関数
    //private
    

    //public
    /// <summary>初期化処理</summary>
    public void Init()
    {
        mAisleCount = 0;
        cAisles = new List<Aisle>();
        cJoints = new List<RoomJoint>();
    }
    /// <summary>廊下の追加生成</summary>
    public int  AddAisle(Lurd[] tlurds, Point[] tpoints, int[] tids)
    {
        cAisles.Add(new Aisle(tlurds, tpoints, tids,mAisleCount,this));
        mAisleCount++;
        return mAisleCount - 1;
    }
    /// <summary>廊下タイルの追加</summary>
    public void AddAisleTiles()
    {

    }
    /// <summary>接続部分をリスト化</summary>
    public void CreateJointList()
    {
        for(int i = 0; i < cAisles.Count; i++) 
        {
            RoomJoint[] Rjs = cAisles[i].GetRoomJoint();
            for(int j = 0;j < Rjs.Length; j++)
            {
                cJoints.Add(Rjs[j]);
                Debug.Log($"接続部分は{Rjs[j].GetPos()}です");
            }
        }
    }
    /// <summary>プレイヤが接続部分にいるか調べる</summary>
    public PosId ChangeJoint(PosId tpi)
    {
        int num = -1;

        for(int i = 0; i < cJoints.Count;i++)
        {
            if(tpi.GetPos() == cJoints[i].GetPos())
            {
                num = i; 
                break;
            }
        }

        if(num == -1)
        {
            Debug.Log("部屋変化なし");
            return tpi;
        }

        if(tpi.GetRA() == RoomAisle.ROOM)
        {
            tpi.SetId(cJoints[num].GetId(RoomAisle.AISLE),RoomAisle.AISLE);

        }
        else if(tpi.GetRA() == RoomAisle.AISLE)
        {
            tpi.SetId(cJoints[num].GetId(RoomAisle.ROOM), RoomAisle.ROOM);

        }

        return tpi;
    }
    /// <summary>廊下の表示</summary>
    public void OpenOneAisle(int num)
    {
        cAisles[num].OpenAllTiles();
    }

    //Set関数

    //Get関数
    /// <summary>廊下の数を返す</summary>
    public int GetAisleCount()
    {
        return mAisleCount;
    }
    /// <summary>指定されたIDの廊下を返す</summary>
    public Aisle GetAisle(int num)
    {
        return cAisles[num];
    }

    //コンストラクタ
    public AisleManager() 
    {
        Init();
    }
}
