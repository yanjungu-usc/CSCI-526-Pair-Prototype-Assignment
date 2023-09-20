using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;  // UI文本组件，用于显示时间
    public float totalTime = 60f;  // 总时间（秒）
    public GameObject gameOverPanel;  // 游戏结束面板
    public GameObject playground;  
    public GameObject timer; 



    private float countdown;  // 当前剩余时间

    void Start()
    {
        countdown = totalTime;  // 初始化倒计时时间
        gameOverPanel.SetActive(false);  // 初始时设置面板为不可见

    }

    void Update()
    {
        countdown -= Time.deltaTime;  // 更新倒计时

        if (countdown <= 0f)
        {
            countdown = 0f;
            // 这里可以添加时间到了之后需要执行的代码
            gameOverPanel.SetActive(true);  // 激活游戏结束面板
            playground.SetActive(false);
            timer.SetActive(false);
            return;  // 结束Update函数
        }

        int seconds = (int)(countdown % 60);  // 计算剩余秒数

        timerText.text = string.Format("Remaining Time: {0:D2}", seconds);  // 更新UI文本
    }
}
