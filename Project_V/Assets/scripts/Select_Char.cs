using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Select_Char : MonoBehaviour {
    public string _ID;
    public int Score;
    public Sprite ch;

    public Select_Char(string _id , int score, Sprite cha)
    {
        _ID = _id;
        Score = score;
        ch = cha;
    }

}
