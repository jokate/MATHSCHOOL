using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHandler : MonoBehaviour
{
    #region Fields
    #endregion Fields

    #region Members
    private Animator m_Animator;

    #endregion Members


    #region Methods
    void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void EnterNextScene()
    {

    }

    public void OnEnterNextScene()
    {
        Time.timeScale = 0;

    }

    public void StartEnd()
    {
        Destroy(gameObject);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }
        #endregion Methods



}
