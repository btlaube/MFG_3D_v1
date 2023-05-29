using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceableObject : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject visualObject;

    private bool isPlacingObject = false;
    private Vector3 initialPosition;
    private Camera m_Camera;

    void Awake()
    {
        m_Camera = Camera.main;
    }

    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            if (!isPlacingObject)
            {
                if (CheckClickedObject(mouse))
                {
                    // StartPlacingObject();
                }
            }
            else
            {
                StopPlacingObject();
            }
        }

        if (isPlacingObject)
        {
            UpdateObjectPosition();
        }
    }

    private bool CheckClickedObject(Mouse mouse)
    {
        Vector3 mousePosition = mouse.position.ReadValue();
        Ray ray = m_Camera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return (hit.collider.gameObject == visualObject);
        }
        return false;
    }

    public void StartPlacingObject()
    {
        isPlacingObject = true;
        initialPosition = transform.position;
    }

    private void StopPlacingObject()
    {
        isPlacingObject = false;
    }

    private void UpdateObjectPosition()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPositionPlayerView();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        gameObject.transform.position = grid.CellToWorld(gridPosition);
    }

}

    
