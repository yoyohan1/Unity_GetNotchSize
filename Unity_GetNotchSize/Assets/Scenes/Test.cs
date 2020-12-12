using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(yoyohan.YouDaSdkTool.YouDaSdkMgr.instance.getNotchSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
