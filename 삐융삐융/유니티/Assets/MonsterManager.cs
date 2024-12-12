using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager instance;
    public static MonsterManager I_MonsterManager
    {
        get
        {
            if (instance == null)
                return null;
            else
                return instance;
        }
    }

    public List<GameObject> m_gl_Monster;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        m_gl_Monster = new List<GameObject>();
    }

}
