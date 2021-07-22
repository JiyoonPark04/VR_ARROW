using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float m_Speed = 2000.0f;
    public Transform m_Tip = null;

    private Rigidbody m_Rigidbody = null;
    private bool m_IsStopped = true;
    private Vector3 m_LastPosition = Vector3.zero;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        if (m_IsStopped)
            return;

        // Rotate
        m_Rigidbody.MoveRotation(Quaternion.LookRotation(m_Rigidbody.velocity, transform.up));
        //rotation으로 Rigidbody를 회전한다
        //public void MoveRotation(Quaternion rot);

        // Collision
        if(Physics.Linecast(m_LastPosition,m_Tip.position))
        {
            Stop();
        }

        // Store Position
        m_LastPosition = m_Tip.position;
    }

    private void Stop()
    {
        m_IsStopped = true;
        m_Rigidbody.isKinematic = true;
        m_Rigidbody.useGravity = false;
    }

    public void Fire(float pullValue)
    {
        m_IsStopped = false;
        transform.parent = null;

        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.AddForce(transform.forward * (pullValue * m_Speed));

        //5초뒤에 이미 쏘아진 화살이 사라지게 함
        Destroy(gameObject, 5.0f);
    }
}
