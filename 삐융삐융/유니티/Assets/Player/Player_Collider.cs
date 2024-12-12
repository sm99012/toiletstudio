using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player_Collider
public partial class Player : MonoBehaviour
{
    [SerializeField] Dimension1_Function F1 = new Dimension1_Function(0, 0, 0, E_DIMENSION1_CATEGORY.XZ, 0); // 플레이어가 바라보는 방향의 일차함수
    [SerializeField] Dimension1_Function F2 = new Dimension1_Function(0, 0, 0, E_DIMENSION1_CATEGORY.XZ, 0); // F1과 수직을 이루는 일차함수(m_vCoordinate_F0 을 지난다.)
    [SerializeField] Dimension1_Function F3 = new Dimension1_Function(0, 0, 0, E_DIMENSION1_CATEGORY.XZ, 0); // F1과 수직을 이루는 일차함수(m_vCoordinate_F1 을 지난다.)
    // 플레이어 푸시 동작 시전 중 몹 밀려나는 함수
    Vector3 m_vPush_Area = new Vector3(0.55f, 0.55f, 0.55f);
    private int Check_Player_Push()
    {
        Vector3 m_vPush_Area = new Vector3(0.55f, 0.55f, 0.55f);

        float fangles_y = this.gameObject.transform.rotation.eulerAngles.y;

        F1.m_fangles = fangles_y;
        F2.m_fangles = fangles_y;
        F2.m_fangles = fangles_y;
        F1.m_fSlope = (float)System.Math.Round(Mathf.Tan((fangles_y * Mathf.Deg2Rad)), 2);
        if (Mathf.Abs((float)F1.m_fSlope) <= 100 && Mathf.Abs((float)F1.m_fSlope) >= 0.001f)
        {
            F1.m_eDimension_Category = E_DIMENSION1_CATEGORY.XZ;
            F1.m_fSlope = (float)System.Math.Round(Mathf.Tan((fangles_y * Mathf.Deg2Rad)), 2);
            //F1.m_fEtc = 0;
            //Debug.Log("1차 함수 : z = " + F1.m_fSlope + "x");
            F2.m_eDimension_Category = E_DIMENSION1_CATEGORY.XZ;
            F2.m_fSlope = (-1) / F1.m_fSlope;
            F3.m_eDimension_Category = E_DIMENSION1_CATEGORY.XZ;
            F3.m_fSlope = (-1) / F1.m_fSlope;
        }
        else if (Mathf.Abs((float)F1.m_fSlope) < 0.001f)
        {
            F1.m_eDimension_Category = E_DIMENSION1_CATEGORY.Z;
            F1.m_fSlope = 0;
            //F1.m_fEtc = this.gameObject.transform.position.z;
            //Debug.Log("1차 함수 : z = " + F1.m_fEtc);
            F2.m_eDimension_Category = E_DIMENSION1_CATEGORY.X;
            F2.m_fSlope = 0;
            F3.m_eDimension_Category = E_DIMENSION1_CATEGORY.X;
            F3.m_fSlope = 0;
        }
        else
        {
            F1.m_eDimension_Category = E_DIMENSION1_CATEGORY.X;
            F1.m_fSlope = 0;
            //F1.m_fEtc = this.gameObject.transform.position.x;
            //Debug.Log("1차 함수 : x = " + F1.m_fEtc);
            F2.m_eDimension_Category = E_DIMENSION1_CATEGORY.Z;
            F2.m_fSlope = 0;
            F3.m_eDimension_Category = E_DIMENSION1_CATEGORY.Z;
            F3.m_fSlope = 0;
        }

        m_vCoordinate_F0 = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        //vcoordinate_min = F1.Get_Coordinate_By_1Cordinate_1Distance(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z, 2, -1);
        m_vCoordinate_F1 = F1.Get_Coordinate_By_1Cordinate_1Distance(m_vCoordinate_F0.x, m_vCoordinate_F0.y, m_vCoordinate_F0.z, 1.1f, +1);

        m_vCoordinate_F2_Min = F2.Get_Coordinate_By_1Cordinate_1Distance(m_vCoordinate_F0.x, m_vCoordinate_F0.y, m_vCoordinate_F0.z, 0.55f, -1);
        m_vCoordinate_F2_Min_Down = m_vCoordinate_F2_Min - Vector3.up * 0.7f;
        m_vCoordinate_F2_Max = F2.Get_Coordinate_By_1Cordinate_1Distance(m_vCoordinate_F0.x, m_vCoordinate_F0.y, m_vCoordinate_F0.z, 0.55f, +1);
        m_vCoordinate_F2_Max_Down = m_vCoordinate_F2_Max - Vector3.up * 0.7f;

        m_vCoordinate_F3_Min = F2.Get_Coordinate_By_1Cordinate_1Distance(m_vCoordinate_F1.x, m_vCoordinate_F1.y, m_vCoordinate_F1.z, 0.55f, -1);
        m_vCoordinate_F3_Min_Down = m_vCoordinate_F3_Min - Vector3.up * 0.7f;
        m_vCoordinate_F3_Max = F2.Get_Coordinate_By_1Cordinate_1Distance(m_vCoordinate_F1.x, m_vCoordinate_F1.y, m_vCoordinate_F1.z, 0.55f, +1);
        m_vCoordinate_F3_Max_Down = m_vCoordinate_F3_Max - Vector3.up * 0.7f;

        E_MONSTER_ATTACKED_TYPE emat = E_MONSTER_ATTACKED_TYPE.NULL;

        for (int i = 0; i < MonsterManager.I_MonsterManager.m_gl_Monster.Count; i++)
        {
            emat = E_MONSTER_ATTACKED_TYPE.NULL;

            // Monster_Head
            if (this.transform.position.y + 1 - 0.7f >= MonsterManager.I_MonsterManager.m_gl_Monster[i].GetComponent<Monster>().m_fa_Collider_Y_Head[0] &&
                this.transform.position.y + 1 <= MonsterManager.I_MonsterManager.m_gl_Monster[i].GetComponent<Monster>().m_fa_Collider_Y_Head[1])
            {
                for (int x = 0; x < 2; x++)
                {
                    if (emat == E_MONSTER_ATTACKED_TYPE.NULL)
                    {
                        for (int z = 0; z < 2; z++)
                        {
                            Vector3 vmonsterpos = new Vector3(MonsterManager.I_MonsterManager.m_gl_Monster[i].GetComponent<Monster>().m_fa_Collider_X_Head[x],
                                                              m_vCoordinate_F0.y,
                                                              MonsterManager.I_MonsterManager.m_gl_Monster[i].GetComponent<Monster>().m_fa_Collider_Z_Head[z]);

                            if (Check_Collider(vmonsterpos, m_vCoordinate_F2_Min, m_vCoordinate_F3_Max, m_vCoordinate_F2_Max) == true)
                            {
                                emat = E_MONSTER_ATTACKED_TYPE.HEAD;
                                break;
                            }
                            else
                            {
                                if (Check_Collider(vmonsterpos, m_vCoordinate_F2_Min, m_vCoordinate_F3_Max, m_vCoordinate_F3_Min) == true)
                                {
                                    emat = E_MONSTER_ATTACKED_TYPE.HEAD;
                                    break;
                                }
                            }
                        }
                    }
                    else
                        break;
                }
            }

            // Monster_Body
            if (emat == E_MONSTER_ATTACKED_TYPE.NULL)
            {
                if (this.transform.position.y + 1 - 0.7f >= MonsterManager.I_MonsterManager.m_gl_Monster[i].GetComponent<Monster>().m_fa_Collider_Y_Body[0] &&
                    this.transform.position.y + 1 <= MonsterManager.I_MonsterManager.m_gl_Monster[i].GetComponent<Monster>().m_fa_Collider_Y_Body[1])
                {
                    for (int x = 0; x < 2; x++)
                    {
                        if (emat == E_MONSTER_ATTACKED_TYPE.NULL)
                        {
                            for (int z = 0; z < 2; z++)
                            {
                                Vector3 vmonsterpos = new Vector3(MonsterManager.I_MonsterManager.m_gl_Monster[i].GetComponent<Monster>().m_fa_Collider_X_Body[x],
                                                                  m_vCoordinate_F0.y,
                                                                  MonsterManager.I_MonsterManager.m_gl_Monster[i].GetComponent<Monster>().m_fa_Collider_Z_Body[z]);

                                if (Check_Collider(vmonsterpos, m_vCoordinate_F2_Min, m_vCoordinate_F3_Max, m_vCoordinate_F2_Max) == true)
                                {
                                    emat = E_MONSTER_ATTACKED_TYPE.BODY;
                                    break;
                                }
                                else
                                {
                                    if (Check_Collider(vmonsterpos, m_vCoordinate_F2_Min, m_vCoordinate_F3_Max, m_vCoordinate_F3_Min) == true)
                                    {
                                        emat = E_MONSTER_ATTACKED_TYPE.BODY;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                            break;
                    }
                }
            }

            if (emat == E_MONSTER_ATTACKED_TYPE.BODY)
            {
                Vector3 vknockbackdir = new Vector3(MonsterManager.I_MonsterManager.m_gl_Monster[i].transform.position.x - this.transform.position.x,
                                                    0,
                                                    MonsterManager.I_MonsterManager.m_gl_Monster[i].transform.position.z - this.transform.position.z);
                MonsterManager.I_MonsterManager.m_gl_Monster[i].GetComponent<Monster>().Monster_Pushed(vknockbackdir, 0.5f);
            }
            else if (emat == E_MONSTER_ATTACKED_TYPE.HEAD)
            {
                Vector3 vknockbackdir = new Vector3(MonsterManager.I_MonsterManager.m_gl_Monster[i].transform.position.x - this.transform.position.x,
                                                    0,
                                                    MonsterManager.I_MonsterManager.m_gl_Monster[i].transform.position.z - this.transform.position.z);
                MonsterManager.I_MonsterManager.m_gl_Monster[i].GetComponent<Monster>().Monster_Pushed(vknockbackdir, 0.5f);
            }
        }


        return 0;
    }

    public bool Check_Collider(Vector3 vtargerpos, Vector3 vD1, Vector3 vD2, Vector3 vD3)
    {
        if (Bae_Math.I_Bae_Math.Calculate_Triangle_Area(vD1, vD2, vD3) == (float)System.Math.Round(Bae_Math.I_Bae_Math.Calculate_Triangle_Area(vD1, vD2, vtargerpos) + Bae_Math.I_Bae_Math.Calculate_Triangle_Area(vD2, vD3, vtargerpos) + Bae_Math.I_Bae_Math.Calculate_Triangle_Area(vD3, vD1, vtargerpos), 4))
            return true;
        else
            return false;
    }

    [SerializeField] Vector3 m_vCoordinate_F0;     // 플레이어 위치 좌표
    [SerializeField] Vector3 m_vCoordinate_F1;     // 플레이어가 바라보는 방향의 특정 위치 좌표(두 점 m_vCoordinate_F0, m_vCoordinate_F1 사이의 거리 = 특정 거리(fDistance))
    [SerializeField] Vector3 m_vCoordinate_F2_Min; // F1과 수직을 이루는 일차함수(m_vCoordinate_F0 을 지난다.) - min
    [SerializeField] Vector3 m_vCoordinate_F2_Max; // F1과 수직을 이루는 일차함수(m_vCoordinate_F0 을 지난다.) - max
    [SerializeField] Vector3 m_vCoordinate_F3_Min; // F1과 수직을 이루는 일차함수(m_vCoordinate_F1 을 지난다.) - min
    [SerializeField] Vector3 m_vCoordinate_F3_Max; // F1과 수직을 이루는 일차함수(m_vCoordinate_F1 을 지난다.) - max

    [SerializeField] Vector3 m_vCoordinate_F2_Min_Down; // F1과 수직을 이루는 일차함수(m_vCoordinate_F0 을 지난다.) - min
    [SerializeField] Vector3 m_vCoordinate_F2_Max_Down; // F1과 수직을 이루는 일차함수(m_vCoordinate_F0 을 지난다.) - max
    [SerializeField] Vector3 m_vCoordinate_F3_Min_Down; // F1과 수직을 이루는 일차함수(m_vCoordinate_F1 을 지난다.) - min
    [SerializeField] Vector3 m_vCoordinate_F3_Max_Down; // F1과 수직을 이루는 일차함수(m_vCoordinate_F1 을 지난다.) - max

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(m_vCoordinate_F0, m_vCoordinate_F1);
        //Gizmos.DrawLine(m_vCoordinate_F2_Min, m_vCoordinate_F2_Max);
        //Gizmos.DrawLine(m_vCoordinate_F3_Min, m_vCoordinate_F3_Max);

        Gizmos.DrawLine(m_vCoordinate_F2_Min, m_vCoordinate_F2_Max);
        Gizmos.DrawLine(m_vCoordinate_F2_Min, m_vCoordinate_F3_Min);
        Gizmos.DrawLine(m_vCoordinate_F3_Min, m_vCoordinate_F3_Max);
        Gizmos.DrawLine(m_vCoordinate_F2_Max, m_vCoordinate_F3_Max);

        Gizmos.DrawLine(m_vCoordinate_F2_Min, m_vCoordinate_F2_Min_Down);
        Gizmos.DrawLine(m_vCoordinate_F2_Max, m_vCoordinate_F2_Max_Down);
        Gizmos.DrawLine(m_vCoordinate_F3_Min, m_vCoordinate_F3_Min_Down);
        Gizmos.DrawLine(m_vCoordinate_F3_Max, m_vCoordinate_F3_Max_Down);

        Gizmos.DrawLine(m_vCoordinate_F2_Min_Down, m_vCoordinate_F2_Max_Down);
        Gizmos.DrawLine(m_vCoordinate_F2_Min_Down, m_vCoordinate_F3_Min_Down);
        Gizmos.DrawLine(m_vCoordinate_F3_Min_Down, m_vCoordinate_F3_Max_Down);
        Gizmos.DrawLine(m_vCoordinate_F2_Max_Down, m_vCoordinate_F3_Max_Down);

        //Gizmos.DrawLine(m_vCoordinate_F2_Min, m_vCoordinate_F3_Max);

        //Gizmos.color = Color.red;
        //Gizmos.DrawCube(m_vCoordinate_F2_Min, Vector3.one * 0.1f);
        //Gizmos.color = Color.green;
        //Gizmos.DrawCube(m_vCoordinate_F2_Max, Vector3.one * 0.1f);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawCube(m_vCoordinate_F3_Min, Vector3.one * 0.1f);
        //Gizmos.color = Color.white;
        //Gizmos.DrawCube(m_vCoordinate_F3_Max, Vector3.one * 0.1f);
    }
}
