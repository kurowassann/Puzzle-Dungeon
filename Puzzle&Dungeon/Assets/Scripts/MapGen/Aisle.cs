using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>廊下の情報</summary>
public class Aisle
{
    //オブジェクト
    /// <summary>表示されるべきものか</summary>
    private bool isActive;
    /// <summary>管理元</summary>
    private AisleManager cAm;
    /// <summary>通路の座標</summary>
    private List<GameObject> cAisleTiles;
    /// <summary>自身のID</summary>
    private int cAisleId;
    /// <summary>左右の部屋ID</summary>
    private RoomJoint[] cRoomJoints;
    /// <summary>位置情報</summary>
    private Lurd[] cLurds;

    //メンバ変数

    //メンバ関数
    /// <summary>廊下のポイントの追加</summary>
    public void AddTilePoints()
    {

    }
    /// <summary>廊下の表示</summary>
    public bool OpenAllTiles()
    {
        if(isActive)
        {
            return true;
        }

        for(int i = 0; i < cAisleTiles.Count;i++)
        {
            cAisleTiles[i].SetActive(true);
        }
        isActive = true;

        return false;
    }

    //Set関数
    //
    public void SetTiles(List<GameObject> tgos)
    {
        for(int i = 0;i < tgos.Count;i++)
        {
            cAisleTiles.Add(tgos[i]);
        }
    }

    //Get関数
    public int GetLength()
    {
        return cLurds.Length;
    }
    //
    public Lurd GetLurd(int num)
    {
        return cLurds[num];
    }
    /// <summary>接続部分を返す</summary>
    public RoomJoint[] GetRoomJoint()
    {
        return cRoomJoints;
    }

    //コンストラクタ
    public Aisle(Lurd[] tlurds, RoomJoint[] trj,int num,AisleManager tam) 
    {
        cLurds = tlurds;
        
        cAisleTiles= new List<GameObject>();
        cAisleId = num;
        cRoomJoints = trj;
        cRoomJoints[0].SetAisleId(num);
        cRoomJoints[1].SetAisleId(num);


        cAm = tam;
    }


}
