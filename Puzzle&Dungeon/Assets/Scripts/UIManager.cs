using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Tooltip("仮の横幅")]
    private float width = 10;
    [Tooltip("仮の縦幅")]
    private float height = 10;



    [Header("ミニマップ系")]
    [Tooltip("ミニマップパネル")]
    private GameObject mapPanel;


    [Header("グリッド線系")]
    // todo:グリッド線
    [Tooltip("グリッド用のLineRendererPrefab")]
    private GameObject gridPrefab;
    [Tooltip("グリッド生成用の親オブジェクト")]
    private Transform gridTransform;



    [Header("HP系")]
    [Tooltip("HPプレハブ")]
    private GameObject playerHPPrefab;
    [Tooltip("プレイヤーのHPの親オブジェクト")]
    private Transform playerHPParent;
    [Tooltip("HPのリスト")]
    private List<Transform> playerHPList;


    [Header("ログ系")]
    [Tooltip("ログの最大保存数")]
    private const int MAX_LOG_NAM = 5;
    [Tooltip("ログが表示される時間")]
    private const float LOG_ACTIVE_TIME = 1.2f;
    [Tooltip("ログが消える時間")]
    private const float LOG_FADE_OUT = 0.5f;
    [Tooltip("ログ表示用のタイマー")]
    private float logTimer = 0; 
    [Tooltip("ログのパネル")]
    private Image logPanel;
    [Tooltip("出力先のテキストオブジェクト")]
    private Text logTextOutputObj;
    [Tooltip("ログのリスト")]
    private List<string> logTexts;

    //todo:攻撃、倒した、

    // Start is called before the first frame update
    void Start()
    {
        // UIの初期化
        UIInit();


        //GeneratePlayerHP(2);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    AddLog("プレイヤーの攻撃");
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    AddLog("敵の攻撃");
        //}

        if(Input.GetKeyDown(KeyCode.M))
        {
            mapPanel.SetActive(!mapPanel.activeSelf);
        }

        // ログの表示を制御
        LogControll();
    }

    /// <summary>
    /// UIの初期化処理
    /// </summary>
    void UIInit()
    {
        mapPanel = GameObject.Find("PanelMap");
        mapPanel.SetActive(true);

        playerHPPrefab = Resources.Load<GameObject>("Prefabs/HPImagePrefab");
        playerHPParent = GameObject.Find("PlayerHPParent").transform;

        logPanel = GameObject.Find("PanelLog").GetComponent<Image>();
        var col = logPanel.color;
        col.a = 0;
        logPanel.color = col;

        logTextOutputObj = GameObject.Find("TextLog").GetComponent<Text>();
        col = logTextOutputObj.color;
        col.a = 0;
        logTextOutputObj.color = col;

        logTexts = new List<string>();
        logTimer = 0;

        // グリッド線の描画
        //CreateGrid();
    }

    /// <summary>
    /// グリッド線描画処理
    /// </summary>
    void CreateGrid()
    {
        // Prefab読み込み
        gridPrefab = Resources.Load<GameObject>("Prefabs/Line");
        // 生成する親オブジェクト取得
        gridTransform = transform.GetChild(0);

        // 縦線の描画
        for(int i = 0; i <= width; i++)
        {
            var line = Instantiate(gridPrefab, gridTransform).GetComponent<LineRenderer>();
            line.startWidth = 0.01f;
            line.endWidth = 0.01f;
            Vector3[] linePoss = new Vector3[2]
            {
                new Vector3(i,       0, 0),
                new Vector3(i, -height, 0),
            };
            line.SetPositions(linePoss);
        }

        // 横線の描画
        for (int i = 0; i <= height; i++)
        {
            var line = Instantiate(gridPrefab, gridTransform).GetComponent<LineRenderer>();
            line.startWidth = 0.01f;
            line.endWidth = 0.01f;
            Vector3[] linePoss = new Vector3[2]
            {
                new Vector3(    0, -i, 0),
                new Vector3(width, -i, 0),
            };
            line.SetPositions(linePoss);
        }
    }



    /// <summary>
    /// プレイヤー初期化時のHPを生成する処理
    /// </summary>
    /// <param name="_maxHP">HPの最大値</param>
    public void GeneratePlayerHP(int _maxHP)
    {
        playerHPList = new List<Transform>();
        playerHPPrefab = Resources.Load<GameObject>("Prefabs/HPImagePrefab");
        playerHPParent = GameObject.Find("PlayerHPParent").transform;
        for (int i = 0; i < _maxHP; i++)
        {
            var obj = Instantiate(playerHPPrefab, playerHPParent);
            playerHPList.Add(obj.transform);
        }
    }



    /// <summary>
    /// プレイヤーのHPをUIに表示する処理
    /// </summary>
    /// <param name="_currentHP">現在のHP</param>
    public void DisplayPlayerHP(int _currentHP)
    {
        foreach (var obj in playerHPList)
        {
            obj.GetChild(1).gameObject.SetActive(false);
        }

        for (int i = 0; i < _currentHP; i++)
        {
            playerHPList[i].GetChild(1).gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// ログ制御処理
    /// 時間で徐々に消える
    /// </summary>
    void LogControll()
    {
        // タイマーが正の間は表示し続ける
        if (logTimer > 0)
        {
            // タイマーの減算
            logTimer -= Time.deltaTime;
            // ログの表示
            logPanel.gameObject.SetActive(true);
            var col = logPanel.color;
            col.a = 1;
            logPanel.color = col;
            logTextOutputObj.gameObject.SetActive(true);
            col = logTextOutputObj.color;
            col.a = 1;
            logTextOutputObj.color = col;

        }
        else
        {
            // フェードアウトさせる処理
            if (logTextOutputObj.color.a > 0)
            {
                var col = logTextOutputObj.color;
                col.a -= Time.deltaTime / LOG_FADE_OUT;
                logTextOutputObj.color = col;
                col = logPanel.color;
                col.a -= Time.deltaTime / LOG_FADE_OUT;
                logPanel.color = col;
            }
            else
            {
                logPanel.gameObject.SetActive(false);
                logTextOutputObj.gameObject.SetActive(false);
            }
        }
    }


    /// <summary>
    /// ログを追加する処理
    /// 追加時にあふれた分を自動で削除する
    /// </summary>
    /// <param name="_logText">追加するテキスト</param>
    public void AddLog(string _logText)
    {
        // ログの追加
        logTexts.Add(_logText);
        // 古いものから超過している分削除
        if (logTexts.Count > MAX_LOG_NAM)
        {
            logTexts.RemoveRange(0, logTexts.Count - MAX_LOG_NAM);
        }


        // 表示処理の実行
        if (logTexts.Count != 0)
        {
            DisplayLog();
        }
    }

    /// <summary>
    /// ログを表示する処理
    /// </summary>
    void DisplayLog()
    {
        // タイマーのセット
        logTimer = LOG_ACTIVE_TIME;
        // 表示ログの準備
        var logBuilder = new StringBuilder();
        // テキストに改行付きで一行ずつ追加
        foreach (var log in logTexts)
        {
            logBuilder.AppendLine(log);
        }
        // 出力
        logTextOutputObj.text = logBuilder.ToString().TrimEnd();
    }
}
