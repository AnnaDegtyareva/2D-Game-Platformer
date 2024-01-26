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
    [SerializeField] TextMeshProUGUI TextPlayers;
    public GameObject test;
    GameObject LastChunk;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefab.name, transform.position + new Vector3(3, 7, 0), transform.rotation);

        if (PhotonNetwork.IsMasterClient)
        {
            //int ChunkId = UnityEngine.Random.Range(0, Chunks.Length);
            LastChunk = PhotonNetwork.Instantiate(test.name, Vector3.zero, Quaternion.identity);
            for (int i = 0; i < 10; i++)
            {
                //int ChunkId = UnityEngine.Random.Range(0, Chunks.Length);
                float SpawnPos = LastChunk.transform.localPosition.x + LastChunk.GetComponent<Chunk>().end.localPosition.x - test.GetComponent<Chunk>().start.transform.localPosition.x;
                LastChunk = PhotonNetwork.Instantiate(test.name, new Vector3(SpawnPos, 0, 0), Quaternion.identity);
            }
        }
        
        
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //RefreshPlayers();
        
        TextPlayers.text = "Player " + newPlayer.NickName + " joing room";
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RefreshPlayers();
        TextPlayers.text = "Player " + otherPlayer.NickName + " left room";
    }
}