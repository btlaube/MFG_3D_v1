using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    private PlayerInventory playerInventory;
    private GridManager gridManager;

    private Camera m_Camera;

    private PlayerInput playerInput;
    private Vector3 mousePosition;
    private Vector3Int gridPosition;
    private bool holdingObject = false;

    void Awake()
    {
        m_Camera = Camera.main;
        playerInventory = GetComponentInParent<PlayerInventory>();
        gridManager = GameObject.Find("Grid").GetComponent<GridManager>();

        playerInput = new PlayerInput();
        playerInput.Player.Enable();
    }

    private void OnEnable() {
        playerInput.Player.Interact.performed += MoveObject;
    }

    private void OnDisable() {
        playerInput.Player.Interact.performed -= MoveObject;
    }

    private void Update() {
        //Move active cell indicator
        mousePosition = inputManager.GetSelectedMapPositionPlayerView();
        gridPosition = grid.WorldToCell(mousePosition);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    private void MoveObject(InputAction.CallbackContext context)
    {
        if (holdingObject)
        {
            if(CheckPlacement(playerInventory.heldObject))
            {
                playerInventory.heldObject.GetComponent<PlaceableObject>().StopPlacingObject();
                holdingObject = false;
            }
        }
        else
        {
            // if((colliders = Physics.OverlapSphere(grid.CellToWorld(gridPosition), 0.5f)).Length > 1)
            if(gridManager.objectsOnGrid.Count > 1)
            {
                foreach(GameObject boardObject in gridManager.objectsOnGrid)
                {
                    if(boardObject.transform.position == grid.CellToWorld(gridPosition))
                    {
                        var go = boardObject.gameObject;
                        if(go == gameObject) continue;
                        playerInventory.HoldMovableObject(boardObject);
                        PlaceableObject placeableObject = go.GetComponentInParent<PlaceableObject>();
                        if (placeableObject != null)
                        {
                            placeableObject.StartPlacingObject();
                            holdingObject = true;
                        }
                        //Return after picking up one object
                        return;
                    }
                }
            }
        }
    }

// Returns true if the object can be placed in its current location; false otherwise (if there is already a board object in the current location).
    private bool CheckPlacement(GameObject objectToPlace)
    {
        foreach(GameObject boardObject in gridManager.objectsOnGrid)
        {
            if(boardObject == objectToPlace) continue; //skip objectToPlace
            if(boardObject.transform.position == objectToPlace.transform.position)
            {
                return false;
            }
        }
        return true;
    }

}
