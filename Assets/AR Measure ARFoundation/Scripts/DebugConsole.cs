using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    public static DebugConsole DC;
    Text debugTxt;

    // Start is called before the first frame update
    void Start()
    {
        DC = this;
        debugTxt = GetComponent<Text>();
        debugTxt.text = "debug";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Log(string str)
    {
        debugTxt.text = str + "\n" + debugTxt.text;
    }
}
