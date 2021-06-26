#import <UIKit/UIKit.h>

/*
 * iPhone刘海屏工具类
 */
@interface GetNotchSize : NSObject
 
// 判断是否是刘海屏
+(BOOL)getIsNotch;
 
// 获取刘海屏高度
+(CGFloat)getNotchSize;

// 判断是否是横条Home
+(BOOL)getIsHome;
 
@end
