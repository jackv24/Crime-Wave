using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using InControl;
using UnityEngine.UI;

public class UIFunctions : MonoBehaviour
{
    public Animator introAnim;

    public Button[] buttons;
    private int selectedButton = 0;

    private InputDevice device;

    void Start()
    {
        if (buttons.Length > 0)
            buttons[0].Select();
    }

    void Update()
    {
        device = InputManager.ActiveDevice;

        if (introAnim)
        {
            if (Input.GetButtonDown("Submit") || device.Action1.WasPressed)
                introAnim.SetTrigger("progress");
        }

        if(buttons.Length > 1)
        {
            if (device.DPadRight.WasPressed || device.DPadDown.WasPressed || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
                selectedButton++;
            else if (device.DPadLeft.WasPressed || device.DPadUp.WasPressed || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
                selectedButton--;

            selectedButton = Mathf.Clamp(selectedButton, 0, buttons.Length - 1);

            buttons[selectedButton].Select();

            if (Input.GetButtonDown("Submit") || device.Action1.WasPressed)
                buttons[selectedButton].onClick.Invoke();
        }
    }

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
}
