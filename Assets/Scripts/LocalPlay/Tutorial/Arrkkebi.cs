using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrkkebi : MonoBehaviour
{
    public static GameObject Target;

    private void Update()
    {
        if (Target != null)
        {
            Vector2 newvec = Target.transform.position - gameObject.transform.position;
            float angle = Vector2.Angle(Vector2.down, newvec.normalized);
            if(newvec.x > 0)
            {
                angle = -angle;
            }
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90 - angle));
        } else
        {
            gameObject.SetActive(false);
        }
    }

    float GetAngle(Vector2 start, Vector2 end)
    {

        float dot = Vector3.Dot(start, end);
        float mag = Vector3.Magnitude(start) * Vector3.Magnitude(end);
        float angle = (dot / mag) * Mathf.Rad2Deg;
        return angle;
    }
}
