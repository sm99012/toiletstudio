using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bae_Math : MonoBehaviour
{
    private static Bae_Math instance;
    public static Bae_Math I_Bae_Math
    {
        get
        {
            if (instance == null)
                return null;
            else
                return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public float Calculate_Triangle_Area(Vector3 vD1, Vector3 vD2, Vector3 vD3)
    {
        Vector3 vD1_D2 = vD2 - vD1;
        Vector3 vD1_D3 = vD3 - vD1;

        return (float)System.Math.Round((Vector3.Cross(vD1_D2, vD1_D3).magnitude / 2), 4);
    }
}

// XZ평면 일차함수 클래스
public enum E_DIMENSION1_CATEGORY { X, Z, XZ }
public class Dimension1_Function
{
    public float m_fangles;
    public float m_fSlope;
    //public float m_fIntercept_Z;

    public E_DIMENSION1_CATEGORY m_eDimension_Category;
    public float m_fEtc;

    public Dimension1_Function(float fangles, float fslope, float fintercept_z, E_DIMENSION1_CATEGORY edc, float fetc)
    {
        this.m_fangles = fangles;
        this.m_fSlope = fslope;
        //this.m_fIntercept_Z = fintercept_z;

        this.m_eDimension_Category = edc;
        this.m_fEtc = fetc;
    }

    //public double Get_Z(float x)
    //{
    //    return (m_fSlope * x) + m_fIntercept_Z;
    //}

    // n > 0 : return coordinate max / n < 0 : return coordinate min
    // 일반 수학적인 xz좌표 평면 상의 회전은 반시계 방향으로, 유니티 내부 회전은 시계방향으로 이루어져 관련 처리를 추가했다.
    public Vector3 Get_Coordinate_By_1Cordinate_1Distance(float fx, float fy, float fz, float fdistance, int n)
    {
        if (m_eDimension_Category == E_DIMENSION1_CATEGORY.XZ)
        {
            float fvx, fvz;
            
            fvx = n > 0 ? Mathf.Sqrt((fdistance * fdistance) / (1 + (m_fSlope * m_fSlope))) * (+ 1) : Mathf.Sqrt((fdistance * fdistance) / (1 + (m_fSlope * m_fSlope))) * (- 1);
            fvz = fvx * m_fSlope;

            if ((m_fangles > 0 && m_fangles < 90) || (m_fangles > 270 && m_fangles < 360))
                return new Vector3(fvz + fx, fy, fvx + fz);
            else
                return new Vector3(-fvz + fx, fy, -fvx + fz);
        }
        else if (m_eDimension_Category == E_DIMENSION1_CATEGORY.X)
        {
            if (m_fangles == 90)
            {
                if (n > 0)
                    return new Vector3(fx + fdistance, fy, fz);
                else
                    return new Vector3(fx - fdistance, fy, fz);
            }
            else
            {
                if (n > 0)
                    return new Vector3(fx - fdistance, fy, fz);
                else
                    return new Vector3(fx + fdistance, fy, fz);
            }
        }
        else // if (m_eDimension_Category == E_DIMENSION1_CATEGORY.Z)
        {
            if (m_fangles == 0)
            {
                if (n > 0)
                    return new Vector3(fx, fy, fz + fdistance);
                else
                    return new Vector3(fx, fy, fz - fdistance);
            }
            else
            {
                if (n > 0)
                    return new Vector3(fx, fy, fz - fdistance);
                else
                    return new Vector3(fx, fy, fz + fdistance);
            }
        }
    }
}