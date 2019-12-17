using System;

public class EntityEventBroker
{

    #region Subscriber Events
    /// <summary>
    /// param1: Invoker
    /// param2: Recepient
    /// </summary>
    public static event Action<WorldEntity, WorldEntity> OnEntityDeath;

    /// <summary>
    /// param1: Entity to be enrolled
    /// </summary>
    public static event Action<WorldEntity> OnEntityEnroll;

    /// <summary>
    /// param1: Object to pick up
    /// </summary>
    public static event Action<InteractableObject> OnObjectPickUpSuccess;

    /// <summary>
    /// param1: Object to pick up
    /// </summary>
    public static event Action<InteractableObject> OnObjectPickUpFail;

    /// <summary>
    /// param1: Character to interact with
    /// </summary>
    public static event Action<InteractableCharacter> OnCharacterInteract;

    #endregion

    #region Publisher Events
    public static void EntityDeath(WorldEntity invoker, WorldEntity recepient)
    {
        OnEntityDeath?.Invoke(invoker, recepient);
    }

    public static void EnrollEntity(WorldEntity entity)
    {
        OnEntityEnroll?.Invoke(entity);
    }

    public static bool PickUpObject(InteractableObject interactableObject)
    {
        if (!CanPlayerPickUpObject())
        {
            OnObjectPickUpFail?.Invoke(interactableObject);
            return false;
        }

        OnObjectPickUpSuccess?.Invoke(interactableObject);
        return true;
    }

    public static void InteractWithCharacter(InteractableCharacter interactableCharacter)
    {
        OnCharacterInteract?.Invoke(interactableCharacter);
    }


    #endregion

    private static bool CanPlayerPickUpObject()
    {
        return true;  // TODO check condition to pickup fail
    }
}
