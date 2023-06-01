using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public new string name;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Collected");
            other.transform.parent.GetComponent<PlayerInventory>().PickUpMaterial(gameObject);
            Destroy(gameObject);
        }
        
    }
}
