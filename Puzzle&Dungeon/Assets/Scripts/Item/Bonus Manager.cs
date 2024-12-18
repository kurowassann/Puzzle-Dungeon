using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public GameObject allHeelPrefab;  // ALLHEEL�ɑΉ�����Prefab
    public GameObject heelUpPrefab;   // HEELUP�ɑΉ�����Prefab
    public GameObject attack1UpPrefab; // ATTACK1UP�ɑΉ�����Prefab
    public GameObject guardPrefab;    // GUARD�ɑΉ�����Prefab
    public GameObject oneHitAttackPrefab; // ONEHITATTACK�ɑΉ�����Prefab
    public GameObject floorClearPrefab;  // FLOORCLEAR�ɑΉ�����Prefab
    public GameObject enemyRoomPrefab;   // ENEMYROOM�ɑΉ�����Prefab
    public GameObject heelDownPrefab;  // HEELDOWN�ɑΉ�����Prefab
    public GameObject attack1DownPrefab; // ATTACK1DOWN�ɑΉ�����Prefab
    public GameObject eAttack1UpPrefab; // EATTACK1UP�ɑΉ�����Prefab
    public GameObject life1DownPrefab; // LIFE1DOWN�ɑΉ�����Prefab
    public GameObject eHeel1UpPrefab; // EHEEL1UP�ɑΉ�����Prefab
    public GameObject space2ClearPrefab; // SPACE_2_CLEAR�ɑΉ�����Prefab

    private GameObject currentBonusObject;  // ���ݕ\�����̃{�[�i�X�I�u�W�F�N�g��ێ�����ϐ�

    /// <summary>���̊K�w�ɍs�����ۂ̃{�[�i�X���I</summary>
    public AllBonus LotNextBonus()
    {
        AllBonus bonus = AllBonus.ENEMYROOM;

        int num = UnityEngine.Random.Range(0, 100); // 0�`99�̃����_�����l

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

        // �{�[�i�X�ɑΉ�����I�u�W�F�N�g��\������
        ShowBonusObject(bonus);

        return bonus;
    }

    // ���I���ʂɊ�Â��ăI�u�W�F�N�g��\�����郁�\�b�h
    private void ShowBonusObject(AllBonus bonus)
    {
        // �ȑO�̃{�[�i�X�I�u�W�F�N�g���폜
        if (currentBonusObject != null)
        {
            Destroy(currentBonusObject);
        }

        // �V�����{�[�i�X�I�u�W�F�N�g���C���X�^���X�����ĕ\��
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
