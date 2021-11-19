using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

//Hand animations on local
public class HandPresence : MonoBehaviour
{
    public GameObject handModelPrefab;

    private GameObject spawnedHandModel;
    private Animator handAnimator;

    [SerializeField] InputActionReference grip;
    [SerializeField] InputActionReference trigger;

    void Awake()
    {
        grip.action.performed += GripPress;
        trigger.action.performed += TriggerPress;

        spawnedHandModel = Instantiate(handModelPrefab, transform);
        handAnimator = spawnedHandModel.GetComponent<Animator>();
    }

    //Actions
    private void GripPress(InputAction.CallbackContext obj)
    {
        handAnimator.SetFloat("Grip", obj.ReadValue<float>());
    }
    private void TriggerPress(InputAction.CallbackContext obj)
    {
        handAnimator.SetFloat("Trigger", obj.ReadValue<float>());
    }
}
