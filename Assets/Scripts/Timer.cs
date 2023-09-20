using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;  // UI�ı������������ʾʱ��
    public float totalTime = 60f;  // ��ʱ�䣨�룩
    public GameObject gameOverPanel;  // ��Ϸ�������
    public GameObject playground;  
    public GameObject timer; 



    private float countdown;  // ��ǰʣ��ʱ��

    void Start()
    {
        countdown = totalTime;  // ��ʼ������ʱʱ��
        gameOverPanel.SetActive(false);  // ��ʼʱ�������Ϊ���ɼ�

    }

    void Update()
    {
        countdown -= Time.deltaTime;  // ���µ���ʱ

        if (countdown <= 0f)
        {
            countdown = 0f;
            // ����������ʱ�䵽��֮����Ҫִ�еĴ���
            gameOverPanel.SetActive(true);  // ������Ϸ�������
            playground.SetActive(false);
            timer.SetActive(false);
            return;  // ����Update����
        }

        int seconds = (int)(countdown % 60);  // ����ʣ������

        timerText.text = string.Format("Remaining Time: {0:D2}", seconds);  // ����UI�ı�
    }
}
