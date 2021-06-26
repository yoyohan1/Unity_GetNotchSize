using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yoyohan.YouDaSdkTool;

namespace yoyohan.getnotchsize
{
    public class GetNotchSizeMgr
    {
        private static GetNotchSizeMgr _instance;
        public static GetNotchSizeMgr instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GetNotchSizeMgr();
                }
                return _instance;
            }
        }

        public float notchSize;
        public float homeSize;
        public Action OnNotchSizeChangedAction;

        private const string requestId = "GETNOTCHSIZE";
        private string packageName = "com.yoyohan.getnotchsize.MainActivity";


        private GetNotchSizeMgr()
        {
            YouDaSdkMgr.instance.OnGetSDKResponce += OnGetSDKResponce;
        }

        ~GetNotchSizeMgr()
        {
            YouDaSdkMgr.instance.OnGetSDKResponce -= OnGetSDKResponce;
        }




        #region 刘海屏的代码
        //GitHub地址： https://github.com/yoyohan1/UnityJar_GetNotchSize
#if UNITY_IOS
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern bool getIsNotch_iOS();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern float getNotchSize_iOS();
#endif

        public void getNotchSize()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                OnGetNotchSize(100, 10);
                return;
            }

#if UNITY_ANDROID
            YouDaSdkMgr.instance.SendMessageToAndroidByPackage(packageName, "getNotchSize", false);
#elif UNITY_IOS
            OnGetNotchSize(getNotchSize_iOS(),getNotchSize_iOS());
#endif
        }
        #endregion


        void OnGetSDKResponce(ResponceMessage responceMessage)
        {
            if (responceMessage.requestId == requestId)
            {
                OnGetNotchSize(responceMessage.msg.GetValue<int>("notchSize"), 0);
            }
        }

        public void OnGetNotchSize(float nSize, float hSize)
        {
            notchSize = nSize;
            homeSize = hSize;
            if (OnNotchSizeChangedAction != null)
            {
                Debug.Log("OnNotchSizeChangedAction触发！notchSize：" + notchSize + " homeSize：" + homeSize);
                OnNotchSizeChangedAction();
            }
        }
    }
}

