using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text m_text;
    Player m_player;

    public void SetUp(Player player)
    {
        m_player = player;
        m_text.text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(m_player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

}
