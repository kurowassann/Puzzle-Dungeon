using System;
using System.Drawing;
using UnityEngine;
using Common;
using Data;


public class ItemManager : MonoBehaviour
{
    /// <summary>���̊K�w�ɂ������ۂ̃{�[�i�X���I</summary>
    public NextFloorBonus LotNextBonus()
    {
        NextFloorBonus Bonus = NextFloorBonus.NONE;

        //���I�p���l
        int num = UnityEngine.Random.Range(0, 4);// 0, 1, 2, 3 �̂����ꂩ

        //���I




        //���I���ʂ̕���
        switch (num)
        {
            case 0:
                Bonus = NextFloorBonus.HEEL;    
                break;
            case 1:
                Bonus = NextFloorBonus.ATTACK;
                break;
            case 2:
                Bonus = NextFloorBonus.GUARD;
                break;
            case 3:
                Bonus = NextFloorBonus.MOVE;
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