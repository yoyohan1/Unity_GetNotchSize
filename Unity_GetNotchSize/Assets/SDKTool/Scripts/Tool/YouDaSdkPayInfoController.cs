//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//namespace yoyohan.YouDaSdkTool
//{
//    public class YouDaSdkPayInfoController : YouDaSdkControllerBase
//    {
//        public YouDaSdkPayInfoController(YouDaSdk youDaSdk) : base(youDaSdk) { }

//        private static List<RechargeConfig> _lisRechargeCfg = null;
//        public static List<RechargeConfig> lisRechargeCfg
//        {
//            get
//            {
//                if (_lisRechargeCfg == null)
//                {
//                    _lisRechargeCfg = new List<RechargeConfig>();
//                    _lisRechargeCfg.Add(new RechargeConfig() { productID = "com.foyogame.helu.ios.20", localizedPriceString = "￥1.00", virtualCurrencyAmount = 20 });
//                    _lisRechargeCfg.Add(new RechargeConfig() { productID = "com.foyogame.helu.ios.120", localizedPriceString = "￥6.00", virtualCurrencyAmount = 120 });
//                    _lisRechargeCfg.Add(new RechargeConfig() { productID = "com.foyogame.helu.ios.250", localizedPriceString = "￥12.00", virtualCurrencyAmount = 250 });
//                    _lisRechargeCfg.Add(new RechargeConfig() { productID = "com.foyogame.helu.ios.650", localizedPriceString = "￥30.00", virtualCurrencyAmount = 650 });
//                    _lisRechargeCfg.Add(new RechargeConfig() { productID = "com.foyogame.helu.ios.1400", localizedPriceString = "￥68.00", virtualCurrencyAmount = 1400 });
//                    _lisRechargeCfg.Add(new RechargeConfig() { productID = "com.foyogame.helu.ios.4500", localizedPriceString = "￥198.00", virtualCurrencyAmount = 4500 });
//                    _lisRechargeCfg.Add(new RechargeConfig() { productID = "com.foyogame.helu.ios.1", localizedPriceString = "￥1.00", virtualCurrencyAmount = 0 });
//                    _lisRechargeCfg.Add(new RechargeConfig() { productID = "com.foyogame.helu.ios.6", localizedPriceString = "￥6.00", virtualCurrencyAmount = 100 });
//                    _lisRechargeCfg.Add(new RechargeConfig() { productID = "com.foyogame.helu.ios.12", localizedPriceString = "￥12.00", virtualCurrencyAmount = 200 });
//                }
//                return _lisRechargeCfg;
//            }
//            set
//            {
//                _lisRechargeCfg = value;
//            }
//        }

//        public RechargeConfig GetRechargeConfigByID(int id)
//        {
//            return lisRechargeCfg.TryGet<RechargeConfig>(id);
//        }

//        public override void OnSucceed(string msg, int requestId = 0)
//        {
//            Debug.Log("成功，接收到Sdk返回的数据—商品信息：" + msg);
//        }

//        public override void OnFailed(string msg, int requestId = 0)
//        {
//            Debug.LogError("失败，接收到Sdk返回的数据—商品信息：" + msg);
//        }
//    }


//    [Serializable]
//    public class RechargeConfig
//    {
//        /// <summary>
//        ///(必选) 计费点ID(或苹果iapID)
//        /// </summary>
//        public string productID;
//        /// <summary>
//        ///(必选) 现金货币
//        /// </summary>
//        public double currencyAmount;
//        /// <summary>
//        ///(必选) 虚拟货币
//        /// </summary>
//        public double virtualCurrencyAmount;
//        /// <summary>
//        /// 本地化价格字符串
//        /// </summary>
//        public string localizedPriceString;//本地化价格例如 ￥6.00，用于按钮展示
//        /// <summary>
//        /// 本地化名称
//        /// </summary>
//        public string localizedTitle;
//        /// <summary>
//        /// 本地化描述
//        /// </summary>
//        public string localizedDescription;
//        /// <summary>
//        ///(必选) 客户端自己创建的订单号
//        /// </summary>
//        public string out_trade_number;
//        /// <summary>
//        ///(可选) 有的sdk需要下单前验签，把签名传入sdk支付接口作为参数，验签内容一般为MD5（currencyAmount+virtualCurrencyAmount+secret_key等等）
//        /// </summary>
//        public string order_sign;

//        public RechargeConfig()
//        {

//        }

//        public RechargeConfig(string productID, double currencyAmount, double virtualCurrencyAmount, string localizedPriceString, string localizedTitle, string localizedDescription, string out_trade_number, string order_sign = "")
//        {
//            this.productID = productID;
//            this.currencyAmount = currencyAmount;
//            this.virtualCurrencyAmount = virtualCurrencyAmount;
//            this.localizedPriceString = localizedPriceString;
//            this.localizedTitle = localizedTitle;
//            this.localizedDescription = localizedDescription;
//            this.out_trade_number = out_trade_number;
//            this.order_sign = order_sign;
//        }
//    }

//    /*order_sign使用例子
//    /// <summary>
//    /// bilibili下单前需要网络验签
//    /// </summary>
//    IEnumerator IEPay(RechargeConfig rechargeConfig)
//    {
//        string url = "http://111.231.201.80:8081/bilibiliserver/xiadansign.php";
//        WWWForm wwwform = new WWWForm();
//        wwwform.AddField("virtualMoney", (int)rechargeConfig.virtualCurrencyAmount);
//        wwwform.AddField("fee", (int)rechargeConfig.currencyAmount);
//        wwwform.AddField("out_trade_number", rechargeConfig.out_trade_number);
//        yield return StartCoroutine(GameTools.HttpPost(url, wwwform, new Action<WWW>((WWW www) => {
//            Debug.Log("网络中验签过的order_sign:" + rechargeConfig.order_sign);
//            rechargeConfig.order_sign = www.text;
//            YouDaSdkTool.YouDaSdk.instance.GetControllerById(1).ToController<YouDaSdkTool.YouDaSdkPayController>().Pay(rechargeConfig, OnPaySucceed);
//        })));
//    }
//     */


//}
