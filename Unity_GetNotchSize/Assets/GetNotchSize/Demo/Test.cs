using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using yoyohan.YouDaSdkTool;

namespace yoyohan.getnotchsize.demo
{
    public class Test : MonoBehaviour
    {
        private bool temp = true;

        void Start()
        {
            //Debug.Log(Screen.fullScreen);
            //Screen.fullScreen = true;
            transform.GetComponent<Button>().onClick.AddListener(onGetNotchSizeBtnClick);
        }

        void onGetNotchSizeBtnClick()
        {
            if (temp)
            {
                GetNotchSizeMgr.instance.getNotchSize();
            }
            else
            {
                GetNotchSizeMgr.instance.OnGetNotchSize(0, 0);
            }
            temp = !temp;
        }
    }
}