using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public int playerIndex;

    private Vector2 movementValue;
    public float speed = 5.0f; // プレイヤーの移動速度
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        int deviceInt = PlayerPrefs.GetInt($"PlayerDeviceID_{playerIndex}", -1);
        if (deviceInt == -1) Debug.LogError($"PlayerDeviceID_{playerIndex} is not set.");
        var device = InputSystem.GetDeviceById(deviceInt);
        
        // デバイスがGamepadだった場合
        if (device is Gamepad gamepad) {
            GamepadMovement(gamepad);
        } else if (device is Keyboard keyboard) {
            // 本来はキーボードの動きを書く
            KeyboardMovement(keyboard);
        }        
    }

    void GamepadMovement(Gamepad gamepad)
    {
        Vector2 movementValue = gamepad.leftStick.ReadValue(); // 左スティックの値を読み取る
        Vector3 movement = new Vector3(movementValue.x, 0f, movementValue.y) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        if (gamepad.buttonSouth.isPressed)  {
            speed = 10f;
        } else {
            speed = 5f;
        }
    }

    void KeyboardMovement(Keyboard keyboard)
    {
        // WASDキーの入力を基に移動方向を計算
        float moveX = 0f;
        float moveZ = 0f;

        if (keyboard.wKey.isPressed) {
            moveZ = 1f;
        }
        if (keyboard.sKey.isPressed) {
            moveZ = -1f;
        }
        if (keyboard.aKey.isPressed) {
            moveX = -1f;
        }
        if (keyboard.dKey.isPressed) {
            moveX = 1f;
        }

        // Shiftキーが押されている場合は速度を上げる
        if (keyboard.shiftKey.isPressed) {
            speed = 10f;
        } else {
            speed = 5f;
        }

        // 計算された移動方向と速度を使用してプレイヤーを移動させる
        Vector3 movement = new Vector3(moveX, 0f, moveZ) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }
}