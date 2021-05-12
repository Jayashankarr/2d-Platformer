using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Action<Vector2> OnMoveBtn; 
    
    void Start()
    {
        
    }

    
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal") , Input.GetAxis("Vertical")); 
        OnMoveBtn.Invoke(input);
    }
}
