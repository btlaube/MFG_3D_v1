using UnityEngine;
using UnityEngine.InputSystem;
public class Clicker : MonoBehaviour
{
   Camera m_Camera;
   void Awake()
   {
       m_Camera = Camera.main;
   }
   void Update()
   {
       Mouse mouse = Mouse.current;
       if (mouse.leftButton.wasPressedThisFrame)
       {
           Vector3 mousePosition = mouse.position.ReadValue();
           Ray ray = m_Camera.ScreenPointToRay(mousePosition);
           if (Physics.Raycast(ray, out RaycastHit hit))
           {
               Debug.Log(hit.collider.gameObject == this);
           }
       }
   }
}