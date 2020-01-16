using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        //EntityEventBroker.OnObjectPickUpSuccess += GetInterObj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetInterObj(InteractableObject obj)
    {
        gameObject.SetActive(false);
    }
}
