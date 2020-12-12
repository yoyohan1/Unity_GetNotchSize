using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 对CountTimeModel的调用做一层封装，添加了一些处理
/// </summary>
namespace yoyohan
{
    /// <summary>
    /// 描述：
    /// 功能：
    /// 作者：yoyohan
    /// 创建时间：2019-11-24 10:05:59
    /// </summary>
    public class CountTimeMgr
    {
        public static CountTimeGameObject CountTimeObject;

        /// <summary>
        /// 计时
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static CountTimeModel Count(float time)
        {
            if (!checkTimeAvailable(time))
            {
                return null;
            }
            checkCountTimeObject();
            CountTimeModel timeModel = new CountTimeModel(time);
            return timeModel;
        }

        /// <summary>
        /// 计时
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="level">刷新频率（调用update），默认为0.1s</param>
        /// <returns></returns>
        public static CountTimeModel Count(float time, UpdateLevel level)
        {
            if (!checkTimeAvailable(time))
            {
                return null;
            }
            checkCountTimeObject();
            CountTimeModel timeModel = new CountTimeModel(time, level);
            return timeModel;
        }

        /// <summary>
        /// 计时
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="isRealTime">是否真实时间，默认为游戏中时间</param>
        /// <returns></returns>
        public static CountTimeModel Count(float time, bool isRealTime)
        {
            if (!checkTimeAvailable(time))
            {
                return null;
            }
            checkCountTimeObject();
            CountTimeModel timeModel = new CountTimeModel(time, isRealTime);
            return timeModel;
        }

        /// <summary>
        /// 计时
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="level">刷新频率（调用update），默认为0.1s</param>
        /// <param name="isRealTime">是否真实时间，默认为游戏中时间</param>
        /// <returns></returns>
        public static CountTimeModel Count(float time, UpdateLevel level, bool isRealTime)
        {
            if (!checkTimeAvailable(time))
            {
                return null;
            }
            checkCountTimeObject();
            CountTimeModel timeModel = new CountTimeModel(time, isRealTime);
            return timeModel;
        }

        static void checkCountTimeObject()
        {
            if (CountTimeObject == null)
            {
                GameObject tGO = new GameObject("【CountTimeObject】");
                CountTimeObject = tGO.AddComponent<CountTimeGameObject>();
            }
        }

        static bool checkTimeAvailable(float time)
        {
            if (time <= 0)
                return false;
            return true;
        }
    }

    /// <summary>
    /// 调用携程，需要借助这个类  例如：GetComponent<CountTimeGameObject>().StartCoroutine()
    /// </summary>
    public class CountTimeGameObject : MonoBehaviour
    {
        public static void PrintT(string msg)
        {
            Debug.Log(msg);
        }
    }

    /// <summary>
    /// 更新频率,执行update的频率
    /// </summary>
    public enum UpdateLevel
    {
        S,//1秒
        S0,//0.1秒
        S00//0.01秒
    }

    /// <summary>
    /// 计时器的三种状态
    /// </summary>
    public enum CountTimeStateType
    {
        STOPED,
        PLAYING,
        PAUSED
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        NORMAL,//正常类型
        PAUSED,//暂停操作
        STOP//停止操作
    }


    public class CountTimeModel
    {
        private float _aimTime;
        private float _currentTime = 0;
        private float _updateValue = 0.1f;
        private bool _isRealTime = false;

        Action _beginAction;        //开始
        Action<float> _updateAction;//更新
        Action _endAction;          //正常结束
        Action _stopAction;         //中途停止

        //记录当前状态
        private CountTimeStateType _state = CountTimeStateType.STOPED;
        //记录上一步执行的操作
        private OperationType _operation = OperationType.NORMAL;

        public CountTimeModel(float cTime)
        {
            _aimTime = cTime;
        }

        public CountTimeModel(float cTime, bool isRealTime)
        {
            _aimTime = cTime;
            _isRealTime = isRealTime;
        }

        public CountTimeModel(float cTime, UpdateLevel level)
        {
            _aimTime = cTime;
            setUpdateLevel(level);
        }

        public CountTimeModel(float cTime, UpdateLevel level, bool isRealTime)
        {
            _aimTime = cTime;
            setUpdateLevel(level);
            _isRealTime = isRealTime;
        }

        void setUpdateLevel(UpdateLevel level)
        {
            switch (level)
            {
                case UpdateLevel.S:
                    _updateValue = 1f;
                    break;
                case UpdateLevel.S0:
                    _updateValue = 0.1f;
                    break;
                case UpdateLevel.S00:
                    _updateValue = 0.01f;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Begin()
        {
            if (_state != CountTimeStateType.STOPED)
                return;
            if (_beginAction != null)
            {
                _beginAction();
            }
            _currentTime = 0;
            CountTimeMgr.CountTimeObject.StartCoroutine(updateTime());
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (_state != CountTimeStateType.PLAYING)
                return;
            _operation = OperationType.PAUSED;
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Continue()
        {
            if (_state != CountTimeStateType.PAUSED)
                return;
            _operation = OperationType.NORMAL;
            CountTimeMgr.CountTimeObject.StartCoroutine(updateTime());
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (_state == CountTimeStateType.STOPED)
                return;
            _operation = OperationType.STOP;
        }

        IEnumerator updateTime()
        {
            _state = CountTimeStateType.PLAYING;
            if (_isRealTime)
            {
                while (_currentTime < _aimTime)
                {
                    //停止计时
                    if (_operation != OperationType.NORMAL)
                        break;
                    yield return new WaitForSecondsRealtime(_updateValue);
                    _currentTime += _updateValue;
                    if (_updateAction != null)
                        _updateAction(_currentTime);
                }
            }
            else
            {
                while (_currentTime < _aimTime)
                {
                    //停止计时
                    if (_operation != OperationType.NORMAL)
                        break;
                    yield return new WaitForSeconds(_updateValue);
                    _currentTime += _updateValue;
                    if (_updateAction != null)
                        _updateAction(_currentTime);
                }
            }
            if (_operation == OperationType.STOP)
            {
                if (_stopAction != null)
                    _stopAction();
                _state = CountTimeStateType.STOPED;
            }
            else if (_operation == OperationType.NORMAL)
            {
                if (_endAction != null)
                    _endAction();
                _state = CountTimeStateType.STOPED;
            }
            else if (_operation == OperationType.PAUSED)
            {
                _state = CountTimeStateType.PAUSED;
            }
            _operation = OperationType.NORMAL;
            yield return 0;
        }

        public CountTimeModel OnBegin(Action ac)
        {
            _beginAction = ac;
            return this;
        }

        public CountTimeModel OnUpdate(Action<float> ac)
        {
            _updateAction = ac;
            return this;
        }

        public CountTimeModel OnEnd(Action ac)
        {
            _endAction = ac;
            return this;
        }

        public CountTimeModel OnStop(Action ac)
        {
            _stopAction = ac;
            return this;
        }
    }
}



