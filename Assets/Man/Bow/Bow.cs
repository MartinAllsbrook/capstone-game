using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Bow : MonoBehaviour
{
    public static Bow Instance;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject thirdPersonCamera;
    [SerializeField] private Transform arrowSpawnPosition;
    
    private float chargeTime = 0;
    private float strength = 0;
    private int selectedArrow = 0;
    private float chargeMultiplier = 3;

    
    
    private void Awake()
    {
        // Create singleton
        if (Instance == null)
            Instance = this;
    }

    public void CycleArrow()
    {
        // Cycle through arrows
        selectedArrow++;
        if (selectedArrow >= arrows.Length)
            selectedArrow = 0;
        
        // Update hud
        HUDController.Instance.SetArrow(arrows[selectedArrow].GetComponent<Arrow>().ArrowName);
    }

    // "Charge" arrow based on how long the player is holding the trigger
    public void ChargeArrow()
    {
        // The longer the player holds the trigger the slower strength builds up
        chargeTime += Time.deltaTime * chargeMultiplier;
        strength = Mathf.Pow(chargeTime, 1/chargeMultiplier);
        
        // Change fov to match arrow charge
        thirdPersonCamera.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = strength * 4 + 45;
    }
    public void FireArrow()
    {
        // Use camera rotation to aim the arrow
        Vector3 rotation = camera.transform.rotation.eulerAngles;
        var arrowInstance = Instantiate(arrows[selectedArrow], arrowSpawnPosition.position, Quaternion.Euler(rotation));
        
        // Add force to the arrow equal to strength
        arrowInstance.GetComponent<Arrow>().Fire(strength);
        
        // Reset FOV and vars
        thirdPersonCamera.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = 45;
        strength = 0;
        chargeTime = 0;
    }
}
