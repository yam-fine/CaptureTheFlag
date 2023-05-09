using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
public class GameManager : Singleton<GameManager>
{
    public GameObject Flag;
    public ControlledPlayer controlledPlayer;
    public NpcPlayer npcPlayer;
    public GameObject goalOwner;
    public GameObject goalClient;
    private bool hostIsIt = true;
    public bool HostIsIt { get { return hostIsIt; } set { hostIsIt = value; 
            if (hostIsIt) GameMenuManager.Instance.UpdateText("Host is It");
            else GameMenuManager.Instance.UpdateText("Client is It");
        } }


    public void UpdateScore(GameMenuManager.Side sideToUpdate)
    {
        GameMenuManager.Instance.UpdateScore(1, sideToUpdate);
    }
}
