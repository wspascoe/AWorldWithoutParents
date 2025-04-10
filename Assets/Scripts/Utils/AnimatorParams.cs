using UnityEngine;

public static class AnimatorParams
{
    public static int Vertical = Animator.StringToHash("Vertical");
    public static int Horizontal = Animator.StringToHash("Horizontal");
    public static int IsFiring = Animator.StringToHash("IsFiring");
    public static int IsAiming = Animator.StringToHash("IsAiming");
    public static int Death = Animator.StringToHash("Death");
    public static int Speed = Animator.StringToHash("Speed");
    public static int Grounded = Animator.StringToHash("Grounded");
    public static int Jump = Animator.StringToHash("Jump");
    public static int FreeFall = Animator.StringToHash("FreeFall");
    public static int Throw = Animator.StringToHash("Throw");
}