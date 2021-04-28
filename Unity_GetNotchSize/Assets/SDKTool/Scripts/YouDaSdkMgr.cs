using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yoyohan.YouDaSdkTool
{
    public class YouDaSdkMgr
    {
        private static readonly YouDaSdkMgr _instance = new YouDaSdkMgr();
        public static YouDaSdkMgr instance { get { return _instance; } }

        public Action<ResponceMessage> OnGetSDKResponce;


        #region 核心代码
        private AndroidJavaClass unityClass;
        private AndroidJavaObject currActivity;

        private YouDaSdkMgr()
        {
            //1.生成接收SDK消息的Mono
            if (YouDaSdkMono.instance == null)
            {
                GameObject youDaSdkGo = new GameObject("YouDaSdk");
                youDaSdkGo.AddComponent<YouDaSdkMono>();
            }

            //2.初始化AndroidJavaClass
            if (Application.platform == RuntimePlatform.Android)
            {
                unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            }
            Debug.Log("YouDaSdkMgr初始化完成！");
        }

        public void SendMessageToAndroid(string methodName, bool isStaticMethod = false, params object[] para)
        {
            Debug.Log("向SDk发送" + methodName + "请求！para:" + para.Length);

            if (Application.platform != RuntimePlatform.Android) return;

            if (isStaticMethod)
            {
                currActivity.CallStatic(methodName, para);
            }
            else
            {
                currActivity.Call(methodName, para);
            }
        }

        public Tvalue SendMessageToAndroid<Tvalue>(string methodName, bool isStaticMethod = false, params object[] para)
        {
            Debug.Log("向SDk发送" + methodName + "请求！para:" + para.Length);

            if (Application.platform != RuntimePlatform.Android) return default(Tvalue);

            if (isStaticMethod)
            {
                return currActivity.CallStatic<Tvalue>(methodName, para);
            }
            else
            {
                return currActivity.Call<Tvalue>(methodName, para);
            }
        }

        /// <summary>
        /// 调用指定包名、指定类
        /// </summary>
        public void SendMessageToAndroidByPackage(string packageName, string methodName, bool isStaticMethod = false, params object[] para)
        {
            Debug.Log("向SDk发送" + methodName + "请求！ para:" + string.Join(" para:", this.ConvertObjectArrayToStringArray(para)));

            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJavaObject activity = new AndroidJavaObject(packageName);

            if (isStaticMethod)
            {
                activity.CallStatic(methodName, para);
            }
            else
            {
                activity.Call(methodName, para);
            }
        }


        /// <summary>
        /// 调用指定包名、指定类
        /// </summary>
        public Tvalue SendMessageToAndroidByPackage<Tvalue>(string packageName, string methodName, bool isStaticMethod = false, params object[] para)
        {
            Debug.Log("向SDk发送" + methodName + "请求！ para:" + string.Join(" para:", this.ConvertObjectArrayToStringArray(para)));

            if (Application.platform != RuntimePlatform.Android) return default(Tvalue);

            AndroidJavaObject activity = new AndroidJavaObject(packageName);

            if (isStaticMethod)
            {
                return activity.CallStatic<Tvalue>(methodName, para);
            }
            else
            {
                return activity.Call<Tvalue>(methodName, para);
            }

        }
        #endregion


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
#if UNITY_EDITOR
            OnGetNotchSize(100, 10);
#elif UNITY_ANDROID
            SendMessageToAndroidByPackage("com.yoyohan.getnotchsize.MainActivity", "getNotchSize", false);
#elif UNITY_IOS
            OnGetNotchSize(getNotchSize_iOS(),getNotchSize_iOS());
#else
            OnGetNotchSize(0, 0);
#endif
        }

        public float notchSize;
        public float homeSize;
        public Action OnNotchSizeChangedAction;

        //获取NotchSize成功
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

        #endregion



        /// <summary>
        /// 转换object数组为string数组，方便打印
        /// </summary>
        private String[] ConvertObjectArrayToStringArray(object[] para)
        {
            string[] pataStr = new string[para.Length];
            for (int i = 0; i < para.Length; i++)
            {
                pataStr[i] = para[i].ToString();
            }

            return pataStr;
        }

    }


}