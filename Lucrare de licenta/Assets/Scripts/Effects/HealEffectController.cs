using UnityEngine;

public class HealEffectController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
