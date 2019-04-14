using UnityEngine;
using System.Collections;
using creXa.GameBase;

public class TESTING : MonoBehaviour {

    [IntRange(1, 10)]
    public IntRange inttest;

    [FloatRange(1, 10)]
    public FloatRange floattest;

    [ReadOnly]
    [SerializeField] float time = 3.0f; 

    [Angle]
    public float angle;

    public AnimationCurve curve;

    public ZTBezier b;
    public GameObject obj;

    public RandomInt x;
    public RandomFloat y;

    float t = 0;

    public ZText text;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

        t += Time.deltaTime * 0.1f;
        if (t > 1) t = 0;
        obj.transform.position = b.GetPointAt(t);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ZThemeSys.It.Theme < 2)
                ZThemeSys.It.Theme++;
            else
                ZThemeSys.It.Theme = 0;

            text.gameObject.SetActive(!text.gameObject.activeSelf);
        }

    }
}
