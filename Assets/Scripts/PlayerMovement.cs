using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private DeviceDetector deviceDetector;

    [SerializeField]
    private int deviceId;
    [SerializeField]
    private string deviceName;

    public int playerId;

    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        // DeviceDetectorオブジェクトを探し、playerDeviceSelectionsから自分のデバイスIDを取得する
        deviceDetector = FindObjectOfType<DeviceDetector>();
        ManageDevice();
        CheckDeviceDetector();
        
        // deviceIdとオブジェクト名をログで出力
        Debug.Log($"Device ID: {deviceId} Device name: {deviceName}");

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {   
        ManageDevice();
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void ManageDevice() {
        // プレイヤーのデバイスIDを取得（更新されていれば変更されるようにする）
        deviceId = deviceDetector.GetPlayerDeviceSelections()[playerId];
        // 新しいデバイスIDをログで出力
        Debug.Log($"{playerId}の新しい Device ID: {deviceId}");

        var device = InputSystem.GetDeviceById(deviceId);
        
        if (device != null) {
            deviceName = device.name;
        } else {
            Debug.LogError("Device not found with ID: " + deviceId);
        }
    }

    void CheckDeviceDetector()
    {
        if (deviceDetector == null || deviceId == -1) {
            if (deviceDetector == null)
                Debug.LogError("DeviceDetector component not found on " + name);
            if (deviceId == -1)
                Debug.LogError("Device ID not set on " + name);
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }
}
