using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetDevicesTest : MonoBehaviour
{
    private void Start()
    {
        // デバイス一覧を取得
        foreach (var device in InputSystem.devices)
        {
            // デバイス名と番号をログ出力
            Debug.Log($"Device name: {device.name} Device number: {device.deviceId}");
        }
    }
}
