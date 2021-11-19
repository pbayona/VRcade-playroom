using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Custom run in place method - Run without moving and simulate your run on virtual scene
public class RIPMovement : MonoBehaviour
{
    State currentState;

    [SerializeField] private InputActionAsset myActionsAsset;
    private InputAction fixCamera;

    //Position values
    float Cup = 0.08f;
    float Cdown = 0.13f;

    float Hup; //0 < Xrot < 90
    float Hdown; //0 > Xrot > -90

    float moveRange = 0.025f;
    float maxMoveRange = 0.2f;

    float Xrot;
    float Vmin = 1.0f;
    float Vmax = 5.0f;

    Queue runSteps;

    float[] maxAmplitudes; //Max and min height on each cycle

    float v = 0.0f;

    float stateStartTime;

    Vector3 dir;

    float defaultTimeLimit = 0.3f;
    float timeLimitState = 0.3f;   //0.3s by def
    float tmax = 0.42f;
    float tmin = 0.3f;

    bool startedCycle = false;

    CharacterController player;
    private void Start()
    {
        currentState = State.Idle;
        runSteps = new Queue();
        maxAmplitudes = new float[2];

        player = GetComponent<CharacterController>();

        dir = Camera.main.transform.forward;    //Initialize velocity direction
    }

    void Update()
    {
        RIPStateMachine();
        move();
    }

    public void startMovement()
    {
        currentState = State.MotionStart;
    }

    void addToCycle(State s)
    {
        if (!runSteps.Contains(s))
        {
            runSteps.Enqueue(s);
        }

    }

    void RIPStateMachine()  //RIP method state machine
    {
        switch (currentState)
        {
            //Each state change adds itself to a queue
            case State.Idle:
                //If event running, start by changing to MotionStart
                //At the moment, we will make it a transitional state on start
                v = 0.0f;
                runSteps.Clear();
                currentState = State.MotionStart;
                startedCycle = false;
                break;
            case State.MotionStart:

                //Calibrate eye line
                float Hinitial = Camera.main.transform.localPosition.y;
                Xrot = Camera.main.transform.localRotation.eulerAngles.x;
                Hup = Hinitial + Cup * Mathf.Sin(Xrot * 2 * Mathf.PI / 360);
                Hdown = Hinitial + Cdown * Mathf.Sin(Xrot * 2 * Mathf.PI / 360);

                resetAmplitudes();

                Debug.Log("Calibration State. H is " + Hup + "|" + Hdown);

                //Change State to MiddlePos
                currentState = State.MiddlePos;
                Debug.Log("Mid");
                stateStartTime = Time.time;

                break;
            case State.UpPos:

                if (checkTimeLimit()) break;
                //If queue has all the states, recompute velocity 
                //then free the queue and restart the cycle

                if (startedCycle)
                {
                    addToCycle(State.UpPos);
                    updateAmplitude(State.UpPos, Camera.main.transform.localPosition.y);
                    checkFullCycle();
                }

                Xrot = Camera.main.transform.localRotation.eulerAngles.x;

                //If height is near of eye line, change to MiddlePos
                if (Xrot >= 0.0f) //Positive head titl
                {
                    if (Camera.main.transform.localPosition.y < (Hup + moveRange))
                    {
                        startedCycle = true;
                        currentState = State.MiddlePos;
                        stateStartTime = Time.time;
                        Debug.Log("Mid");
                    }
                }
                else //Negative head tilt
                {
                    if (Camera.main.transform.localPosition.y < (Hdown + moveRange))
                    {
                        startedCycle = true;
                        currentState = State.MiddlePos;
                        stateStartTime = Time.time;
                        Debug.Log("Mid");
                    }
                }

                break;
            case State.MiddlePos:

                if (checkTimeLimit()) break;

                addToCycle(State.MiddlePos);

                Xrot = Camera.main.transform.localRotation.eulerAngles.x;
                if (Xrot >= 0.0f) //Positive head tilt
                {
                    //If height is bigger than a umbral+eyeline change to UpPos
                    if (Camera.main.transform.localPosition.y > (Hup + moveRange))
                    {
                        currentState = State.UpPos;
                        stateStartTime = Time.time;
                        Debug.Log("Top");
                    }
                    //Else if height is lower than eyeline-umbral change to DownPos
                    else if (Camera.main.transform.localPosition.y < (Hup - moveRange))
                    {
                        currentState = State.DownPos;
                        stateStartTime = Time.time;
                        Debug.Log("Bot");
                    }
                }
                else //Negative head tilt
                {

                    if (Camera.main.transform.localPosition.y > (Hdown + moveRange))
                    {
                        currentState = State.UpPos;
                        stateStartTime = Time.time;
                        Debug.Log("Top");
                    }
                    //Else if height is lower than eyeline-umbral change to DownPos
                    else if (Camera.main.transform.localPosition.y < (Hdown - moveRange))
                    {
                        currentState = State.DownPos;
                        stateStartTime = Time.time;
                        Debug.Log("Bot");
                    }
                }
                break;
            case State.DownPos:

                if (checkTimeLimit()) break;
                //Debug.Log("Bottom Position");
                addToCycle(State.DownPos);
                updateAmplitude(State.DownPos, Camera.main.transform.localPosition.y);

                Xrot = Camera.main.transform.localRotation.eulerAngles.x;

                // If height is near to eyeline change to MiddlePos
                if (Xrot >= 0.0f) //Positive head titl
                {
                    if (Camera.main.transform.localPosition.y > (Hup - moveRange))
                    {
                        currentState = State.MiddlePos;
                        stateStartTime = Time.time;
                        Debug.Log("Mid");
                    }
                }
                else //Negative head tilt
                {
                    if (Camera.main.transform.localPosition.y > (Hdown - moveRange))
                    {
                        currentState = State.MiddlePos;
                        stateStartTime = Time.time;
                        Debug.Log("Mid");
                    }
                }
                break;


        }
    }

