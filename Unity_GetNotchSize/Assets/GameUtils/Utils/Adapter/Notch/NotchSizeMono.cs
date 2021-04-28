using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yoyohan.YouDaSdkTool;

namespace yoyohan
{
    /// <summary>
    /// 描述：
    /// 功能：
    /// 作者：yoyohan
    /// 创建时间：2020-03-18 17:32:44
    /// </summary>
    public class NotchSizeMono : MonoBehaviour
    {
        public bool isLandscape;
        public AnchorType anchorType;

        private Rect canvasRect;
        private RectTransform rect;
        private Vector2 preAnchoredPos;
        private Vector2 preOffsetMax;
        private Vector2 preOffsetMin;
        private bool isInit = false;



        private float uiNotchSize
        {
            get
            {
                float notchRadio = YouDaSdkMgr.instance.notchSize / (isLandscape ? Screen.width : Screen.height);
                return (isLandscape ? canvasRect.width : canvasRect.height) * notchRadio;
            }
        }

        private float uiHomeSize
        {
            get
            {
                float homeRadio = YouDaSdkMgr.instance.homeSize / (isLandscape ? Screen.width : Screen.height);
                return (isLandscape ? canvasRect.width : canvasRect.height) * homeRadio;
            }
        }


        private void Init()
        {
            canvasRect = transform.root.GetComponent<RectTransform>().rect;
            rect = transform as RectTransform;
            preAnchoredPos = rect.anchoredPosition;
            preOffsetMax = rect.offsetMax;
            preOffsetMin = rect.offsetMin;
            isInit = true;
        }

        public void RefershNotchSize()
        {
            if (isInit == false)
            {
                Init();
            }

            Debug.Log("RefershNotchSize! uiNotchSize:" + uiNotchSize + " uiHomeSize:" + uiHomeSize, gameObject);
            switch (anchorType)
            {
                case AnchorType.Notch:
                    if (isLandscape)
                    {
                        rect.anchoredPosition = preAnchoredPos + Vector2.right * uiNotchSize;
                    }
                    else
                    {
                        rect.anchoredPosition = preAnchoredPos - Vector2.up * uiNotchSize;
                    }
                    break;
                case AnchorType.Home:
                    if (isLandscape)
                    {
                        rect.anchoredPosition = preAnchoredPos - Vector2.right * uiHomeSize;
                    }
                    else
                    {
                        rect.anchoredPosition = preAnchoredPos + Vector2.up * uiHomeSize;
                    }
                    break;
                case AnchorType.NotchAndHome:
                    if (isLandscape)
                    {
                        rect.offsetMin = new Vector2(preOffsetMin.x + uiNotchSize, preOffsetMin.y);

                        //横屏iphone无需适配导航栏的高度
                        if (Application.platform != RuntimePlatform.IPhonePlayer)
                        {
                            rect.offsetMax = new Vector2(preOffsetMax.x - uiHomeSize, preOffsetMax.y);
                        }
                    }
                    else
                    {
                        rect.offsetMin = new Vector2(preOffsetMin.x, preOffsetMin.y + uiHomeSize);
                        rect.offsetMax = new Vector2(preOffsetMax.x, preOffsetMax.y - uiNotchSize);
                    }
                    break;
                default:
                    break;
            }
        }


        IEnumerator IERefershNotchSize()
        {
            yield return null;
            RefershNotchSize();
        }

        private void OnEnable()
        {
            StartCoroutine(IERefershNotchSize());
            YouDaSdkMgr.instance.OnNotchSizeChangedAction += RefershNotchSize;
        }

        private void OnDisable()
        {
            YouDaSdkMgr.instance.OnNotchSizeChangedAction -= RefershNotchSize;
        }




        public enum AnchorType
        {
            Notch,//锚点竖屏靠上或横屏靠左，仅使用刘海高度
            Home,//锚点竖屏靠下或横屏靠右，仅使用导航栏或苹果Home键高度
            NotchAndHome,//锚点竖屏上下拉伸或横屏左右拉伸，使用刘海高度+导航栏或苹果Home键高度
        }
    }
}




/*
private bool isLandscape
{
    get
    {
        bool isLandscape = Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight ? true : false;
#if UNITY_EDITOR
        isLandscape = canvasRect.width > canvasRect.height ? true : false;
#endif
        return isLandscape;
    }
}
*/


