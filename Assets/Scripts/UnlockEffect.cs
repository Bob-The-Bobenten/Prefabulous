using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockEffect : MonoBehaviour
{
    public Animator screenAnimatorHit;
    public Animator screenAnimatorClimb;

    // Call this to play the animation
    public void PlayDeathAnimation()
    {
        screenAnimatorHit.SetTrigger("Play");
    }

    public void PlayClimbAnimation()
    {
        screenAnimatorHit.SetTrigger("PlayClimb");
    }
}