    void checkFullCycle()   //Check if cycle has been complete, going through all states
    {
        Debug.Log("Pasos del ciclo: " + runSteps.Count);
        if (runSteps.Count >= 3)
        {
            runSteps.Clear();
            Debug.Log("Step done");
            applyVelocity();
            computeStateTimeLimit();
            resetAmplitudes();
        }
    }

    bool checkTimeLimit()
    {
        //Time limits varies depending on time completing last cycle
        if (Time.time - stateStartTime >= timeLimitState) //If in a state more than timelimit, then go back to idle
        {
            currentState = State.Idle;
            return true;
        }
        return false;
    }

    void updateAmplitude(State s, float height) //Updates max amplitudes so we can know if player is braking
    {
        switch (s)
        {
            case (State.UpPos):
                if (height > maxAmplitudes[0] || maxAmplitudes[0] == 0)
                    maxAmplitudes[0] = height;
                break;
            case (State.DownPos):
                if (height < maxAmplitudes[1] || maxAmplitudes[1] == 0)
                    maxAmplitudes[1] = height;
                break;
        }

    }

    float computeVelocity()
    {
        float S = maxAmplitudes[0] - maxAmplitudes[1];  //maxAmplitude of this step cycle
        if (S > 1) S = 1;  //Speed can't be greater than Vmax
        else if (S < 0) S = 0; //Neither lower than 0

        Debug.Log("Max amplitudes: " + maxAmplitudes[0] + ", " + maxAmplitudes[1]);
        resetAmplitudes();

        Debug.Log("S is " + S);
        float amplitudePercentage = S / (2 * maxMoveRange); //2*maxMoveRange is maxAmplitude of WIP detection: 0.12+H - (H-0.12)
        Debug.Log("Amplitude percentage is " + amplitudePercentage);
        float vel = Vmin + amplitudePercentage * (Vmax - Vmin);
        return vel;

    }

    void resetAmplitudes()
    {
        maxAmplitudes[0] = 0;
        maxAmplitudes[1] = 0;
    }
    void applyVelocity()
    {
        Debug.Log("Apply velocity");
        v = computeVelocity();
    }

    void move() //Moves toward current camera look at vector
    {

        dir = Camera.main.transform.forward;    //Current camera direction
        dir.y = 0.0f;
        dir.Normalize();

        Vector3 vel = dir * v;
        if (v != 0) Debug.Log(vel);
        player.Move(vel * Time.deltaTime);
    }

    void computeStateTimeLimit()
    {
        if (v != 0.0f)
        {
            float percentage = (1 - (v - Vmin) / (Vmax - Vmin));
            timeLimitState = tmin + percentage * (tmax - tmin);
            Debug.Log("New time limit = " + timeLimitState);
            Debug.Log("Current velocity is = " + v);
        }
        else
        {
            timeLimitState = defaultTimeLimit;
        }
    }
}

public enum State
{
    Idle,
    MotionStart,
    UpPos,
    MiddlePos,
    DownPos
}
