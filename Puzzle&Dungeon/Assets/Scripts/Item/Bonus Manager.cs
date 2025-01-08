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

    // �֐�1: �o�t���f�o�t�������_���őI��������
    public void BuffOrDebuff()
    {
        int num = UnityEngine.Random.Range(0, 100); // 0�`99�̃����_�����l

        if (num < 70) // �o�t��I������m��
        {
            LotNextBonus1(); // �o�t�̏ڍׂ�����
            print("�o�t���I������܂���");
        }
        else // �f�o�t��I������m��
        {
            LotNextBonus2(); // �f�o�t�̏ڍׂ�����
            print("�f�o�t���I������܂���");
        }

        // �{�[�i�X�ɑΉ�����I�u�W�F�N�g��\������
        ShowBonusObject(bonus);
    }

    // �֐�2: �o�t�̏ڍׂ�����
    public void LotNextBonus1()
    {
        int num = UnityEngine.Random.Range(0, 70); // 0�`69�̃����_�����l

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

        // �{�[�i�X�ɑΉ�����I�u�W�F�N�g��\������
        ShowBonusObject(bonus);
    }

    // �֐�3: �f�o�t�̏ڍׂ�����
    public void LotNextBonus2()
    {
        int num = UnityEngine.Random.Range(0, 60); // 0�`59�̃����_�����l

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

        // �{�[�i�X�ɑΉ�����I�u�W�F�N�g��\������
        ShowBonusObject(bonus);
    }

    // ���I���ʂɊ�Â��ăI�u�W�F�N�g��\�����郁�\�b�h
    private void ShowBonusObject(AllBonus bonus)
    {
        // ���ׂẴ{�[�i�X�I�u�W�F�N�g���\���ɂ���
        /*foreach (GameObject buff in Buff)
        {
            buff.SetActive(true);
        }
        foreach (GameObject debuff in DeBuff)
        {
            debuff.SetActive(false);
        }*/

        // �V�����{�[�i�X�I�u�W�F�N�g���C���X�^���X�����ĕ\��
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
        BuffOrDebuff(); // �o�t���f�o�t��I�сA���̌�ɏڍׂ�����
        print(bonus); // �I�΂ꂽ�{�[�i�X�̎�ނ�\��
    }
}

