using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // 충돌처리 관련 변수
    float[] m_fa_Collider_X = new float[2];
    float[] m_fa_Collider_Y = new float[2];
    float[] m_fa_Collider_Z = new float[2];

    private void Update()
    {
        Set_Collider();
    }

    void Set_Collider()
    {

    }
}
