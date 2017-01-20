using UnityEngine;
using System.Collections;
using InControl;

public class PlayerInput : MonoBehaviour
{
    private InputDevice device;

    private CharacterMove characterMove;
    private CharacterAttack characterAttack;
    private CharacterStats characterStats;

    private int lastDirection = 1;

    void Awake()
    {
        characterMove = GetComponent<CharacterMove>();
        characterAttack = GetComponent<CharacterAttack>();
        characterStats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        //Get active controller from input manager
        device = InputManager.ActiveDevice;

        if(characterMove)
        {
            //Get input direction (clamped)
            float direction = Mathf.Clamp(Input.GetAxisRaw("Horizontal") + device.DPadX, -1, 1);

            if (characterStats && characterStats.isHidden)
                direction = 0;

            if (direction != 0)
                lastDirection = (direction >= 0 ? 1 : -1);

            //Move in input direction
            characterMove.Move(direction);

            //Pressed jump
            if (Input.GetButtonDown("Jump") || device.Action1.WasPressed)
                characterMove.Jump(true);
            //Released jump
            else if (Input.GetButtonUp("Jump") || device.Action1.WasReleased)
                characterMove.Jump(false);
        }

        if (characterAttack)
        {
            if (Input.GetButtonDown("Submit") || device.Action3.WasPressed)
                characterAttack.Attack(lastDirection);
        }

        if(characterStats)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || device.DPadUp.WasPressed)
                characterStats.Hide();
            else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || device.DPadUp.WasReleased)
                characterStats.UnHide();
        }
    }
}
