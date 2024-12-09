using System;
using System.Drawing;
using UnityEngine;
using Common;
using Data;
using Common;

public class ItemManager : MonoBehaviour
{
    /// <summary>次の階層にいった際のボーナス抽選</summary>
    public NextFloorBonus LotNextBonus()
    {
        NextFloorBonus Bonus = NextFloorBonus.NONE;

        //抽選用数値
        int num = 0;

        //抽選
        num = 0;

        //抽選結果の分岐
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