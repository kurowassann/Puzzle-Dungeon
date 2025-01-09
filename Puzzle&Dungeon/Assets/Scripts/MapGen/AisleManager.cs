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
