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
    /// <summary>ターンの終わりを確認</summary>
    private bool TurnCheck()
    {
        if(cPlayer.GetAction())
        {
            return true;
        }

        return false;
    }
    /// <summary>ターンの終了</summary>
    private void TurnEnd()
    {
        cPlayer.TurnEnd();
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
        PosId posId = cMm.GetRandomPlayer();
        cPlayer.Init(this, cMm,posId,1,"p");
        cPlayer.transform.position = cMm.GetTilePos(posId.GetPos());
    }

    //
    private void Update()
    {
        if(TurnCheck())
        {
            TurnEnd();
        }


        
    }
}
