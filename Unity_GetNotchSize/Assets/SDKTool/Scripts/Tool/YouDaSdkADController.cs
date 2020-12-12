//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace yoyohan.YouDaSdkTool
//{
//    public class YouDaSdkADController : YouDaSdkControllerBase
//    {
//        public YouDaSdkADController(YouDaSdk youDaSdk)
//            : base(youDaSdk)
//        {

//        }

//        private Action callbackSucceed = null;
//        private Action callbackFailed = null;

//        /// <summary>
//        /// 加载广告
//        /// </summary>
//        /// <param name="adType">1横幅,2插屏,3视频</param>
//        public void LoadAD(int adType)
//        {
//            string methodName = "";

//            if (adType == 1)
//            {
//                methodName = "loadBanner";
//            }
//            else if (adType == 2)
//            {
//                methodName = "loadInsert";
//            }
//            else if (adType == 3)
//            {
//                methodName = "loadVideo";
//            }
//            youDaSdk.SendMessageToAndroid(methodName);
//        }


//        /// <summary>
//        /// 展示广告
//        /// </summary>
//        /// <param name="adType">1横幅,2插屏,3视频</param>
//        public void ShowAD(int adType, Action callbackSucceed, Action callbackFailed=null)
//        {
//            this.callbackSucceed = callbackSucceed;
//            this.callbackFailed = callbackFailed;
//            string methodName = "";

//            if (adType == 1)
//            {
//                methodName = "showBanner";
//            }
//            else if (adType == 2)
//            {
//                methodName = "showInsert";
//            }
//            else if (adType == 3)
//            {
//                methodName = "showVideo";
//            }
//            youDaSdk.SendMessageToAndroid(methodName);
//        }

//        public override void OnSucceed(string msg, int requestId=0)
//        {
//            Debug.Log("成功，接收到Sdk返回的数据:" + msg);

//            if (callbackSucceed != null)
//            {
//                callbackSucceed();
//                callbackSucceed = null;
//                callbackFailed = null;
//            }
//        }

//        public override void OnFailed(string msg, int requestId = 0)
//        {
//            Debug.Log("失败，接收到Sdk返回的数据:" + msg);
//            if (callbackFailed != null)
//            {
//                callbackFailed();
//                callbackSucceed = null;
//                callbackFailed = null;
//            }
//        }
//    }
//}


