using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Json;


public class OptionManager : MonoBehaviour
{
    // todo:applyの確認ダイアログ

    /// <summary>
    /// オプション画面の選択メニュー
    /// </summary>
    public enum SelectableOption
    {
        SOUND,
        LIGHT,

        CLOSE_OPT,
        BACK_TITLE,
    }

    /// <summary>
    /// 設定項目の中身
    /// </summary>
    public enum SettingObject
    {
        TYPE,
        VALUE,
        SLIDER,
    }

    [Tooltip("インスタンス")]
    public static OptionManager Instance { get; private set; }

    [Tooltip("オプションを開いているかどうか")]
    public bool isOptionOpened { get; private set; }


    [Tooltip("設定の親オブジェクト配列"), SerializeField]
    Transform[] settingParents;
    [Tooltip("保存テキスト"), SerializeField]
    Transform saveSettingText;
    [Tooltip("非保存で閉じるテキスト"), SerializeField]
    Transform unsaveSettingText;
    [Tooltip("タイトルに戻るテキスト"), SerializeField]
    Transform backToTitleText;

    [Tooltip("選択中のカーソル"), SerializeField]
    GameObject selectedCursor;
    [Tooltip("選択中のメニュー"), SerializeField]
    SelectableOption selectedMenu;


    [Tooltip("描画するキャンバス")]
    Canvas canvas;
    [Tooltip("タイトルシーンかどうか")]
    bool isTitleScene;
    [Tooltip("ライト用のキャンバス")]
    Canvas canvasLight;


