using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
public class GameManager : Singleton<GameManager>
{
    public GameObject Flag;
    public ControlledPlayer controlledPlayer;
    public NpcPlayer npcPlayer;

    public void UpdateScore(GameMenuManager.Side sideToUpdate)
    {
        GameMenuManager.Instance.UpdateScore(1, sideToUpdate);
    }
}
