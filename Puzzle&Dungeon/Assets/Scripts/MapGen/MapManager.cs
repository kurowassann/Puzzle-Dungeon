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

    //メンバ変数

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



        cRm = new RoomManager();

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
                cMg.TileOpen();
            }
        }
    }

}
