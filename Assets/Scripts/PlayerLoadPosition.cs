using UnityEngine;

public class PlayerLoadPosition : MonoBehaviour
{
    void Start()
    {
        // No checks needed! Just snap to whatever the SaveManager has.
        TeleportToSave();
    }

    public void TeleportToSave()
    {
        Vector3 target = SaveManager.instance.currentSave.lastSavePos;

        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false; // Disable to allow teleport

        transform.position = target;

        if (cc != null) cc.enabled = true; // Re-enable
        Debug.Log("Player snapped to saved position: " + target);
    }
}