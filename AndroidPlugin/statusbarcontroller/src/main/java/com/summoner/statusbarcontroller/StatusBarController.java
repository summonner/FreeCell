package com.Summoner.statusbarcontroller;

import android.app.Activity;
import android.graphics.Color;
import android.graphics.Rect;
import android.os.Build;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;

public class StatusBarController {
    public static void MakeTransparent( final Window window ) {
        int version = Build.VERSION.SDK_INT;
        if ( version >= 19 && version < 21 ) {
            window.addFlags( WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS );
        }
        if ( version >= 21 ) {
            window.clearFlags( WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS );
            window.addFlags( WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS );
            window.setStatusBarColor( Color.TRANSPARENT );
        }
    }

    private static final int flagShowStatusBar = WindowManager.LayoutParams.FLAG_FORCE_NOT_FULLSCREEN
                                               | WindowManager.LayoutParams.FLAG_LAYOUT_IN_SCREEN;
    public static void Show( final Window window, final boolean show ) {
        if ( show ) {
            window.addFlags( flagShowStatusBar );
        }
        else {
            window.clearFlags( flagShowStatusBar );
        }
    }

    public static int GetCurrentHeight( final Window window ) {
        Rect rect = new Rect();
        window.getDecorView().getWindowVisibleDisplayFrame( rect );
        return rect.top;
    }

    public static int GetHeightFromResource( final Activity activity ) {
        int statusBarHeightId = activity.getResources().getIdentifier( "status_bar_height", "dimen", "android" );
        if ( statusBarHeightId <= 0 ) {
            return 0;
        }
        return activity.getResources().getDimensionPixelSize( statusBarHeightId );
    }
}
