using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Vector3 position;
    public Quaternion rotation;
    public bool move;

    public RenderTexture render;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
        }
    }
}
