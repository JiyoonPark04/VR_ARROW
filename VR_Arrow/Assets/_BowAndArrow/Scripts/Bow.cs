using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Assets")]
    public GameObject m_ArrowPrefab = null;

    [Header("Bow")]
    public float m_GrabThreshold = 0.15f;
    public Transform m_Start = null;
    public Transform m_End = null;
    public Transform m_Socket = null;

    private Transform m_PullingHand = null;
    private Arrow m_CurrentArrow = null;
    private Animator m_Animator = null;

    private float m_PullValue = 0.0f;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(CreatArrow(0.0f));
        // public Coroutine StartCoroutine(IEnumerator routine);
        // 코루틴을 시작한다
        // 코루틴이란 실행을 중지하여 유니티에 제어권을 돌려주고, 그러나 계속할 때는 다음 프레임에서 중지한 곳부터
        //실행을 계속 할 수 있는 기능이다.
    }

    private void Update()
    {
        if (!m_PullingHand || !m_CurrentArrow)
            return;

        m_PullValue = CalculatePull(m_PullingHand);
        m_PullValue = Mathf.Clamp(m_PullValue, 0.0f, 1.0f);
        //public static float Clamp(float value, float min, float max);
        //최대,최소값 사이의 float값이 value 범위 외의 값이 되지 않도록 합니다.

        m_Animator.SetFloat("Blend", m_PullValue);
        //public void SetFloat(string propertyName, float value);
        //프로퍼티 이름에서 float 값을 설정합니다.
    }

    private float CalculatePull(Transform pullHand)
    {
        Vector3 direction = m_End.position - m_Start.position;
        float magnitude = direction.magnitude;

        direction.Normalize();
        Vector3 difference = pullHand.position - m_Start.position;

        return Vector3.Dot(difference, direction) / magnitude;
        //public static float Dot(Vector3 lhs, Vector3 rhs);
        //두 벡터의 내적.
    }

    private IEnumerator CreatArrow(float waitTime)
    {
        // Wait
        yield return new WaitForSeconds(waitTime);

        // Creat, child
        GameObject arrowObject = Instantiate(m_ArrowPrefab, m_Socket);

        // Orient
        arrowObject.transform.localPosition = new Vector3(0, 0, 0.425f);
        arrowObject.transform.localEulerAngles = Vector3.zero;

        // Set
        m_CurrentArrow = arrowObject.GetComponent<Arrow>();
    }

    public void Pull(Transform hand)
        //hand position 과 start position 사이 간격 체크
    {
        float distance = Vector3.Distance(hand.position, m_Start.position);

        if (distance > m_GrabThreshold)
            return;

        m_PullingHand = hand;
    }

    public void Release()
    {
        if (m_PullValue > 0.25f)
            FireArrow();

        m_PullingHand = null;

        m_PullValue = 0.0f;
        m_Animator.SetFloat("Blend", 0);

        //Creat new arrow
        if (!m_CurrentArrow)
            StartCoroutine(CreatArrow(0.25f));
    }
    private void FireArrow()
    {
        m_CurrentArrow.Fire(m_PullValue);
        m_CurrentArrow = null;
    }

}
