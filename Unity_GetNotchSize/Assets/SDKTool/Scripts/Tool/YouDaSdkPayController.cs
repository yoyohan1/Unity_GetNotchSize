//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System;

//namespace yoyohan.YouDaSdkTool
//{
//    public class YouDaSdkPayController : YouDaSdkControllerBase
//    {

//        private Action<RechargeConfig> callbackSucceed = null;

//        private Action callbackFailed = null;
//#if UNITY_IOS&&USE_IAP
//        private YouDaSdkPayiOSController youDaSdkPayiOSController;
//#endif

//        public YouDaSdkPayController(YouDaSdk youDaSdk)
//            : base(youDaSdk)
//        {
//#if UNITY_IOS&&USE_IAP
//           youDaSdkPayiOSController = new YouDaSdkPayiOSController();
//           youDaSdkPayiOSController.iOSPurchaseInit(); //IOS内购初始化
//#endif
//        }

//        public void Pay(RechargeConfig rechargeConfig, Action<RechargeConfig> callbackSucceed, Action callbackFailed = null)
//        {
//            this.callbackSucceed = callbackSucceed;
//            this.callbackFailed = callbackFailed;

//            /*RechargeConfig rechargeConfig = new RechargeConfig();
//            rechargeConfig.productID = MShopCfgRowData.id.ToString();
//            rechargeConfig.currencyAmount = MShopCfgRowData.rmb;
//            rechargeConfig.virtualCurrencyAmount = MShopCfgRowData.PackaginggameObjectes[0].PackagingNum;
//            rechargeConfig.localizedDescription = MShopCfgRowData.name;
//            rechargeConfig.out_trade_number = GameTools.GetNowTimeStampMillis().ToString();
//            rechargeConfig.order_sign = "";*/

//#if UNITY_EDITOR
//            OnSucceed(JsonUtility.ToJson(rechargeConfig));
//#elif UNITY_ANDROID
//             youDaSdk.SendMessageToAndroid("MyPay", rechargeConfig.productID.ToString());
//#elif UNITY_IPHONE&&USE_IAP
//			//youDaSdkPayiOSController.MyPay(payInfo.money, payInfo.productName, payInfo.sku, payInfo.UserId);
//             youDaSdkPayiOSController.m_Controller.InitiatePurchase(rechargeConfig.productID);
//#endif

//#if OpenTalkingData
//            TDGAVirtualCurrency.OnChargeRequest(rechargeConfig.out_trade_number, rechargeConfig.productID + "", rechargeConfig.currencyAmount, "CH", rechargeConfig.virtualCurrencyAmount, "火星渠道支付");
//            Debug.Log("发送OnChargeRequest埋点:" + " currentOrderid:" + rechargeConfig.out_trade_number + " iapID:" + rechargeConfig.productID + "" + " currencyAmount:" + rechargeConfig.currencyAmount + " virtualCurrencyAmount:" + rechargeConfig.virtualCurrencyAmount);
//#endif

//        }


//        public override void OnSucceed(string msg, int requestId = 0)
//        {
//            Debug.Log("成功，接收到Sdk返回的数据—商品信息：" + msg);

//            RechargeConfig rc = JsonUtility.FromJson<RechargeConfig>(msg);

//            if (callbackSucceed != null)
//            {
//                callbackSucceed(rc);
//            }

//            callbackSucceed = null;
//            callbackFailed = null;

//#if OpenTalkingData
//            TDGAVirtualCurrency.OnChargeSuccess(rc.out_trade_number);
//            Debug.Log("发送OnChargeSuccess埋点:" + " currentOrderid:" + rc.out_trade_number);
//#endif
//        }

//        public override void OnFailed(string msg, int requestId = 0)
//        {
//            Debug.LogError("失败，接收到Sdk返回的数据—商品信息：" + msg);

//            if (callbackFailed != null)
//            {
//                callbackFailed();
//            }

//            callbackSucceed = null;
//            callbackFailed = null;
//        }


//    }



//}