using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yoyohan
{
    public class GOPingMuShiPei : MonoBehaviour
    {
        public GameObject main;

        void Start()
        {
            float pre = (float)720 / 1280;

            float now = (float)Screen.width / Screen.height;

            float bili = now / pre;

            Debug.LogError("bili:" + bili);

            //GameObject层不会自适应，所以需调节
            if (bili < 1)
            {
                main.transform.localScale = main.transform.localScale / bili;
            }
        }
    }
}