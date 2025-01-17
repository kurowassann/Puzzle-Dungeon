using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    public Text Scoretext;
    private int maxcan;
    private int can;

    // Start is called before the first frame update
    void Start()
    {
        maxcan = 10;
        can = 0;
        Scoretext.text = "Score:" + can + "/" + maxcan;
    }
    // Update is called once per frame
    void Update()
    {
        Scoretext.text = "Score:" + can + "/" + maxcan;
        if (Input.GetKey(KeyCode.Space))
        {
            can++;
        }
    }
}