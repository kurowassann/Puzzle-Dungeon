using Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

/// <summary>ゲームを動かす</summary>
public class GameManager : MonoBehaviour 
{
    //オブジェクト
    [Tooltip("マップの管理オブジェクト"),SerializeField]
    private MapManager cMm;
    [Tooltip("プレイヤ管理オブジェクト"), SerializeField]
    private Player cPlayer;

    //メンバ変数

    //メンバ関数
    //private
    /// <summary>プレイヤの初期位置を決める</summary>
    private PosId SelectPlayerFirstPos()
    {
        PosId posId = cMm.GetRandomPlayer();

        return posId;
    }

    //public
   

    //Set関数

    //Get関数

    /// <summary>起動時に呼ぶ</summary>
    private void Start()
    {
        //マップの生成
        cMm.Init();

        //プレイヤの生成
        //生成位置の決定
        PosId posId = SelectPlayerFirstPos();
        cPlayer.Init(this,posId,1,"p");
        cPlayer.transform.position = cMm.GetTilePos(posId.GetPos());


    }

    //
    private void Update()
    {
        
    }
}
