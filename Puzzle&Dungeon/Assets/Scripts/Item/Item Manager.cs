using System;
using System.Drawing;
using UnityEngine;
using Common;
using Data;


public class ItemManager : MonoBehaviour
{
    /// <summary>次の階層にいった際のボーナス抽選</summary>
    public NextFloorBonus LotNextBonus()
    {
        NextFloorBonus Bonus = NextFloorBonus.NONE;

        //抽選用数値
        int num = UnityEngine.Random.Range(0, 4);// 0, 1, 2, 3 のいずれか

        //抽選




        //抽選結果の分岐
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