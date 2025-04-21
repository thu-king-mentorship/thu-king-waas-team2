using UnityEngine;
public class GolemPart : MonoBehaviour
{
    public GolemDisassembler golemRoot;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lighter"))
        {
            golemRoot.OnHit();
        }
    }
}
