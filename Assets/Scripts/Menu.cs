using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using System.Linq;
public class Menu : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text PlayersText;
    private IEnumerator _AutoConnect;
    [SerializeField] TMP_Text RoomName;
    [SerializeField] TMP_InputField TextRoom;
    [SerializeField] GameObject load;
    [SerializeField] GameObject first;
    [SerializeField] GameObject second;
    [SerializeField] GameObject forHost;
    [SerializeField] GameObject forPlayers;
    private void Start()
    {
        load.SetActive(true);
        PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();       
    }
    void RefreshPlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ShowPlayers", RpcTarget.All, "players" + "\n" + string.Join("\n", FixPlayerNumbers(PhotonNetwork.PlayerList)));
        }
    }
    public void CreateRoom()
    {
        string NewRoomName = Random.Range(100000, 999999).ToString();
        PhotonNetwork.CreateRoom(NewRoomName, new Photon.Realtime.RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 16, CleanupCacheOnLeave = true }, Photon.Realtime.TypedLobby.Default);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
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
        while (true)
        {
            yield return new WaitForSeconds(.05f);
            if (PhotonNetwork.NetworkClientState != ClientState.Joining)
            {
                if (PhotonNetwork.NetworkClientState != ClientState.ConnectingToGameServer)
                {
                    if(PhotonNetwork.NetworkClientState != ClientState.Authenticating)
                    {
                        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
                        {
                            yield return new WaitForSeconds(.15f);
                            CreateRoom();
                        }
                        yield break;
                    }
                }
            }
        }        
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
    public void ExitRoom()
    {
        ChangeMasterClient();
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            
        }
        PhotonNetwork.LeaveRoom();
    }
    public void ChangeMasterClient()
    {
        Player NewHost = PhotonNetwork.MasterClient;
        //Debug.Log(PhotonNetwork.CurrentRoom.Players.ToStringFull());
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            int key = PhotonNetwork.CurrentRoom.Players.ElementAt(i).Key;
            if (PhotonNetwork.CurrentRoom.Players[key] != PhotonNetwork.MasterClient)
            {
                NewHost = PhotonNetwork.CurrentRoom.Players[key];
            }
        }
        PhotonNetwork.SetMasterClient(NewHost);
    }
    [PunRPC]
    public void ShowPlayers(string players)
    {
        PlayersText.text = players;
    }
    public override void OnJoinedRoom()
    {
        RefreshPlayers();
        RoomName.text = "Room - " + PhotonNetwork.CurrentRoom.Name;
        if (_AutoConnect != null)
        {
            StopCoroutine(_AutoConnect);
        }
        _AutoConnect = null;
        first.SetActive(false);
        second.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            forHost.SetActive(true);
            forPlayers.SetActive(false);
        }
        else
        {
            forHost.SetActive(false);
            forPlayers.SetActive(true);
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            forHost.SetActive(true);
            forPlayers.SetActive(false);
        }
        else
        {
            forHost.SetActive(false);
            forPlayers.SetActive(true);
        }
    }
    public void OnApplicationQuit()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            ChangeMasterClient();
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RefreshPlayers();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RefreshPlayers();
    }
    public override void OnConnectedToMaster()
    {
        load.SetActive(false);
        first.SetActive(true);
        second.SetActive(false);
    }
    public override void OnLeftRoom()
    {
        first.SetActive(true);
        second.SetActive(false);
    }
    public string[] FixPlayerNumbers(Player[] Players)
    {
        string[] result = new string[Players.Length];
        for (int i = 0; i < Players.Length; i++)
        {
            result[i] = (i+1).ToString() + " -" + Players[i].ToString().Substring(3);
        }
        return result;
    }
}