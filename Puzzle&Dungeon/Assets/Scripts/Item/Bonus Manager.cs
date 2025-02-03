using Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Buff;//バフのオブジェクトを格納する配列です。

    [SerializeField]
    GameObject[] DeBuff;//デバフのオブジェクトを格納する配列です。

    [SerializeField]
    Text[] BuffNumbertext;//バフのカウントを表示するための Text コンポーネントの配列です。
    [SerializeField]
    Text[] DeBuffNumbertext;//デバフのカウントを表示するための Text コンポーネントの配列です。

    [SerializeField]
    Text[] FiveTurntext; //5ターン減少する数値、5ターンの制限がある効果（バフやデバフ）の残りターン数を表示するための Text コンポーネントの配列です。例えば、特定の効果が5ターンだけ有効で、そのカウントダウンを表示します。

    // 抽選を行う間隔（秒）
    public float lotteryInterval = 5f; //抽選を行う間隔（秒）です。ここでは、5秒ごとに抽選が行われます。
    private float timeSinceLastLottery = 0f; // 最後の抽選から経過した時間　最後の抽選から経過した時間を管理するための変数です。毎フレーム加算され、設定された間隔を超えた場合に抽選を行います。


    // バフとデバフのカウント
    private int[] buffCounts;//バフのカウント
    private int[] debuffCounts;//デバフのカウント
    private int[] fiveCounts; //5ターン最高で1ターンずつマイナスになる最低0
   

    AllBonus bonus = AllBonus.ENEMYROOM;

    void Start()
    {
        // バフとデバフのカウント配列を初期化（Buff と DeBuff の数と一致するように設定）
        buffCounts = new int[Buff.Length];
        debuffCounts = new int[DeBuff.Length];
        fiveCounts = new int[FiveTurntext.Length];
       
        // バフのカウントを0にリセットし、テキストも更新
        for (int i = 0; i < BuffNumbertext.Length; i++)
        {
            buffCounts[i] = 0;
            BuffNumbertext[i].text = buffCounts[i].ToString();  // テキストを0に設定
        }

        // デバフのカウントを0にリセットし、テキストも更新
        for (int i = 0; i < DeBuffNumbertext.Length; i++)
        {
            debuffCounts[i] = 0;
            DeBuffNumbertext[i].text = debuffCounts[i].ToString();  // テキストを0に設定
        }

        // 5ターンカウントを0にリセットし、テキストも更新
        for (int i = 0; i < FiveTurntext.Length; i++)
        {
            fiveCounts[i] = 0;
            FiveTurntext[i].text = fiveCounts[i].ToString();  // テキストを0に設定
        }

       
    }

    void Update()
    {
            PerformLottery();

        // 時間が経過したら抽選を行う
        //最後の抽選から経過した時間を管理するための変数です。毎フレーム加算され、設定された間隔を超えた場合に抽選を行います。
        timeSinceLastLottery += Time.deltaTime; // 経過時間を加算

        if (timeSinceLastLottery >= lotteryInterval)
        {
            // ここで抽選を行う
            //LotNextBonus2();
            // 経過時間をリセットして次の抽選までカウント
            timeSinceLastLottery = 0f;
        }

        // Spaceキーが押されたときにカウントを減らす
        if (Input.GetKeyDown(KeyCode.A))
        {
            // FLOORCLEARのカウントダウン
            if (fiveCounts[0] > 0)
            {
                fiveCounts[0]--;
                FiveTurntext[0].text = fiveCounts[0].ToString();
                if (fiveCounts[0] == 0)
                {
                    FiveTurntext[0].gameObject.SetActive(true); // 0になったら非表示
                    Buff[5].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            // SPACE_2_CLEARのカウントダウン
            if (fiveCounts[1] > 0)
            {
                fiveCounts[1]--;
                FiveTurntext[1].text = fiveCounts[1].ToString();
                if (fiveCounts[1] == 0)
                {
                    FiveTurntext[1].gameObject.SetActive(true); // 0になったら非表示

                    DeBuff[6].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                }
            }
        }

    }

    // 抽選を行うメソッド
    private void PerformLottery()
    {
        // 複数回抽選を行いたい場合、forループで回す
        for (int i = 0; i < 3; i++) // 3回抽選を行う
        {
            BuffOrDebuff(); // バフかデバフを選び、その後に詳細を決定
            //Debug.Log("抽選結果: " + buffCounts[0] + " " + debuffCounts[0]);
        }
    }

    // 関数1: バフかデバフをランダムで選択させる
    public void BuffOrDebuff()
    {
        int num = UnityEngine.Random.Range(0, 100); // 0～99のランダム数値

        if (num < 70) // バフを選択する確率
        {
            LotNextBonus1(); // バフの詳細を決定
            print("バフが選択されました");
        }
        else // デバフを選択する確率
        {
            LotNextBonus2(); // デバフの詳細を決定
            print("デバフが選択されました");
        }

        // ボーナスに対応するオブジェクトを表示する
        //ShowBonusObject(bonus);
    }

    // 関数2: バフの詳細を決定
    public void LotNextBonus1()
    {
        int num = UnityEngine.Random.Range(0, 70); // 0～69のランダム数値

        switch (num)
        {
            case int n when (n < 11):
                bonus = AllBonus.ALLHEEL;//全回復
                Debug.Log("ライフ全回復");
                break;
            case int n when (n >= 11 && n < 22)://回復の上限+1
                bonus = AllBonus.HEELUP;
                Debug.Log("ライフ上限+1");
                break;
            case int n when (n >= 22 && n < 33)://攻撃力アップ
                bonus = AllBonus.ATTACK1UP;
                Debug.Log("攻撃力+1");
                break;
            case int n when (n >= 33 && n < 44)://一回敵からの攻撃を無効
                bonus = AllBonus.GUARD;
                Debug.Log("ダメージ1回無効");
                break;
            case int n when (n >= 44 && n < 55)://次のターンで敵に攻撃を一撃入れる
                bonus = AllBonus.ONEHITATTACK;
                Debug.Log("次の攻撃一撃");
                break;
            case int n when (n >= 55 && n < 65)://５ターンの間マップ全体が見れる
                bonus = AllBonus.FLOORCLEAR;
                Debug.Log("5ターンの間フロア全体が見える");
                break;
            case int n when (n >= 65 && n < 70):
                bonus = AllBonus.ALLENEMYATTACK;//elemtal5
                Debug.Log("部屋内の敵を全滅");
                break;
            default:
                break;
        }

        // ボーナスに対応するオブジェクトを表示する
        ShowBonusObject(bonus);
    }

    // 関数3: デバフの詳細を決定
    public void LotNextBonus2()
    {
        int num = UnityEngine.Random.Range(0, 60); // 0～59のランダム数値

        switch (num)
        {
            case int n when (n < 10):
                bonus = AllBonus.HEELDOWN;
                Debug.Log("ライフ-1");
                break;
            case int n when (n >= 10 && n < 20):
                bonus = AllBonus.ATTACK1DOWN;
                Debug.Log("攻撃力-1");
                break;
            case int n when (n >= 20 && n < 30):
                bonus = AllBonus.EATTACK1UP;
                Debug.Log("敵の攻撃力+1");
                break;
            case int n when (n >= 30 && n < 40):
                bonus = AllBonus.LIFE1DOWN;
                Debug.Log("ライフ上限-1");
                break;
            case int n when (n >= 40 && n < 50):
                bonus = AllBonus.EHEEL1UP;
                Debug.Log("敵のライフ上限+1");
                break;
            case int n when (n >= 50 && n < 55):
                bonus = AllBonus.SPACE_2_CLEAR;
                Debug.Log("5ターンの間周囲2マスしか見えない");
                break;
            case int n when (n >= 55 && n < 60):
                bonus = AllBonus.EHEEL1UP1;
                Debug.Log("部屋内に敵を生成する");
                break;
            default:
                break;
        }

        // ボーナスに対応するオブジェクトを表示する
        ShowBonusObject(bonus);
    }

    // 抽選結果に基づいてオブジェクトを表示するメソッド
    private void ShowBonusObject(AllBonus bonus)
    {
        // すべてのボーナスオブジェクトを非表示にする
        /*foreach (GameObject buff in Buff)
        {
            buff.SetActive(true);
        }
        foreach (GameObject debuff in DeBuff)
        {
            debuff.SetActive(false);
        }*/

        // 新しいボーナスオブジェクトをインスタンス化して表示
        switch (bonus)
        {
            /*case AllBonus.ALLHEEL:
                if (buffCounts[0] < 1)  // buffCounts[0]が1未満の場合のみ増加
                {
                    buffCounts[0]++;  // カウントを増加
                }
                Buff[0].SetActive(true);
                Buff[0].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                BuffNumbertext[0].text = buffCounts[0].ToString();  // テキストを更新

                break;*/
            case AllBonus.HEELUP:
                Buff[1].SetActive(true);
                buffCounts[0]++;  // カウントを増加
                //バフの効果がいくつ適用されているかをカウントする配列です。各バフに対して適用回数を記録します。
                BuffNumbertext[0].text = buffCounts[1].ToString();  // テキストを更新
                Buff[0].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                //DeBuff[0].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                break;
            case AllBonus.ATTACK1UP:
                Buff[1].SetActive(true);
                buffCounts[1]++;  // カウントを増加
                BuffNumbertext[1].text = buffCounts[2].ToString();  // テキストを更新
                Buff[1].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                //DeBuff[1].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                break;
           case AllBonus.GUARD:
                Buff[2].SetActive(true);
                if (buffCounts[2] < 1)  // buffCounts[0]が1未満の場合のみ増加
                {
                    buffCounts[2]++;  // カウントを増加
                }
                Buff[2].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                BuffNumbertext[2].text = buffCounts[2].ToString();  // テキストを更新
                break;
            case AllBonus.ONEHITATTACK:
                Buff[3].SetActive(true);
                if (buffCounts[4] < 1)  // buffCounts[0]が1未満の場合のみ増加
                {
                    buffCounts[4]++;  // カウントを増加
                }
                Buff[3].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                BuffNumbertext[3].text = buffCounts[4].ToString();  // テキストを更新
                break;
            case AllBonus.FLOORCLEAR:
                Buff[4].SetActive(true);
                //DeBuff[6].SetActive(false);
                FiveTurntext[0].gameObject.SetActive(true);
                fiveCounts[0] = 5; // 5ターンに設定
                FiveTurntext[0].text = fiveCounts[0].ToString();
                Buff[4].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                DeBuff[4].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                break;
            /*case AllBonus.ALLENEMYATTACK:
                //Buff[6].SetActive(true);
                if (buffCounts[5] < 1)  // buffCounts[0]が1未満の場合のみ増加
                {
                    buffCounts[5]++;  // カウントを増加
                }
                Buff[6].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                BuffNumbertext[5].text = buffCounts[5].ToString();  // テキストを更新
                break;*/
            case AllBonus.HEELDOWN://プレイヤーのhp down
                DeBuff[0].SetActive(true);
                debuffCounts[0]++;
                DeBuffNumbertext[0].text = debuffCounts[0].ToString();
                DeBuff[0].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                break;
            case AllBonus.ATTACK1DOWN://プレイヤーの攻撃力down
                DeBuff[1].SetActive(true);
                debuffCounts[1]++;
                DeBuffNumbertext[1].text = debuffCounts[1].ToString();
                DeBuff[1].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                break;
            case AllBonus.EHEEL1UP://敵のHPup
                DeBuff[2].SetActive(true);
                debuffCounts[2]++;
                DeBuffNumbertext[2].text = debuffCounts[2].ToString();
                DeBuff[2].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                break;
           
            case AllBonus.EATTACK1UP://敵の攻撃力up
                DeBuff[3].SetActive(true);
                debuffCounts[3]++;
                DeBuffNumbertext[3].text = debuffCounts[3].ToString();
                DeBuff[3].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                break;
            //case AllBonus.EHEEL1UP:
            //    DeBuff[4].SetActive(true);
            //    debuffCounts[4]++;
            //    DeBuffNumbertext[4].text = debuffCounts[4].ToString();
            //    DeBuff[4].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            //    break;
            //case AllBonus.EHEEL1UP1:
            //    DeBuff[5].SetActive(true);
            //    debuffCounts[5]++;  // debuffCounts[5] をインクリメント
            //    // UIの色を変更
            //    DeBuff[5].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            //    // debuffCounts[5] を表示するためにテキストを更新
            //    DeBuffNumbertext[5].text = debuffCounts[5].ToString();  // debuffCounts[5] を表示
            //    break;
            case AllBonus.SPACE_2_CLEAR:
                //Buff[5].SetActive(false);
                DeBuff[4].SetActive(true);
                FiveTurntext[1].gameObject.SetActive(true);
                fiveCounts[1] = 5; // 5ターンに設定
                FiveTurntext[1].text = fiveCounts[1].ToString();
                DeBuff[4].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                Buff[4].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                break;
            default:
                break;
        }
    }

   

    // バフとデバフのカウントをリセットするメソッド
    public void ResetCounts()
    {
        // バフのカウントをリセット
        for (int i = 0; i < buffCounts.Length; i++)
        {
            buffCounts[i] = 0;
            BuffNumbertext[i].text = buffCounts[i].ToString();  // テキストを更新
        }

        // デバフのカウントをリセット
        for (int i = 0; i < debuffCounts.Length; i++)
        {
            debuffCounts[i] = 0;
            DeBuffNumbertext[i].text = debuffCounts[i].ToString();  // テキストを更新
        }

        // 5ターンカウントもリセット
        for (int i = 0; i < fiveCounts.Length; i++)
        {
            fiveCounts[i] = 0;
            FiveTurntext[i].text = fiveCounts[i].ToString();  // テキストを更新
        }

        
    }


    public void Lottery()
    {
        BuffOrDebuff(); // バフかデバフを選び、その後に詳細を決定
        print(bonus); // 選ばれたボーナスの種類を表示
        print(BuffNumbertext);//バフ
        print(DeBuffNumbertext);//デバフ
       
    }
}

