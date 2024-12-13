using System;
using System.Drawing;
using UnityEngine;
using Common;
using Data;


public class ItemManager : MonoBehaviour
{
    /// <summary>���̊K�w�ɂ������ۂ̃{�[�i�X���I</summary>
    public AllBonus LotNextBonus()
    {
        AllBonus Bonus = AllBonus.ENEMYROOM;

        //���I�p���l
        int num = UnityEngine.Random.Range(0, 100);// 0, 1, 2, 3 �̂����ꂩ

        //���I



        //�m����t���鎖


        //���I���ʂ̕���
        switch (num)
        {
            //�����b�g70%

            case int n when (n < 11):            // 0�`11�i11%�̊m���j
                Bonus = AllBonus.ALLHEEL;     
                break;
            case int n when (n >= 11 && n < 22): // 11�`21�i11%�̊m���j
                Bonus = AllBonus.HEELUP;  
                break;
            case int n when (n >= 22 && n < 33): // 23�`32�i11%�̊m���j
                Bonus = AllBonus.ATTACK1UP;   
                break;
            case int n when (n >= 33 && n < 44): // 32�`41�i11%�̊m���j
                Bonus = AllBonus.GUARD;     
                break;
            case int n when (n >= 44 && n < 55): // 41�`50�i11%�̊m���j
                Bonus = AllBonus.ONEHITATTACK;    
                break;
            case int n when (n >= 55 && n < 65): // 30�`59�i10%�̊m���j
                Bonus = AllBonus.FLOORCLEAR;   
                break;
            case int n when (n >= 65 && n < 70): // 60�`89�i5%�̊m���j
                Bonus = AllBonus.ALLENEMYATTACK;  
                break;
                
                //�f�����b�g30%

            case int n when (n >= 70 && n < 75): // 90�`99�i5%�̊m���j
                Bonus = AllBonus.HEELDOWN;     
                break;
            case int n when (n >= 75 && n < 80): // 0�`29�i5%�̊m���j
                Bonus = AllBonus.ATTACK1DOWN;   
                break;
            case int n when (n >= 80 && n < 85): // 30�`59�i5%�̊m���j
                Bonus = AllBonus.EATTACK1UP;  
                break;
            case int n when (n >= 85 && n < 90): // 60�`89�i5%�̊m���j
                Bonus = AllBonus.LIFE1DOWN;    
                break;
            case int n when (n >= 90 && n < 95): // 90�`99�i5%�̊m���j
                Bonus = AllBonus.EHEEL1UP;     
                break;
            case int n when (n >= 95 && n < 97.5): // 0�`29�i2.5%�̊m���j
                Bonus = AllBonus.SPACE_2_CLEAR;     
                break;
            case int n when (n >= 97.5 && n < 99): // 96�`98.5�i2.5%�̊m���j
                Bonus = AllBonus.ENEMYROOM;   
                break;
            default:
                break;
        }


        return Bonus;
    }

    //
    public void Lottery()
    {
        print(LotNextBonus());
    }
}