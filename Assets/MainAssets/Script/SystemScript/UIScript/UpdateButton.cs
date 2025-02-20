using UnityEngine;

public class UpdateButton : MonoBehaviour
{
    public StatusWindow statusWindow;

    public void OnButtonClick()
    {
        PlayerStatus playerStatus = statusWindow.playerStatus;
        statusWindow.UpdatePlayerStatus(
            playerStatus.NAME,
            playerStatus.MAXHP,
            playerStatus.MAXMP,
            playerStatus.ATK,
            playerStatus.DEF,
            playerStatus.INT,
            playerStatus.MDEF,
            playerStatus.AGI,
            playerStatus.LV,
            playerStatus.EXP,
            playerStatus.MAXEXP,
            playerStatus.HAVEGOLD,
            playerStatus.StatusPoint
            );
    }
}
