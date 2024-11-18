using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickTime : MonoBehaviour
{
    public ChasingNPC qteNPC;
    public bool isQuickTimeActive = false;
    [SerializeField] GameObject quickTimePanel;
    Slider quickTimeSlider;
    [SerializeField] float quickTimeSliderSpeed = 0.1f;
    [SerializeField] int quickTimeIncrement = 5;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        quickTimeSlider = quickTimePanel.transform.GetChild(0).GetComponent<Slider>();
    }

    private void FixedUpdate()
    {
        if(quickTimePanel.activeSelf)
        {
            quickTimeSlider.value -= quickTimeSliderSpeed;
        }
        quickTimeSlider.value = Mathf.Clamp(quickTimeSlider.value, 0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Q) && !isQuickTimeActive)
        // {
        //     StartQuickTimeEvent();
        // }
        if(quickTimePanel.activeSelf){
            if(Input.GetKeyDown(KeyCode.Space))
            {
                quickTimeSlider.value += quickTimeIncrement;
            }
            if(quickTimeSlider.value >= 100)
            {
                EndQuickTimeEvent();
            }
        }
    }

    public void StartQuickTimeEvent()
    {
        //pause time
        Time.timeScale = 0;
        quickTimeSlider.value = 10;
        quickTimePanel.SetActive(true);
    }

    void EndQuickTimeEvent()
    {
        //resume time
        Time.timeScale = 1;
        quickTimePanel.SetActive(false);
        if(qteNPC == null){
            Debug.Log("qteNPC is null, called from QuickTime");
        }
        qteNPC.canMove = true;
        Debug.Log("qteNPC.setDestination.Length = " + qteNPC.setDestination.Length);
        qteNPC.target = qteNPC.setDestination[Random.Range(0, qteNPC.setDestination.Length)];   
    }
}
