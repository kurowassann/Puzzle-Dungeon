using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>廊下の情報</summary>
public class Aisle
{
    //オブジェクト
    /// <summary>通路の座標</summary>
    private List<GameObject> cAisleTiles;
    /// <summary>自身のID</summary>
    private int cAisleId;
    /// <summary>左右の部屋ID</summary>
    private int[] cRoomId;

    //メンバ変数

    //メンバ関数
    /// <summary>廊下のポイントの追加</summary>
    public void AddTilePoints()
    {

    }


    //コンストラクタ
    public Aisle(int num) 
    {
        cAisleTiles= new List<GameObject>();
        cAisleId = num;
        cRoomId = new int[2];
    }


}
