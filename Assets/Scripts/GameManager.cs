using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public WorldObjectsPrebabs WorldPrefabs;
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] TextMeshProUGUI textPlayers;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefab.name, transform.position, transform.rotation);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //RefreshPlayers();
        
        textPlayers.text = "Player " + newPlayer.NickName + " joing room";
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RefreshPlayers();
        textPlayers.text = "Player " + otherPlayer.NickName + " left room";
    }
}