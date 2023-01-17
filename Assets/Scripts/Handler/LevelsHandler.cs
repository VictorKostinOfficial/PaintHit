using UnityEngine;

public class LevelsHandler : MonoBehaviour
{
    public static int currentLevel;
    public static int ballsCount;
    public static int totalCircles;
    public static Color currentColor;

    void Awake()
    {
        if(PlayerPrefs.GetInt("FirstTime", 0) == 0)
        {
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.SetInt("C_Level", 1);
        }

        UpgradeLevel();
    }

    public void UpgradeLevel()
    {
        currentLevel = PlayerPrefs.GetInt("C_Level", 1);

        switch(currentLevel)
        {
            case 1:
                ballsCount = 3;
                totalCircles = 2;
                UpgradeDificulty();
                break;
            case 2:
                ballsCount = 3;
                totalCircles = 3;
                UpgradeDificulty(150.0f,40.0f,2.8f,0.2f);
                break;
            case 3:
                ballsCount = 4;
                totalCircles = 4;
                UpgradeDificulty(180.0f,40.0f,2.4f,0.2f);
                break;
            case 4:
                ballsCount = 5;
                totalCircles = 5;
                UpgradeDificulty(200.0f,50.0f,2.2f,0.3f);
                break;
            case 5:
                ballsCount = 6;
                totalCircles = 6;
                UpgradeDificulty(220.0f,50.0f,2.2f,0.35f);
                break;
            default:
                ballsCount = 7;
                totalCircles = currentLevel;
                UpgradeDificulty(220.0f,50.0f,2.2f,0.4f);
                break;
        }
    }

    public void MakeHurldes(int numberBlackPieces)
    {
        GameObject gameObject = GameObject.Find($"Circle{BallHandler.circleNumber}");

        for(int i = 0; i < numberBlackPieces; i++)
        {   
            int index = 0;
            while(true)
            {
                index = Random.Range(1, 24);

                if(gameObject.transform.GetChild(index).gameObject.GetComponent<MeshRenderer>().material.color != currentColor)
                    break;
            }
             
            gameObject.transform.GetChild(index).gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.transform.GetChild(index).gameObject.GetComponent<MeshRenderer>().material.color = currentColor;
            gameObject.transform.GetChild(index).gameObject.tag = "red";
        }
    }

    void UpgradeDificulty(float initialSpeed = 140.0f, float incrementSpeed = 40.0f, float rotationTime = 3.0f, float decremetnRotationTime = 0.2f)
    {
        Circle.rotationCircleSpeed = initialSpeed;
        Circle.incremetnRotationSpeed = incrementSpeed;

        Circle.rotationTime = rotationTime;
        Circle.decremetnRotationTime = decremetnRotationTime;
    }
}
