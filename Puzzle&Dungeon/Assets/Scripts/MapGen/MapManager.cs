using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>生成したマップを管理するクラス</summary>
public class MapManager : MonoBehaviour
{
    //SerializeField unity内参照
    [Tooltip("デバッグを表示するか"), SerializeField]
    private bool isDebug;
    [Tooltip("自分で開始するか"),SerializeField]
    private bool isStart;
    [Tooltip("マップ生成オブジェクト"), SerializeField]
    private GameObject mapGen;

    //オブジェクト
    /// <summary>部屋管理用クラス</summary>
    private RoomManager cRm;
    /// <summary>マップ生成用クラス</summary>
    private MapGeneretor cMg;
    /// <summary>タイル情報</summary>
    private string[,] cTiles;
    /// <summary>タイルのオブジェクト</summary>
    private GameObject[,] cTileObjects;

    //メンバ変数
    private int mNum;

    //メンバ関数
    //private

    //public
    public void Init()
    {
        //マップ生成オブジェクトの生成
        GameObject clone = Instantiate(mapGen, this.transform);
        cMg = clone.GetComponent<MapGeneretor>();
        if(isStart)
        {
            cMg.Init(40,40,15);
        }

        //生成したマップの情報を受け取る
        cTiles = cMg.GetStrings();
        cTileObjects = cMg.GetTileObjects();
        cRm = cMg.GetRoomManager();


        mNum = 0;
    }



    /// <summary>スタート関数</summary>
    public void Start()
    {
        Debug.Log($"{this.name}スタート");

        if(isStart) { Init(); }
    }

    public void Update() 
    {
        if(isDebug)
        {
            if(Input.GetKeyDown(KeyCode.Space) ) 
            {
                if (mNum < cRm.GetRoomCount())
                {
                    print($"部屋番号{mNum}を表示します");
                    cRm.OpenOneRoom(mNum); 
                    
                    mNum++;
                }
                else
                {
                    print($"表示できる部屋は{mNum}で終わりです");
                }

            }

            if(Input.GetKeyDown(KeyCode.O))
            {
                print("マップ全体を表示");
                cMg.TileOpen();
            }
        }
    }

}
