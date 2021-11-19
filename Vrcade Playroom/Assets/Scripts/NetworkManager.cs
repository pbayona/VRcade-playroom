using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//Server initialization
public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] RIPMovement playerRun;

    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Conectando al servidor...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexión establecida!");
        base.OnConnectedToMaster();
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4;
        ro.IsVisible = true;
        ro.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom("Bowling Room", ro, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Conectado a la sala");
        playerRun.startMovement();
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Se ha unido un nuevo jugador");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
