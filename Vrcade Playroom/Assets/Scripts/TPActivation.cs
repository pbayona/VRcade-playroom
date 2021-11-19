using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

//Trigger button and teleportation
public class TPActivation : MonoBehaviour
{
    public XRController teleportRay;
    public float activationVal = 0.1f;
    [SerializeField] InputActionReference trigger;
    bool isActive = false;


    void Start()
    {
        trigger.action.performed += TriggerPress;
    }

    void Update()
    {
        if (teleportRay)
        {
            teleportRay.gameObject.SetActive(isActive);
        }
    }

    private void TriggerPress(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() > activationVal)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
    }
}
