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

    // �o�t�ƃf�o�t�̃J�E���g
    private int[] buffCounts;
    private int[] debuffCounts;

    AllBonus bonus = AllBonus.ENEMYROOM;

    void Start()
    {
        // �o�t�ƃf�o�t�̃J�E���g�z����������iBuff �� DeBuff �̐��ƈ�v����悤�ɐݒ�j
        buffCounts = new int[Buff.Length];
        debuffCounts = new int[DeBuff.Length];
    }

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
        //ShowBonusObject(bonus);
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
                buffCounts[0]++;  // �J�E���g�𑝉�
                BuffNumbertext[0].text = buffCounts[0].ToString();  // �e�L�X�g���X�V
                break;
            case AllBonus.HEELUP:
                Buff[1].SetActive(true);
                buffCounts[1]++;  // �J�E���g�𑝉�
                BuffNumbertext[1].text = buffCounts[1].ToString();  // �e�L�X�g���X�V
                break;
            case AllBonus.ATTACK1UP:
                Buff[2].SetActive(true);
                buffCounts[2]++;  // �J�E���g�𑝉�
                BuffNumbertext[2].text = buffCounts[2].ToString();  // �e�L�X�g���X�V
                break;
            case AllBonus.GUARD:
                Buff[3].SetActive(true);
                buffCounts[3]++;  // �J�E���g�𑝉�
                BuffNumbertext[3].text = buffCounts[3].ToString();  // �e�L�X�g���X�V
                break;
            case AllBonus.ONEHITATTACK:
                Buff[4].SetActive(true);
                buffCounts[4]++;  // �J�E���g�𑝉�
                BuffNumbertext[4].text = buffCounts[4].ToString();  // �e�L�X�g���X�V
                break;
            case AllBonus.FLOORCLEAR:
                Buff[5].SetActive(true);
                buffCounts[5]++;  // �J�E���g�𑝉�
                BuffNumbertext[5].text = buffCounts[5].ToString();  // �e�L�X�g���X�V
                break;
            case AllBonus.ENEMYROOM:
                Buff[6].SetActive(true);
                buffCounts[6]++;  // �J�E���g�𑝉�
                BuffNumbertext[6].text = buffCounts[6].ToString();  // �e�L�X�g���X�V
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
        BuffOrDebuff(); // �o�t���f�o�t��I�сA���̌�ɏڍׂ�����
        print(bonus); // �I�΂ꂽ�{�[�i�X�̎�ނ�\��
        print(BuffNumbertext);
        print(DeBuffNumbertext);
    }
}

