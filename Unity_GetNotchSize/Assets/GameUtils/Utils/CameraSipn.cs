using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace yoyohan
{
    /// <summary>
    /// 描述：
    /// 功能：
    /// 作者：yoyohan
    /// 创建时间：2019-04-19 17:13:21
    /// </summary>
    public class CameraSipn : MonoBehaviour
    {
        private GameObject __mGameObject;
        private Transform __mTransform;
        public GameObject mGameObject { get { if (__mGameObject == null) __mGameObject = this.gameObject; return __mGameObject; } }
        public Transform mTransform { get { if (__mTransform == null) __mTransform = this.transform; return __mTransform; } }


        [Tooltip("摄像机视角中心点\n如果不设置 将自动生成pos为(0,0,0)的中心点")]
        public Transform _camViewTarget;
        /// <summary>
        /// 1.摄像机视角中心点；2.如果不设置 将自定生成位置为(0,0,0)的中心点；3.设置即copy传入的坐标
        /// </summary>
        private Transform camViewTarget
        {
            get
            {
                if (_camViewTarget == null)
                {
                    _camViewTarget = new GameObject("CamViewTarget").transform;
                    _camViewTarget.position = Vector3.zero;
                }
                return _camViewTarget;
            }
        }

        [Tooltip("摄像机\n如果不设置 将自动查找")]
        public Transform _cameraNode;
        private Transform cameraNode
        {
            get
            {
                if (_cameraNode == null)
                {
                    if (this.GetComponent<Camera>() != null)
                    {
                        _cameraNode = mTransform;
                    }
                    else if (GameObject.FindWithTag("MainCamera"))
                    {
                        _cameraNode = GameObject.FindWithTag("MainCamera").transform;
                    }
                    else
                    {
                        throw new Exception("未找到摄像机！");
                    }
                }

                return _cameraNode;
            }
            set
            {
                _cameraNode = value;
            }
        }


        //设置旋转角度
        public float x = 0;
        public float y = 0;
        public float z = 0;
        //旋转速度值
        public float xSpeed = 5;
        public float ySpeed = 5;
        public float mSpeed = 50;

        //y轴角度限制，设置成一样则该轴不旋转
        public float yMinLimit = -365;
        public float yMaxLimit = 365;

        //x轴角度限制，同上
        public float leftMax = -365;
        public float rightMax = 365;

        //缩放值
        private float G_fMaxZoom = 90;
        private float G_fMinZoom = 5;
        public float G_fZoomSpeed = -16;

        [Header("最大最小缩放距离")]
        private float distance = -1;
        public float minDistance = 5;
        public float maxDistance = 100;

        [Header("右键控制平移的速度 对应的距离为100")]
        public float middleSpeed = 20;
        [Header("触摸控制平移的速度 对应的距离为100")]
        public float moveSpeed_touch = -0.025f;

        [Header("旋转的阻尼值 值越小 效果越明显")]
        public bool needDamping = true;
        public float damping = 5;


        Quaternion rotation = Quaternion.identity;
        Vector3 position = Vector3.zero;

        private bool bIsMouseDown = false;//记录鼠标左键按下 抬起
        private bool isMiddleDown = false;//记录鼠标中键按下 抬起
        private bool isNeedAdjust = false;//记录下一次 旋转时是否需要自动对焦调节

        private Vector3 preCamViewTargetPos;//记录这个值 用来限制上下左右移动的距离

        void Start()
        {
            Input.multiTouchEnabled = true;

            SetCamPoint(transform);

            preCamViewTargetPos = camViewTarget.position;
        }


        #region 设置摄像机点位
        /// <summary>
        /// 设置摄像机为指定点
        /// </summary>
        public void SetCamPoint(Transform pointTr)
        {
            SetCamPos(pointTr.position);
        }
        /// <summary>
        /// 设置摄像机为指定位置
        /// </summary>
        public void SetCamPos(Vector3 pos)
        {
            transform.position = pos;
            Quaternion quat = Quaternion.FromToRotation(Vector3.forward, camViewTarget.position - cameraNode.position);//面向目标点
            transform.rotation = quat;
            AdjustValue();
            Vector3 disVector = new Vector3(0, 0, -distance);
            transform.position = transform.rotation * disVector + camViewTarget.position;
        }
        public void SetTargetPoint(Transform targetTr)
        {
            SetTargetPos(targetTr.position);
        }
        public void SetTargetPos(Vector3 pos)
        {
            camViewTarget.position = pos;
        }
        public void AdjustValue()
        {
            distance = Vector3.Distance(cameraNode.position, camViewTarget.position);
            position = transform.position;
            x = cameraNode.rotation.eulerAngles.x;
            y = cameraNode.rotation.eulerAngles.y;
            z = 0;
            rotation = Quaternion.Euler(new Vector3(x, y, z));
            transform.rotation = rotation;

            //此步骤必须 防止第一次旋转计算出x值 然后限制为-90到90 而这时x不属于-90到90的范围会跑掉
            if (x > 180)
            {
                x -= 360;
            }
        }
        #endregion



        float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle = angle + 360;

            if (angle > 360)
                angle = angle - 360;

            return Mathf.Clamp(angle, min, max);
        }


        void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())//|| EventSystem.current.currentSelectedGameObject != null   加上这个会造成点击ui 后鼠标必须点一下不是ui的地方  才可以操作
