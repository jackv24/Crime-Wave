using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.SceneManagement;

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

        if (device.Action1.IsPressed && device.Action2.IsPressed && device.Action3.IsPressed && device.Action4.IsPressed)
            SceneManager.LoadScene(0);

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
            if (Input.GetKeyDown(KeyCode.B) || device.Action2.WasPressed)
                characterStats.Hide();
            else if (Input.GetKeyUp(KeyCode.B) || device.Action2.WasReleased)
                characterStats.UnHide();
        }
    }
}
