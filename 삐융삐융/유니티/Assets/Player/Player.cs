using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_MOVE_TYPE { P_IDLE, P_IDLE_AIM, P_WALK, P_WALK_AIM, P_WALK_LEFT, P_WALK_LEFT_AIM, P_WALK_RIGHT, P_WALK_RIGHT_AIM, P_RUN, P_RUN_AIM , P_PUSH }//, , P_PUSH, P_JUMP }
public enum E_AIM_TYPE { TRUE, FALSE }
public enum E_MOVE_VALUE { V_MINUS, V_ZERO, V_PLUS }

public partial class Player : MonoBehaviour
{
    // static Player value. Singleton Pattern
    private static Player S_Player;

    // Player_Move Set Direction
    [SerializeField] private GameObject m_g_Dir_Front;
    [SerializeField] private GameObject m_g_Dir_Back;
    [SerializeField] private GameObject m_g_Dir_Right;
    [SerializeField] private GameObject m_g_Dir_Left;
    [SerializeField] private Vector3 m_v_Dir_Move;

    // Player_Move
    public E_MOVE_TYPE m_eMove_Type;
    public E_AIM_TYPE m_eAim_Type;
    private float m_fSpeed_Walk;
    private float m_fSpeed_Run;
    [SerializeField] private bool m_bInput_GetKey_Shift;
        
    [SerializeField] private E_MOVE_VALUE m_eCoordinate_z_Move_Value;
    [SerializeField] private E_MOVE_VALUE m_eCoordinate_x_Move_Value;

    // Player_Move Check Collider
    public Vector3 m_v_Player_Move_1;
    public bool m_b_Player_Move_1;
    Ray m_r_Player_Move_1;
    RaycastHit m_rh_Player_Move_1;

    public Vector3 m_v_Player_Move_2;
    public bool m_b_Player_Move_2;
    Ray m_r_Player_Move_2;
    RaycastHit m_rh_Player_Move_2;

    public Material m_mt_Material_Hit_true;  // m_r_Player_Move_1 와 접촉한 매쉬의 메테리얼
    public Material m_mt_Material_Hit_false; // m_r_Player_Move_1 와 접촉하지 않은 매쉬의 메테리얼
    public GameObject m_gm_Hit;              // m_r_Player_Move_1 와 접촉한 게임 오브젝트(매쉬를 가지고 있는)

    public static Player I_Player
    {
        get
        {
            if (S_Player == null)
            {
                return null;
            }
            else
                return S_Player;
        }
    }
    private void Awake()
    {
        if (S_Player == null)
        {
            S_Player = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(S_Player);
        }

        InitialSet_Player_Animation();
        InitialSet_Player_Ray();

        m_eMove_Type = E_MOVE_TYPE.P_IDLE;
        m_eAim_Type = E_AIM_TYPE.FALSE;

        m_fSpeed_Walk = 2f;
        m_fSpeed_Run = 4f;
        m_bInput_GetKey_Shift = false;

        m_eCoordinate_z_Move_Value = E_MOVE_VALUE.V_ZERO;
        m_eCoordinate_x_Move_Value = E_MOVE_VALUE.V_ZERO;

        m_v_Player_Move_1 = new Vector3(0, 0.2f, 0);
        m_v_Player_Move_2 = new Vector3(0, 0.4f, 0);

        m_g_Dir_Front = this.gameObject.transform.Find("Dir_Front").gameObject;
        m_g_Dir_Back = this.gameObject.transform.Find("Dir_Back").gameObject;
        m_g_Dir_Right = this.gameObject.transform.Find("Dir_Right").gameObject;
        m_g_Dir_Left = this.gameObject.transform.Find("Dir_Left").gameObject;

        //Time.timeScale = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // MOVE
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            m_eCoordinate_z_Move_Value = E_MOVE_VALUE.V_ZERO;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            m_eCoordinate_z_Move_Value = E_MOVE_VALUE.V_PLUS;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            m_eCoordinate_z_Move_Value = E_MOVE_VALUE.V_MINUS;
        }
        else
        {
            m_eCoordinate_z_Move_Value = E_MOVE_VALUE.V_ZERO;
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            m_eCoordinate_x_Move_Value = E_MOVE_VALUE.V_ZERO;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            m_eCoordinate_x_Move_Value = E_MOVE_VALUE.V_PLUS;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            m_eCoordinate_x_Move_Value = E_MOVE_VALUE.V_MINUS;
        }
        else
        {
            m_eCoordinate_x_Move_Value = E_MOVE_VALUE.V_ZERO;
        }

