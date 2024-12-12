using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Monster_MO_01 : Monster
{
    override protected void Set_Collider()
    {
        // Monster_Body
        m_fa_Collider_X_Body[0] = this.gameObject.transform.position.x - 0.25f;
        m_fa_Collider_X_Body[1] = this.gameObject.transform.position.x + 0.25f;

        m_fa_Collider_Y_Body[0] = this.gameObject.transform.position.y;
        m_fa_Collider_Y_Body[1] = this.gameObject.transform.position.y + 1f;

        m_fa_Collider_Z_Body[0] = this.gameObject.transform.position.z - 0.25f;
        m_fa_Collider_Z_Body[1] = this.gameObject.transform.position.z + 0.25f;

        // Monster_Head
        m_fa_Collider_X_Head[0] = this.gameObject.transform.position.x - 0.1f;
        m_fa_Collider_X_Head[1] = this.gameObject.transform.position.x + 0.1f;

        m_fa_Collider_Y_Head[0] = this.gameObject.transform.position.y + 1f;
        m_fa_Collider_Y_Head[1] = this.gameObject.transform.position.y + 1.25f;

        m_fa_Collider_Z_Head[0] = this.gameObject.transform.position.z + 0.1f;
        m_fa_Collider_Z_Head[1] = this.gameObject.transform.position.z + 0.3f;
    }
}
