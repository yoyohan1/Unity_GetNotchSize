#if UNITY_IOS&&USE_IAP
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Purchasing;

namespace yoyohan.YouDaSdkTool
{

    public class YouDaSdkPayiOSController : YouDaSdkPayController, IStoreListener
    {

        //[DllImport("__Internal")]
        //private static extern void MyPay(string money, string productName, string productId, string userId);

        public IStoreController m_Controller;

        private IAppleExtensions m_AppleExtensions;

        public void iOSPurchaseInit()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                var module = StandardPurchasingModule.Instance();
                var builder = ConfigurationBuilder.Instance(module);
                //添加多个ID到builder里，ID命名方式建议字母+数字结尾，比如ShuiJing_1,ShuiJing_2，
                //注意ProductType的类型，Consumable是可以无限购买(比如水晶)，NonConsumable是只能购买一次(比如关卡)，Subscription是每月订阅(比如VIP)

                foreach (RechargeConfig item in YouDaSdkPayInfoController.lisRechargeCfg)
                {
                    builder.AddProduct(item.productID, ProductType.Consumable, new IDs { { item.productID, AppleAppStore.Name } });
                }

                //开始初始化
                UnityPurchasing.Initialize(this, builder);
            }
        }
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("Billing failed to initialize!");
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            m_Controller = controller;
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();

            m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

            Debug.Log("Available items:");
            foreach (var item in controller.products.all)
            {
                if (item.availableToPurchase)
                {
                    Debug.Log(string.Join(" - ",
                        new[]
                        {
                            item.metadata.localizedTitle,
                            item.metadata.localizedDescription,
                            item.metadata.isoCurrencyCode,
                            item.metadata.localizedPrice.ToString(),
                            item.metadata.localizedPriceString,
                            item.transactionID,
                            item.receipt
                        }));
                }
            }

            //把价格列表进行修改
            for (int i = 0; i < controller.products.all.Length; i++)
            {
                RechargeConfig rc = youDaSdk.GetControllerById(2).ToController<YouDaSdkPayInfoController>().GetRechargeConfigByID(i);
                rc.localizedPriceString = controller.products.all[i].metadata.localizedPriceString;
                rc.localizedDescription = controller.products.all[i].metadata.localizedDescription;
                rc.localizedTitle = controller.products.all[i].metadata.localizedTitle;
                rc.currencyAmount = (double)controller.products.all[i].metadata.localizedPrice;
            }
        }

        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            Debug.Log("苹果内购Purchase failed: " + i.definition.id + " 原因:" + p.ToString());

            base.OnFailed(p.ToString());
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            Debug.Log("苹果内购Purchase OK: " + e.purchasedProduct.definition.id + " Receipt:" + e.purchasedProduct.receipt);

            base.OnSucceed(e.purchasedProduct.definition.id);

            return PurchaseProcessingResult.Complete;
        }

        private void OnDeferred(Product item)
        {
            Debug.Log("Purchase deferred: " + item.definition.id);
        }


    }

}


#endif