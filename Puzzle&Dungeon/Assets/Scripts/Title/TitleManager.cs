using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Common;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [Tooltip("選択中のメニュー")]
    SelectedMenu selectedMenu;
    [Tooltip("選択できるUIの配列")]
    Image[] selectableImages;
    [Tooltip("選択できるUIの親オブジェクト"), SerializeField]
    Transform selectableParent;

    //オブジェクト
    [Tooltip("スティック"), SerializeField]
    private InputAction inputMover;
    [Tooltip("スティック"), SerializeField]
    private InputAction inputSelect;
    [Tooltip("スティック"), SerializeField]
    private InputActionAsset inputActions;

    bool isInput = false;

    /// <summary>
    /// 選択可能UIの列挙
    /// </summary>
    enum SelectedMenu
    {
        START,
        OPTION,
        QUIT,
    }



    // Start is called before the first frame update
    void Start()
    {
        Init();

        inputMover = inputActions.FindAction("UI/Move");
        inputSelect = new InputAction("AButton", binding: "<Gamepad>/buttonEast");

        inputMover.Enable();
        inputSelect.Enable();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Init()
    {
        // カーソルを一番上で初期化
        selectedMenu = 0;

        // 選択できるUIをすべて取得
        selectableImages = selectableParent.GetComponentsInChildren<Image>();
        // カーソル位置の初期化
        MainCursorDisplay();

        //// オプションを非表示
        //optionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // オプション操作モードでないなら
        if (!OptionManager.Instance.isOptionOpened)
        {
            // 決定ボタン押されたらカーソル位置に応じて処理分け
            if (PushSubmit())
            {
                SoundManager.Instance.PlaySE(Sounds.SEType.Submit);
                return;
            }
            // 上下入力があったらカーソルを変更し表示
            if (SelectMainCursor())
            {
                MainCursorDisplay();
                SoundManager.Instance.PlaySE(Sounds.SEType.CursorMove);
            }
        }


    }

    /// <summary>
    /// 決定ボタン押下時の処理
    /// </summary>
    /// <returns></returns>
    bool PushSubmit()
    {
        bool ret = false;

        float buttonValue = inputSelect.ReadValue<float>();

        if (buttonValue > 0.5f)
        {
            // 入力があった
            ret = true;

            // 選択中のUIで分岐
            switch (selectedMenu)
            {
                // ゲーム開始
                case SelectedMenu.START:
                    PushStart();
                    break;
                // オプションを開く
                case SelectedMenu.OPTION:
                    PushOption();
                    break;
                // ゲーム終了
                case SelectedMenu.QUIT:
                    PushQuit();
                    break;

                default:
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 入力があった
            ret = true;

            // 選択中のUIで分岐
            switch (selectedMenu)
            {
                // ゲーム開始
                case SelectedMenu.START:
                    PushStart();
                    break;
                // オプションを開く
                case SelectedMenu.OPTION:
                    PushOption();
                    break;
                // ゲーム終了
                case SelectedMenu.QUIT:
                    PushQuit();
                    break;

                default:
                    break;
            }
        }

        return ret;
    }

    /// <summary>
    /// プレイヤーの入力でselectedNumを増減させる
    /// </summary>
    /// <returns>入力があったらtrue</returns>
    bool SelectMainCursor()
    {
        // 戻り値の初期化
        bool ret = false;

        if (inputMover.ReadValue<Vector2>() == new Vector2(0, 0))
        {
            isInput = false;
        }

        // 上方向の入力
        if (inputMover.ReadValue<Vector2>().y == 1.0f && isInput == false)
        {
            // 入力アリ
            ret = true;
            isInput = true;
            selectedMenu--;
            // 最下端に行ったら上に戻る
            if (selectedMenu < 0)
            {
                selectedMenu = (SelectedMenu)selectableImages.Length - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 入力アリ
            ret = true;
            selectedMenu--;
            // 最下端に行ったら上に戻る
            if (selectedMenu < 0)
            {
                selectedMenu = (SelectedMenu)selectableImages.Length - 1;
            }
        }

        // 下方向の入力
        if (inputMover.ReadValue<Vector2>().y == -1.0f && isInput == false)
        {
            // 入力アリ
            ret = true;
            isInput = true;
            selectedMenu++;
            // 最上端に行ったら下に戻る
            if (selectedMenu > (SelectedMenu)selectableImages.Length - 1)
            {
                selectedMenu = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 入力アリ
            ret = true;
            selectedMenu++;
            // 最上端に行ったら下に戻る
            if (selectedMenu > (SelectedMenu)selectableImages.Length - 1)
            {
                selectedMenu = 0;
            }
        }

        return ret;

    }

    /// <summary>
    /// 選択中の番号によって
    /// </summary>
    void MainCursorDisplay()
    {
        for (int i = 0; i < selectableImages.Length; i++)
        {
            if (i == (int)selectedMenu)
            {
                selectableImages[i].transform.localScale = Vector3.one * 1.1f;
                selectableImages[i].color = Color.white;
            }
            // 非選択中はグレーアウト
            else
            {
                selectableImages[i].transform.localScale = Vector3.one;
                selectableImages[i].color = Color.gray;
            }
        }
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    void PushStart()
    {
        print("シーン遷移");
        //
        Common.Common.LoadScene(Common.Common.SCENE_SELECT);
    }


    /// <summary>
    /// オプションの表示
    /// </summary>
    void PushOption()
    {
        // OptionManagerの処理を呼び出し
        OptionManager.Instance.OpenOption();

        //print("オプション表示");
    }


    /// <summary>
    /// ゲーム終了
    /// </summary>
    void PushQuit()
    {
        print("終了");
        Common.Common.GameQuit();
    }
}
