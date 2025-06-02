using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _pickableLayerMask;
    [SerializeField] private Transform _playerCameraTransform;
    [SerializeField] private GameObject _pickUPUI;
    [SerializeField] private Transform _pickUpParent;
    [SerializeField] private GameObject _inHandItem;
    [SerializeField]
    [Min(1)]
    private float _hitRange = 3;
    [SerializeField] private InputActionReference _interactionInput, _dropInput, _useInput, _throwInput;
    [SerializeField] private AudioSource pickUpSource;

    private RaycastHit _hit;

    private void Start()
    {
        _interactionInput.action.performed += PickUp;
        _dropInput.action.performed += Drop;
        _useInput.action.performed += Use;
        _throwInput.action.performed += Throw;
    }

    private void Throw(InputAction.CallbackContext context)
    {
        if (_inHandItem != null)
        {
            _inHandItem.transform.SetParent(null);
            Rigidbody rb = _inHandItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(_playerCameraTransform.forward * 10f, ForceMode.Impulse);
            }
            _inHandItem = null;
        }
    }

    private void Use(InputAction.CallbackContext context)
    {
        if (_inHandItem != null)
        {
            IUseable usable = _inHandItem.GetComponent<IUseable>();
            if (usable != null)
            {
                usable.Use(this.gameObject);
            }
        }
    }

    private void Drop(InputAction.CallbackContext context)
    {
        if (_inHandItem != null)
        {
            _inHandItem.transform.SetParent(null);
            Rigidbody rb = _inHandItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            _inHandItem = null;
        }
    }

    private void PickUp(InputAction.CallbackContext context)
    {
        if (_hit.collider != null && _inHandItem == null)
        {
            IPickable pickable = _hit.collider.GetComponent<IPickable>();
            if (pickable != null)
            {
                pickUpSource.Play();
                _inHandItem = pickable.PickUp();
                _inHandItem.transform.SetParent(_pickUpParent.transform, pickable.KeepWorldPosition);
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
        Debug.DrawRay(_playerCameraTransform.position, _playerCameraTransform.forward * _hitRange, Color.red);

        if (_hit.collider != null)
        {
            //hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            _hit.collider.GetComponent<Outline>().enabled = false;
            _pickUPUI.SetActive(false);
        }
        if (_inHandItem != null)
        {
            return;
        }
        if (Physics.Raycast(_playerCameraTransform.position, _playerCameraTransform.forward, out _hit, _hitRange, _pickableLayerMask))
        {
            //hit.collider.GetComponent<Highlight>().ToggleHighlight(true);
            _hit.collider.GetComponent<Outline>().enabled = true;
            _pickUPUI.SetActive(true);
        }
    }

    public void AddHealth(int healthBoost)
    {
        Debug.Log($"Boost health by {healthBoost}");
    }
}
