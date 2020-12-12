using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yoyohan
{
    /// <summary>
    /// 描述：
    /// 功能：
    /// 作者：yoyohan
    /// 创建时间：2020-03-18 17:32:44
    /// </summary>
    public class HomeSizeMono : MonoBehaviour
    {
        private float homeSize;

        private void Awake()
        {
            RectTransform rect = transform as RectTransform;
            //Debug.Log(gameObject.name + "  sizeDelta:" + rect.sizeDelta);
            //Debug.Log(gameObject.name + "  anchoredPosition:" + rect.anchoredPosition);
            //Debug.Log(gameObject.name + "  offsetMin:" + rect.offsetMin);
            //Debug.Log(gameObject.name + "  offsetMax:" + rect.offsetMax);
            if (rect.anchorMin == rect.anchorMax)
            {
                rect.anchoredPosition += Vector2.up * homeSize;
            }
            else
            {
                Vector2 recOffsetMax = rect.offsetMax;
                rect.anchoredPosition += Vector2.up * homeSize;
                rect.offsetMax = new Vector2(rect.offsetMax.x, recOffsetMax.y);
            }
        }

    }
}
