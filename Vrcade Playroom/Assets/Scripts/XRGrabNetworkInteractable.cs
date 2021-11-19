using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

//Online custom grab component
public class XRGrabNetworkInteractable : XRGrabInteractable
{
    PhotonView id;

    void Start()
    {
        id = GetComponent<PhotonView>();
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        id.RequestOwnership();
        base.OnSelectEntered(interactor);
    }
}
