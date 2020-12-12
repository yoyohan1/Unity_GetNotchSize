//using UnityEngine;
//using System.Collections;
//using System.Runtime.InteropServices;

//namespace yoyohan.YouDaSdkTool
//{
//    public class YouDaSdkLoginController : YouDaSdkControllerBase
//    {

//#if UNITY_IPHONE&&USE_iOSLOGIN
//	[DllImport("__Internal")]
//	private static extern void MyJiheyeLogin();

//	[DllImport("__Internal")]
//	private static extern void MyLoginOut();

//	[DllImport("__Internal")]
//	private static extern void MyMaiDian(string content);

//#endif

//        public YouDaSdkLoginController(YouDaSdk youDaSdk)
//            : base(youDaSdk)
//        {

//        }

//        /// <summary>
//        /// 登录
//        /// </summary>
//        /// <param name="loginType"> 1FaceBook,2Twitter,3Guest,4JiHeYe</param>
//        public void Login(int loginType)
//        {
//#if UNITY_ANDROID
//            string methodName = "";

//            if (loginType == 1)
//            {
//                methodName = "MyFaceBookLogin";
//            }
//            else if (loginType == 2)
//            {
//                methodName = "MyTwitterLogin";
//            }
//            else if (loginType == 3)
//            {
//                methodName = "MyYoukeLogin";
//            }
//            else if (loginType == 4)
//            {
//                methodName = "MyJiHeYe";
//            }
//            youDaSdk.SendMessageToAndroid("methodName");
//#elif UNITY_IPHONE&&USE_iOSLOGIN
//			MyJiheyeLogin();
//#endif

//        }

//        public void LoginOut()
//        {
//            PlayerPrefs.DeleteKey("ycUserId");
//            //currentYCUser = null;
//            //Application.LoadLevel("MainScene");

//#if UNITY_ANDROID
//            youDaSdk.SendMessageToAndroid("MyLoginOut");
//#elif UNITY_IPHONE&&USE_iOSLOGIN
//			MyLoginOut();
//#endif
//        }

//        public override void OnSucceed(string msg, int requestId = 0)
//        {
//            Debug.Log("OnSucceed接收到Sdk返回的用户信息：" + msg);

//            //TODO..
//            //string[] arr = msg.Split('|');
//            //currentYCUser = new YCUser() { userId = arr[0], token = arr[1], sign = arr[2], avatar = arr[3], userName = arr[4], platform = arr[5] };
//            //PlayerPrefs.SetString("ycUserId", currentYCUser.userId);
//            //PlayerPrefs.SetString("ycUserIdTime", System.DateTime.Now.ToShortDateString());
//        }

//        public override void OnFailed(string msg, int requestId = 0)
//        {
//            Debug.Log("OnFailed接收到Sdk返回的用户信息：" + msg);
//        }
//    }



//    public class YCUser
//    {
//        public string userId; // ⽤用户ID， 唯⼀一
//        public string token;// token 保留留字段，⽆无意义
//        public string sign; // 第三⽅方 标示(facebook id ,twitter id)
//        public string avatar; // 头像 游客登录 ⽆无头像
//        public string userName;// ⽤用户名 游客登录 默认 visitor
//        public string platform; //  登录⽅方式 0 游客 1 facebook 2 twitter
//    }





//}

