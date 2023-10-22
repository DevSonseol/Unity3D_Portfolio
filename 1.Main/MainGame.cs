using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public partial class Main : MonoBehaviour
{
    [HideInInspector] public string playerName;
    private PlayerPrefs playerPrefs;

    private IngameMap playMap = IngameMap.Town;
    private Vector3 playerPos = Vector3.zero;

    private Player mainPlayer;
    private Player[] playerDatas = new Player[Constants.MaxSavedCharSlot];

    public Dictionary<Stat, float> playerIngameStats; //player instancestat + itemstat;

    public Player Player
    {
        get { return mainPlayer; }
    }

    public Player SetPlayer()
    {
        Player player = null;

        return player;
    }

    void MakePlayer() 
    {
        PrefabContainer prefabContainer = PrefabContainer.Instance;

        GameObject ingamePlayer = Instantiate(prefabContainer.PlayerPrefab, this.transform);

        mainPlayer = ingamePlayer.GetComponent<Player>();
        mainPlayer.defaultData.playerName = "수박서리";
        cameraTarget = ingamePlayer;

        playerIngameStats = new Dictionary<Stat, float>(mainPlayer.statData.stats);
    }

}
