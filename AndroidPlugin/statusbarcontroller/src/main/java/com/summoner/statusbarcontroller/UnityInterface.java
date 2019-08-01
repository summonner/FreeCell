package com.Summoner.statusbarcontroller;

import android.view.Window;

import com.unity3d.player.UnityPlayer;

public class UnityInterface {
    private static Window GetWindow() {
        return UnityPlayer.currentActivity.getWindow();
    }

    public static void MakeTransparent() {
        UnityPlayer.currentActivity.runOnUiThread( new Runnable() {
            @Override
            public void run() {
                StatusBarController.MakeTransparent( GetWindow() );
            }
        } );
    }

    public static void Show( final boolean show ) {
        UnityPlayer.currentActivity.runOnUiThread( new Runnable() {
            @Override
            public void run() {
                StatusBarController.Show( GetWindow(), show );
            }
        } );
    }

    public static int GetCurrentHeight() {
        return StatusBarController.GetCurrentHeight( GetWindow() );
    }

    public static int GetHeightFromResource() {
        return StatusBarController.GetHeightFromResource( UnityPlayer.currentActivity );
    }
}