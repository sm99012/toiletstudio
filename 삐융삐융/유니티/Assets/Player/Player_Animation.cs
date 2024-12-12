using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player_Animation
public partial class Player : MonoBehaviour
{
    // 플레이어
    private Animator m_Animator_Player_Rifle;

    // 라이플
    // 라이플 - K2
    private Animator m_Animator_Rifle_K2;

    public void InitialSet_Player_Animation()
    {
        m_Animator_Player_Rifle = this.gameObject.GetComponent<Animator>();

        m_Animator_Rifle_K2 = this.gameObject.transform.Find("Rifle_K2").gameObject.GetComponent<Animator>();
    }

    // 플레이어 애니메이션 변경 함수
    private E_PLAYER_MOVE_TYPE Set_PlayerMoveType(E_PLAYER_MOVE_TYPE emt)
    {
        if (m_eMove_Type != emt)
        {
            switch (emt)
            {
                case E_PLAYER_MOVE_TYPE.P_IDLE:
                    {
                        Set_m_Player_Rifle_Animator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                        Set_Rifle_K2_Animator_Parameters("K2_Idle", 0);
                        //float startingtime = m_Animator_Player_Rifle.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    }
                    break;
                case E_PLAYER_MOVE_TYPE.P_IDLE_AIM:
                    {
                        Set_m_Player_Rifle_Animator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                        Set_Rifle_K2_Animator_Parameters("K2_Idle_Aim", 0);
                        //float startingtime = m_Animator_Player_Rifle.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    }
                    break;
                case E_PLAYER_MOVE_TYPE.P_WALK:
                    {
                        if (m_eAim_Type == E_AIM_TYPE.FALSE)
                        {
                            Set_m_Player_Rifle_Animator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                            Set_Rifle_K2_Animator_Parameters("K2_Walk", 0);
                        }
                    }
                    break;
                case E_PLAYER_MOVE_TYPE.P_WALK_AIM:
                    {
                        Set_m_Player_Rifle_Animator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                        //Set_Rifle_K2_Animator_Parameters("K2_Walk_Aim", 0);
                        m_Animator_Rifle_K2.Play("K2_Walk_Aim");
                    }
                    break;
                case E_PLAYER_MOVE_TYPE.P_WALK_LEFT:
                    {
                        if (m_eAim_Type == E_AIM_TYPE.FALSE)
                        {
                            Set_m_Player_Rifle_Animator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                            Set_Rifle_K2_Animator_Parameters("K2_Walk_Left", 0);
                        }
                    }
                    break;
                case E_PLAYER_MOVE_TYPE.P_WALK_LEFT_AIM:
                    {
                        Set_m_Player_Rifle_Animator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                        Set_Rifle_K2_Animator_Parameters("K2_Walk_Left_Aim", 0);
                    }
                    break;
                case E_PLAYER_MOVE_TYPE.P_WALK_RIGHT:
                    {
                        if (m_eAim_Type == E_AIM_TYPE.FALSE)
                        {
                            Set_m_Player_Rifle_Animator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                            Set_Rifle_K2_Animator_Parameters("K2_Walk_Right", 0);
                        }
                    }
                    break;
                case E_PLAYER_MOVE_TYPE.P_WALK_RIGHT_AIM:
                    {
                        Set_m_Player_Rifle_Animator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                        Set_Rifle_K2_Animator_Parameters("K2_Walk_Right_Aim", 0);
                    }
                    break;
                case E_PLAYER_MOVE_TYPE.P_RUN:
                    {
                        if (m_eAim_Type == E_AIM_TYPE.FALSE)
                        {
                            Set_m_Player_Rifle_Animator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                            Set_Rifle_K2_Animator_Parameters("K2_Run", 0);
                        }
                    }
                    break;
                case E_PLAYER_MOVE_TYPE.P_RUN_AIM:
                    {
                        Set_m_Player_Rifle_Animator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                        Set_Rifle_K2_Animator_Parameters("K2_Run_Aim", 0);
                    }
                    break;
                case E_PLAYER_MOVE_TYPE.P_PUSH:
                    {
                        Set_m_Player_Rifle_Animator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                        Set_Rifle_K2_Animator_Parameters("K2_Push", 0);
                    }
                    break;
                    //case E_PLAYER_MOVE_TYPE.P_JUMP:
                    //    {
                    //        Set_PlayerAnimator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                    //        Set_Rifle_K2_Animator_Parameters("K2_Jump", 0);
                    //    }
                    //    break;
                    //case E_PLAYER_MOVE_TYPE.P_PUSH:
                    //    {
                    //        Set_PlayerAnimator_Parameters(emt.ToString(), m_eMove_Type.ToString());
                    //        Set_Rifle_K2_Animator_Parameters("K2_Push", 0);
                    //    }
                    //    break;
            }

            return emt;
        }
        else
            return m_eMove_Type;
    }
    // 플레이어 애니메이션 변경
    private void Set_m_Player_Rifle_Animator_Parameters(string sparameter_true, string sparameter_false)
    {
        m_Animator_Player_Rifle.SetBool(sparameter_true, true);
        m_Animator_Player_Rifle.SetBool(sparameter_false, false);
    }
    // 총기 애니메이션 변경
    private void Set_Rifle_K2_Animator_Parameters(string sparameter, float fstarttime)
    {
        m_Animator_Rifle_K2.Play(sparameter, 0, fstarttime);
    }

    // 플레이어 푸쉬 해제
    private void End_Player_Push()
    {
        m_eMove_Type = Set_PlayerMoveType(E_PLAYER_MOVE_TYPE.P_IDLE);
    }
    // 플레이어 점프 해제
}
