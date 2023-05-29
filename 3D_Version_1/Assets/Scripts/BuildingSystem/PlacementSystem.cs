using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    private Camera m_Camera;

    void Awake()
    {
        m_Camera = Camera.main;
    }


    private void Update() {
        Vector3 mousePosition = inputManager.GetSelectedMapPositionPlayerView();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);


        //Check for mouse input
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            // GameObject clickedObject = CheckClickedObject(mouse);
            Collider[] colliders;
            if((colliders = Physics.OverlapSphere(grid.CellToWorld(gridPosition), 0.5f /* Radius */)).Length > 1) //Presuming the object you are testing also has a collider 0 otherwise
            {
                foreach(var collider in colliders)
                {
                    Debug.Log(collider.name);
                    var go = collider.gameObject; //This is the game object you collided with
                    if(go == gameObject) continue; //Skip the object itself
                    //Do something
                    PlaceableObject placeableObject = go.GetComponentInParent<PlaceableObject>();
                    placeableObject.StartPlacingObject();
                    return;
                }
            }
            // if (clickedObject != null)
            // {
            //     PlaceableObject placeableObject = clickedObject.GetComponentInParent<PlaceableObject>();
            //     placeableObject.StartPlacingObject();
            // }
            // if (moveableObject != null)
            // {
            //     moveableObject.StartPlacingObject();
            // }
        }
    }

    private GameObject CheckClickedObject(Mouse mouse)
    {
        Vector3 mousePosition = mouse.position.ReadValue();
        Ray ray = m_Camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // return (hit.collider.gameObject.tag == "MoveableObject") ? hit.collider.gameObject : null;
            if (hit.collider.gameObject.tag == "MoveableObject")
            {
                Debug.Log(hit.collider.gameObject);
                return hit.collider.gameObject;
            }
        }
        return null;
    }
}
