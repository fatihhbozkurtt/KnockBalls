using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    public event System.Action MouseButtonDown;
    public event System.Action MouseButtonUp;

    public void OnPointerDown()
    {
        MouseButtonDown?.Invoke();
    }

    public void OnPointerUp()
    {
        MouseButtonUp?.Invoke();
    }

    bool DidMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

}
