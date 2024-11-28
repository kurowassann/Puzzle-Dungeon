using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


using Data;

public class StageSelectManager : MonoBehaviour
{
    const float TMP_SIZE = 2.6f / 1.7f;


    enum CursorLine
    {
        OPTIONS,
        STAGES,
    }


    [Header("ステージ選択系")]
    [Tooltip("ステージの最大数")]
    const float MAX_STAGE_NUM = 4;
    [Tooltip("ステージ点の親"), SerializeField]
    Transform stagesParent;
    [Tooltip("ステージ点のPrefab"), SerializeField]
    GameObject stagePointPrefab;
    [Tooltip("ステージ点のリスト")]
    List<GameObject> stages;

    [Header("ステージ選択のボタン系")]

    [Tooltip("ボタンのリスト"), SerializeField]
    List<StageSelectButton> stageSelectButtons;
    [Tooltip("左ボタンのアニメーター"), SerializeField]
    Animator leftAnimator;
    [Tooltip("右ボタンのアニメーター"), SerializeField]
    Animator rightAnimator;


    [Tooltip("現在選択中のステージ番号,0始まり"), SerializeField]
    public int currentIndex;



    [Header("ステージの詳細情報系")]
    [Tooltip("ステージ情報のマスターデータ")]
    JsonStageMaster jsonStageMaster;
    [Tooltip("敵のマスターデータ")]
    JsonEnemyMaster jsonEnemyMaster;
    [Tooltip("ステージの詳細パネル"), SerializeField]
    GameObject stageDetailPanel;
    [Tooltip("ステージ名"), SerializeField]
    Text stageName;
    [Tooltip("ステージの説明テキスト"), SerializeField]
    Text detailText;
    [Tooltip("ステージに出てくる雑魚アイコン"), SerializeField]
    Image detailWeakEnemyIcon;
    [Tooltip("ステージに出てくるボスアイコン"), SerializeField]
    Image detailBossEnemyIcon;



    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!OptionManager.Instance.isOptionOpened)
        {
            // 入力の受け取り
            if (SelectInput())
            {
                // ステージ情報の表示
                StageDetailDisplay();
                UIScaler();
            }
        }
        
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Init()
    {
        // ステージ情報の取得
        jsonStageMaster = LoadData.LoadAllStageData();
        // 敵情報取得
        jsonEnemyMaster = LoadData.LoadAllEnemyData();

        // ステージ点の生成
        StagePointGen();
        // todo:直前にクリアしたステージのカーソル位置を記憶する
        currentIndex = 0;

        // ボタンの取得
        // StageSelectButton型を全取得
        stageSelectButtons = FindObjectsOfType<StageSelectButton>().ToList();


        // ステージ情報の表示
        StageDetailDisplay();
        
        UIScaler();
    }

    /// <summary>
    /// ステージの数だけポイントを生成
    /// </summary>
    void StagePointGen()
    {
        if (jsonStageMaster == null)
        {
            print("ステージ情報が読み込めてない");
            return;
        }
        stages = new List<GameObject>();
        // 生成
        for (int i = 0; i < jsonStageMaster.stage_datas.Length; i++)
        {
            
            if (jsonStageMaster.stage_datas[i].stage_id != Common.Common.ERROR_ID)
            {
                var stage = Instantiate(stagePointPrefab, stagesParent);
                if (stage != null)
                {
                    var stageIdName = jsonStageMaster.stage_datas[i].stage_id_name;
                    stage.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + stageIdName + "_BG");
                    stages.Add(stage);
                }
                else
                {
                    print("生成に失敗");
                }
            }
        }
    }


    /// <summary>
    /// ステージ選択状態の入力処理
    /// </summary>
    bool SelectInput()
    {
        // todo:黒点を移動できる、端はループ
        var ret = false;

        // カーソルが左に移動
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = stages.Count - 1;
            }
            leftAnimator.SetTrigger("Push");
            ret = true;
        }

        // カーソルが右に移動
        if (Input.GetKeyDown(KeyCode.D))
        {

            currentIndex++;
            if (currentIndex > stages.Count - 1)
            {
                currentIndex = 0;
            }
            rightAnimator.SetTrigger("Push");

            ret = true;
        }

        // 各ボタンの処理
        foreach (var button in stageSelectButtons)
        {
            // ボタンに対応するキーが押されている
            if (Input.GetKey(button.GetKeyCode()))
            {
                // タイマーが超過したか
                if(button.AddTime())
                {
                    // 長押し完了処理
                    button.GetAction().Invoke();
                    button.ResetTimer();
                }
            }
            else
            {
                // タイマーの減算
                button.SubTime();
            }
        }
        return ret;
    }

    /// <summary>
    /// サイズ調整でカーソルを表現
    /// </summary>
    void UIScaler()
    {
        // ステージのサイズ変更
        for (int i = 0; i < stages.Count; i++)
        {
            if (i == currentIndex)
            {
                stages[i].transform.localScale = Vector3.one * TMP_SIZE;
            }
            else
            {
                stages[i].transform.localScale = Vector3.one;
            }
        }
    }

    /// <summary>
    /// ステージの詳細情報表示
    /// </summary>
    void StageDetailDisplay()
    {
        stageName.text = jsonStageMaster.stage_datas[currentIndex].stage_name;
        detailText.text = jsonStageMaster.stage_datas[currentIndex].stage_desc;
        var len = jsonStageMaster.stage_datas[currentIndex].enemy_datas.Length;
        var cnt = 0;
        for(int i = 0; i < len; i++)
        {
            if (jsonStageMaster.stage_datas[currentIndex].enemy_datas[i] != null)
            {
                cnt++;
            }
        }
        if(cnt < 2)
        {
            var eId = jsonStageMaster.stage_datas[currentIndex].enemy_datas[0].enemy_id;
            var eName = jsonEnemyMaster.GetEnemyData(eId).enemy_id_name;

            detailWeakEnemyIcon.sprite = Resources.Load<Sprite>("Images/Enemy/" + eName + "/" +eName +  "_anim01");
            detailWeakEnemyIcon.gameObject.SetActive(true);
            detailBossEnemyIcon.gameObject.SetActive(false);
        }
        else
        {
            var eId = jsonStageMaster.stage_datas[currentIndex].enemy_datas[0].enemy_id;
            var eName = jsonEnemyMaster.GetEnemyData(eId).enemy_id_name;
            detailWeakEnemyIcon.sprite = Resources.Load<Sprite>("Images/Enemy/" + eName + "/" + eName + "_anim01");

            //print("Images/Enemy/" + eName + "/" + eName + "_anim01");

            eId = jsonStageMaster.stage_datas[currentIndex].enemy_datas[1].enemy_id;
            eName = jsonEnemyMaster.GetEnemyData(eId).enemy_id_name;
            detailBossEnemyIcon.sprite = Resources.Load<Sprite>("Images/Enemy/" + eName + "/" + eName + "_anim01");

            detailWeakEnemyIcon.gameObject.SetActive(true);
            detailBossEnemyIcon.gameObject.SetActive(true);

        }
    }

    /// <summary>
    /// 決定ボタン長押し
    /// </summary>
    public void OnSubmitButton()
    {
        print("ステージ決定時の処理");
        var stageId = jsonStageMaster.stage_datas[currentIndex].stage_id;
        PlayerPrefs.SetInt(Common.Common.KEY_SELECTED_STAGE_ID, stageId);
        PlayerPrefs.Save();
        
        Common.Common.LoadScene("GameScene");

    }

    /// <summary>
    /// タイトルに戻るボタン
    /// </summary>
    public void OnBackTitleButton()
    {
        Common.Common.LoadScene("Title");
    }

    /// <summary>
    /// オプションボタン
    /// </summary>
    public void OnOptionButton()
    {
        OptionManager.Instance.OpenOption();
    }
}