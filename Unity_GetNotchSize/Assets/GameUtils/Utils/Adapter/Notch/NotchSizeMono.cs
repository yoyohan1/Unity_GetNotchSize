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
        private Vector2 preAnchoredPos;

        private void Awake()
        {
            preAnchoredPos = (transform as RectTransform).anchoredPosition;
        }

        public void RefershNotchSize()
        {
            Rect canvasRect = transform.root.GetComponent<RectTransform>().rect;
            bool isLandscape = Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight ? true : false;
#if UNITY_EDITOR
            isLandscape = canvasRect.width > canvasRect.height ? true : false;
#endif
            float notchRadio = YouDaSdkMgr.instance.notchSize / (isLandscape ? Screen.width : Screen.height);
            float uiNotchSize = (isLandscape ? canvasRect.width : canvasRect.height) * notchRadio;

            RectTransform rect = transform as RectTransform;

            if (isLandscape)
            {
                if (rect.anchorMin == rect.anchorMax)
                {
                    rect.anchoredPosition = preAnchoredPos + Vector2.right * uiNotchSize;
                }
                else
                {
                    rect.anchoredPosition = preAnchoredPos + Vector2.right * uiNotchSize;
                    rect.offsetMax = new Vector2(0, rect.offsetMin.y);
                }
            }
            else
            {
                if (rect.anchorMin == rect.anchorMax)
                {
                    rect.anchoredPosition = preAnchoredPos - Vector2.up * uiNotchSize;
                }
                else
                {
                    rect.anchoredPosition = preAnchoredPos - Vector2.up * uiNotchSize;
                    rect.offsetMin = new Vector2(rect.offsetMin.x, 0);
                }
            }
        }


        private void OnEnable()
        {
            RefershNotchSize();
            YouDaSdkMgr.instance.OnNotchSizeChangedAction += RefershNotchSize;
        }

        private void OnDisable()
        {
            RefershNotchSize();
            YouDaSdkMgr.instance.OnNotchSizeChangedAction -= RefershNotchSize;
        }

    }
}
