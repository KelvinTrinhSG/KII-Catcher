using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(GameTag.Bottle.ToString()))
        {
            GameManager.Ins.Gameover();
            Destroy(col.gameObject);
        }
    }
}