        // WALK / RUN
        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_bInput_GetKey_Shift = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            m_bInput_GetKey_Shift = false;
        }

        // AIM
        if (Input.GetMouseButton(1))
        {
            m_eAim_Type = E_AIM_TYPE.TRUE;
        }
        if (Input.GetMouseButtonUp(1))
        {
            m_eAim_Type = E_AIM_TYPE.FALSE;
        }

        // PUSH
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (m_eMove_Type != E_MOVE_TYPE.P_PUSH)// && m_eMove_Type != E_MOVE_TYPE.JUMP)
            {
                m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_PUSH);
            }
        }

        if (m_eMove_Type != E_MOVE_TYPE.P_PUSH)// && m_eMove_Type != E_MOVE_TYPE.JUMP)
            Player_Move();
        //Player_Move_Check_Collider();

        //m_vTest = Check_Ray_Down();
        //if (m_vTest.y != -1)
        //{
        //    this.transform.position -= m_vTest;
        //}
        this.transform.position += Check_Ray();
    }
    [SerializeField] Vector3 m_vTest;

    void Player_Move()
    {
        // 플레이어 애니메이션 변경 함수 + 플레이어 이동 함수
        if (m_eCoordinate_z_Move_Value == E_MOVE_VALUE.V_PLUS && m_eCoordinate_x_Move_Value == E_MOVE_VALUE.V_ZERO)
        {
            if (m_bInput_GetKey_Shift == true)
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN_AIM);

                this.transform.position += (m_g_Dir_Front.transform.position - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Run;
            }
            else
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_AIM);

                this.transform.position += (m_g_Dir_Front.transform.position - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Walk;
            }
        }
        if (m_eCoordinate_z_Move_Value == E_MOVE_VALUE.V_PLUS && m_eCoordinate_x_Move_Value == E_MOVE_VALUE.V_PLUS)
        {
            if (m_bInput_GetKey_Shift == true)
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN_AIM);

                this.transform.position += ((m_g_Dir_Front.transform.position + m_g_Dir_Right.transform.position) / 2 - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Run * 0.75f;
            }
            else
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_RIGHT);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_RIGHT_AIM);

                this.transform.position += ((m_g_Dir_Front.transform.position + m_g_Dir_Right.transform.position) / 2 - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Walk * 0.75f;
            }
        }
        if (m_eCoordinate_z_Move_Value == E_MOVE_VALUE.V_ZERO && m_eCoordinate_x_Move_Value == E_MOVE_VALUE.V_PLUS)
        {
            if (m_bInput_GetKey_Shift == true)
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN_AIM);

                this.transform.position += (m_g_Dir_Right.transform.position - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Run;
            }
            else
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_RIGHT);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_RIGHT_AIM);

                this.transform.position += (m_g_Dir_Right.transform.position - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Walk;
            }
        }
        if (m_eCoordinate_z_Move_Value == E_MOVE_VALUE.V_MINUS && m_eCoordinate_x_Move_Value == E_MOVE_VALUE.V_PLUS)
        {
            //if (m_bInput_GetKey_Shift == true)
            //{
            //    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN);
            //    this.transform.position += ((m_g_Dir_Back.transform.position + m_g_Dir_Right.transform.position) / 2 - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Run * 0.75f;
            //}
            //else
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_RIGHT);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_RIGHT_AIM);

                this.transform.position += ((m_g_Dir_Back.transform.position + m_g_Dir_Right.transform.position) / 2 - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Walk * 0.75f;
            }
        }
        if (m_eCoordinate_z_Move_Value == E_MOVE_VALUE.V_MINUS && m_eCoordinate_x_Move_Value == E_MOVE_VALUE.V_ZERO)
        {
            //if (m_bInput_GetKey_Shift == true)
            //{
            //    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN);
            //    this.transform.position += (m_g_Dir_Back.transform.position - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Run;
            //}
            //else
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_AIM);

                this.transform.position += (m_g_Dir_Back.transform.position - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Walk;
            }
        }
        if (m_eCoordinate_z_Move_Value == E_MOVE_VALUE.V_MINUS && m_eCoordinate_x_Move_Value == E_MOVE_VALUE.V_MINUS)
        {
            //if (m_bInput_GetKey_Shift == true)
            //{
            //    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN);
            //    this.transform.position += ((m_g_Dir_Back.transform.position + m_g_Dir_Left.transform.position) / 2 - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Run * 0.75f;
            //}
            //else
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_LEFT);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_LEFT_AIM);

                this.transform.position += ((m_g_Dir_Back.transform.position + m_g_Dir_Left.transform.position) / 2 - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Walk * 0.75f;
            }
        }
        if (m_eCoordinate_z_Move_Value == E_MOVE_VALUE.V_ZERO && m_eCoordinate_x_Move_Value == E_MOVE_VALUE.V_MINUS)
        {
            if (m_bInput_GetKey_Shift == true)
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN_AIM);

                this.transform.position += (m_g_Dir_Left.transform.position - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Run;
            }
            else
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_LEFT);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_LEFT_AIM);

                this.transform.position += (m_g_Dir_Left.transform.position - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Walk;
            }
        }
        if (m_eCoordinate_z_Move_Value == E_MOVE_VALUE.V_PLUS && m_eCoordinate_x_Move_Value == E_MOVE_VALUE.V_MINUS)
        {
            if (m_bInput_GetKey_Shift == true)
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN);
                else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_RUN_AIM);

                this.transform.position += ((m_g_Dir_Front.transform.position + m_g_Dir_Left.transform.position) / 2 - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Run * 0.75f;
            }
            else
            {
                if (m_eAim_Type == E_AIM_TYPE.FALSE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_LEFT);
                else if  (m_eAim_Type == E_AIM_TYPE.TRUE)
                    m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_WALK_LEFT_AIM);

                this.transform.position += ((m_g_Dir_Front.transform.position + m_g_Dir_Left.transform.position) / 2 - this.transform.position).normalized * Time.deltaTime * m_fSpeed_Walk * 0.75f;
            }
        }
        if (m_eCoordinate_z_Move_Value == E_MOVE_VALUE.V_ZERO && m_eCoordinate_x_Move_Value == E_MOVE_VALUE.V_ZERO)
        {
            if (m_eAim_Type == E_AIM_TYPE.FALSE)
                m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_IDLE);
            else if (m_eAim_Type == E_AIM_TYPE.TRUE)
                m_eMove_Type = Set_PlayerMoveType(E_MOVE_TYPE.P_IDLE_AIM);
        }
    }
    void Player_Move_Check_Collider()
    {
        //m_v_Dir_Front = this.gameObject.transform.Find("Dir_Front").gameObject.transform.position;

        m_r_Player_Move_1 = new Ray(this.transform.position + m_v_Player_Move_1, transform.forward);
        m_r_Player_Move_2 = new Ray(this.transform.position + m_v_Player_Move_2, transform.forward);

        m_b_Player_Move_1 = Physics.Raycast(m_r_Player_Move_1, out m_rh_Player_Move_1, 1f, 1 << LayerMask.NameToLayer("Ground"));
        m_b_Player_Move_2 = Physics.Raycast(m_r_Player_Move_2, out m_rh_Player_Move_2, 2f, 1 << LayerMask.NameToLayer("Ground"));

        //if (m_gm_Hit != null)
        //    m_gm_Hit.GetComponent<MeshRenderer>().material = m_mt_Material_Hit_false;

        if (m_b_Player_Move_1 == true && m_b_Player_Move_2 == true)
        {
            //Debug.Log(m_rh_Player_Move_1.transform.gameObject.name);
            m_gm_Hit = m_rh_Player_Move_1.transform.gameObject;
            //m_rh_Player_Move_1.transform.gameObject.GetComponent<MeshRenderer>().material = m_mt_Material_Hit_true;
        }
        else
        {

        }
    }

    public Vector3 Get_PlayerDir()
    {
        return (m_g_Dir_Front.transform.position - this.transform.position).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
    }

    //private void OnDrawGizmos()
    //{
    //    if (m_b_Player_Move_1 == true)
    //    {
    //        Gizmos.color = Color.red;
    //        //Gizmos.DrawRay(m_r_Player_Move_1);
    //        Gizmos.DrawRay(this.transform.position + m_v_Player_Move_1, transform.forward);
    //    }
    //    else
    //    {
    //        Gizmos.color = Color.white;
    //        //Gizmos.DrawRay(m_r_Player_Move_1);
    //        Gizmos.DrawRay(this.transform.position + m_v_Player_Move_1, transform.forward);
    //    }

    //    if (m_b_Player_Move_2 == true)
    //    {
    //        Gizmos.color = Color.red;
    //        //Gizmos.DrawRay(m_r_Player_Move_2);
    //        Gizmos.DrawRay(this.transform.position + m_v_Player_Move_2, (transform.forward) * 2);
    //    }
    //    else
    //    {
    //        Gizmos.color = Color.white;
    //        //Gizmos.DrawRay(m_r_Player_Move_2);
    //        Gizmos.DrawRay(this.transform.position + m_v_Player_Move_2, (transform.forward) * 2);
    //    }
    //}
}
