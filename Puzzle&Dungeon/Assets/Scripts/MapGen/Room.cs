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
    
    //オブジェクト
    /// <summary>管理元</summary>
    private RoomManager cRm;
    /// <summary>部屋番号</summary>
    private int cRoomId;
    /// <summary>部屋の頂点</summary>
    private Lurd cLurd;
    /// <summary>横の長さ</summary>
    private int cWidth;
    /// <summary>縦の長さ</summary>
    private int cHeight;
    /// <summary>自身のタイル</summary>
    private List<GameObject> cTiles;

    //メンバ変数


    //メンバ関数
    //private
    //public
    public void OpenTiles()
    {
        for(int i = 0;i < cTiles.Count;i++) 
        {
            cTiles[i].SetActive(true);
        }
    }

    //Set関数
    /// <summary>エリアのオブジェクトを取得</summary>
    public void SetTiles(List<GameObject> ttiles)
    {
        cTiles = ttiles;
    }

    //Get関数
    /// <summary>ランダムな部屋の位置を渡す</summary>
    public Point GetRandomPoint()
    {
        Point point = new Point();

        point.X = Random.Range(cLurd.GetValue(Value.LEFT), cLurd.GetValue(Value.RIGHT));
        point.Y = Random.Range(cLurd.GetValue(Value.TOP), cLurd.GetValue(Value.BOTTOM));
        
        return point;
    }
    /// <summary>自身を渡す</summary>
    public Room GetRoom()
    {
        return this;
    }
    /// <summary>横幅を渡す</summary>
    public int GetWidth()
    {
        return cWidth;
    }
    /// <summary>縦幅を返す</summary>
    public int Height()
    {
        return cHeight;
    }
    //
    public int GetValue(Value tvalue)
    {
        return cLurd.GetValue(tvalue);
    }



    ///<summary>コンストラクタ</summary>
    public Room(Lurd tlurd,int tid, RoomManager trm)
    {
        Debug.Log($"部屋が生成　部屋ID:{tid}");
        cLurd = tlurd;
        cRoomId = tid;
        cWidth = cLurd.GetValue(Value.RIGHT)-cLurd.GetValue(Value.LEFT);
        cWidth = cLurd.GetValue(Value.BOTTOM) -cLurd.GetValue(Value.TOP);
        cTiles = new List<GameObject>();
        cRm = trm;

    }

}
