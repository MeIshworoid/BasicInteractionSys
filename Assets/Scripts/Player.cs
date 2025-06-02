using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask pickableLayerMask;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private GameObject pickUPUI;
    [SerializeField]
    [Min(1)]
    private float hitRange = 3;
    [SerializeField] private InputActionReference interactionInput, dropInput, useInput, throwInput;

    private RaycastHit hit;

    private void Start()
    {
        interactionInput.action.performed += Interact;
        dropInput.action.performed += Drop;
        useInput.action.performed += Use;
        throwInput.action.performed += Throw;
    }

    private void Throw(InputAction.CallbackContext context)
    {
        
    }

    private void Use(InputAction.CallbackContext context)
    {
       
    }

    private void Drop(InputAction.CallbackContext context)
    {
       
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
        }
    }

    private void Update()
    {
        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);

        if (hit.collider != null)
        {
            //hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            hit.collider.GetComponent<Outline>().enabled = false;
            pickUPUI.SetActive(false);
        }
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, pickableLayerMask))
        {
            //hit.collider.GetComponent<Highlight>().ToggleHighlight(true);
            hit.collider.GetComponent<Outline>().enabled = true;
            pickUPUI.SetActive(true);
        }
    }
}