    /// <summary>
    /// シングルトン化処理
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoded;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    /// <summary>
    /// オブジェクト破棄時の処理
    /// </summary>
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoded;
    }

    /// <summary>
    /// シーンが切り替わるたびに呼ばれる処理
    /// <para><seealso cref="Init"/>初期化</para>
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoded(Scene scene, LoadSceneMode mode)
    {
        // 初期化
        Init();
        if(scene.name == "Title")
        {
            isTitleScene = true;
            backToTitleText.gameObject.SetActive(false);
        }
        else
        {
            isTitleScene = false;
            backToTitleText.gameObject.SetActive(true);
        }
    }


    void Update()
    {
        // オプションを開いている間のみ処理を行う
        if (isOptionOpened)
        {
            // 入力があったか
            if (OptionInput())
            {
                // 変更をUIに反映する
                OptionDisplay();
            }
        }

    }


    /// <summary>
    /// 初期化処理
    /// <para>キャンバスのカメラを再設定</para>
    /// <para>キャンバスの非表示</para>
    /// </summary>
    void Init()
    {
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        canvasLight = transform.GetChild(1).GetComponent<Canvas>();
        // キャンバスのカメラ設定
        // メインカメラを再設定
        if (Camera.main != null)
        {
            canvas.worldCamera = Camera.main;
            canvasLight.worldCamera = Camera.main;
        }
        else
        {
            print("カメラがありません");
            return;
        }
        // フラグの初期化
        isOptionOpened = false;
        // 負荷軽減のためcanvasはオフに
        canvas.gameObject.SetActive(false);

        var set = LoadSettings();
        if(set != null)
        {
            var soundSlider = settingParents[(int)SelectableOption.SOUND].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
            var lightSlider = settingParents[(int)SelectableOption.LIGHT].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
            soundSlider.value = set.sound_volume;
            lightSlider.value = set.screen_light;
        }
        // 設定反映
        SetVolume();
        SetLight();
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    bool OptionInput()
    {

        // カーソル移動の入力
        if (Cursorinput())
        {
            return true;
        }

        // 設定値変更の入力
        if (Volumeinput())
        {
            return true;
        }

        // スペース押したときの処理
        SubmitInput();


        return false;
    }

    /// <summary>
    /// カーソルを移動させる入力処理
    /// </summary>
    /// <returns></returns>
    bool Cursorinput()
    {
        bool ret = false;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedMenu--;
            if (selectedMenu < 0)
            {
                selectedMenu = 0;
            }
            ret = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {


            selectedMenu++;

            if (isTitleScene && selectedMenu == SelectableOption.BACK_TITLE)
            {
                selectedMenu--;
            }


            if (selectedMenu > (SelectableOption)Common.Common.GetEnumLength<SelectableOption>() - 1)
            {
                selectedMenu = (SelectableOption)Common.Common.GetEnumLength<SelectableOption>() - 1;

            }
            ret = true;
        }

        return ret;
    }

    /// <summary>
    /// 設定値を変更する入力処理
    /// </summary>
    /// <returns></returns>
    bool Volumeinput()
    {
        // 変更する場所

        bool ret = false;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            switch (selectedMenu)
            {
                case SelectableOption.SOUND:
                case SelectableOption.LIGHT:
                    var slider = settingParents[(int)selectedMenu].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
                    slider.value -= 1;
                    // 音量をセット
                    SetVolume();

                    // 明るさのセット
                    SetLight();
                    break;
            }
            ret = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            switch (selectedMenu)
            {
                case SelectableOption.SOUND:
                case SelectableOption.LIGHT:
                    var slider = settingParents[(int)selectedMenu].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
                    slider.value += 1;
                    // 音量をセット
                    SetVolume();

                    // 明るさのセット
                    SetLight();
                    break;
            }
            ret = true;
        }

        return ret;
    }

    /// <summary>
    /// 決定ボタンを押した時の処理
    /// </summary>
    /// <returns></returns>
    void SubmitInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (selectedMenu)
            {
                case SelectableOption.CLOSE_OPT:
                    // 設定の保存
                    SaveSettings();
                    break;
                case SelectableOption.BACK_TITLE:

                    // 確認ダイアログを出す
                    Common.Common.LoadScene("TItle");
                    break;
            }
        }
    }


    /// <summary>
    /// 入力を描画する処理
    /// </summary>
    void OptionDisplay()
    {
        // カーソルの座標を反映
        CursorDisplay();

        // 設定値をUIに反映
        SettingsDisplay();
    }

    /// <summary>
    /// カーソルの座標を更新する処理
    /// </summary>
    void CursorDisplay()
    {
        var pos = selectedCursor.transform.position;
        // カーソルの座標更新
        switch (selectedMenu)
        {
            case SelectableOption.SOUND:
            case SelectableOption.LIGHT:
                pos = settingParents[(int)selectedMenu].GetChild((int)SettingObject.TYPE).position;
                break;
            case SelectableOption.CLOSE_OPT:
                pos = unsaveSettingText.position;
                break;
            case SelectableOption.BACK_TITLE:
                pos = backToTitleText.position;
                break;

        }
        selectedCursor.transform.position = pos;
    }

    /// <summary>
    /// スライダーを数値UIに反映する処理
    /// </summary>
    void SettingsDisplay()
    {
        // 音量と明るさでループ
        for (int i = 0; i < settingParents.Length; i++)
        {
            // todo:入力をUIに反映
            var textVal = settingParents[i].GetChild((int)SettingObject.VALUE).GetComponent<Text>();
            var slider = settingParents[i].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
            textVal.text = slider.value.ToString();
        }
    }


    /// <summary>
    /// オプションを開く処理、各マネージャーから呼び出す
    /// </summary>
    public void OpenOption()
    {
        //print("オプションを開く");
        // todo:オプション画面の初期化.設定の読み込みとUIに反映
        var settings = LoadSettings();



        if (settings != null)
        {
            var soundSlider = settingParents[(int)SelectableOption.SOUND].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
            var lightSlider = settingParents[(int)SelectableOption.LIGHT].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
            soundSlider.value = settings.sound_volume;
            lightSlider.value = settings.screen_light;
            SettingsDisplay();
        }

        selectedMenu = 0;
        CursorDisplay();
        isOptionOpened = true;
        canvas.gameObject.SetActive(true);
        // カーソル位置の初期化
        selectedCursor.transform.position = settingParents[(int)SelectableOption.SOUND].GetChild((int)SettingObject.TYPE).position;
    }

    /// <summary>
    /// オプションを閉じる処理
    /// </summary>
    void CloseOption()
    {
        canvas.gameObject.SetActive(false);
        isOptionOpened = false;
        //print("オプションを閉じる");
        //todo:再度読み込んだデータで値を変更
    }

    /// <summary>
    /// 設定を保存
    /// </summary>
    void SaveSettings()
    {
        var settings = new JsonSettings();
        var soundSlider = settingParents[(int)SelectableOption.SOUND].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
        var lightSlider = settingParents[(int)SelectableOption.LIGHT].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
        settings.sound_volume = soundSlider.value;
        settings.screen_light = lightSlider.value;

        var txt = JsonUtility.ToJson(settings);
        print("保存前データ:" + txt);
        PlayerPrefs.SetString(Common.Common.KEY_SETTINGS, txt);
        PlayerPrefs.Save();

        // 保存して終了
        Invoke("CloseOption", 0.1f);
    }

    /// <summary>
    /// 音量の設定
    /// </summary>
    public void SetVolume()
    {
        // SoundManagerの音量をセットする
        var soundSlider = settingParents[(int)SelectableOption.SOUND].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
        var normaliazedVol = Common.Common.NormalizedFunc(soundSlider.value, soundSlider.minValue, soundSlider.maxValue, 0, 1);
        SoundManager.Instance?.SetVolumeRate(normaliazedVol);
		//SoundManager.Instance?.SetSoundsVolume(normaliazedVol);
        //SoundManager.Instance?.SetSEVolume(normaliazedVol);
    }

    public void SetLight()
    {
        var img = canvasLight.transform.GetChild(0).GetComponent<Image>();
        var col = img.color;
        var lightSlider = settingParents[(int)SelectableOption.LIGHT].GetChild((int)SettingObject.SLIDER).GetComponent<Slider>();
        var normVal = Common.Common.NormalizedFunc(lightSlider.value, lightSlider.minValue, lightSlider.maxValue, 0.2f, 0.8f);
        col.a = 1 - normVal;
        img.color = col;
    }

    /// <summary>
    /// 設定ファイル読み込み
    /// </summary>
    /// <returns></returns>
    JsonSettings LoadSettings()
    {
        var txt = PlayerPrefs.GetString(Common.Common.KEY_SETTINGS, null);
        print("読み込んだ設定データ:" + txt);
        if (txt != null)
        {
            return JsonUtility.FromJson<JsonSettings>(txt);
        }
        else
        {
            print("読み込み失敗");
            return null;
        }
    }
}
