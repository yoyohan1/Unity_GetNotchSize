using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace yoyohan.YouDaSdkTool
{
    public class YouDaSdkMono : MonoBehaviour
    {
        public static YouDaSdkMono instance;

        void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// 当sdk返回消息进行处理
        /// </summary>
        public void OnOperationResponce(string returnStr)
        {
            Debug.Log("得到Sdk端的响应:" + returnStr);

            ResponceMessage responceMessage = new ResponceMessage(JsonMapper.ToObject(returnStr));

            if (responceMessage.requestId== "GETNOTCHSIZE")
            {
                YouDaSdkMgr.instance.OnGetNotchSize(responceMessage.msg.GetValue<int>("notchSize"));
            }

            if (YouDaSdkMgr.instance.OnGetSDKResponce != null)
            {
                YouDaSdkMgr.instance.OnGetSDKResponce(responceMessage);
            }
        }


    }


    [Serializable]
    public class ResponceMessage
    {
        /*  
        * requestId 发送的请求ID Unity根据此值判断做出响应
        * msg 消息体 Json格式
        */

        public string requestId;
        public JsonData msg;

        public ResponceMessage(JsonData jsonData)
        {
            this.requestId = jsonData.GetValue<string>("requestId");
            this.msg = jsonData.GetValue<JsonData>("msg");
        }
    }

}
