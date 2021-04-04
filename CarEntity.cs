using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEntity : MonoBehaviour
{
    //hello
    
    public GameObject wheelFR;
    public GameObject wheelFL;
    public GameObject wheelBL;
    public GameObject wheelBR;
    float m_FrontWheelAngle = 0;
    const float WHEEL_ANGLE_LIMIT = 40f;
    public float turnAngularVelocity = 20f;

    float m_Velocity;
    public float acceleraion = 3f;
    public float deceleraion = 10f;
    public float maxVelocity = 60f;
    float m_DeltaMovement;
    float CarLength = 1f;
    [SerializeField] SpriteRenderer[] m_Renderers = new SpriteRenderer[5];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_Velocity = Mathf.Min(maxVelocity, m_Velocity + Time.fixedDeltaTime * acceleraion);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_Velocity = Mathf.Max(-0.5f* maxVelocity, m_Velocity - Time.fixedDeltaTime * deceleraion);
        }

        m_DeltaMovement = m_Velocity * Time.fixedDeltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_FrontWheelAngle = Mathf.Clamp(
                m_FrontWheelAngle + Time.fixedDeltaTime * turnAngularVelocity, -WHEEL_ANGLE_LIMIT, WHEEL_ANGLE_LIMIT);
            UpdateWheel();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_FrontWheelAngle = Mathf.Clamp(
                m_FrontWheelAngle - Time.fixedDeltaTime * turnAngularVelocity, -WHEEL_ANGLE_LIMIT, WHEEL_ANGLE_LIMIT);
            UpdateWheel();
        }
        this.transform.Rotate(0f, 0f, 1 / CarLength * Mathf.Tan(Mathf.Deg2Rad * m_FrontWheelAngle) * Mathf.Rad2Deg * m_DeltaMovement);
        this.transform.Translate(Vector3.right * m_DeltaMovement);
    }
    void UpdateWheel()
    {
        Vector3 localEulerAngles = new Vector3(0f, 0f, m_FrontWheelAngle);
        wheelFR.transform.localEulerAngles = localEulerAngles;
        wheelFL.transform.localEulerAngles = localEulerAngles;
    }
    void ResetColor()
    {
        ChangeColor(Color.white);
    }
    void ChangeColor(Color color)
    {
        foreach(SpriteRenderer r in m_Renderers)
        {
            r.color = color;
        }
    }
    void Stop()
    {
        m_Velocity = 0;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Stop();
        ChangeColor(Color.red);
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        Stop();
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        ResetColor();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        CheckPoint checkPoint = collision.gameObject.GetComponent<CheckPoint>();
        if (checkPoint != null)
        {
            ChangeColor(Color.green);
            this.Invoke("ResetColor", 0.9f);
        }
    }
}
