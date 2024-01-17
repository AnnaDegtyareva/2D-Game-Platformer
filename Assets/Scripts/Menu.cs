using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class Menu : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text PlayersText;
    private IEnumerator _AutoConnect;
    [SerializeField] TMP_Text RoomName;
    [SerializeField] TMP_InputField TextRoom;

    private void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("StartLobby");
    }
    void RefreshPlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ShowPlayers", RpcTarget.All, "players" + "\n" + string.Join("\n", PhotonNetwork.PlayerList.ToStringFull()));
        }
    }
    public void CreateRoom()
    {
        string NewRoomName = Random.Range(100000, 999999).ToString();
        PhotonNetwork.CreateRoom(NewRoomName, new Photon.Realtime.RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 16, CleanupCacheOnLeave = true }, Photon.Realtime.TypedLobby.Default);
        Debug.Log("Create");
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Join");
    }
    public void JoinToRoomWithName()
    {
        PhotonNetwork.JoinRoom(TextRoom.text);
        TextRoom.text = "";
    }
    public void AutoGame()
    {
        if (_AutoConnect == null)
        {
            _AutoConnect = AutoConnect();
            StartCoroutine(_AutoConnect);
        }
    }
    public IEnumerator AutoConnect()
    {
        JoinRoom();
        yield return new WaitForSeconds(1);
        CreateRoom();
        _AutoConnect = null;
    }
    public void LoadLvl()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game");
            //PhotonNetwork.CurrentRoom.IsOpen = false;
            //PhotonNetwork.CurrentRoom.IsVisible = false;
        }
    }

    [PunRPC]
    public void ShowPlayers(string players)
    {
        PlayersText.text = players;
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("JoinToRoom");
        RefreshPlayers();
        RoomName.text = "Room - " + PhotonNetwork.CurrentRoom.Name;
        if (_AutoConnect != null)
        {
            StopCoroutine(_AutoConnect);
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        RefreshPlayers();
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RefreshPlayers();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("LoadServer");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Leave");
    }
}
