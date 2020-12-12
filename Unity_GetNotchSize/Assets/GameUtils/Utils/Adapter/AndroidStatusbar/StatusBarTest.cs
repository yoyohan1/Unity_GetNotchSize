using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusBarTest : MonoBehaviour
{
    public Button toggleDimmedButton;

    [Header("状态栏")]
    public Button visibleButton;
    public Button visibleOverContentButton;
    public Button translucentOverContentButton;
    public Button hiddenButton;

    [Header("导航栏")]
    public Button nvisibleButton;
    public Button nvisibleOverContentButton;
    public Button ntranslucentOverContentButton;
    public Button nhiddenButton;

    [Space(20)]
    public Button fullScreenButton;
    public Button clolorButton;
    public Button nclolorButton;


    private bool isDefaultColor = true;
    private bool nisDefaultColor = true;


    void Start()
    {
        //当AndroidStatusBar.dimmed=false时，状态栏显示所有状态及通知图标
        //当AndroidStatusBar.dimmed=true时，状态栏仅电量和时间，不显示其他状态及通知
        if (toggleDimmedButton != null)
        {
            toggleDimmedButton.onClick.AddListener(delegate
            {
                AndroidStatusBar.dimmed = !AndroidStatusBar.dimmed;
            });
        }
        //显示状态栏，占用屏幕最上方的一部分像素
        if (visibleButton != null)
        {
            visibleButton.onClick.AddListener(delegate
            {
                AndroidStatusBar.statusBarState = AndroidStatusBar.States.Visible;
            });
        }
        //悬浮显示状态栏，不占用屏幕像素
        if (visibleOverContentButton != null)
        {
            visibleOverContentButton.onClick.AddListener(delegate
            {
                AndroidStatusBar.statusBarState = AndroidStatusBar.States.VisibleOverContent;
            });
        }
        //透明悬浮显示状态栏，不占用屏幕像素
        if (translucentOverContentButton != null)
        {
            translucentOverContentButton.onClick.AddListener(delegate
            {
                AndroidStatusBar.statusBarState = AndroidStatusBar.States.TranslucentOverContent;
            });
        }
        //隐藏状态栏
        if (hiddenButton != null)
        {
            hiddenButton.onClick.AddListener(delegate
            {
                AndroidStatusBar.statusBarState = AndroidStatusBar.States.Hidden;
            });
        }
        //显示下方虚拟按键
        if (fullScreenButton != null)
        {
            fullScreenButton.onClick.AddListener(delegate
            {
                Screen.fullScreen = !Screen.fullScreen;
            });
        }



        //显示导航栏，占用屏幕最上方的一部分像素
        if (nvisibleButton != null)
        {
            nvisibleButton.onClick.AddListener(delegate
            {
                AndroidStatusBar.navigationBarState = AndroidStatusBar.States.Visible;
            });
        }
        //悬浮显示导航栏，不占用屏幕像素
        if (nvisibleOverContentButton != null)
        {
            nvisibleOverContentButton.onClick.AddListener(delegate
            {
                AndroidStatusBar.navigationBarState = AndroidStatusBar.States.VisibleOverContent;
            });
        }
        //透明悬浮显示导航栏，不占用屏幕像素
        if (ntranslucentOverContentButton != null)
        {
            ntranslucentOverContentButton.onClick.AddListener(delegate
            {
                AndroidStatusBar.navigationBarState = AndroidStatusBar.States.TranslucentOverContent;
            });
        }
        //隐藏导航栏
        if (nhiddenButton != null)
        {
            nhiddenButton.onClick.AddListener(delegate
            {
                AndroidStatusBar.navigationBarState = AndroidStatusBar.States.Hidden;
            });
        }



        //设置状态栏颜色为绿色
        if (clolorButton != null)
        {
            clolorButton.onClick.AddListener(delegate
            {
                isDefaultColor = !isDefaultColor;
                if (isDefaultColor)
                {
                    AndroidStatusBar.statusBarColor = 0xff000000;
                }
                else
                {
                    AndroidStatusBar.statusBarColor = 0xff0000ff;//注意是argb格式,不是rgba
                }
            });
        }

        //设置导航栏颜色为绿色
        if (nclolorButton != null)
        {
            nclolorButton.onClick.AddListener(delegate
            {
                nisDefaultColor = !nisDefaultColor;
                if (nisDefaultColor)
                {
                    AndroidStatusBar.navigationBarColor = 0xff000000;
                }
                else
                {
                    AndroidStatusBar.navigationBarColor = 0xff00ff00;//注意是argb格式,不是rgba
                }
            });
        }

       

    }

}