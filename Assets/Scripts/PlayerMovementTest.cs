using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementTest : MonoBehaviour
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
        } else {
            // 本来はキーボードの動きを書く
            Debug.LogError($"Player {playerIndex} is not Gamepad.");
        }        
    }

    void GamepadMovement(Gamepad gamepad)
    {
        Vector2 movementValue = gamepad.leftStick.ReadValue(); // 左スティックの値を読み取る
        Vector3 movement = new Vector3(movementValue.x, 0f, movementValue.y) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        // ランボタン（通常はAボタン）が押されているかどうかをチェック
        if (gamepad.buttonSouth.isPressed)  {
            speed = 10f;
        } else {
            speed = 5f;
        }
    }
}