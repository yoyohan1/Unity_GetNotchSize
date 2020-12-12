using UnityEngine;
using System;
using System.IO;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Events;

namespace yoyohan
{
    /// <summary>
    /// 描述：
    /// 功能：
    /// 作者：yoyohan
    /// 创建时间：2019-11-24 10:03:20
    /// </summary>
    public class GameTools
    {
        /// <summary>
        /// 通过index获取枚举类型的字符串
        /// </summary>
        public static string GetEnumStrById<T>(int id)
        {
            return System.Enum.ToObject(typeof(T), id).ToString();
        }
        /// <summary>
        /// 通过index获取枚举类型
        /// </summary>
        public static T GetEnumById<T>(int id)
        {
            return (T)System.Enum.ToObject(typeof(T), id);
        }
        /// <summary>
        /// 通过字符串获取枚举类型
        /// </summary>
        public static T GetEnumByStr<T>(string type)
        {
            return (T)System.Enum.Parse(typeof(T), type);
        }

        /// <summary>
        /// 尝试获取Resources下的图片，没get到返回null
        /// </summary>
        public static void TryGetResourcesImage(string path, out Sprite sp)
        {
            sp = Resources.Load(path, typeof(Sprite)) as Sprite;
        }
        /// <summary>
        /// 获取Resources下的图片
        /// </summary>
        public static Sprite GetResourcesImage(string path)
        {
            return Resources.Load(path, typeof(Sprite)) as Sprite;
        }

        /// <summary>
        /// 转换秒为分钟str
        /// </summary>
        /// <returns></returns>
        public static string ConvertSecondsToMinute(int seconds)
        {
            string time;
            int hour = seconds / 3600;
            int minute = (seconds % 3600) / 60;
            int second = (seconds % 3600) % 60;
            string str_hour = "" + hour;
            string str_minute = "" + minute;
            string str_second = "" + second;
            if (hour < 10)
            {
                str_hour = "0" + hour;
            }
            if (minute < 10)
            {
                str_minute = "0" + minute;
            }
            if (second < 10)
            {
                str_second = "0" + second;
            }

            if (hour > 0)
            {
                time = str_hour + ":" + str_minute + ":" + str_second;
            }
            else
            {
                time = str_minute + ":" + str_second;
            }
            return time;
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetNowTimeStamp()
        {
            DateTime date1 = DateTime.UtcNow;
            DateTime date2 = DateTime.Parse("01/01/1970");
            TimeSpan ts = date1 - date2;
            long totalSeconds = Convert.ToInt64(ts.TotalSeconds);
            return totalSeconds;
        }

        /// <summary>
        /// 获取当前13位时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetNowTimeStampMillis()
        {
            DateTime date1 = DateTime.UtcNow;
            DateTime date2 = DateTime.Parse("01/01/1970");
            TimeSpan ts = date1 - date2;
            long totalSeconds = Convert.ToInt64(ts.TotalMilliseconds);
            return totalSeconds;
        }

        /// <summary>
        /// 自己写的Debug.Log,可以打印该日志的物体和脚本名称
        /// </summary>
        /// <param name="content"></param>
        public static void MyDebug(MonoBehaviour mono, string content)
        {
            Debug.Log(mono.gameObject.name + "-----" + mono.GetType() + "-----" + content);
        }

        /// <summary>
        /// 转换（10位）时间戳为DateTime
        /// </summary>
        public static DateTime ConvertTimeStampToDateTime(long timeStamp)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区

            return startTime.AddSeconds(timeStamp);
        }

        /// <summary>
        /// 转换object数组为string数组，方便打印
        /// </summary>
        public static String[] ConvertObjectArrayToStringArray(object[] para)
        {
            string[] pataStr = new string[para.Length];
            for (int i = 0; i < para.Length; i++)
            {
                pataStr[i] = para[i].ToString();
            }

            return pataStr;
        }

        /// <summary>
        /// version1与version2版本号比较 0相等,1小于,2大于,3解析错误
        /// </summary>
        public static int CompareVersion(string version1, string version2)
        {
            int k1, k2 = 0;
            int.TryParse(version1.Replace(".", ""), out k1);
            int.TryParse(version2.Replace(".", ""), out k2);

            if (k1 == 0 || k2 == 0)
                return 3;
            else if (k1 == k2)
                return 0;
            else if (k1 < k2)
                return 1;
            else if (k1 > k2)
                return 2;
            else
                return 3;
        }


