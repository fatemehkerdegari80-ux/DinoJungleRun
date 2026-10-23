
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public float normalSpeed = 3f;
    public float dashSpeed = 8f;

    private InputAction dashAction;

    public float CurrentSpeed
    {
        get
        {
            if (dashAction != null && dashAction.IsPressed())
                return dashSpeed;

            return normalSpeed;
        }
    }

    private void Awake()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        dashAction = input.actions.FindAction("Dash");
    }
}
