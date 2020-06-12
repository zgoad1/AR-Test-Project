using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour {

    public Animator anim;

    private Vector3 prevPos;
    private float tick_NeckIdle = 5, tick_TailIdle = 30;

    void Reset() {
        anim = GetComponentInChildren<Animator>();
    }

    private void AnimationUpdate() {
        float speed = (transform.position - prevPos).sqrMagnitude;
        anim.SetFloat("Speed", speed);
        prevPos = transform.position;

        // If we're not moving, do idle tick
        if(speed < 0.001f) {
            // Neck idle loop
            tick_NeckIdle -= Time.deltaTime;
            if(tick_NeckIdle <= 0) {
                int index = Random.Range(1, 5);
                anim.CrossFade("Armature|Idle - look around " + index, 0, 1, 0);
                tick_NeckIdle = Random.Range(8f, 16f);
            }
        } else {
            if(!anim.IsInTransition(1)) {
                anim.CrossFade("Empty", 0.1f, 1, 0);
            }
        }
    }

    private void TailAnimationUpdate() {
        // Tail idle loop
        tick_TailIdle -= Time.deltaTime;
        if(tick_TailIdle <= 0) {
            anim.CrossFade("Armature|Idle - tail flick", 0, 2, 0);
            tick_TailIdle = Random.Range(10f, 40f);
        }
    }

    void Update() {
        AnimationUpdate();
        TailAnimationUpdate();
        prevPos = transform.position;
    }
}
