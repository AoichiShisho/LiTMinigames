using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReceiveNotification : MonoBehaviour
{
    // プレイヤー入室時に受け取る
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        print($"プレイヤー#{playerInput.user.index}が入室しました");
    }

    // プレイヤー退出時に受け取る
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        print($"プレイヤー#{playerInput.user.index}が退出しました");
    }
}