#elif UNITY_ANDROID || UNITY_IPHONE
            if (Input.touchCount > 0 &&EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
            {
                return;
            }

            if (Input.touchCount > 0)
                RotateByTouch();
            else
                RotateByMouse();


            if (Input.touchCount > 0)
                FarAndNearByTouch();
            else
                FarAndNearByMouse();


            if (Input.touchCount <= 0)
                HorVerMoveByMouse();

            ProcessDamping();
        }

        /// <summary>
        /// 处理阻尼感
        /// </summary>
        void ProcessDamping()
        {
            if (!isNeedAdjust)
            {
                //阻尼感
                if (needDamping)
                {
                    if (position != Vector3.zero)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
                        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
                    }
                }
                else
                {
                    transform.rotation = rotation;
                    transform.position = position;
                }
            }
        }


        #region 旋转视角
        /// <summary>
        /// 处理摄像机旋转和位置（触屏）
        /// </summary>
        void RotateByTouch()
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (bIsMouseDown == false && isNeedAdjust == true)
                {
                    AdjustValue();
                    isNeedAdjust = false;
                }
                bIsMouseDown = true;
            }
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                bIsMouseDown = false;
            }

            if (bIsMouseDown == false)
                return;


            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //判断是否需要反向旋转
                if ((x > 90f && x < 270f) || (x < -90 && x > -270f))
                    y -= Input.GetTouch(0).deltaPosition.x * ySpeed * 0.04f;
                else
                    y += Input.GetTouch(0).deltaPosition.x * ySpeed * 0.04f;

                x += Input.GetTouch(0).deltaPosition.y * xSpeed * -0.04f;

                //不需要反向旋转
                //x += Input.GetTouch(0).deltaPosition.y * xSpeed * -0.04f;
                //y += Input.GetTouch(0).deltaPosition.x * ySpeed * 0.04f;

                ProcessRotate();
            }
        }

        /// <summary>
        /// 处理摄像机的旋转和位置（鼠标）
        /// </summary>
        void RotateByMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (bIsMouseDown == false && isNeedAdjust == true)
                {
                    SetCamPos(transform.position);
                    isNeedAdjust = false;
                }
                bIsMouseDown = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                bIsMouseDown = false;
            }

            if (bIsMouseDown == false)
                return;


            if (Input.GetMouseButton(0))
            {
                //判断是否需要反向旋转
                if ((x > 90 && x < 270) || (x < -90 && x > -270))
                    y = y - Input.GetAxis("Mouse X") * ySpeed;
                else
                    y = y + Input.GetAxis("Mouse X") * ySpeed;

                x = x + Input.GetAxis("Mouse Y") * -xSpeed;

                //不需要反向旋转
                //x += Input.GetAxis("Mouse Y") * -xSpeed;
                //y += Input.GetAxis("Mouse X") * ySpeed;

                this.ProcessRotate();
            }
        }

        void ProcessRotate()
        {
            x = ClampAngle(x, yMinLimit, yMaxLimit);
            y = ClampAngle(y, leftMax, rightMax);

            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            rotation = Quaternion.Euler(x, y, z);
            Vector3 disVector = new Vector3(0, 0, -distance);
            position = rotation * disVector + camViewTarget.position;
        }
        #endregion




        #region 远近
        /// <summary>
        /// 处理摄像机远近（鼠标）
        /// </summary>
        void FarAndNearByMouse()
        {
            //处理电脑端 摄像机视角
            float fScroll = Input.GetAxis("Mouse ScrollWheel");

            this.ProcessFarAndNear(fScroll);
        }

        private Touch oldTouch0;
        private Touch oldTouch1;
        private Touch newTouch0;
        private Touch newTouch1;

        /// <summary>
        /// 处理摄像机远近（触屏）
        /// </summary>
        private void FarAndNearByTouch()
        {
            if (Input.touchCount < 2)
            {
                return;
            }

            newTouch0 = Input.GetTouch(0);
            newTouch1 = Input.GetTouch(1);

            if (newTouch1.phase == TouchPhase.Began || newTouch0.phase == TouchPhase.Began)
            {
                oldTouch0 = newTouch0;
                oldTouch1 = newTouch1;
                return;
            }

            if (newTouch0.phase == TouchPhase.Ended || newTouch1.phase == TouchPhase.Ended || newTouch0.phase == TouchPhase.Canceled || newTouch1.phase == TouchPhase.Canceled)
            {
                SetCamPos(transform.position);
                return;
            }

            float offset = Vector2.Distance(newTouch0.position, newTouch1.position) - Vector2.Distance(oldTouch0.position, oldTouch1.position);

            this.ProcessFarAndNear(offset / 100f);
            this.HorVerMoveByTouch();

            oldTouch0 = newTouch0;
            oldTouch1 = newTouch1;
        }

        Vector3 nexPos;
        Vector3 nearPos;
        Vector3 farPos;

        private void ProcessFarAndNear(float scaleFactor)
        {
            //3.第三种调节远近距离
            if (Mathf.Abs(scaleFactor) > 0.02f)
            {
                nexPos = cameraNode.position + (cameraNode.position - camViewTarget.position).normalized * scaleFactor * G_fZoomSpeed * 0.5f;

                nearPos = camViewTarget.position - (camViewTarget.position - cameraNode.position).normalized * minDistance;
                farPos = camViewTarget.position - (camViewTarget.position - cameraNode.position).normalized * maxDistance;

                nexPos.x = nearPos.x < farPos.x ? Mathf.Clamp(nexPos.x, nearPos.x, farPos.x) : Mathf.Clamp(nexPos.x, farPos.x, nearPos.x);
                nexPos.y = nearPos.y < farPos.y ? Mathf.Clamp(nexPos.y, nearPos.y, farPos.y) : Mathf.Clamp(nexPos.y, farPos.y, nearPos.y);
                nexPos.z = nearPos.z < farPos.z ? Mathf.Clamp(nexPos.z, nearPos.z, farPos.z) : Mathf.Clamp(nexPos.z, farPos.z, nearPos.z);

                //mTransform.position = nexPos;//直接设置效果
                cameraNode.position = Vector3.Lerp(cameraNode.position, nexPos, 0.5f);//阻尼效果

                isNeedAdjust = true;
            }
        }


        #endregion




        #region 上下左右

        Vector2 dirVec2;
        Vector3 moveTemp;

        /// <summary>
        /// 处理上下左右平移移动视角（鼠标）
        /// </summary>
        void HorVerMoveByMouse()
        {
            if (Input.GetMouseButtonDown(1))
            {
                isMiddleDown = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                isMiddleDown = false;
            }

            if (isMiddleDown == false)
                return;

            isNeedAdjust = true;

            dirVec2.x = -Input.GetAxis("Mouse X");
            dirVec2.y = -Input.GetAxis("Mouse Y");

            float belv = (Vector3.Distance(camViewTarget.position, cameraNode.position) - minDistance) / 100 + 1;
            moveTemp = cameraNode.rotation * dirVec2 * Time.fixedDeltaTime * (middleSpeed * belv);

            cameraNode.position += moveTemp;
            camViewTarget.position += moveTemp;

            //TODO..限制上下左右可移动的范围
        }


        /// <summary>
        /// 处理上下左右平移移动视角（触屏）
        /// </summary>
        void HorVerMoveByTouch()
        {
            dirVec2 = (newTouch0.position + newTouch1.position) / 2 - (oldTouch0.position + oldTouch1.position) / 2;

            if (dirVec2.magnitude > 0.02f)
            {
                float belv = (Vector3.Distance(camViewTarget.position, cameraNode.position) - minDistance) / 100 + 1;
                moveTemp = cameraNode.rotation * dirVec2 * (moveSpeed_touch * belv);

                cameraNode.position = cameraNode.position + moveTemp;
                camViewTarget.position = camViewTarget.position + moveTemp;

                isNeedAdjust = true;

                //TODO..限制上下左右可移动的范围
            }
        }

        #endregion



    }

}
//1.第一种放大缩小模型
//Vector3 localScal = transform.localScale;
//Vector3 scale = new Vector3(localScal.x + scaleFactor,
//    localScal.y + scaleFactor, localScal.z + scaleFactor);
//if (scale.x >= .2f && scale.y >= .2f && scale.z >= .2f)
//{
//    transform.localScale = scale;
//}

//2.第二种调整视角
//cam.fieldOfView = cam.fieldOfView + scaleFactor * G_fZoomSpeed;
//cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, G_fMinZoom, G_fMaxZoom);


//还原 BackUp方法
//if (previousFiledView == 0)
//    previousFiledView = cam.fieldOfView;

//cam.fieldOfView = previousFiledView;


//Start 和 字段
//private float previousFiledView;
//private float previousDistance;
//private Vector3 previousPosition;
//private Quaternion previousRotation;
//private Vector3 previousCurrObjPosition;

//previousFiledView = cam.fieldOfView;
//previousDistance = distance;
//previousPosition = cam.transform.position;
//previousRotation = cam.transform.rotation;
