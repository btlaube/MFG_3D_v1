using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceableObject : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    private bool isPlacingObject = false;
    private Vector3 initialPosition;
    private Camera m_Camera;

    void Awake()
    {
        m_Camera = Camera.main;
    }

    void Update()
    {
        if (isPlacingObject)
        {
            UpdateObjectPosition();
        }
    }

    public void StartPlacingObject()
    {
        isPlacingObject = true;
        initialPosition = transform.position;
    }

    public void StopPlacingObject()
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

    
