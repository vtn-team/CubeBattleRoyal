using System.Collections.Generic;
using UnityEngine;

public class MasterCtrl
{
    static MasterCtrl _instance = new MasterCtrl();
    public static MasterCtrl Instance => _instance;
    public MasterCtrl() { }

    List<Player> players = new List<Player>();

    public void Register(Player player)
    {
        players.Add(player);
    }

    public Player GetRandom()
    {
        int index = UnityEngine.Random.Range(0, players.Count);
        return players[index];
    }
}
