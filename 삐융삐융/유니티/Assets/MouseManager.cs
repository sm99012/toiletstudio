using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseManager : MonoBehaviour
{
    [SerializeField] Vector3 m_vMouse_Position;
    [SerializeField] Vector2 m_vMouse_Position_Center;
    [SerializeField] bool m_bMouse_Fix;
    [SerializeField] int m_nCenter_Coordinate_x;
    [SerializeField] int m_nCenter_Coordinate_y;

    [SerializeField] int m_nFixValue_x;
    [SerializeField] int m_nFixValue_y;

    public float m_fSensitivity_x; // 마우스 감도
    public float m_fSensitivity_y; // 마우스 감도

    private void Awake()
    {
        m_nCenter_Coordinate_x = Screen.width / 2;
        m_nCenter_Coordinate_y = Screen.height / 2;

        m_bMouse_Fix = true;
        Cursor.visible = false;

        m_nFixValue_x = Screen.width / 10;
        m_nFixValue_y = Screen.height / 10;
        m_vMouse_Position_Center = new Vector2(m_nCenter_Coordinate_x, m_nCenter_Coordinate_y);

        m_fSensitivity_x = 10;
        m_fSensitivity_y = 10;
    }
    private void Start()
    {
        //Debug.Log("Screen Size : " + Screen.width + ", " + Screen.height);
    }
    void Update()
    {
        if (m_bMouse_Fix == true)
        {
            m_vMouse_Position = Input.mousePosition;

            Player.I_Player.transform.Rotate(Vector3.up * ((m_vMouse_Position.x - m_nCenter_Coordinate_x) / 360) * m_fSensitivity_x);

            CameraManager.I_CameraManager.Update_Camera_Transform_z((m_nCenter_Coordinate_y - m_vMouse_Position.y) * m_fSensitivity_y);

            Mouse.current.WarpCursorPosition(m_vMouse_Position_Center);
            //Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_bMouse_Fix = !m_bMouse_Fix;
            Cursor.visible = !Cursor.visible;
        }
    }
}
