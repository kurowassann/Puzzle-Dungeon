using System;
using System.Drawing;
using UnityEngine;
using Common;
using Data;


public class ItemManager : MonoBehaviour
{
    /// <summary>次の階層にいった際のボーナス抽選</summary>
    public AllBonus LotNextBonus()
    {
        AllBonus Bonus = AllBonus.NONE;

        //抽選用数値
        int num = UnityEngine.Random.Range(0, 100);// 0, 1, 2, 3 のいずれか

        //抽選



        //確率を付ける事


        //抽選結果の分岐
        switch (num)
        {
            case int n when (n < 30):            // 0～29（30%の確率）
                Bonus = AllBonus.ALLHEEL;     //30
                break;
            case int n when (n >= 30 && n < 60): // 30～59（30%の確率）
                Bonus = AllBonus.HEELUP;   //30
                break;
            case int n when (n >= 60 && n < 90): // 60～89（30%の確率）
                Bonus = AllBonus.ATTACK1UP;    //30
                break;
            case int n when (n >= 90):           // 90～99（10%の確率）
                Bonus = AllBonus.GUARD;     //10
                break;
            case int n when (n < 30):            // 0～29（30%の確率）
                Bonus = AllBonus.ALLHEEL;     //30
                break;
            case int n when (n >= 30 && n < 60): // 30～59（30%の確率）
                Bonus = AllBonus.HEELUP;   //30
                break;
            case int n when (n >= 60 && n < 90): // 60～89（30%の確率）
                Bonus = AllBonus.ATTACK1UP;    //30
                break;
            case int n when (n >= 90):           // 90～99（10%の確率）
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