# Unity_GetNotchSize
### 介绍

1. Unity 安卓应用适配刘海屏分以下几种情况

   + 没有`Render outside safe area`时：
   		- AndroidP以下使用代码判断四大机型获取刘海屏高度 

		- AndroidP以上设置`shortEdges`然后使用`DisplayCutout`获取刘海屏高度 

   + 有`Render outside safe area`时：
   		- 必须勾选`Render outside safe area`，不然设置`shortEdges`没有作用，因为游戏场景的渲染刘海是由Unity来管理的
		
		- AndroidP以下使用代码判断四大机型获取刘海屏高度
		
		- AndroidP以上直接使用DisplayCutout获取刘海屏高度，但需要通过`shortEdges`来克服splash的刘海黑边
		

2. Unity iOS应用适配刘海屏通过iOS11.0以后的`safeAreaInsets`方法获取，iOS11.0之前在不考虑降低系统的情况下不会有刘海屏（iPhoneX第一代苹果刘海屏自带iOS11.0）

   > `Render outside safe area`：2018.3以后PlayerSettings才有该配置选项，勾选后游戏场景没有刘海屏黑边，splash有刘海屏黑边需要通过代码或者style设置为`shortEdges`模式 
   >
   > `shortEdges`：`windowLayoutInDisplayCutoutMode`中的渲染刘海模式


### 接入

##### Unity

1. 把插件放到项目对应目录
2. 在使用NotchSizeMono.cs之前时需要提前调用getNotchSize()

##### 安卓

 已在getsizemono.aar中集成 无需自己添加

```
//1.在<application>中加入
<meta-data android:name="android.notch_support" android:value="true"/>
<meta-data android:name="notch.config" android:value="portrait|landscape" />
```
```
//2.增加values-v28 style.xml并加入
<item name="android:windowLayoutInDisplayCutoutMode">shortEdges</item>
```

##### iOS：

无

### 效果图

![image](https://github.com/yoyohan1/Unity_GetNotchSize/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE.gif)



