using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public GameObject GreyBlock;

    [Range(1,10)]
    public int length = 10;

    [Range(1, 10)]
    public int height = 10;

    void Start()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                GameObject cube = Instantiate(GreyBlock) as GameObject;
                //cube.AddComponent<Rigidbody>();
                cube.transform.position = new Vector3(x, y, 1);
                //cube.GetComponentInChildren<Animator>().enabled = false;
            }
        }
    }

}
