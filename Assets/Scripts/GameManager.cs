using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public WorldObjectsPrebabs WorldPrefabs;
    [SerializeField] GameObject PlayerPrefab;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefab.name, transform.position, transform.rotation);
    }
}