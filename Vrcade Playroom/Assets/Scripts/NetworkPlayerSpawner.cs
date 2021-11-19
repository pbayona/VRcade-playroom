using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Network player prefab spawner
public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject player;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.Instantiate("Player", transform.position, transform.rotation);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(player);
    }
}
