using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            anim.SetLayerWeight(1, 1f);
            anim.SetTrigger("IsAttacking");
            print("Attacking!");
        }
	}
}
