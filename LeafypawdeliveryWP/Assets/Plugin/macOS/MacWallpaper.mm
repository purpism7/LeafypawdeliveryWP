#import <Cocoa/Cocoa.h>

extern "C" {
    void SetMacWallpaperMode(const char* windowName, int x, int y, int width, int height) {
        NSString* title = [NSString stringWithUTF8String:windowName];
        
        for (NSWindow* window in [NSApplication sharedApplication].windows) {
            if ([window.title isEqualToString:title]) {
                // 1. 창 스타일을 테두리 없는 스타일로 변경
                [window setStyleMask:NSWindowStyleMaskBorderless];
                
                // 2. 창 레벨을 바탕화면 수준으로 낮춤 (바탕화면 아이콘보다는 위, 일반 창보다는 아래)
                [window setLevel:kCGDesktopWindowLevel];
                
                // 3. 모든 가상 데스크톱(Spaces)에서 보이도록 설정
                [window setCollectionBehavior:NSWindowCollectionBehaviorCanJoinAllSpaces | NSWindowCollectionBehaviorStationary];
                
                // 4. 위치 및 크기 설정
                NSRect frame = NSMakeRect(x, y, width, height);
                [window setFrame:frame display:YES];
                
                // 5. 배경을 투명하게 하거나 그림자 제거 (필요 시)
                [window setHasShadow:NO];
                [window setOpaque:NO];
                [window setBackgroundColor:[NSColor clearColor]];
                
                break;
            }
        }
    }
}