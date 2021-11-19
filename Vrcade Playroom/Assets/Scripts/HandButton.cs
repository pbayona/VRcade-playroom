using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

//Button manager. Buttons can be pressed by player's hand
public class HandButton : XRBaseInteractable
{

    public UnityEvent OnPress = null;
    bool previousPress = false;

    float hMin = 0.0f;
    float hMax = 0.0f;
    private float previousHandHeight = 0.0f;
    private XRBaseInteractor hoverInteractor = null;

    void Start()
    {
        SetLimits();
    }
    protected override void Awake()
    {
        base.Awake();
        onHoverEnter.AddListener(StartPress);
        onHoverExit.AddListener(EndPress);
    }

    private void OnDestroy()
    {
        onHoverEnter.RemoveListener(StartPress);
        onHoverExit.RemoveListener(EndPress);
    }

    private void StartPress(XRBaseInteractor interactor)
    {
        hoverInteractor = interactor;
        previousHandHeight = GetLocalYPos(interactor.transform.position);
    }

    private void EndPress(XRBaseInteractor interactor)
    {
        hoverInteractor = null;
        previousHandHeight = 0.0f;

        previousPress = false;
        SetYPos(hMax);
    }

    void SetLimits()
    {
        Collider c = GetComponent<Collider>();
        hMin = transform.localPosition.y - (c.bounds.size.y * 0.5f);
        hMax = transform.localPosition.y;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (hoverInteractor)
        {
            float newHandHeight = GetLocalYPos(hoverInteractor.transform.position);
            float handDif = previousHandHeight - newHandHeight;
            previousHandHeight = newHandHeight;

            if (!previousPress)
            {
                float newPos = transform.localPosition.y - handDif;
                SetYPos(newPos);

                checkPressed();
            }
        }
    }

    private float GetLocalYPos(Vector3 pos)
    {
        Vector3 localPos = transform.root.InverseTransformPoint(pos);
        return localPos.y;
    }

    private void SetYPos(float y)
    {
        Vector3 pos = transform.localPosition;
        pos.y = Mathf.Clamp(y, hMin, hMax);
        transform.localPosition = pos;
    }

    private void checkPressed()
    {
        bool inPos = InPosition();

        if (inPos && !previousPress)
        {
            OnPress.Invoke();
        }
        previousPress = inPos;
    }

    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, hMin, hMin + 0.02f);
        return transform.localPosition.y == inRange;
    }

}
