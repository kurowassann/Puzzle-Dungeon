using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public GameObject allHeelPrefab;  // ALLHEELに対応するPrefab
    public GameObject heelUpPrefab;   // HEELUPに対応するPrefab
    public GameObject attack1UpPrefab; // ATTACK1UPに対応するPrefab
    public GameObject guardPrefab;    // GUARDに対応するPrefab
    public GameObject oneHitAttackPrefab; // ONEHITATTACKに対応するPrefab
    public GameObject floorClearPrefab;  // FLOORCLEARに対応するPrefab
    public GameObject enemyRoomPrefab;   // ENEMYROOMに対応するPrefab
    public GameObject heelDownPrefab;  // HEELDOWNに対応するPrefab
    public GameObject attack1DownPrefab; // ATTACK1DOWNに対応するPrefab
    public GameObject eAttack1UpPrefab; // EATTACK1UPに対応するPrefab
    public GameObject life1DownPrefab; // LIFE1DOWNに対応するPrefab
    public GameObject eHeel1UpPrefab; // EHEEL1UPに対応するPrefab
    public GameObject space2ClearPrefab; // SPACE_2_CLEARに対応するPrefab

    private GameObject currentBonusObject;  // 現在表示中のボーナスオブジェクトを保持する変数

    /// <summary>次の階層に行った際のボーナス抽選</summary>
    public AllBonus LotNextBonus()
    {
        AllBonus bonus = AllBonus.ENEMYROOM;

        int num = UnityEngine.Random.Range(0, 100); // 0～99のランダム数値

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
            case int n when (n >= 70 && n < 75):
                bonus = AllBonus.HEELDOWN;
                break;
            case int n when (n >= 75 && n < 80):
                bonus = AllBonus.ATTACK1DOWN;
                break;
            case int n when (n >= 80 && n < 85):
                bonus = AllBonus.EATTACK1UP;
                break;
            case int n when (n >= 85 && n < 90):
                bonus = AllBonus.LIFE1DOWN;
                break;
            case int n when (n >= 90 && n < 95):
                bonus = AllBonus.EHEEL1UP;
                break;
            case int n when (n >= 95 && n < 97.5):
                bonus = AllBonus.SPACE_2_CLEAR;
                break;
            case int n when (n >= 97.5 && n < 99):
                bonus = AllBonus.ENEMYROOM;
                break;
            default:
                break;
        }

        // ボーナスに対応するオブジェクトを表示する
        ShowBonusObject(bonus);

        return bonus;
    }

    // 抽選結果に基づいてオブジェクトを表示するメソッド
    private void ShowBonusObject(AllBonus bonus)
    {
        // 以前のボーナスオブジェクトを削除
        if (currentBonusObject != null)
        {
            Destroy(currentBonusObject);
        }

        // 新しいボーナスオブジェクトをインスタンス化して表示
        switch (bonus)
        {
            case AllBonus.ALLHEEL:
                currentBonusObject = Instantiate(allHeelPrefab);
                break;
            case AllBonus.HEELUP:
                currentBonusObject = Instantiate(heelUpPrefab);
                break;
            case AllBonus.ATTACK1UP:
                currentBonusObject = Instantiate(attack1UpPrefab);
                break;
            case AllBonus.GUARD:
                currentBonusObject = Instantiate(guardPrefab);
                break;
            case AllBonus.ONEHITATTACK:
                currentBonusObject = Instantiate(oneHitAttackPrefab);
                break;
            case AllBonus.FLOORCLEAR:
                currentBonusObject = Instantiate(floorClearPrefab);
                break;
            case AllBonus.ENEMYROOM:
                currentBonusObject = Instantiate(enemyRoomPrefab);
                break;
            case AllBonus.HEELDOWN:
                currentBonusObject = Instantiate(heelDownPrefab);
                break;
            case AllBonus.ATTACK1DOWN:
                currentBonusObject = Instantiate(attack1DownPrefab);
                break;
            case AllBonus.EATTACK1UP:
                currentBonusObject = Instantiate(eAttack1UpPrefab);
                break;
            case AllBonus.LIFE1DOWN:
                currentBonusObject = Instantiate(life1DownPrefab);
                break;
            case AllBonus.EHEEL1UP:
                currentBonusObject = Instantiate(eHeel1UpPrefab);
                break;
            case AllBonus.SPACE_2_CLEAR:
                currentBonusObject = Instantiate(space2ClearPrefab);
                break;
            default:
                break;
        }
    }

    public void Lottery()
    {
        print(LotNextBonus());
    }
}
