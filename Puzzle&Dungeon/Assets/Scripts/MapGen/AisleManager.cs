using Common;
using System.Collections;
using System.Collections.Generic;
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
    }
    /// <summary>廊下の追加生成</summary>
    public void AddAisle()
    {
        cAisles.Add(new Aisle(mAisleCount));
    }
    /// <summary>廊下タイルの追加</summary>
    public void AddAisleTiles()
    {

    }

    //Set関数

    //Get関数


    //コンストラクタ
    public AisleManager() 
    {
        Init();
    }
}
