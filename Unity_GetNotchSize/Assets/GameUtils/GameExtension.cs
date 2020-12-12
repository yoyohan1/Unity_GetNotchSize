using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace yoyohan
{
    public static class GameExtension
    {
        /// <summary>
        /// 尝试根据key得到value，得到了直接返回，没有得到返回null
        /// </summary>
        public static Tvalue TryGet<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
        {
            Tvalue value;
            dict.TryGetValue(key, out value);
            if (value == null && (typeof(Tkey).Name != "UIPanelType" && !key.ToString().EndsWith("Panel")))
            {
                Debug.LogError("Dict没能找到该Key对应的Value，Key：" + key);
            }
            return value;
        }

        /// <summary>
        /// 尝试根据ID得到value，得到了直接返回，没有得到返回null
        /// </summary>
        public static Tvalue TryGet<Tvalue>(this List<Tvalue> lis, int id) where Tvalue : class
        {
            Tvalue value;
            if (id < 0 || id > lis.Count - 1)
            {
                value = null;

                if (typeof(Tvalue).Name != "BasePanel")
                {
                    Debug.LogError("List中没能找到该id对应的Value，id：" + id + typeof(Tvalue).Name);
                }
            }
            else
            {
                value = lis[id];
            }
            return value;
        }

        /// <summary>
        /// 根据与self的距离排序
        /// </summary>
        /// <param name="enemyList"></param>
        /// <param name="self"></param>
        public static void SortDistance(this List<GameObject> enemyList, Transform self)
        {
            //排序
            Comparison<GameObject> comparison = new Comparison<GameObject>((GameObject x, GameObject y) =>
            {
                if (Vector3.Distance(self.position, x.transform.position) >= Vector3.Distance(self.position, y.transform.position))
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
            enemyList.Sort(comparison);
        }



        private static List<GameObject> destoryList = new List<GameObject>();
        /// <summary>
        /// 自写销毁物体的方法
        /// </summary>
        public static void MyDestory(this GameObject go)
        {
            go.SetActive(false);
            if (destoryList.IndexOf(go) < 0)
            {

                destoryList.Add(go);
            }
            if (destoryList.Count >= 20)
            {
                for (int i = 0; i < destoryList.Count; i++)
                {
                    UnityEngine.Object.Destroy(destoryList[i]);
                }

                destoryList.Clear();
            }
        }

        /// <summary>
        /// 扩展方法，把MonoBehaviour类转换为子类
        /// </summary>
        public static T ToClass<T>(this MonoBehaviour mono) where T : MonoBehaviour
        {
            return (T)mono;
        }


        /// <summary>
        /// 查找本游戏物体下的特定名称的子物体系统，并将其返回
        /// </summary>
        /// <param name="_target">要在其中进行查找的父物体</param>
        /// <param name="_childName">待查找的子物体名称，可以是"/"分割的多级名称</param>
        /// <returns></returns>
        public static Transform FindDeepChild(this GameObject _target, string _childName)
        {
            Transform resultTrs = null;
            resultTrs = _target.transform.Find(_childName);
            if (resultTrs == null)
            {
                foreach (Transform trs in _target.transform)
                {
                    resultTrs = trs.gameObject.FindDeepChild(_childName);
                    if (resultTrs != null)
                        return resultTrs;
                }
            }
            return resultTrs;
        }

        /// <summary>
        /// 查找本游戏物体下的特定名称的子物体系统的特定组件，并将其返回
        /// </summary>
        /// <param name="_target">要在其中进行查找的父物体</param>
        /// <param name="_childName">待查找的子物体名称，可以是"/"分割的多级名称</param>
        /// <returns>返回找到的符合条件的第一个自物体下的指定组件</returns>
        public static T FindDeepChild<T>(this GameObject _target, string _childName) where T : Component
        {
            Transform resultTrs = _target.FindDeepChild(_childName);
            if (resultTrs != null)
                return resultTrs.gameObject.GetComponent<T>();
            return (T)((object)null);
        }

        public static string ReplaceSpace(this string str)
        {
            return str.Replace(" ", "<color=#00000000>.</color>");
        }

    }
}