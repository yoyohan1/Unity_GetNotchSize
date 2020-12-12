using UnityEngine;
using System.Collections.Generic;

namespace yoyohan
{
    public class GUILog : MonoBehaviour
    {
        //*********计算帧率所用*********
        private float m_LastUpdateShowTime = 0f;  //上一次更新帧率的时间;  
        private float m_UpdateShowDeltaTime = 0.01f;//更新帧率的时间间隔;  
        private int m_FrameUpdate = 0;//帧数;  
        private float m_FPS = 0;

        //*********打印Unity的Log所用*********
        List<sLogData> MessageList = new List<sLogData>();
        eLogType currType = eLogType.All;

        Vector2 scrollPosition = Vector2.zero;
        Vector2 showScrollPosition = Vector2.zero;
        bool canShow;
        int maxCount;
        int currIndex;
        float width = Screen.width - 150;
        float height = Screen.height * 0.5f;
        float offetHeight = 50;
        bool canShowInfo;
        int touchNum;
        Color ScrollbarColor = Color.black;
        List<sLogData> temp = new List<sLogData>();
        
        void Awake()
        {
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(this.gameObject);
            Application.logMessageReceivedThreaded += OnLog;
        }

        void Start()
        {
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
            Info();
        }

        void Info()
        {
            string systemInfo = "当前系统基础信息：\n设备模型：" + SystemInfo.deviceModel + "\n设备名称：" + SystemInfo.deviceName + "\n设备类型：" + SystemInfo.deviceType +
                    "\n设备唯一标识符：" + SystemInfo.deviceUniqueIdentifier + "\n显卡标识符：" + SystemInfo.graphicsDeviceID +
                    "\n显卡设备名称：" + SystemInfo.graphicsDeviceName + "\n显卡厂商：" + SystemInfo.graphicsDeviceVendor +
                    "\n显卡厂商ID:" + SystemInfo.graphicsDeviceVendorID + "\n显卡支持版本:" + SystemInfo.graphicsDeviceVersion +
                    "\n显存（M）：" + SystemInfo.graphicsMemorySize + "\n显卡像素填充率(百万像素/秒)，-1未知填充率：" + SystemInfo.graphicsPixelFillrate +
                    "\n显卡支持Shader层级：" + SystemInfo.graphicsShaderLevel + "\n支持最大图片尺寸：" + SystemInfo.maxTextureSize +
                    "\nnpotSupport：" + SystemInfo.npotSupport + "\n操作系统：" + SystemInfo.operatingSystem +
                    "\nCPU处理核数：" + SystemInfo.processorCount + "\nCPU类型：" + SystemInfo.processorType +
                    "\nsupportedRenderTargetCount：" + SystemInfo.supportedRenderTargetCount + "\nsupports3DTextures：" + SystemInfo.supports3DTextures +
                    "\nsupportsAccelerometer：" + SystemInfo.supportsAccelerometer + "\nsupportsComputeShaders：" + SystemInfo.supportsComputeShaders +
                    "\nsupportsGyroscope：" + SystemInfo.supportsGyroscope + "\nsupportsImageEffects：" + SystemInfo.supportsImageEffects +
                    "\nsupportsInstancing：" + SystemInfo.supportsInstancing + "\nsupportsLocationService：" + SystemInfo.supportsLocationService +
                    "\nsupportsRenderTextures：" + SystemInfo.supportsRenderTextures + "\nsupportsRenderToCubemap：" + SystemInfo.supportsRenderToCubemap +
                    "\nsupportsShadows：" + SystemInfo.supportsShadows + "\nsupportsSparseTextures：" + SystemInfo.supportsSparseTextures +
                    "\nsupportsStencil：" + SystemInfo.supportsStencil + "\nsupportsVertexPrograms：" + SystemInfo.supportsVertexPrograms +
                    "\nsupportsVibration：" + SystemInfo.supportsVibration + "\n内存大小：" + SystemInfo.systemMemorySize;

            Debug.Log(systemInfo);
        }


        void Update()
        {
            m_FrameUpdate++;
            if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
            {
                m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
                m_FrameUpdate = 0;
                m_LastUpdateShowTime = Time.realtimeSinceStartup;
            }

        }


        void OnLog(string condition, string stackTrace, UnityEngine.LogType type)
        {
            if (!canShow)
            {
                if (MessageList.Count > 100)
                {
                    MessageList.Clear();
                }
            }
            sLogData mess = new sLogData();
            mess.message = condition;
            mess.stackTrace = stackTrace;
            mess.type = (eLogType)(int)type;
            if (type == UnityEngine.LogType.Error || type == UnityEngine.LogType.Assert || type == UnityEngine.LogType.Exception)
            {
                mess.color = Color.red;

            }
            else if (type == UnityEngine.LogType.Log)
            {
                mess.color = Color.white;
            }
            else if (type == UnityEngine.LogType.Warning)
            {
                mess.color = Color.yellow;
            }
            MessageList.Add(mess);
        }


