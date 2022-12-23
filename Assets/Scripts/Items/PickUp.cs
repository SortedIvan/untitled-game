using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Reference to the camera so we can shoot a pick-up range ray
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _pickUpRange;
    [SerializeField] private Material _selectedItemMaterial;
    [SerializeField] private string _selectableItemTag;

    private Material _defaultItemMaterial;
    private Transform _selectedObject;

    private void Update()
    {
        SelectItem();
    }


    private void SelectItem()
    {
        //Transform cameraTransform = Camera.main.transform;
        if(_selectedObject != null)
        {
            _selectedObject.gameObject.GetComponent<Renderer>().material = _defaultItemMaterial;
            _defaultItemMaterial = null;
            _selectedObject = null;
        }


        //var selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var selectionRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit HitInfo;
        Debug.DrawLine(selectionRay.origin, selectionRay.origin + selectionRay.direction * _pickUpRange, Color.red);
        //Debug.DrawLine(_playerCamera.transform.position, _playerCamera.transform.position * _pickUpRange);
        if (Physics.Raycast(selectionRay, out HitInfo, _pickUpRange))
        {
            //Debug.Log(HitInfo.transform.gameObject.name + " is the object");
            var selectableTransform = HitInfo.transform;
            if (selectableTransform.CompareTag(_selectableItemTag))
            {
                if (HitInfo.transform.GetComponent<Renderer>() != null)
                {
                    _defaultItemMaterial = HitInfo.transform.GetComponent<Renderer>().material;
                    HitInfo.transform.GetComponent<Renderer>().material = _selectedItemMaterial;
                }
                IPickable pickable = selectableTransform.gameObject.GetComponent<IPickable>();
                if (pickable != null && Input.GetKeyDown(KeyCode.E))
                {
                    pickable.PickUp();
                }
            }
        }


    }
}
