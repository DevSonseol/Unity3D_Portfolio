using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public float destroyTime = 1f;

    private void OnEnable()
    {
        Invoke("DestroySelf", destroyTime);
    }

    void DestroySelf()
    {
        Main main = Main.Instance;
        main.Destroy(this.gameObject);
    }
}