        void OnGUI()
        {
            GUI.contentColor = Color.red;
            GUI.Label(new Rect(0, 0, 150, 50), "FPS:" + m_FPS);
            //if (!canShow) return;
            if (canShow == false)
            {
                GUI.contentColor = Color.red;
                GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                if (GUI.Button(new Rect(width * 0.5f, 0, 150, 50), "Console"))
                {
                    //重新设置分辨率
                    width = Screen.width - 150;
                    height = Screen.height * 0.5f;
                    canShow = true;
                }
                return;
            }

            GUI.backgroundColor = Color.black;
            ///关闭
            GUI.contentColor = Color.red;
            GUI.backgroundColor = Color.red;
            GUI.skin.button.fontStyle = FontStyle.Bold;
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            //GUI.skin.button.fontSize = 20;
            GUI.skin.button.wordWrap = true;
            if (GUI.Button(new Rect(width + 20, 0, 150, 80), "关闭"))
            {
                canShow = false;
            }
            ///清空
            GUI.contentColor = Color.green;
            GUI.backgroundColor = Color.green;

            if (GUI.Button(new Rect(width + 20, height - 160, 150, 80), "清空"))
            {
                MessageList.Clear();
                maxCount = 0;
                canShowInfo = false;
                ScrollbarColor = Color.green;
                return;
            }
            GUI.contentColor = Color.blue;
            GUI.backgroundColor = Color.blue;

            if (GUI.Button(new Rect(width + 20, height - 60, 150, 80), "全部"))
            {
                currType = eLogType.All;
                ScrollbarColor = Color.blue;
            }
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.white;
            if (GUI.Button(new Rect(width + 20, height + 20, 150, 80), "输出"))
            {
                ScrollbarColor = Color.white;
                currType = eLogType.Log;
            }
            GUI.contentColor = Color.red;
            GUI.backgroundColor = Color.red;
            if (GUI.Button(new Rect(width + 20, height + 100, 150, 80), "错误"))
            {
                ScrollbarColor = Color.red;
                currType = eLogType.Error;
            }
            GUI.contentColor = Color.yellow;
            GUI.backgroundColor = Color.yellow;
            if (GUI.Button(new Rect(width + 20, height + 180, 150, 80), "警告"))
            {
                ScrollbarColor = Color.yellow;
                currType = eLogType.Warning;
            }

            temp.Clear();
            for (int i = 0; i < MessageList.Count; i++)
            {
                if (currType != eLogType.All)
                {
                    if (currType == eLogType.Error)
                    {
                        if (MessageList[i].type != eLogType.Assert && MessageList[i].type != eLogType.Error && MessageList[i].type != eLogType.Exception)
                            continue;
                    }
                    else if (currType != MessageList[i].type)
                        continue;
                }
                temp.Add(MessageList[i]);
            }
            if (MessageList.Count > maxCount)
            {
                scrollPosition = new Vector2(0, temp.Count * offetHeight);
            }
            GUI.backgroundColor = ScrollbarColor;
            scrollPosition = GUI.BeginScrollView(new Rect(0, height, width + 20, height), scrollPosition, new Rect(0, 0, width, temp.Count * offetHeight));
            GUI.backgroundColor = Color.black;
            GUI.skin.verticalScrollbar.stretchWidth = true;
            GUI.skin.verticalScrollbar.fixedWidth = 100;
            GUI.skin.verticalScrollbarThumb.fixedWidth = 100;
            for (int i = 0; i < temp.Count; i++)
            {
                GUI.contentColor = temp[i].color;
                GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                GUI.skin.button.fontSize = 15;
                GUI.skin.button.wordWrap = false;
                int tempIndex = i;
                if (GUI.Button(new Rect(0, i * offetHeight, width, offetHeight), temp[i].message))
                {
                    currIndex = tempIndex;
                    canShowInfo = true;
                }
            }
            GUI.EndScrollView();
            maxCount = MessageList.Count;

            if (canShowInfo)
            {
                if (currIndex >= 0 && currIndex < temp.Count)
                {
                    GUI.backgroundColor = ScrollbarColor;
                    //showScrollPosition = GUILayout.BeginScrollView(showScrollPosition, GUILayout.Width(width + 20), GUILayout.Height(height));
                    GUI.backgroundColor = Color.black;
                    //GUI.contentColor = Color.white;
                    GUI.contentColor = temp[currIndex].color;
                    GUI.skin.button.alignment = TextAnchor.UpperLeft;
                    //GUILayout.Button(temp[currIndex].message);
                    GUILayout.TextArea(temp[currIndex].message, GUILayout.Width(width + 20), GUILayout.Height(height / 2 - 10));
                    //GUILayout.Button(temp[currIndex].stackTrace);
                    GUILayout.TextArea(temp[currIndex].stackTrace, GUILayout.Width(width + 20), GUILayout.Height(height / 2 ));
                    //GUILayout.EndScrollView();
                }
            }
        }

        private Vector2 tempMsgScrollViewPos = Vector2.zero;

        void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= OnLog;
        }
    }


    public struct sLogData
    {
        public string message;
        public string stackTrace;
        public Color color;
        public eLogType type;
    }
    public enum eLogType
    {
        Error = 0,
        Assert = 1,
        Warning = 2,
        Log = 3,
        Exception = 4,
        All = 5,
    }

}
