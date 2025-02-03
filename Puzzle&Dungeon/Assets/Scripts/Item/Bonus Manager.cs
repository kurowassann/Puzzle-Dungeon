using Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Buff;//�o�t�̃I�u�W�F�N�g���i�[����z��ł��B

    [SerializeField]
    GameObject[] DeBuff;//�f�o�t�̃I�u�W�F�N�g���i�[����z��ł��B

    [SerializeField]
    Text[] BuffNumbertext;//�o�t�̃J�E���g��\�����邽�߂� Text �R���|�[�l���g�̔z��ł��B
    [SerializeField]
    Text[] DeBuffNumbertext;//�f�o�t�̃J�E���g��\�����邽�߂� Text �R���|�[�l���g�̔z��ł��B

    [SerializeField]
    Text[] FiveTurntext; //5�^�[���������鐔�l�A5�^�[���̐�����������ʁi�o�t��f�o�t�j�̎c��^�[������\�����邽�߂� Text �R���|�[�l���g�̔z��ł��B�Ⴆ�΁A����̌��ʂ�5�^�[�������L���ŁA���̃J�E���g�_�E����\�����܂��B

    // ���I���s���Ԋu�i�b�j
    public float lotteryInterval = 5f; //���I���s���Ԋu�i�b�j�ł��B�����ł́A5�b���Ƃɒ��I���s���܂��B
    private float timeSinceLastLottery = 0f; // �Ō�̒��I����o�߂������ԁ@�Ō�̒��I����o�߂������Ԃ��Ǘ����邽�߂̕ϐ��ł��B���t���[�����Z����A�ݒ肳�ꂽ�Ԋu�𒴂����ꍇ�ɒ��I���s���܂��B


    // �o�t�ƃf�o�t�̃J�E���g
    private int[] buffCounts;//�o�t�̃J�E���g
    private int[] debuffCounts;//�f�o�t�̃J�E���g
    private int[] fiveCounts; //5�^�[���ō���1�^�[�����}�C�i�X�ɂȂ�Œ�0
   

    AllBonus bonus = AllBonus.ENEMYROOM;

    void Start()
    {
        // �o�t�ƃf�o�t�̃J�E���g�z����������iBuff �� DeBuff �̐��ƈ�v����悤�ɐݒ�j
        buffCounts = new int[Buff.Length];
        debuffCounts = new int[DeBuff.Length];
        fiveCounts = new int[FiveTurntext.Length];
       
        // �o�t�̃J�E���g��0�Ƀ��Z�b�g���A�e�L�X�g���X�V
        for (int i = 0; i < BuffNumbertext.Length; i++)
        {
            buffCounts[i] = 0;
            BuffNumbertext[i].text = buffCounts[i].ToString();  // �e�L�X�g��0�ɐݒ�
        }

        // �f�o�t�̃J�E���g��0�Ƀ��Z�b�g���A�e�L�X�g���X�V
        for (int i = 0; i < DeBuffNumbertext.Length; i++)
        {
            debuffCounts[i] = 0;
            DeBuffNumbertext[i].text = debuffCounts[i].ToString();  // �e�L�X�g��0�ɐݒ�
        }

        // 5�^�[���J�E���g��0�Ƀ��Z�b�g���A�e�L�X�g���X�V
        for (int i = 0; i < FiveTurntext.Length; i++)
        {
            fiveCounts[i] = 0;
            FiveTurntext[i].text = fiveCounts[i].ToString();  // �e�L�X�g��0�ɐݒ�
        }

       
    }

    void Update()
    {
            PerformLottery();

        // ���Ԃ��o�߂����璊�I���s��
        //�Ō�̒��I����o�߂������Ԃ��Ǘ����邽�߂̕ϐ��ł��B���t���[�����Z����A�ݒ肳�ꂽ�Ԋu�𒴂����ꍇ�ɒ��I���s���܂��B
        timeSinceLastLottery += Time.deltaTime; // �o�ߎ��Ԃ����Z

        if (timeSinceLastLottery >= lotteryInterval)
        {
            // �����Œ��I���s��
            //LotNextBonus2();
            // �o�ߎ��Ԃ����Z�b�g���Ď��̒��I�܂ŃJ�E���g
            timeSinceLastLottery = 0f;
        }

        // Space�L�[�������ꂽ�Ƃ��ɃJ�E���g�����炷
        if (Input.GetKeyDown(KeyCode.A))
        {
            // FLOORCLEAR�̃J�E���g�_�E��
            if (fiveCounts[0] > 0)
            {
                fiveCounts[0]--;
                FiveTurntext[0].text = fiveCounts[0].ToString();
                if (fiveCounts[0] == 0)
                {
                    FiveTurntext[0].gameObject.SetActive(true); // 0�ɂȂ������\��
                    Buff[5].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            // SPACE_2_CLEAR�̃J�E���g�_�E��
            if (fiveCounts[1] > 0)
            {
                fiveCounts[1]--;
                FiveTurntext[1].text = fiveCounts[1].ToString();
                if (fiveCounts[1] == 0)
                {
                    FiveTurntext[1].gameObject.SetActive(true); // 0�ɂȂ������\��

                    DeBuff[6].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                }
            }
        }

    }

    // ���I���s�����\�b�h
    private void PerformLottery()
    {
        // �����񒊑I���s�������ꍇ�Afor���[�v�ŉ�
        for (int i = 0; i < 3; i++) // 3�񒊑I���s��
        {
            BuffOrDebuff(); // �o�t���f�o�t��I�сA���̌�ɏڍׂ�����
            //Debug.Log("���I����: " + buffCounts[0] + " " + debuffCounts[0]);
        }
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
                bonus = AllBonus.ALLHEEL;//�S��
                Debug.Log("���C�t�S��");
                break;
            case int n when (n >= 11 && n < 22)://�񕜂̏��+1
                bonus = AllBonus.HEELUP;
                Debug.Log("���C�t���+1");
                break;
            case int n when (n >= 22 && n < 33)://�U���̓A�b�v
                bonus = AllBonus.ATTACK1UP;
                Debug.Log("�U����+1");
                break;
            case int n when (n >= 33 && n < 44)://���G����̍U���𖳌�
                bonus = AllBonus.GUARD;
                Debug.Log("�_���[�W1�񖳌�");
                break;
            case int n when (n >= 44 && n < 55)://���̃^�[���œG�ɍU�����ꌂ�����
                bonus = AllBonus.ONEHITATTACK;
                Debug.Log("���̍U���ꌂ");
                break;
            case int n when (n >= 55 && n < 65)://�T�^�[���̊ԃ}�b�v�S�̂������
                bonus = AllBonus.FLOORCLEAR;
                Debug.Log("5�^�[���̊ԃt���A�S�̂�������");
                break;
            case int n when (n >= 65 && n < 70):
                bonus = AllBonus.ALLENEMYATTACK;//elemtal5
                Debug.Log("�������̓G��S��");
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
                Debug.Log("���C�t-1");
                break;
            case int n when (n >= 10 && n < 20):
                bonus = AllBonus.ATTACK1DOWN;
                Debug.Log("�U����-1");
                break;
            case int n when (n >= 20 && n < 30):
                bonus = AllBonus.EATTACK1UP;
                Debug.Log("�G�̍U����+1");
                break;
            case int n when (n >= 30 && n < 40):
                bonus = AllBonus.LIFE1DOWN;
                Debug.Log("���C�t���-1");
                break;
            case int n when (n >= 40 && n < 50):
                bonus = AllBonus.EHEEL1UP;
                Debug.Log("�G�̃��C�t���+1");
                break;
            case int n when (n >= 50 && n < 55):
                bonus = AllBonus.SPACE_2_CLEAR;
                Debug.Log("5�^�[���̊Ԏ���2�}�X���������Ȃ�");
                break;
            case int n when (n >= 55 && n < 60):
                bonus = AllBonus.EHEEL1UP1;
                Debug.Log("�������ɓG�𐶐�����");
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
            /*case AllBonus.ALLHEEL:
                if (buffCounts[0] < 1)  // buffCounts[0]��1�����̏ꍇ�̂ݑ���
                {
                    buffCounts[0]++;  // �J�E���g�𑝉�
                }
                Buff[0].SetActive(true);
                Buff[0].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                BuffNumbertext[0].text = buffCounts[0].ToString();  // �e�L�X�g���X�V

                break;*/
            case AllBonus.HEELUP:
                Buff[1].SetActive(true);
                buffCounts[0]++;  // �J�E���g�𑝉�
                //�o�t�̌��ʂ������K�p����Ă��邩���J�E���g����z��ł��B�e�o�t�ɑ΂��ēK�p�񐔂��L�^���܂��B
                BuffNumbertext[0].text = buffCounts[1].ToString();  // �e�L�X�g���X�V
                Buff[0].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                //DeBuff[0].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                break;
            case AllBonus.ATTACK1UP:
                Buff[1].SetActive(true);
                buffCounts[1]++;  // �J�E���g�𑝉�
                BuffNumbertext[1].text = buffCounts[2].ToString();  // �e�L�X�g���X�V
                Buff[1].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                //DeBuff[1].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                break;
           case AllBonus.GUARD:
                Buff[2].SetActive(true);
                if (buffCounts[2] < 1)  // buffCounts[0]��1�����̏ꍇ�̂ݑ���
                {
                    buffCounts[2]++;  // �J�E���g�𑝉�
                }
                Buff[2].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                BuffNumbertext[2].text = buffCounts[2].ToString();  // �e�L�X�g���X�V
                break;
            case AllBonus.ONEHITATTACK:
                Buff[3].SetActive(true);
                if (buffCounts[4] < 1)  // buffCounts[0]��1�����̏ꍇ�̂ݑ���
                {
                    buffCounts[4]++;  // �J�E���g�𑝉�
                }
                Buff[3].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                BuffNumbertext[3].text = buffCounts[4].ToString();  // �e�L�X�g���X�V
                break;
            case AllBonus.FLOORCLEAR:
                Buff[4].SetActive(true);
                //DeBuff[6].SetActive(false);
                FiveTurntext[0].gameObject.SetActive(true);
                fiveCounts[0] = 5; // 5�^�[���ɐݒ�
                FiveTurntext[0].text = fiveCounts[0].ToString();
                Buff[4].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                DeBuff[4].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                break;
            /*case AllBonus.ALLENEMYATTACK:
                //Buff[6].SetActive(true);
                if (buffCounts[5] < 1)  // buffCounts[0]��1�����̏ꍇ�̂ݑ���
                {
                    buffCounts[5]++;  // �J�E���g�𑝉�
                }
                Buff[6].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                BuffNumbertext[5].text = buffCounts[5].ToString();  // �e�L�X�g���X�V
                break;*/
            case AllBonus.HEELDOWN://�v���C���[��hp down
                DeBuff[0].SetActive(true);
                debuffCounts[0]++;
                DeBuffNumbertext[0].text = debuffCounts[0].ToString();
                DeBuff[0].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                break;
            case AllBonus.ATTACK1DOWN://�v���C���[�̍U����down
                DeBuff[1].SetActive(true);
                debuffCounts[1]++;
                DeBuffNumbertext[1].text = debuffCounts[1].ToString();
                DeBuff[1].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                break;
            case AllBonus.EHEEL1UP://�G��HPup
                DeBuff[2].SetActive(true);
                debuffCounts[2]++;
                DeBuffNumbertext[2].text = debuffCounts[2].ToString();
                DeBuff[2].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                break;
           
            case AllBonus.EATTACK1UP://�G�̍U����up
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
            //    debuffCounts[5]++;  // debuffCounts[5] ���C���N�������g
            //    // UI�̐F��ύX
            //    DeBuff[5].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            //    // debuffCounts[5] ��\�����邽�߂Ƀe�L�X�g���X�V
            //    DeBuffNumbertext[5].text = debuffCounts[5].ToString();  // debuffCounts[5] ��\��
            //    break;
            case AllBonus.SPACE_2_CLEAR:
                //Buff[5].SetActive(false);
                DeBuff[4].SetActive(true);
                FiveTurntext[1].gameObject.SetActive(true);
                fiveCounts[1] = 5; // 5�^�[���ɐݒ�
                FiveTurntext[1].text = fiveCounts[1].ToString();
                DeBuff[4].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                Buff[4].gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                break;
            default:
                break;
        }
    }

   

    // �o�t�ƃf�o�t�̃J�E���g�����Z�b�g���郁�\�b�h
    public void ResetCounts()
    {
        // �o�t�̃J�E���g�����Z�b�g
        for (int i = 0; i < buffCounts.Length; i++)
        {
            buffCounts[i] = 0;
            BuffNumbertext[i].text = buffCounts[i].ToString();  // �e�L�X�g���X�V
        }

        // �f�o�t�̃J�E���g�����Z�b�g
        for (int i = 0; i < debuffCounts.Length; i++)
        {
            debuffCounts[i] = 0;
            DeBuffNumbertext[i].text = debuffCounts[i].ToString();  // �e�L�X�g���X�V
        }

        // 5�^�[���J�E���g�����Z�b�g
        for (int i = 0; i < fiveCounts.Length; i++)
        {
            fiveCounts[i] = 0;
            FiveTurntext[i].text = fiveCounts[i].ToString();  // �e�L�X�g���X�V
        }

        
    }


    public void Lottery()
    {
        BuffOrDebuff(); // �o�t���f�o�t��I�сA���̌�ɏڍׂ�����
        print(bonus); // �I�΂ꂽ�{�[�i�X�̎�ނ�\��
        print(BuffNumbertext);//�o�t
        print(DeBuffNumbertext);//�f�o�t
       
    }
}

