using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    [SerializeField]
    public Transform Mid;
    [SerializeField]
    float Spring, LerpSpeed;


    private void Update()
    {
        SpringLerp(transform, Mid, ref NEXT_POSITION, Spring, LerpSpeed);
    }
    private Vector3 NEXT_POSITION;
    void SpringLerp(Transform OBJECT_TO_SPRING_LERP, Transform ENDPOINT, ref Vector3 CACHED_POSITION, float SPRING_MULTIPLIER, float LERP_SPEED)
    {
        CACHED_POSITION = Vector3.LerpUnclamped(CACHED_POSITION, (ENDPOINT.position - OBJECT_TO_SPRING_LERP.position) * SPRING_MULTIPLIER, Time.deltaTime * LERP_SPEED);
        OBJECT_TO_SPRING_LERP.position += CACHED_POSITION;
    }
}

  