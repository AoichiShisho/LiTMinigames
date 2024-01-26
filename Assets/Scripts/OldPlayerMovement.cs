using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class OldPlayerMovement : MonoBehaviour
{
    public InputActionAsset inputActions;
    private InputAction moveAction;
    private DeviceDetector deviceDetector;
    private MovementInputs movementInputs;
    private Vector2 movementValue;

    [SerializeField]
    private int deviceId;
    [SerializeField]
    private string deviceName;

    public int playerId;

    [SerializeField]
    private float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // TagからDeviceDetectorオブジェクトを探し、playerDeviceSelectionsから自分のデバイスIDを取得する
        deviceDetector = FindObjectOfType<DeviceDetector>();
        deviceId = deviceDetector.GetPlayerDeviceSelections()[playerId];

        ManageDevice();
        CheckDeviceDetector();

        InitializeMovementInputs();
        
        // deviceIdとオブジェクト名をログで出力
        Debug.Log($"PlayerId: {playerId} Device ID: {deviceId} Device name: {deviceName}");

    }

    private void InitializeMovementInputs()
    {
        movementInputs = new MovementInputs();

        var moveAction = movementInputs.Player.Move;
        SetActionBinding(moveAction, deviceId);
        moveAction.started += OnMove;
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        var runAction = movementInputs.Player.Run;
        SetActionBinding(runAction, deviceId);
        runAction.performed += OnRun;
        runAction.canceled += OnRun;

        movementInputs.Enable();
    }

    private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // シーン遷移時には必ずDispose()を呼ぶ必要がある
        movementInputs.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movementValue = context.ReadValue<Vector2>();
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        // 押されている間はspeedを2倍にする
        if (context.performed) {
            speed = 10f;
        }
        // 離されたら元に戻す
        if (context.canceled) {
            speed = 5f;
        }
    }

    void Update()
    {   
        // ManageDevice();

        Vector3 movement = new Vector3(movementValue.x, 0f, movementValue.y) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void ManageDevice() {
        // プレイヤーのデバイスIDを取得（更新されていれば変更されるようにする）
        deviceId = deviceDetector.GetPlayerDeviceSelections()[playerId];

        var device = InputSystem.GetDeviceById(deviceId);
        
        if (device != null) {
            deviceName = device.name;
        } else {
            Debug.LogError("Player " + playerId + ", Device not found with ID: " + deviceId);
        }
    }

    private void SetActionBinding(InputAction action, int deviceId)
    {
        var device = InputSystem.GetDeviceById(deviceId);
        if (device != null) {
            string layoutName = device.layout;
            action.bindingMask = InputBinding.MaskByGroup(layoutName);
            action.Enable();
        }
    }

    void CheckDeviceDetector()
    {
        if (deviceDetector == null || deviceId == -1) {
            if (deviceDetector == null)
                Debug.LogError("DeviceDetector component not found on " + name);
            if (deviceId == -1)
                Debug.LogError("Player " + playerId + ", Device ID not set on " + name);
            // ここでポーズ画面を表示してデバイスを選択できるようにする
        }
    }
}