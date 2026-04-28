#ifdef UNITY_4_0 || UNITY_5_0
#import "iPhone_View.h"
#else
extern UIViewController* UnityGetGLViewController();
#import <Photos/Photos.h>
#endif

// Credit: https://github.com/ChrisMaire/unity-native-sharing

extern "C" void _NativeShare_Share( const char* files[], int filesCount, char* subject, const char* text ) 
{
 	NSMutableArray *items = [NSMutableArray new];

 	if( strlen( text ) > 0 )
  		[items addObject:[NSString stringWithUTF8String:text]];

 	// Credit: https://answers.unity.com/answers/862224/view.html
 	for( int i = 0; i < filesCount; i++ ) 
 	{
  		NSString *filePath = [NSString stringWithUTF8String:files[i]];
  		UIImage *image = [UIImage imageWithContentsOfFile:filePath];
 		if( image != nil )
  			 [items addObject:image];
  	else
   		[items addObject:[NSURL fileURLWithPath:filePath]];
 }

 	UIActivityViewController *activity = [[UIActivityViewController alloc] initWithActivityItems:items applicationActivities:nil];
 	if( strlen( subject ) > 0 )
  		[activity setValue:[NSString stringWithUTF8String:subject] forKey:@"subject"];

 	UIViewController *rootViewController = UnityGetGLViewController();
 	if( UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone ) // iPhone
 	{
  		[rootViewController presentViewController:activity animated:YES completion:nil];
 	}
 	else // iPad
 	{
  		UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activity];
  		[popup presentPopoverFromRect:CGRectMake( rootViewController.view.frame.size.width/2, rootViewController.view.frame.size.height/4, 0, 0 ) inView:rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionUp animated:YES];
 	}
}

extern "C" int _Is_Facebook_Installed()
{
 	NSURL *faceebokURL = [NSURL URLWithString:@"fb://"];

 	if ([[UIApplication sharedApplication] canOpenURL:faceebokURL])
 	{
  		return 1;
 	}
 	else
 	{
  		return 0;
 	}
}

extern "C" int _Is_Instagramm_Installed()
{
 	NSURL *instagramURL = [NSURL URLWithString:@"instagram://app"];

 	if ([[UIApplication sharedApplication] canOpenURL:instagramURL])
 	{
  		return 1;
 	}
 	else
 	{
  		return 0;
 	}
}

extern "C" void _Share_To_Instagram( const char* filePath ,const char* text)
{
 NSURL *instagramURL = [NSURL URLWithString:@"instagram://app"];

 if ([[UIApplication sharedApplication] canOpenURL:instagramURL])
 {

    NSString *media = [NSString stringWithUTF8String:filePath];
    BOOL isVideo = YES;
        
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_BACKGROUND, 0), ^{

        NSData *urlData = [NSData dataWithContentsOfFile:media];
        if ( urlData ) {
            
            NSString *filePath = [NSTemporaryDirectory() stringByAppendingPathComponent: isVideo? @"instagramShare.mp4" : @"instagramShare.jpg"];
            
            NSURL *outputFileURL = [NSURL URLWithString:filePath];
            
            dispatch_async(dispatch_get_main_queue(), ^{

                if (![urlData writeToFile:filePath atomically:YES]) {
                   
                    return;
                }

                // Check authorization status.
                [PHPhotoLibrary requestAuthorization:^( PHAuthorizationStatus status ) {
                    if ( status == PHAuthorizationStatusAuthorized ) {

                        // Save the movie file to the photo library and cleanup.
                        [[PHPhotoLibrary sharedPhotoLibrary] performChanges:^{
                            // In iOS 9 and later, it's possible to move the file into the photo library without duplicating the file data.
                            // This avoids using double the disk space during save, which can make a difference on devices with limited free disk space.                            
                            PHAssetResourceCreationOptions *options = [[PHAssetResourceCreationOptions alloc] init];
                            options.shouldMoveFile = YES;
							PHAssetCreationRequest *changeRequest = [PHAssetCreationRequest creationRequestForAsset];
                            if (isVideo)
                                [changeRequest addResourceWithType:PHAssetResourceTypeVideo fileURL:outputFileURL options:options];
                            else
                                [changeRequest addResourceWithType:PHAssetResourceTypePhoto fileURL:outputFileURL options:options];

                        } completionHandler:^( BOOL success, NSError *error ) {

                            if ( success ) {
                                PHFetchOptions *fetchOptions = [[PHFetchOptions alloc] init];
                                fetchOptions.sortDescriptors = @[[NSSortDescriptor sortDescriptorWithKey:@"creationDate" ascending:NO]];
                                PHFetchResult *fetchResult;
                                if (isVideo)
                                    fetchResult = [PHAsset fetchAssetsWithMediaType:PHAssetMediaTypeVideo options:fetchOptions];
                                else
                                    fetchResult = [PHAsset fetchAssetsWithMediaType:PHAssetMediaTypeImage options:fetchOptions];
                                PHObject *lastAsset = fetchResult.firstObject;
                                if (lastAsset != nil) {
                                    NSString *localIdentifier = lastAsset.localIdentifier;
                                    
                                    NSString *u = [NSString stringWithFormat:@"%@%@",
                                                   @"instagram://library?LocalIdentifier=",
                                                   localIdentifier];
                                    
                                    NSURL *url = [NSURL URLWithString:u];
                                    
                                    dispatch_async(dispatch_get_main_queue(), ^{
                                        if ([[UIApplication sharedApplication] canOpenURL:url]) {
                                            [[UIApplication sharedApplication] openURL:url options:@{} completionHandler:nil];
                                        }
                                    });
                                }
                            }
                            
                        }];
                    }                   
                }];
            });
        }
       
    });
}
}