        public static void CopyFile(string rootPathFrom, string rootPathTo, bool isOveride)
        {
            //copy文件夹
            string[] dirArray = Directory.GetDirectories(rootPathFrom, "*", SearchOption.AllDirectories);
            string tempDirPath = "";
            for (int i = 0; i < dirArray.Length; i++)
            {
                tempDirPath = dirArray[i].Replace(rootPathFrom, rootPathTo);
                if (!Directory.Exists(tempDirPath))
                {
                    Debug.Log("创建" + tempDirPath);
                    Directory.CreateDirectory(tempDirPath);
                }
            }

            //copy文件
            string[] fileArray = Directory.GetFiles(rootPathFrom, "*", SearchOption.AllDirectories);
            string tempFilePath = "";
            for (int i = 0; i < fileArray.Length; i++)
            {
                tempFilePath = fileArray[i].Replace(rootPathFrom, rootPathTo);
                if (isOveride == false && !fileArray[i].EndsWith(".meta") && !File.Exists(tempFilePath))
                {
                    Debug.Log("拷贝" + tempFilePath);
                    File.Copy(fileArray[i], tempFilePath, false);
                }

                if (isOveride == true && !fileArray[i].EndsWith(".meta") && File.Exists(tempFilePath))
                {
                    Debug.Log("覆盖" + tempFilePath);
                    File.Copy(fileArray[i], tempFilePath, true);
                }
            }

        }



        /// <summary>
        /// 添加EventTrigger的监听事件
        /// </summary>
        /// <param name="obj">添加监听的对象</param>
        /// <param name="eventID">添加的监听类型</param>
        /// <param name="action">触发方法</param>
        public static void AddTriggersListener(GameObject obj, EventTriggerType eventID, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = obj.AddComponent<EventTrigger>();
            }

            if (trigger.triggers.Count == 0)
            {
                trigger.triggers = new List<EventTrigger.Entry>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventID;
            entry.callback.AddListener(action);

            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// 移除EventTrigger的监听事件
        /// </summary>
        public static void RemoveTriggersListener(GameObject obj, EventTriggerType eventID)
        {
            EventTrigger eventTrigger = obj.GetComponent<EventTrigger>();
            if (eventTrigger == null || eventTrigger.triggers.Count == 0)
                return;

            for (int i = 0; i < eventTrigger.triggers.Count; i++)
            {
                if (eventTrigger.triggers[i].eventID == eventID)
                {
                    eventTrigger.triggers.Remove(eventTrigger.triggers[i]);
                }
            }
        }

        public static void AutoSendUIMessage(GameObject obj, UIEventType type)
        {
            if (type == UIEventType.PointClick)
                ExecuteEvents.Execute<IPointerClickHandler>(obj, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            else if (type == UIEventType.PointDown)
                ExecuteEvents.Execute<IPointerDownHandler>(obj, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            else if (type == UIEventType.PointUp)
                ExecuteEvents.Execute<IPointerUpHandler>(obj, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
            else if (type == UIEventType.Enter)
                ExecuteEvents.Execute<IPointerEnterHandler>(obj, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
            else if (type == UIEventType.Exit)
                ExecuteEvents.Execute<IPointerExitHandler>(obj, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);

        }

        /// <summary>
        /// 获取鼠标点击的UI物体集合的第一个 可以传入Input.mousePosition.x, Input.mousePosition.y
        /// </summary>
        public static GameObject GetClickUIBtnObject(Vector2 vec)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = vec;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results.Count > 0)
            {
                return results[0].gameObject;
            }
            return null;
        }
        public static bool IsClickUIBtn(params string[] objName)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results.Count > 0)
            {
                foreach (string name in objName)
                {
                    if (results[0].gameObject.transform.parent != null && results[0].gameObject.transform.parent.name == name)
                        return true;
                }
            }
            return false;
        }

        public static RectTransform GetNewRectTransform(Transform parent = null, string name = "New GameObject", Vector3 anchoredPosition = new Vector3(), Vector2 sizeDelta = new Vector2(), Vector2 anchorMin = new Vector2(), Vector2 anchorMax = new Vector2(), Vector2 pivot = new Vector2(), Vector3 localScale = new Vector3())
        {
            GameObject testKeyboardGo = new GameObject(name);

            parent = parent == null ? GameObject.FindObjectOfType<Canvas>().transform : parent;
            testKeyboardGo.transform.SetParent(parent);

            RectTransform rect = testKeyboardGo.AddComponent<RectTransform>();
            rect.anchoredPosition3D = anchoredPosition;
            rect.localScale = localScale;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.sizeDelta = sizeDelta;

            return rect;
        }

    }

    public enum UIEventType
    {
        PointClick,
        PointDown,
        PointUp,
        Enter,
        Exit
    }

}
