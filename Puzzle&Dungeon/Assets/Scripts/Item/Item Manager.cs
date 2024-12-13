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
        AllBonus Bonus = AllBonus.ENEMYROOM;

        //抽選用数値
        int num = UnityEngine.Random.Range(0, 100);// 0, 1, 2, 3 のいずれか

        //抽選



        //確率を付ける事


        //抽選結果の分岐
        switch (num)
        {
            //メリット70%

            case int n when (n < 11):            // 0～11（11%の確率）
                Bonus = AllBonus.ALLHEEL;     
                break;
            case int n when (n >= 11 && n < 22): // 11～21（11%の確率）
                Bonus = AllBonus.HEELUP;  
                break;
            case int n when (n >= 22 && n < 33): // 23～32（11%の確率）
                Bonus = AllBonus.ATTACK1UP;   
                break;
            case int n when (n >= 33 && n < 44): // 32～41（11%の確率）
                Bonus = AllBonus.GUARD;     
                break;
            case int n when (n >= 44 && n < 55): // 41～50（11%の確率）
                Bonus = AllBonus.ONEHITATTACK;    
                break;
            case int n when (n >= 55 && n < 65): // 30～59（10%の確率）
                Bonus = AllBonus.FLOORCLEAR;   
                break;
            case int n when (n >= 65 && n < 70): // 60～89（5%の確率）
                Bonus = AllBonus.ALLENEMYATTACK;  
                break;
                
                //デメリット30%

            case int n when (n >= 70 && n < 75): // 90～99（5%の確率）
                Bonus = AllBonus.HEELDOWN;     
                break;
            case int n when (n >= 75 && n < 80): // 0～29（5%の確率）
                Bonus = AllBonus.ATTACK1DOWN;   
                break;
            case int n when (n >= 80 && n < 85): // 30～59（5%の確率）
                Bonus = AllBonus.EATTACK1UP;  
                break;
            case int n when (n >= 85 && n < 90): // 60～89（5%の確率）
                Bonus = AllBonus.LIFE1DOWN;    
                break;
            case int n when (n >= 90 && n < 95): // 90～99（5%の確率）
                Bonus = AllBonus.EHEEL1UP;     
                break;
            case int n when (n >= 95 && n < 97.5): // 0～29（2.5%の確率）
                Bonus = AllBonus.SPACE_2_CLEAR;     
                break;
            case int n when (n >= 97.5 && n < 99): // 96～98.5（2.5%の確率）
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