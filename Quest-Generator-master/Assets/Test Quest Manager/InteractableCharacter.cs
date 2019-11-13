using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : Interactable
{
    public string CharacterName { get { return m_characterName; } }


    [SerializeField] private string m_characterName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        EntityEventBroker.InteractWithCharacter(this);
    }
}
