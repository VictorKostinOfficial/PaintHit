using UnityEngine;

public class ColorScript : MonoBehaviour
{
   public static Color[] colors;

    public Color[] color1;
    public Color[] color2;
    public Color[] color3;

    void Awake()
    {
        ChangeColor();
    }

    void ChangeColor()
    {
        int randomC = Random.Range(0, 2);

        PlayerPrefs.SetInt("ColorSelect", randomC);
        PlayerPrefs.GetInt("ColorSelect");

        switch(PlayerPrefs.GetInt("ColorSelect"))
        {
            case 0:
                colors = color1;
                break;
            case 1:
                colors = color2;
                break;
            case 2:
                colors = color3;
                break;

        }

        if(PlayerPrefs.GetInt("ColorSelect") == 0)
        {
            colors = color1;
        }
    }

    void HeartsFun(GameObject g)
    {
        int @int = PlayerPrefs.GetInt("hearts");
        if(@int == 1)
        {
            FindObjectOfType<BallHandler>().HeartsLow();
        }
    }
}
