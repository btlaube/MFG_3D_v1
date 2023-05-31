using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public GameObject heldObject;
    [SerializeField] private Transform heldObjectParent;

    public void HoldMovableObject(GameObject objectToEquip)
    {
        if (objectToEquip != null) heldObject = objectToEquip;
    }

    public void PlaceHeldObject()
    {
        heldObject = null;
    }
}
