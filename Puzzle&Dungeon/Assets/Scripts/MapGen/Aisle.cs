using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>廊下の情報</summary>
public class Aisle
{
    //オブジェクト
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
    public void OpenAllTiles()
    {
        for(int i = 0; i < cAisleTiles.Count;i++)
        {
            cAisleTiles[i].SetActive(true);
        }
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

    //コンストラクタ
    public Aisle(Lurd[] tlurds, Point[] tpoints,int[] tids,int num,AisleManager tam) 
    {
        cLurds = tlurds;
        
        cAisleTiles= new List<GameObject>();
        cAisleId = num;
        cRoomJoints = new RoomJoint[2];

        cRoomJoints[0] = new RoomJoint(tpoints[0], tids[0], num);
        cRoomJoints[1] = new RoomJoint(tpoints[1], tids[1], num);
        
        cAm = tam;
    }


}
