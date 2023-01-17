using UnityEngine;

public class Circle : MonoBehaviour
{ 
    public static float rotationCircleSpeed = 140.0f;
    public static float incremetnRotationSpeed = 40.0f;
    public static bool isNotTween = true;
    public static float rotationTime = 3.0f;
    public static float decremetnRotationTime = 0.2f;

    void Start()
    {
        int circleMode = BallHandler.circleNumber % 4;

        iTween.MoveTo(base.gameObject, iTween.Hash(new object[]
        {
            "y",
            0,
            "easetype",
            iTween.EaseType.easeInOutQuad,
            "time",
            0.6,
        }));

        switch (circleMode)
        {
            case 0:
                isNotTween = false;
                RotateCircle();              
                break;    
            case 1:
            case 3:
                isNotTween = true;
                RotateCircle1();            
                break;
            case 2:
                isNotTween = false;
                RotateCircle2();               
                break;          
        }
        
    }

    void Update()
    {
        if(isNotTween)
            transform.Rotate(Vector3.up * Time.deltaTime * rotationCircleSpeed);
    }

    void RotateCircle()
    {
        rotationTime -= decremetnRotationTime;
        iTween.RotateBy(base.gameObject, iTween.Hash(new object[]
        {
            "y",
            0.8f,
            "time",
            rotationTime,
            "easeType",
            iTween.EaseType.easeInOutQuad,
            "loopType",
            iTween.LoopType.pingPong,
            "Delay",
            0.4f
        }));
    }

    void RotateCircle1()
    {
        rotationCircleSpeed *= -1;
        if(rotationCircleSpeed > 0)
            rotationCircleSpeed += incremetnRotationSpeed;
        else
            rotationCircleSpeed -= incremetnRotationSpeed;
    }

    void RotateCircle2()
    {
        rotationTime -= decremetnRotationTime;
        iTween.RotateBy(base.gameObject, iTween.Hash(new object[]
        {
            "y",
            0.75f,
            "time",
            rotationTime,
            "easeType",
            iTween.EaseType.easeInOutQuad,
            "loopType",
            iTween.LoopType.pingPong,
            "Delay",
            0.5f
        }));
    }
}
