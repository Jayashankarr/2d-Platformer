using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Action<Vector2> OnMoveBtn;

    public Action<bool> OnJumpBtnTapped; 
    
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal") , Input.GetAxis("Vertical")); 
        OnMoveBtn.Invoke(input);

        if (Input.GetKeyDown (KeyCode.Space)) {
            OnJumpBtnTapped.Invoke(true);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			OnJumpBtnTapped.Invoke (false);
		}
    }
}
