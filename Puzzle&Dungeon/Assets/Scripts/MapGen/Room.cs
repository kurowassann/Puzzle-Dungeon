using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UIElements;
using System.Drawing;
using UnityEngine.EventSystems;
/*
へやが持っていてほしい情報
四隅(長さ)
沸き上限
部屋内のキャラクター
廊下への位置
つながっている部屋
*/


public class Room 
{
    /*
    //オブジェクト
    /// <summary>管理元</summary>
    private RoomManager rm;
    /// <summary>部屋番号</summary>
    private int mRoomId;
    /// <summary>部屋の頂点</summary>
    private Lurd mLurd;
    /// <summary>横の長さ</summary>
    private int mWidth;
    /// <summary>縦の長さ</summary>
    private int mHeight;
    /// <summary>自身のタイル</summary>
    private List<GameObject> mTiles;

    //メンバ変数


    //メンバ関数
    public void OpenTiles()
    {

        /*
        for(int i = 0;i < mTiles.Count;i++) 
        {
            mTiles[i].SetActive(true);
        }
    }

    //Set関数

    //Get関数
    public Point GetRandomPoint()
    {
        Point point = new Point();

        point.X = Random.Range(mLurd.left, mLurd.right);
        point.Y = Random.Range(mLurd.up, mLurd.down);
        
        return point;
    }


    ///<summary>コンストラクタ</summary>
    /// <param name="tlurd"></param>
    /// <param name="twidth"></param>
    /// <param name="theight"></param>
    public Room(Lurd tlurd,int tid,List<GameObject> tgms, RoomManager trm)
    {
        Debug.Log($"部屋が生成　部屋ID:{tid}");
        mLurd = tlurd;
        mRoomId = tid;
        mWidth = mLurd.right-mLurd.left;
        mHeight = mLurd.down-mLurd.up;
        mTiles = tgms;
        Debug.Log(mTiles[10]);
        rm = trm;

    }
*/
}
