using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float intensity;
    public float smooth;
    public float aimDamping;
    private Quaternion origin_rotation;
    // Start is called before the first frame update
    private void Awake()
    {
        origin_rotation = transform.localRotation;
    }

    // Update is called once per frame
    private void Update()
    {
        Sway();
    }

    private void Sway()
    {
        if (Input.GetMouseButton(1))
        {
            float t_x_mouse = Input.GetAxis("Mouse X");
            float t_y_mouse = Input.GetAxis("Mouse Y");

            Quaternion t_x_adj = Quaternion.AngleAxis(-intensity * t_x_mouse * 1/aimDamping, Vector3.up);
            Quaternion t_y_adj = Quaternion.AngleAxis(intensity * t_y_mouse * 1/aimDamping * 3, Vector3.right);
            Quaternion target_rotation = t_x_adj * t_y_adj;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);
        }

        else
        {
            float t_x_mouse = Input.GetAxis("Mouse X");
            float t_y_mouse = Input.GetAxis("Mouse Y");

            Quaternion t_x_adj = Quaternion.AngleAxis(-intensity * t_x_mouse, Vector3.up);
            Quaternion t_y_adj = Quaternion.AngleAxis(intensity * t_y_mouse * 3, Vector3.right);
            Quaternion target_rotation = origin_rotation * t_x_adj * t_y_adj;

            transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);
        }
    }
}
