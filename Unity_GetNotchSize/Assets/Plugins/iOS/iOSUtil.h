#import <UIKit/UIKit.h>

/*
 * iPhone刘海屏工具类
 */
@interface iOSUtil : NSObject
 
// 判断是否是刘海屏
+(BOOL)getIsNotch;
 
// 获取刘海屏高度
+(CGFloat)getNotchSize;

// 判断是否是横条Home
+(BOOL)getIsHome;
 
// 获取横条Home高度
+(CGFloat)getHomeSize;

// 获取刘海屏高度
+(NSString*)getiPhoneType;

// 获取剩余空间
+ (long long)getDiskFreeSize;
 
@end
