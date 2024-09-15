using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StickController : MonoBehaviour
{
    public float speed;
    private float m_curSpeed;
    private float m_limitOffset;
    private float m_minX;
    private float m_maxX;
    private Vector2 m_startingPos;
    private Rigidbody2D m_rb;
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_startingPos = transform.position;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        GetLimitPos();
    }

    private void FixedUpdate()
    {
        if (!GamepadMng.Ins || !m_rb || GameManager.Ins.gameState != GameState.Playing) return;
        if (GamepadMng.Ins.CanMoveLeft)
        {
            m_curSpeed = speed * -1;
        }
        else if (GamepadMng.Ins.CanMoveRight)
        {
            m_curSpeed = speed;
        }
        else
        {
            m_curSpeed = 0;
        }
        m_rb.velocity = Vector2.right * m_curSpeed;
    }

    private void GetLimitOffset()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (!collider) return;
        m_limitOffset = collider.bounds.size.x / 2 + 0.15f;
    }

    private void GetLimitPos()
    {
        GetLimitOffset();
        m_minX = ViewPortUtil.MinX + m_limitOffset;
        m_maxX = ViewPortUtil.MaxX - m_limitOffset;
    }

    private void Update()
    {
        float limitPosX = Mathf.Clamp(transform.position.x, m_minX, m_maxX);
        transform.position = new Vector3(limitPosX, m_startingPos.y, 0f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(GameTag.Bottle.ToString()) && GameManager.Ins.gameState == GameState.Playing)
        {
            var bottleCtr = col.GetComponent<BottleController>();
            if (!bottleCtr) return;
            GameManager.Ins.AddScore(bottleCtr.ScoreBonus);
            Destroy(bottleCtr.gameObject);
        }
    }
}
