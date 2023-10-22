using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellMarker : MonoBehaviour
{

    public void InitMarker(float radius)
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    void Update()
    {
        MoveToMousePosition();
    }

    void MoveToMousePosition()
    {
        RaycastHit hit;
        Main main = Main.Instance;
        Ray ray = main.mainCam.ScreenPointToRay(Input.mousePosition);
        LayerMask layerMask = LayerMask.NameToLayer("Ground");
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f);

        if (raycastHit)
        {
            Vector3 vec = new Vector3(0, 0.1f, 0);
            this.gameObject.transform.position = hit.point + vec;
        }
    }

    private void OnDrawGizmos()
    {
        RaycastHit hit;
        Main main = Main.Instance;
        Ray ray = main.mainCam.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(main.mainCam.transform.position, hit.point);
    }
}
