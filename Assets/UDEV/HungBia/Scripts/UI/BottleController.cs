using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    public float speed;
    public int minScore;
    public int maxScore;
    private Rigidbody2D m_rb;
    public int ScoreBonus { get => Random.Range(minScore, maxScore); }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        if (!m_rb) return;
        m_rb.velocity = Vector2.down * speed;
    }
}
