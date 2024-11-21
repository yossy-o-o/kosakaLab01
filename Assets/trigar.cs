using System;
using UnityEngine;
using Valve.VR;

public class trigar : MonoBehaviour
{
    private SteamVR_Action_Boolean Iui = SteamVR_Actions.default_InteractUI;
    private Boolean interacrtui;
    public GameObject laser;

    private GameObject particle;

    void Start()
    {
        particle.SetActive(false);
    }

    void Update()
    {
        interacrtui = Iui.GetState(SteamVR_Input_Sources.RightHand);

        if (interacrtui)
        {
            laser.SetActive(true);

        }
        else
        {
            laser.SetActive(false);
            particle.SetActive(false);
        }
    }

}
