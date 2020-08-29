using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [HideInInspector]public bool isOpen;
    private int objectsInRange;
    private Animator animator;
    [SerializeField] private BoxCollider2D boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Projectile") && collision.gameObject.layer != LayerMask.NameToLayer("NonPlayerCollider") && collision.gameObject.layer != LayerMask.NameToLayer("BulletShell"))
        {
            objectsInRange++;
            animator.SetBool("isOpen", true);
            Debug.Log(animator.GetBool("isOpen"));
            boxCollider2D.enabled = false;
            isOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Projectile") && collision.gameObject.layer != LayerMask.NameToLayer("NonPlayerCollider") && collision.gameObject.layer != LayerMask.NameToLayer("BulletShell"))
        {
            objectsInRange--;
            if (objectsInRange == 0)
            {
                animator.SetBool("isOpen", false);
                boxCollider2D.enabled = true;
                isOpen = false;
            }
        }
    }
}
