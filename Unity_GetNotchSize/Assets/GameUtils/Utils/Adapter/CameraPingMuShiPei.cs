using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yoyohan
{
    public class CameraPingMuShiPei : MonoBehaviour
    {
        public Camera mainCamera;

        void Start()
        {
            float os = mainCamera.orthographicSize;

            float pre = (float)720 / 1280;
            float preCameraWidth = os * 2 * pre;

            float now = (float)Screen.width / Screen.height;
            float nowCameraWidth = os * 2 * now;

            Debug.Log("preCameraWidth:" + preCameraWidth);
            Debug.Log("nowCameraWidth:" + nowCameraWidth);

            mainCamera.orthographicSize = os / (nowCameraWidth / preCameraWidth);
        }

    }
}