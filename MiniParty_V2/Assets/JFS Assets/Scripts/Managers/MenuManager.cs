using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Singleton
    public static MenuManager instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }
    #endregion


    [SerializeField] Menu[] m_menus;

    public void OpenMenu(string menuName)
    {
        for( int i = 0; i < m_menus.Length; i++ )
        {
            if(m_menus[i].m_menuName == menuName)
            {
                m_menus[i].Open();
            }
            else if(m_menus[i].m_open)
            {
                CloseMenu(m_menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < m_menus.Length; i++)
        {
            if (m_menus[i].m_open)
            {
                menu.Close();
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

}
