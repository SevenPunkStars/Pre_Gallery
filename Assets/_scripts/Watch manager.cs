using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Watchmanager : MonoBehaviour
{
    TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {

        if (text != null) {
            string min = ((DateTime.Now.Minute.ToString().Length == 1) ? "0" : "") + DateTime.Now.Minute.ToString();
            string hour = ((DateTime.Now.Hour.ToString().Length == 1) ? "0" : "") + DateTime.Now.Hour.ToString();

            text.text = hour+"\n"+min;
        }

    }
}
