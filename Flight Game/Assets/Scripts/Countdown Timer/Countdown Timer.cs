using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _timerText;

    [SerializeField]
    private float _remainingTime;

    void Update()
    {
        if (_remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
        }
        else if (_remainingTime < 0)
        {
            _remainingTime = 0;
            _timerText.color = Color.red;
        }
        
        int minutes = Mathf.FloorToInt(_remainingTime / 60);
        int seconds = Mathf.FloorToInt(_remainingTime % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddTime(float seconds)
    {
        _remainingTime += seconds;
    }
}
