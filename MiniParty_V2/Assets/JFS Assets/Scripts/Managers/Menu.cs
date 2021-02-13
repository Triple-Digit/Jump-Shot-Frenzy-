using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string m_menuName;
    [SerializeField] public bool m_open;

    public void Open()
    {
        m_open = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        m_open = false;
        gameObject.SetActive(false);
    }
}
