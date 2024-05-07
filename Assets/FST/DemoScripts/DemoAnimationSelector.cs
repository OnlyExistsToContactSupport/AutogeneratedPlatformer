using System;
using UnityEngine;

public class DemoAnimationSelector : MonoBehaviour
{
    private Animator m_Animator = null;
    private void Start()
    {
        SwitchAnimation(2);
    }
    public void SwitchAnimation(int index)
    {
        if (!m_Animator)
            m_Animator = GetComponentInChildren<Animator>();

        if (m_Animator)
            m_Animator.SetInteger("Mode", index);
    }
}
