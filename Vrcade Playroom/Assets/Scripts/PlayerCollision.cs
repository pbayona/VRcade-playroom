using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Dynamic player collider depending on head position and other basic physics
public class PlayerCollision : MonoBehaviour
{

    private CharacterController player;
    public float gravity = -9.8f;
    private float fallingSpeed;

    private XRRig rig;
    public float heightOffset = 0.2f;

    public LayerMask groundLayer; 


    void Start()
    {
        player = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    private void FixedUpdate()
    {
        cameraColliderSize();

        if (checkIfGrounded())
        {
            fallingSpeed = 0;
        }
        else
        {
            fallingSpeed += gravity * Time.fixedDeltaTime;
        }
        player.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    bool checkIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(player.center);
        float rayLength = player.center.y + 0.01f;

        bool hit = Physics.SphereCast(rayStart, player.radius, Vector3.down, out RaycastHit hitInfo, rayLength);
        return hit;
    }

    void cameraColliderSize()
    {
        player.height = rig.cameraInRigSpaceHeight + heightOffset;
        Vector3 playerCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        player.center = new Vector3(playerCenter.x, player.height / 2 + player.skinWidth, playerCenter.z);
    }
}
