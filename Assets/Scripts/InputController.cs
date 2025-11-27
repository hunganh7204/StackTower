using UnityEngine;

public class InputController : MonoBehaviour
{
    void Update()
    {
        if (GameManager.instance.isGameOver) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Transform cur = GameManager.instance.currentBlock;
            if (cur != null)
            {
                BlockController bc = cur.GetComponent<BlockController>();
                if (bc != null)
                {
                    bc.Drop();
                }
            }
        }
    }
}