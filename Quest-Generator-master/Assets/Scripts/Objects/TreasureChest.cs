using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public bool interactable = false;
    public bool isOpen = false;
    private Animator anim;

    public Rigidbody coinPrefab;
    public Transform spawner;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space))
        {
          isOpen = !isOpen;
          anim.SetBool("isOpen", isOpen);

        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            interactable = false;
        }
    }

    public void OpenChest()
    {
        Rigidbody coinInstance;
        coinInstance = Instantiate(coinPrefab, spawner.position, spawner.rotation) as Rigidbody;
        coinInstance.AddForce(spawner.up * 100);
    }
}
