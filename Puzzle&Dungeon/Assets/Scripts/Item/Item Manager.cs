using System;
using System.Drawing;
using UnityEngine;
using Common;
using Data;
using Common;

public class ItemManager : MonoBehaviour
{
    /// <summary>���̊K�w�ɂ������ۂ̃{�[�i�X���I</summary>
    public NextFloorBonus LotNextBonus()
    {
        NextFloorBonus Bonus = NextFloorBonus.NONE;

        //���I�p���l
        int num = 0;

        //���I
        num = 0;

        //���I���ʂ̕���
        switch(num)
        {
            case 0:
                Bonus = NextFloorBonus.HEEL;    
                break;
            case 1:
                Bonus = NextFloorBonus.ATTACK;
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