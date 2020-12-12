/*
 * File: JsonMapper.cs
 * Desc: Encapsulate MiniJSON with LitJson.JsonMapper interface
 * Created by night.yan(yanningning@gmail.com)  at 2014.03.25
 */

using System.Collections.Generic;

namespace yoyohan
{

    /// <summary>
    /// 描述：
    /// 功能：
    /// 作者：yoyohan
    /// 创建时间：2019-12-24 11:48:16
    /// </summary>
    public class JsonMapper
    {
        /// <summary>
        /// 反序列化为JsonData对象，内核采用MiniJson的Deserialize  注意：不能直接解析float，可以解析double转float；不能解析带\r\n的字符串。
        /// </summary>
        public static JsonData ToObject(string json)
        {
            object obj = MiniJSON.Deserialize(json);
            JsonData jsonData = obj == null ? null : new JsonData(obj);
            return jsonData;
        }

        /// <summary>
        /// 序列化 传入JsonData或object对象（Dictionary、List、bool等）
        /// </summary>
        public static string Serialize(object obj)
        {
            if (obj is JsonData)
            {
                return ((JsonData)obj).ToJson();
            }
            if (obj is List<JsonData>)
            {
                List<JsonData> jsonDatas = (List<JsonData>)obj;
                JsonData jsonData = new JsonData();
                for (int i = 0; i < jsonDatas.Count; i++)
                {
                    jsonData.AddToIList(jsonDatas[i]);
                }
                return jsonData.ToJson();
            }

            return MiniJSON.Serialize(obj);
        }

        /// <summary>
        /// 反序列化为object对象（Dictionary、List、bool等）即直接调用MiniJson的Deserialize 注意：不能直接解析float，可以解析double转float；不能解析带\r\n的字符串。
        /// </summary>
        public static object Deserialize(string json)
        {
            return MiniJSON.Deserialize(json);
        }

    }
}