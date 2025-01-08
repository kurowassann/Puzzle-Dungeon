using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Buff;

    [SerializeField]
    GameObject[] DeBuff;

    AllBonus bonus = AllBonus.ENEMYROOM;

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
        ShowBonusObject(bonus);
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
                break;
            case AllBonus.HEELUP:
                Buff[1].SetActive(true);
                break;
            case AllBonus.ATTACK1UP:
                Buff[2].SetActive(true);
                break;
            case AllBonus.GUARD:
                Buff[3].SetActive(true);
                break;
            case AllBonus.ONEHITATTACK:
                Buff[4].SetActive(true);
                break;
            case AllBonus.FLOORCLEAR:
                Buff[5].SetActive(true);
                break;
            case AllBonus.ENEMYROOM:
                Buff[6].SetActive(true);
                break;
            case AllBonus.HEELDOWN:
                DeBuff[0].SetActive(true);
                break;
            case AllBonus.ATTACK1DOWN:
                DeBuff[1].SetActive(true);
                break;
            case AllBonus.EATTACK1UP:
                DeBuff[2].SetActive(true);
                break;
            case AllBonus.LIFE1DOWN:
                DeBuff[3].SetActive(true);
                break;
            case AllBonus.EHEEL1UP:
                DeBuff[4].SetActive(true);
                break;
            case AllBonus.EHEEL1UP1:
                DeBuff[5].SetActive(true);
                break;
            case AllBonus.SPACE_2_CLEAR:
                DeBuff[6].SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Lottery()
    {
        BuffOrDebuff(); // バフかデバフを選び、その後に詳細を決定
        print(bonus); // 選ばれたボーナスの種類を表示
    }
}

