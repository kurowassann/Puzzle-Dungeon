using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RoomManager 
{
    //オブジェクト
    /// <summary>部屋の管理</summary>
    private List<Room> cRooms;

    //メンバ変数
    //現在の部屋数
    private int mRoomCount;

    //メンバ関数
    /// <summary>初期化処理</summary>
    private void Init()
    {
       cRooms = new List<Room>();
        mRoomCount = 0;
    }
    //部屋の追加
    public int AddRoom(Lurd tlurd)
    {
        cRooms.Add(new Room(tlurd,mRoomCount, this));

        mRoomCount++; 
        return mRoomCount-1;
    }
    /// <summary>指定した部屋を照らす</summary>
    public void OpenOneRoom(int num)
    {
        cRooms[num].OpenTiles();
    }

    //Set関数


    //Get関数
    /// <summary>ランダムな部屋のランダムなポジションを返す</summary>
    public Point GetRandomRoom()
    {
        Point point;
        int num = Random.Range(0, cRooms.Count);
        point = cRooms[num].GetRandomPoint();


        return point;
    }
    /// <summary>プレイヤの生成部屋の表示をONに</summary>
    public Point GetRandomRoom(int tnum)
    {
        Point point;
        int num = Random.Range(0, cRooms.Count);
        point = cRooms[num].GetRandomPoint();

        cRooms[num].OpenTiles();

        return point;
    }
    /// <summary>ルームの数を返す</summary>
    public int GetRoomCount()
    {
        return cRooms.Count;
    }
    /// <summary>指定された部屋を返す</summary>
    public Room GetRoom(int num)
    {
        return cRooms[num].GetRoom();
    }

    public  RoomManager()
    {
        //Debug.Log($"{this}コンストラクタ");

        Init();
    }
    
}
