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
    [SerializeField] private Transform pickUpParent;
    [SerializeField] private GameObject inHandItem;
    [SerializeField]
    [Min(1)]
    private float hitRange = 3;
    [SerializeField] private InputActionReference interactionInput, dropInput, useInput, throwInput;
    [SerializeField] private AudioSource pickUpSource;

    private RaycastHit hit;

    private void Start()
    {
        interactionInput.action.performed += PickUp;
        dropInput.action.performed += Drop;
        useInput.action.performed += Use;
        throwInput.action.performed += Throw;
    }

    private void Throw(InputAction.CallbackContext context)
    {
        if (inHandItem != null)
        {
            inHandItem.transform.SetParent(null);
            Rigidbody rb = inHandItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(playerCameraTransform.forward * 10f, ForceMode.Impulse);
            }
            inHandItem = null;
        }
    }

    private void Use(InputAction.CallbackContext context)
    {
        if (inHandItem != null)
        {
            IUseable usable = inHandItem.GetComponent<IUseable>();
            if (usable != null)
            {
                usable.Use(this.gameObject);
            }
        }
    }

    private void Drop(InputAction.CallbackContext context)
    {
        if (inHandItem != null)
        {
            inHandItem.transform.SetParent(null);
            Rigidbody rb = inHandItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            inHandItem = null;
        }
    }

    private void PickUp(InputAction.CallbackContext context)
    {
        if (hit.collider != null && inHandItem == null)
        {
            IPickable pickable = hit.collider.GetComponent<IPickable>();
            if (pickable != null)
            {
                pickUpSource.Play();
                inHandItem = pickable.PickUp();
                inHandItem.transform.SetParent(pickUpParent.transform, pickable.KeepWorldPosition);
            }
            //Debug.Log(hit.collider.name);
            //Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            //if (hit.collider.GetComponent<Food>() || hit.collider.GetComponent<Weapon>())
            //{
            //    Debug.Log("Food here...");
            //    inHandItem = hit.collider.gameObject;
            //    inHandItem.transform.position = Vector3.zero;
            //    inHandItem.transform.rotation = Quaternion.identity;
            //    inHandItem.transform.SetParent(pickUpParent.transform, false);
            //    if (rb != null)
            //    {
            //        rb.isKinematic = true;
            //    }
            //    return;
            //}
            //if (hit.collider.GetComponent<Item>())
            //{
            //    Debug.Log("Item here...");
            //    inHandItem = hit.collider.gameObject;
            //    inHandItem.transform.SetParent(pickUpParent.transform, true);
            //    if (rb != null)
            //    {
            //        rb.isKinematic = true;
            //    }
            //    return;
            //}
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
        if (inHandItem != null)
        {
            return;
        }
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, pickableLayerMask))
        {
            //hit.collider.GetComponent<Highlight>().ToggleHighlight(true);
            hit.collider.GetComponent<Outline>().enabled = true;
            pickUPUI.SetActive(true);
        }
    }

    public void AddHealth(int healthBoost)
    {
        Debug.Log($"Boost health by {healthBoost}");
    }
}
