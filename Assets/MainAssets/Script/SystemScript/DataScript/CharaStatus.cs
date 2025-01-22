using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create StatusData")]
public class CharaStatus : ScriptableObject
{
    public string NAME;     //ƒLƒƒƒ‰–¼
    public int MAXHP;       //Å‘åHP
    public int MAXMP;       //Å‘åMP
    public int ATK;         //UŒ‚—Í
    public int DEF;         //–hŒä—Í
    public int INT;         //–‚—Í
    public int RES;         //–‚–@’ïR—Í
    public int AGI;         //ˆÚ“®‘¬“x
    public int LV;          //ƒŒƒxƒ‹
    public int GETEXP;      //æ“¾ŒoŒ±’l
    public int GETGOLD;     //æ“¾‚Å‚«‚é‚¨‹à
    public float ShortAttackRange;    //“G‚ÌUŒ‚”ÍˆÍ
    public float EnemyTime;   //“G‚ÌUŒ‚ŠÔŠu
}
