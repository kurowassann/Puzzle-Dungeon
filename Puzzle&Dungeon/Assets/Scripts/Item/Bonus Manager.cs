using Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Buff;

    [SerializeField]
    GameObject[] DeBuff;

    [SerializeField]
    Text[] BuffNumbertext;
    [SerializeField]
    Text[] DeBuffNumbertext;

    // バフとデバフのカウント
    private int[] buffCounts;
    private int[] debuffCounts;

    AllBonus bonus = AllBonus.ENEMYROOM;

    void Start()
    {
        // バフとデバフのカウント配列を初期化（Buff と DeBuff の数と一致するように設定）
        buffCounts = new int[Buff.Length];
        debuffCounts = new int[DeBuff.Length];
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
                bonus = AllBonus.ALLHEEL;
                break;
            case int n when (n >= 11 && n < 22):
                bonus = AllBonus.HEELUP;
                break;
            case int n when (n >= 22 && n < 33):
                bonus = AllBonus.ATTACK1UP;
                break;
            case int n when (n >= 33 && n < 44):
                bonus = AllBonus.GUARD;
                break;
            case int n when (n >= 44 && n < 55):
                bonus = AllBonus.ONEHITATTACK;
                break;
            case int n when (n >= 55 && n < 65):
                bonus = AllBonus.FLOORCLEAR;
                break;
            case int n when (n >= 65 && n < 70):
                bonus = AllBonus.ALLENEMYATTACK;
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
                break;
            case int n when (n >= 10 && n < 20):
                bonus = AllBonus.ATTACK1DOWN;
                break;
            case int n when (n >= 20 && n < 30):
                bonus = AllBonus.EATTACK1UP;
                break;
            case int n when (n >= 30 && n < 40):
                bonus = AllBonus.LIFE1DOWN;
                break;
            case int n when (n >= 40 && n < 50):
                bonus = AllBonus.EHEEL1UP;
                break;
            case int n when (n >= 50 && n < 55):
                bonus = AllBonus.SPACE_2_CLEAR;
                break;
            case int n when (n >= 55 && n < 60):
                bonus = AllBonus.EHEEL1UP1;
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
            case AllBonus.ALLHEEL:
                Buff[0].SetActive(true);
                buffCounts[0]++;  // カウントを増加
                BuffNumbertext[0].text = buffCounts[0].ToString();  // テキストを更新
                break;
            case AllBonus.HEELUP:
                Buff[1].SetActive(true);
                buffCounts[1]++;  // カウントを増加
                BuffNumbertext[1].text = buffCounts[1].ToString();  // テキストを更新
                break;
            case AllBonus.ATTACK1UP:
                Buff[2].SetActive(true);
                buffCounts[2]++;  // カウントを増加
                BuffNumbertext[2].text = buffCounts[2].ToString();  // テキストを更新
                break;
            case AllBonus.GUARD:
                Buff[3].SetActive(true);
                buffCounts[3]++;  // カウントを増加
                BuffNumbertext[3].text = buffCounts[3].ToString();  // テキストを更新
                break;
            case AllBonus.ONEHITATTACK:
                Buff[4].SetActive(true);
                buffCounts[4]++;  // カウントを増加
                BuffNumbertext[4].text = buffCounts[4].ToString();  // テキストを更新
                break;
            case AllBonus.FLOORCLEAR:
                Buff[5].SetActive(true);
                buffCounts[5]++;  // カウントを増加
                BuffNumbertext[5].text = buffCounts[5].ToString();  // テキストを更新
                break;
            case AllBonus.ENEMYROOM:
                Buff[6].SetActive(true);
                buffCounts[6]++;  // カウントを増加
                BuffNumbertext[6].text = buffCounts[6].ToString();  // テキストを更新
                break;
            case AllBonus.HEELDOWN:
                DeBuff[0].SetActive(true);
                debuffCounts[0]++;
                DeBuffNumbertext[0].text = debuffCounts[0].ToString();
                break;
            case AllBonus.ATTACK1DOWN:
                DeBuff[1].SetActive(true);
                debuffCounts[1]++;
                DeBuffNumbertext[1].text = debuffCounts[1].ToString();
                break;
            case AllBonus.EATTACK1UP:
                DeBuff[2].SetActive(true);
                debuffCounts[2]++;
                DeBuffNumbertext[2].text = debuffCounts[2].ToString();
                break;
            case AllBonus.LIFE1DOWN:
                DeBuff[3].SetActive(true);
                debuffCounts[3]++;
                DeBuffNumbertext[3].text = debuffCounts[3].ToString();
                break;
            case AllBonus.EHEEL1UP:
                DeBuff[4].SetActive(true);
                debuffCounts[4]++;
                DeBuffNumbertext[4].text = debuffCounts[4].ToString();
                break;
            case AllBonus.EHEEL1UP1:
                DeBuff[5].SetActive(true);
                debuffCounts[5]++;
                DeBuffNumbertext[5].text = debuffCounts[5].ToString();
                break;
            case AllBonus.SPACE_2_CLEAR:
                DeBuff[6].SetActive(true);
                debuffCounts[6]++;
                DeBuffNumbertext[6].text = debuffCounts[6].ToString();
                break;
            default:
                break;
        }
    }

    public void Lottery()
    {
        BuffOrDebuff(); // バフかデバフを選び、その後に詳細を決定
        print(bonus); // 選ばれたボーナスの種類を表示
        print(BuffNumbertext);
        print(DeBuffNumbertext);
    }
}

