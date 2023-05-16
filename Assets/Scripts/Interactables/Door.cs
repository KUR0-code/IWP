using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    private float Speed = 1f;
    [SerializeField]
    private Vector3 SlideDirection = Vector3.back;
    [SerializeField]
    private float slideAmount = 4.4f;
    private Vector3 startPosition;

    private Coroutine AnimationCoroutine;
    // Start is called before the first frame update
    public void open(Vector3 userPosition)
    {
        AnimationCoroutine = StartCoroutine(DoSlidingOpen());
    }

    public void close(Vector3 userPosition)
    {
        AnimationCoroutine = StartCoroutine(DoSlidingClose());
    }

    private IEnumerator DoSlidingClose()
    {
        Vector3 endPosition = startPosition;
        Vector3 StartPosition = transform.position;
        float time = 0;
        isOpen = false;
        while (time <1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    private IEnumerator DoSlidingOpen()
    {
        Vector3 endPosition = startPosition + slideAmount * SlideDirection;
        Vector3 StartPosition = transform.position;
        float time = 0;
        isOpen = true;

        while (time < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
