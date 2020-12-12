using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yoyohan
{
    public class UIPingMuShiPei : MonoBehaviour
    {
        public GameObject main;
        public float width = 720;
        public float height = 1280;
        public bool isMatchWidth = true;
        private float bili;


        public float GetBiLi()
        {
            return bili;
        }


        void Awake()
        {
            float pre = (float)width / height;

            float now = (float)Screen.width / Screen.height;

            bili = now / pre;

            Debug.LogError("bili:" + bili);

            //横屏
            if (pre > 1)
            {
                //Canvas宽度自适应时，高度会缩小会放大，此时需调节高度
                if (bili > 1 && isMatchWidth)
                {
                    main.transform.localScale = main.transform.localScale / bili;
                    Vector3 temp = main.transform.localPosition;
                }

                //Canvas高度自适应时
                if (bili < 1 && isMatchWidth == false)
                {
                    main.transform.localScale = main.transform.localScale * bili;
                    Vector3 temp = main.transform.localPosition;
                }
            }

            //竖屏
            if (pre < 1)
            {
                //Canvas宽度自适应时
                if (bili > 1 && isMatchWidth)
                {
                    main.transform.localScale = main.transform.localScale / bili;
                    Vector3 temp = main.transform.localPosition;
                }

                //Canvas高度自适应，宽度会缩小会放大，此时需调节宽度
                if (bili < 1 && isMatchWidth == false)
                {
                    main.transform.localScale = main.transform.localScale * bili;
                }
            }

        }


    }

}