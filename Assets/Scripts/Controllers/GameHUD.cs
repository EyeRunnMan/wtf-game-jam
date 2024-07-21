using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public HUDPlayerProgressWidget leftProgressWidget;
    public HUDPlayerProgressWidget rightProgressWidget;
    public Button pauseButton;

    public ScreenController innerLoopScreenController;


    [ContextMenu("Show Win Screen")]
    public void ShowWinScreen()
    {
        innerLoopScreenController.ChangeScreen(ScreenController.SCREEN_TYPE.GAME_OVER_VICTORY);
    }
    [ContextMenu("Show lose Screen")]
    public void ShowLoseScreen()
    {
        innerLoopScreenController.ChangeScreen(ScreenController.SCREEN_TYPE.GAME_OVER_LOSE);
    }

}
