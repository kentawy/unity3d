using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public sealed class AnimationFxDemo : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitBurst;
    [SerializeField] private ParticleSystem hitBurstLocal; 
    private Animator animator;
    private bool moving;
    private static readonly int MovingId = Animator.StringToHash("Moving");
    private static readonly int HitId = Animator.StringToHash("Hit");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;
        
        if (keyboard.mKey.wasPressedThisFrame)
        {
            moving = !moving;
            animator.SetBool(MovingId, moving);
        }
        
        bool busy = animator.IsInTransition(0) || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit");
        if (keyboard.hKey.wasPressedThisFrame && !busy)
            animator.SetTrigger(HitId);
    }

    public void EmitHitBurst()
    {
        if (hitBurst != null) 
        {
            hitBurst.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            hitBurst.Play(true);
        }

        if (hitBurstLocal != null)
        {
            hitBurstLocal.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            hitBurstLocal.Play(true);
        }
    }
}