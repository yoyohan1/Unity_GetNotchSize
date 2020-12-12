using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yoyohan.LitMiniJsonDemo
{
    public class LitMiniJsonDemo : MonoBehaviour
    {
        string str = "{\"Code\":1.2,\"Msg\":\"成功\",\"Data\":{\"ScoreID\":35755827,\"IsRight\":false,\"List\":[1,2,3]}}";


        void Start()
        {
            JsonData jsonData = JsonMapper.ToObject(str);
            Debug.Log("double值 Code:" + jsonData.GetValue<double>("Code"));
            Debug.Log("float值 Code:" + jsonData.GetValue<float>("Code"));
            Debug.Log("解析JsonData里的某一个值，IsRight：" + jsonData.GetValue<JsonData>("Data").GetValue<bool>("IsRight"));
            Debug.Log("解析JsonData里的某一个List，List[0]：" + jsonData.GetValue<JsonData>("Data").GetValue<List<JsonData>>("List")[0]);

            JsonData jd = new JsonData().SetKeyValue("id", 2).SetKeyValue("name","hhh");
            Debug.Log(jd.ToJson());
            Debug.Log(JsonMapper.Serialize(jd));
        }


    }

}
