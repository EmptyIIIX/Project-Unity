using UnityEngine;

public class TestDash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private DashGhost dashGhost;

    void Start()
    {
        dashGhost = GetComponent<DashGhost>();
    }

    // ตอนเริ่ม Dash
    //dashGhost?.StartGhost();

    // ตอน Dash จบ
    //dashGhost?.StopGhost();
}
