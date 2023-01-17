using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BallHandler : MonoBehaviour
{
    public static Color oneColor;

    [HideInInspector] public static int circleNumber;

    public GameObject ball;
    public GameObject dummyBall;
    public GameObject hitButton;
    public GameObject levelComplete;
    public GameObject failScreen;
    public GameObject[] hearts;
    public Color[] changingColors;
    public Image[] balls;
    public Text totalBallsText;
    public Text countBallsText;
    public Text levelCompleteText;

    private float _ballSpeed = 100;
    private Vector3 _ballPosition = new Vector3(0, 0, -8);
    private Vector3 _circlePosition = new Vector3(0, 20, 23);
    private float _circleCreationDelay = 0.4f;
    private int _circlePieceAmount = 24;
    private bool _isGameFail = false;
    private int _ballsCount;
    private int _heartNumber;

    void Start()
    {
        ResetGame();
    }

    void ResetGame()
    {
        circleNumber = 1;
        GameObject instancedRound = Instantiate<GameObject>(Resources.Load("round" + Random.Range(1,6)) as GameObject, 
                                                                _circlePosition, Quaternion.identity);
        instancedRound.name = $"Circle{circleNumber}";

        _ballsCount = LevelsHandler.ballsCount;
        changingColors = ColorScript.colors;
        oneColor = changingColors[circleNumber%(changingColors.Length-1)];
        LevelsHandler.currentColor = oneColor;

        if(_heartNumber == 0)
            PlayerPrefs.SetInt("hearts", 3);
        _heartNumber = PlayerPrefs.GetInt("hearts", 3);

        for( int i = 0 ; i < _heartNumber; i++)
        {
            hearts[i].SetActive(true);
        }

        ChangeBallsCount();
    }

    public void HitBall()
    {

        if(_ballsCount <= 1)
        {
            base.Invoke("MakeANewCircle", _circleCreationDelay);
            StartCoroutine(HideButton());
            //Disable button
        }

        _ballsCount--;
        if(_ballsCount >= 0)
            balls[_ballsCount].enabled = false;      

        GameObject instancedBall = Instantiate<GameObject>(ball, _ballPosition, Quaternion.identity);
        instancedBall.GetComponent<MeshRenderer>().material.color = oneColor;
        instancedBall.GetComponent<Rigidbody>().AddForce(Vector3.forward * _ballSpeed, ForceMode.Impulse);
                
    }

    void ChangeBallsCount()
    {
        _ballsCount = LevelsHandler.ballsCount;
        dummyBall.GetComponent<MeshRenderer>().material.color = oneColor;

        totalBallsText.text = "" + LevelsHandler.totalCircles;
        countBallsText.text = "" + circleNumber;

        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].enabled = false;
        }

        for (int j = 0; j < _ballsCount; j++)
        {
            balls[j].enabled = true;
            balls[j].color = oneColor;
        }
    }

    void MakeANewCircle()
    {   

        if(circleNumber >= LevelsHandler.totalCircles && !_isGameFail)
        {
            StartCoroutine(LevelCompleteScreen());
            return;
        }

        GameObject[] circles = GameObject.FindGameObjectsWithTag("circle");
        GameObject firstCircle = GameObject.Find($"Circle{circleNumber}");

        for (int i = 0; i < _circlePieceAmount; i++)
            firstCircle.transform.GetChild(i).gameObject.SetActive(false);  
        firstCircle.transform.GetComponentInChildren<MeshRenderer>().material.color = BallHandler.oneColor;
        if(firstCircle.GetComponent<iTween>())
            firstCircle.GetComponent<iTween>().enabled = false;           

        foreach(GameObject circle in circles)
        {
            iTween.MoveBy(circle, iTween.Hash(new object[]
            {
                "y",
                -2.98f,
                "easetype",
                iTween.EaseType.spring,
                "time",
                0.5
            }));
        }

        circleNumber++;
        _ballsCount = LevelsHandler.ballsCount;
        oneColor = changingColors[circleNumber%(changingColors.Length-1)];
        LevelsHandler.currentColor = oneColor;

        GameObject instancedRound = Instantiate<GameObject>(Resources.Load("round" + Random.Range(1,6)) as GameObject, 
                                                                _circlePosition, Quaternion.identity);
        instancedRound.name = $"Circle{circleNumber}";

        FindObjectOfType<LevelsHandler>().MakeHurldes(circleNumber);
        ChangeBallsCount();
    }
  
    public void HeartsLow()
    {
        _heartNumber--;
        PlayerPrefs.SetInt("hearts", _heartNumber);
        hearts[_heartNumber].SetActive(false);
    }

    public void FailGame()
    {
        _isGameFail = true;
        Invoke("FailScreen", 1);
        hitButton.SetActive(false);
        StopCircle();
    }

    void StopCircle()
    {
        GameObject circle = GameObject.Find("Circle" + circleNumber);
        circle.transform.GetComponent<MonoBehaviour>().enabled = false;
        if(circle.GetComponent<iTween>())
            circle.GetComponent<iTween>().enabled = false;
    }

    void FailScreen()
    {
        failScreen.SetActive(true);
    }

    IEnumerator LevelCompleteScreen()
    {   
        _isGameFail = true;
        GameObject oldCircle = GameObject.Find("Circle" + circleNumber);
        for(int i = 0; i < _circlePieceAmount; i++)
        {
            oldCircle.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
        }
        oldCircle.transform.GetChild(_circlePieceAmount).gameObject.GetComponent<MeshRenderer>().material.color = oneColor;
        oldCircle.transform.GetComponent<MonoBehaviour>().enabled = false;
        if(oldCircle.GetComponent<iTween>())
            oldCircle.GetComponent<iTween>().enabled = false;
        hitButton.SetActive(false);
        GameObject[] oldCircles = GameObject.FindGameObjectsWithTag("circle");
        yield return new WaitForSeconds(2);
        levelComplete.SetActive(true);
        levelCompleteText.text = "" + LevelsHandler.currentLevel;
        yield return new WaitForSeconds(1);
        foreach (var circle in oldCircles)
        {
            Destroy(circle.gameObject);
        }
        yield return new WaitForSeconds(1);
        int currentLevel = PlayerPrefs.GetInt("C_Level");
        currentLevel++;
        PlayerPrefs.SetInt("C_Level", currentLevel);
        GameObject.FindObjectOfType<LevelsHandler>().UpgradeLevel();
        ResetGame();
        levelComplete.SetActive(false);
        _isGameFail = false;
    }

    IEnumerator HideButton()
    {
        if (!_isGameFail)
        {
            hitButton.SetActive(false);
            yield return new WaitForSeconds(1);
            hitButton.SetActive(true);
        }
    }

    public void OnRestartClick()
    {
        GameObject[] circles = GameObject.FindGameObjectsWithTag("circle");
        foreach (var gameObject in circles)
        {
            Destroy(gameObject.gameObject);
        }
        _isGameFail = false;
        FindObjectOfType<LevelsHandler>().UpgradeLevel();
        ResetGame();
    }
}
