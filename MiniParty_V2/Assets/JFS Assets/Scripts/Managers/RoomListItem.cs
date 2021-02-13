using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text m_text;

    public RoomInfo m_info;

    public void SetUp(RoomInfo roomInfo)
    {
        m_info = roomInfo;
        m_text.text = roomInfo.Name;
    }

    public void OnClick()
    {
        Launcher.instance.JoinRoom(m_info);
    }
}
