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
        AllBonus Bonus = AllBonus.NONE;

        //���I�p���l
        int num = UnityEngine.Random.Range(0, 100);// 0, 1, 2, 3 �̂����ꂩ

        //���I



        //�m����t���鎖


        //���I���ʂ̕���
        switch (num)
        {
            case int n when (n < 30):            // 0�`29�i30%�̊m���j
                Bonus = AllBonus.ALLHEEL;     //30
                break;
            case int n when (n >= 30 && n < 60): // 30�`59�i30%�̊m���j
                Bonus = AllBonus.HEELUP;   //30
                break;
            case int n when (n >= 60 && n < 90): // 60�`89�i30%�̊m���j
                Bonus = AllBonus.ATTACK1UP;    //30
                break;
            case int n when (n >= 90):           // 90�`99�i10%�̊m���j
                Bonus = AllBonus.GUARD;     //10
                break;
            case int n when (n < 30):            // 0�`29�i30%�̊m���j
                Bonus = AllBonus.ALLHEEL;     //30
                break;
            case int n when (n >= 30 && n < 60): // 30�`59�i30%�̊m���j
                Bonus = AllBonus.HEELUP;   //30
                break;
            case int n when (n >= 60 && n < 90): // 60�`89�i30%�̊m���j
                Bonus = AllBonus.ATTACK1UP;    //30
                break;
            case int n when (n >= 90):           // 90�`99�i10%�̊m���j
                Bonus = AllBonus.GUARD;     //10
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