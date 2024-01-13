using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeviceDetector : MonoBehaviour
{
    public List<Dropdown> playerDeviceDropdowns;
    private List<InputDevice> devices;
    private Dictionary<int, int> playerDeviceSelections = new Dictionary<int, int>();

    void Start()
    {
        devices = new List<InputDevice>();
        foreach (var device in InputSystem.devices) {
            if (device is Gamepad || device is Keyboard) {
                devices.Add(device);
            }
        }
        UpdateDeviceDropdowns();
        InputSystem.onDeviceChange += OnDeviceChanged;
    }

    public Dictionary<int, int> GetPlayerDeviceSelections() {
        return playerDeviceSelections;
    }

    void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChanged;
    }

    // デバイスが追加または削除されるたびに呼ばれる
    private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added || change == InputDeviceChange.Removed) {
            devices.Clear();
            foreach (var d in InputSystem.devices) {
                if (d is Gamepad || d is Keyboard) {
                    devices.Add(d);
                }
            }
            InitializeDeviceSelections();
            UpdateDeviceDropdowns();
        }
    }

    private void InitializeDeviceSelections()
    {
        playerDeviceSelections.Clear();
        for (int i = 0; i < playerDeviceDropdowns.Count; i++) {
            Debug.Log($"Player {i + 1} selected deviceId: {PlayerPrefs.GetInt($"PlayerDeviceID_{i}")}");
            playerDeviceSelections.Add(i, devices[i].deviceId);
        }
    }

    private void UpdateDeviceDropdowns()
    {
        for (int i = 0; i < playerDeviceDropdowns.Count; i++) {
            playerDeviceDropdowns[i].onValueChanged.RemoveAllListeners();

            int localIndex = i;
            playerDeviceDropdowns[i].onValueChanged.AddListener((int deviceIndex) => {
                OnDeviceSelected(localIndex, deviceIndex);
            });

            UpdateDropdownOptions(playerDeviceDropdowns[i], i);

            // 利用可能なデバイスの中から最初のものをデフォルトとして設定
            if (devices.Count > i) {
                InputDevice defaultDevice = devices[i];
                playerDeviceDropdowns[i].value = i;
                playerDeviceSelections[i] = defaultDevice.deviceId;
            } else {
                playerDeviceDropdowns[i].value = 0;
                playerDeviceSelections[i] = -1; // 未選択の状態を示す
            }

            playerDeviceDropdowns[i].RefreshShownValue();
        }
    }

    public void OnDeviceSelected(int playerIndex, int deviceIndex)
    {
        InputDevice selectedDevice = devices[deviceIndex];
        playerDeviceSelections[playerIndex] = selectedDevice.deviceId;

        PlayerPrefs.SetInt($"PlayerDeviceID_{playerIndex}", selectedDevice.deviceId);
        PlayerPrefs.Save();

        Debug.Log($"Player {playerIndex + 1} selected device: {selectedDevice.displayName}");
        Debug.Log($"Player {playerIndex + 1} selected deviceId: {selectedDevice.deviceId}");
    }

    private void UpdateDropdownOptions(Dropdown dropdown, int playerIndex)
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        List<int> optionDeviceIds = new List<int>();

        foreach (var device in devices) {
            if (device is Gamepad || device is Keyboard) {
                options.Add(device.displayName);
                optionDeviceIds.Add(device.deviceId);
            }
        }

        dropdown.AddOptions(options);
    }

    public bool HasDeviceSelectionDuplicates() {
        HashSet<int> uniqueDeviceIds = new HashSet<int>();
        foreach (var deviceSelection in playerDeviceSelections.Values) {
            if (deviceSelection != -1 && !uniqueDeviceIds.Add(deviceSelection)) {
                return true;
            }
        }
        return false;
    }
}
