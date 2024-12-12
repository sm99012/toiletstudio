using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Monster_Animation
public partial class Monster : MonoBehaviour
{
    private Animator m_Animator_Monster;

    public void InitialSet_Monster_Animation()
    {
        m_Animator_Monster = this.gameObject.GetComponent<Animator>();
    }

    // 몬스터 애니메이션 변경 함수
    virtual public E_MONSTER_MOVE_TYPE Set_MonsterMoveType(E_MONSTER_MOVE_TYPE emt, float fanimationspeed)
    {
        if (m_eMove_Type != emt)
        {
            switch (emt)
            {
                case E_MONSTER_MOVE_TYPE.M_WALK:
                    {
                        Set_Monster_Animator_Parameters(emt.ToString(), fanimationspeed); // fanimationspeed = 0.5f;
                    }
                    break;
                case E_MONSTER_MOVE_TYPE.M_ATTACK:
                    {
                        Set_Monster_Animator_Parameters(emt.ToString(), fanimationspeed); // fanimationspeed = 1f;
                    }
                    break;
                case E_MONSTER_MOVE_TYPE.M_ATTACKED:
                    {
                        Set_Monster_Animator_Parameters(emt.ToString(), fanimationspeed); // fanimationspeed = 1.5f;
                    }
                    break;
                case E_MONSTER_MOVE_TYPE.M_DEATH:
                    {
                        Set_Monster_Animator_Parameters(emt.ToString(), fanimationspeed); // fanimationspeed = 1f;
                    }
                    break;
            }

            return emt;
        }
        else
            return m_eMove_Type;
    }

    // 총기 애니메이션 변경
    virtual protected void Set_Monster_Animator_Parameters(string sparameter, float fanimationspeed)
    {
        m_Animator_Monster.Play(sparameter, 0, 0);
        m_Animator_Monster.speed = fanimationspeed;
    }
}
