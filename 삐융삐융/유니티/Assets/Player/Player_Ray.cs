using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player_Ray
public partial class Player : MonoBehaviour
{
    // Player_Move Check Collider
    // Ray_Down
    Vector3 m_vRay_Down;
    Vector3 m_vRay_Down_Distance;
    bool m_bRay_Down;
    Ray m_Ray_Down;
    RaycastHit m_RaycastHit_Down;
    GameObject m_gm_Ray_Down;
    // Ray_Up
    Vector3 m_vRay_Up;
    Vector3 m_vRay_Up_Distance;
    bool m_bRay_Up;
    Ray m_Ray_Up;
    RaycastHit m_RaycastHit_Up;
    GameObject m_gm_Ray_Up;

    void InitialSet_Player_Ray()
    {
        // Ray_Down
        m_vRay_Down = new Vector3(0, -1f, 0);
        m_vRay_Down_Distance = Vector3.zero;
        m_bRay_Down = false;
        m_gm_Ray_Down = null;
        // Ray_Up
        m_vRay_Up = new Vector3(0, +1f, 0);
        m_vRay_Up_Distance = Vector3.zero;
        m_bRay_Up = false;
        m_gm_Ray_Up = null;
   }

    Vector3 Check_Ray()
    {
        if (Check_Ray_Down().y == -1 && Check_Ray_Up().y == -1)
        {
            return Vector3.zero;
        }
        else if (Check_Ray_Down().y == -1 && Check_Ray_Up().y != -1)
        {
            return m_vRay_Up_Distance;
        }
        else if (Check_Ray_Down().y != -1 && Check_Ray_Up().y == -1)
        {
            return -m_vRay_Down_Distance;
        }
        else
        // else if (Check_Ray_Down().y != -1 && Check_Ray_Up().y != -1)
        {
            // 플레이어가 위, 아래에 끼는 경우
            return -m_vRay_Down_Distance;
        }

        //if (Check_Ray_Up().y != -1)
        //    return m_vRay_Up_Distance;
        //else
        //{
        //    if (Check_Ray_Down().y != -1)
        //        return -m_vRay_Down_Distance;
        //}
        //return Vector3.zero;
    }

    // m_vRay_Down 방향의 레이에 닿는지 판단
    // return Vector3.y == -1 : 안닿음 / return Vector3.y == 0 ~ 1f : 닿음. 플레이어와 레이 오브젝트의 거리 반환
    Vector3 Check_Ray_Down()
    {
        m_Ray_Down = new Ray(this.transform.position, m_vRay_Down);

        m_bRay_Down = Physics.Raycast(m_Ray_Down, out m_RaycastHit_Down, 1f, 1 << LayerMask.NameToLayer("Ground"));

        //if (m_gm_Ray_Down != null)
        //    m_gm_Ray_Down.GetComponent<MeshRenderer>().material = m_mt_Material_Hit_false;

        if (m_bRay_Down == true)
        {
            m_gm_Ray_Down = m_RaycastHit_Down.transform.gameObject;
            //m_RaycastHit_Down.transform.gameObject.GetComponent<MeshRenderer>().material = m_mt_Material_Hit_true;

            m_vRay_Down_Distance.y = m_RaycastHit_Down.distance;

            return m_vRay_Down_Distance;
        }
        else
        {
            m_vRay_Down_Distance = Vector3.down;

            return m_vRay_Down_Distance;
        }
    }
    // m_vRay_Up 방향의 레이에 닿는지 판단
    // return Vector3.y == -1 : 안닿음 / return Vector3.y == 0 ~ 1.5 : 닿음. 플레이어와 레이 오브젝트의 거리 반환
    Vector3 Check_Ray_Up()
    {
        m_Ray_Up = new Ray(this.transform.position, m_vRay_Up);

        m_bRay_Up = Physics.Raycast(m_Ray_Up, out m_RaycastHit_Up, 1.5f, 1 << LayerMask.NameToLayer("Ground"));

        //if (m_gm_Ray_Up != null)
        //    m_gm_Ray_Up.GetComponent<MeshRenderer>().material = m_mt_Material_Hit_false;

        if (m_bRay_Up == true)
        {
            m_gm_Ray_Up = m_RaycastHit_Up.transform.gameObject;
            //m_RaycastHit_Up.transform.gameObject.GetComponent<MeshRenderer>().material = m_mt_Material_Hit_true;
            m_RaycastHit_Up.transform.gameObject.GetComponent<Create_Mesh_1>().Change();

            m_vRay_Up_Distance.y = m_RaycastHit_Up.distance;

            return m_vRay_Up_Distance;
        }
        else
        {
            m_vRay_Up_Distance = Vector3.down;

            return m_vRay_Up_Distance;
        }
    }
    //Vector3 Check_Ray_Up()
    //{
    //    m_Ray_Up = new Ray(this.transform.position + (Vector3.up * 1.1f), m_vRay_Up);

    //    m_bRay_Up = Physics.Raycast(m_Ray_Up, out m_RaycastHit_Up, 1f, 1 << LayerMask.NameToLayer("Ground"));

    //    if (m_gm_Ray_Up != null)
    //        m_gm_Ray_Up.GetComponent<MeshRenderer>().material = m_mt_Material_Hit_false;

    //    if (m_bRay_Up == true)
    //    {
    //        m_gm_Ray_Up = m_RaycastHit_Up.transform.gameObject;
    //        m_RaycastHit_Up.transform.gameObject.GetComponent<MeshRenderer>().material = m_mt_Material_Hit_true;

    //        m_vRay_Up_Distance.y = m_RaycastHit_Up.distance;

    //        return m_vRay_Up_Distance;
    //    }
    //    else
    //    {
    //        m_vRay_Up_Distance = Vector3.down;

    //        return m_vRay_Up_Distance;
    //    }
    //}

    private void OnDrawGizmos()
    {
        //// Ray_Down
        //if (m_bRay_Down == true)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawRay(this.transform.position, m_vRay_Down * 1f);
        //}
        //else
        //{
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawRay(this.transform.position, m_vRay_Down * 1f);
        //}
        // Ray_Up
        if (m_bRay_Up == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(this.transform.position, m_vRay_Up * 1.5f);
        }
        else
        {
            Gizmos.color = Color.white;
            Gizmos.DrawRay(this.transform.position, m_vRay_Up * 1.5f);
        }
    }
}
