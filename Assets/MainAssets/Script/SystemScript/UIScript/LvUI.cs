using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour
{
    [SerializeField] PlayerStatus charadata;
    [SerializeField] Text NAME;
    [SerializeField] Text LV;

    // Update is called once per frame
    void Update()
    {
        string charaname = charadata.NAME;
        int lv = charadata.LV;

        NAME.text = charaname;
        string lvtext = ($"LV{lv}");
        LV.text = lvtext;
    }
}