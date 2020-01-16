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
    public static event Action<WorldEntity, InteractableObject> OnObjectPickUpSuccess;

    /// <summary>
    /// param1: Object to pick up
    /// </summary>
    public static event Action<InteractableObject> OnObjectPickUpFail;

    /// <summary>
    /// param1: Character to interact with
    /// </summary>
    public static event Action<WorldEntity, InteractableCharacter> OnCharacterInteract;

    /// <summary>
    /// param1: Quest to be invoked
    /// </summary>
    public static event Action<Quest> OnQuestInvoked;

    /// <summary>
    /// param1: Quest to be completed
    /// </summary>
    public static event Action<Quest> OnQuestCompleted;

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

    public static bool PickUpObject(WorldEntity invoker, InteractableObject interactableObject)
    {
        if (!CanInvokerPickUpObject())
        {
            OnObjectPickUpFail?.Invoke(interactableObject);
            return false;
        }

        OnObjectPickUpSuccess?.Invoke(invoker, interactableObject);
        return true;
    }

    public static void InteractWithCharacter(WorldEntity invoker, InteractableCharacter interactableCharacter)
    {
        OnCharacterInteract?.Invoke(invoker, interactableCharacter);
    }

    public static void InvokeQuest(Quest quest)
    {
        OnQuestInvoked?.Invoke(quest);
    }

    public static void QuestCompleted(Quest quest)
    {
        OnQuestCompleted?.Invoke(quest);
    }

    #endregion

    private static bool CanInvokerPickUpObject()
    {
        return true;  // TODO check condition to pickup fail
    }
}
