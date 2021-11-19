using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.InputSystem;

//Controls player movement and animations on network remote instances
public class NetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    [SerializeField] Animator[] handAnimator;
    [SerializeField] InputActionReference[] grip;   //Left + Right
    [SerializeField] InputActionReference[] trigger;

    private PhotonView id;

    private Transform headRig;
    private Transform leftRig;
    private Transform rightRig;

    public void Awake()
    {
        id = GetComponent<PhotonView>();

        if (id.IsMine)
        {
            grip[0].action.performed += LeftGripPress;
            grip[1].action.performed += RightGripPress;

            trigger[0].action.performed += LeftHandTrigger;
            trigger[1].action.performed += RightHandTrigger;

            XRRig rig = FindObjectOfType<XRRig>();
            headRig = rig.transform.Find("Camera Offset/Main Camera");
            leftRig = rig.transform.Find("Camera Offset/LeftHand Controller");
            rightRig = rig.transform.Find("Camera Offset/RightHand Controller");

            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
    }
    private void Update()
    {
        if (id.IsMine)
        {
            UpdateGlobal(head, headRig);
            UpdateGlobal(leftHand, leftRig);
            UpdateGlobal(rightHand, rightRig);
        }
    }

    void UpdateGlobal(Transform target, Transform rig)
    {
        target.position = rig.position;
        target.rotation = rig.rotation;
    }

    //Actions and animations
    private void LeftGripPress(InputAction.CallbackContext obj)
    {
        handAnimator[0].SetFloat("Grip", obj.ReadValue<float>());
    }
    private void RightGripPress(InputAction.CallbackContext obj)
    {
        handAnimator[1].SetFloat("Grip", obj.ReadValue<float>());
    }
    private void LeftHandTrigger(InputAction.CallbackContext obj)
    {
        handAnimator[0].SetFloat("Trigger", obj.ReadValue<float>());
    }
    private void RightHandTrigger(InputAction.CallbackContext obj)
    {
        handAnimator[1].SetFloat("Trigger", obj.ReadValue<float>());
    }



}
