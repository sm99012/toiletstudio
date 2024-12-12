using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 동작 FSM
public enum E_MONSTER_MOVE_TYPE { M_WALK, M_ATTACK, M_ATTACKED, M_DEATH }
// 몬스터 피격 FSM
public enum E_MONSTER_ATTACKED_TYPE { NULL, BODY, HEAD }

public partial class Monster : MonoBehaviour
{
    // Monster_Move
    public E_MONSTER_MOVE_TYPE m_eMove_Type;
    public float m_fSpeed_Walk;

    // 충돌처리 관련 변수
    // Monster_Body
    public float[] m_fa_Collider_X_Body = new float[2];
    public float[] m_fa_Collider_Y_Body = new float[2];
    public float[] m_fa_Collider_Z_Body = new float[2];
    // Monster_Head
    public float[] m_fa_Collider_X_Head = new float[2];
    public float[] m_fa_Collider_Y_Head = new float[2];
    public float[] m_fa_Collider_Z_Head = new float[2];

    private void Start()
    {
        MonsterManager.I_MonsterManager.m_gl_Monster.Add(this.gameObject);

        InitialSet_Monster_Animation();
        InitialSet_Monster_Ray();

        m_fSpeed_Walk = 0.5f;
    }

    private void Update()
    {
        Set_Collider();

        if (m_eMove_Type == E_MONSTER_MOVE_TYPE.M_WALK)
            Monster_Move();

        this.transform.position += Check_Ray();

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    m_eMove_Type = Set_MonsterMoveType(E_MONSTER_MOVE_TYPE.M_WALK, 1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    m_eMove_Type = Set_MonsterMoveType(E_MONSTER_MOVE_TYPE.M_ATTACK, 1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    m_eMove_Type = Set_MonsterMoveType(E_MONSTER_MOVE_TYPE.M_ATTACKED, 1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    m_eMove_Type = Set_MonsterMoveType(E_MONSTER_MOVE_TYPE.M_DEATH, 1);
        //}
    }

    virtual protected void Set_Collider()
    {
        // Monster_Body
        m_fa_Collider_X_Body[0] = this.gameObject.transform.position.x - (this.gameObject.transform.localScale.x / 2);
        m_fa_Collider_X_Body[1] = this.gameObject.transform.position.x + (this.gameObject.transform.localScale.x / 2);

        m_fa_Collider_Y_Body[0] = this.gameObject.transform.position.y - (this.gameObject.transform.localScale.y / 2);
        m_fa_Collider_Y_Body[1] = this.gameObject.transform.position.y + (this.gameObject.transform.localScale.y / 2);

        m_fa_Collider_Z_Body[0] = this.gameObject.transform.position.z - (this.gameObject.transform.localScale.z / 2);
        m_fa_Collider_Z_Body[1] = this.gameObject.transform.position.z + (this.gameObject.transform.localScale.z / 2);

        // Monster_Head
        m_fa_Collider_X_Head[0] = this.gameObject.transform.position.x - (this.gameObject.transform.localScale.x / 2);
        m_fa_Collider_X_Head[1] = this.gameObject.transform.position.x + (this.gameObject.transform.localScale.x / 2);

        m_fa_Collider_Y_Head[0] = this.gameObject.transform.position.y - (this.gameObject.transform.localScale.y / 2);
        m_fa_Collider_Y_Head[1] = this.gameObject.transform.position.y + (this.gameObject.transform.localScale.y / 2);

        m_fa_Collider_Z_Head[0] = this.gameObject.transform.position.z - (this.gameObject.transform.localScale.z / 2);
        m_fa_Collider_Z_Head[1] = this.gameObject.transform.position.z + (this.gameObject.transform.localScale.z / 2);
    }

    void Monster_Set_Dir(Vector3 vdir)
    {
        this.gameObject.transform.LookAt(vdir);
    }

    // Monster_Move
    void Monster_Move()
    {
        Monster_Set_Dir(Player.I_Player.transform.position);

        m_eMove_Type = Set_MonsterMoveType(E_MONSTER_MOVE_TYPE.M_WALK, 0.5f);

        if ((Player.I_Player.transform.position - this.transform.position).magnitude > 0.3f)
            this.transform.position += (Player.I_Player.transform.position - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Walk;
    }

    // Monster_Attack
    void Monster_Attack()
    {
        m_eMove_Type = Set_MonsterMoveType(E_MONSTER_MOVE_TYPE.M_ATTACK, 1);
    }

    // Monster_Attacked
    public void Monster_Attacked(Vector3 vknockbackdir, float ftime_knockback, int ndamage)
    {
        if (m_Coroutine_KnockBack != null)
        {
            StopCoroutine(m_Coroutine_KnockBack);
            m_eMove_Type = Set_MonsterMoveType(E_MONSTER_MOVE_TYPE.M_WALK, 1);
            m_Coroutine_KnockBack = null;
        }

        m_Coroutine_KnockBack = StartCoroutine(Coroutine_KnockBack(vknockbackdir, ftime_knockback));
    }

    // Monster_Pushed
    public void Monster_Pushed(Vector3 vknockbackdir, float ftime_knockback)
    {
        if (m_Coroutine_KnockBack != null)
        {
            StopCoroutine(m_Coroutine_KnockBack);
            m_eMove_Type = Set_MonsterMoveType(E_MONSTER_MOVE_TYPE.M_WALK, 1);
            m_Coroutine_KnockBack = null;
        }

        m_Coroutine_KnockBack = StartCoroutine(Coroutine_KnockBack(vknockbackdir, ftime_knockback));
    }

    Coroutine m_Coroutine_KnockBack;
    IEnumerator Coroutine_KnockBack(Vector3 vknockbackdir, float ftime_knockback)
    {
        m_eMove_Type = Set_MonsterMoveType(E_MONSTER_MOVE_TYPE.M_ATTACKED, 1);
        while (ftime_knockback > 0)
        {
            yield return null;
            ftime_knockback -= Time.deltaTime;

            this.transform.position += vknockbackdir.normalized * Time.deltaTime * m_fSpeed_Walk;
        }

        m_eMove_Type = Set_MonsterMoveType(E_MONSTER_MOVE_TYPE.M_WALK, 1);
        m_Coroutine_KnockBack = null;
    }

    private void OnDrawGizmos()
    {
        // Monster_Body
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[0]), new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[0]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[0]), new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[1]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[1]), new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[1]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[1]), new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[0]));

        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[0]), new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[0]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[0]), new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[0]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[1]), new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[1]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[0], m_fa_Collider_Z_Body[1]), new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[1]));

        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[0]), new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[0]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[0]), new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[1]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[1], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[1]), new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[1]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[1]), new Vector3(m_fa_Collider_X_Body[0], m_fa_Collider_Y_Body[1], m_fa_Collider_Z_Body[0]));

        // Monster_Head
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[0]), new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[0]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[0]), new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[1]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[1]), new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[1]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[1]), new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[0]));

        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[0]), new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[0]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[0]), new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[0]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[1]), new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[1]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[0], m_fa_Collider_Z_Head[1]), new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[1]));

        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[0]), new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[0]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[0]), new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[1]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[1], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[1]), new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[1]));
        Gizmos.DrawLine(new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[1]), new Vector3(m_fa_Collider_X_Head[0], m_fa_Collider_Y_Head[1], m_fa_Collider_Z_Head[0]));
    }
}
