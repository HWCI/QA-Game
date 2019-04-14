using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_energyManager : MonoBehaviour {
    public static Boss_energyManager instance = null;

    Boss_GameManager GameManager;
    
    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        energyReset();

    }

    public void energyReset()
    {
        m_workerEnergy = defaultValue;
        m_moneyEnergy = defaultValue;
        m_clientEnergy = defaultValue;
        m_qualityEnergy = defaultValue;

        workerEnergy = defaultValue;
        moneyEnergy = defaultValue;
        clientEnergy = defaultValue;
        qualityEnergy = defaultValue;

        workerEnergyV.text = Mathf.Floor(workerEnergy).ToString() + "%";
        moneyEnergyV.text = Mathf.Floor(moneyEnergy).ToString() + "%";
        clientEnergyV.text = Mathf.Floor(clientEnergy).ToString() + "%";
        qualityEnergyV.text = Mathf.Floor(qualityEnergy).ToString() + "%";
    }
    private void Awake()
    {
        GameManager = Boss_GameManager.instance;
    }

    [HideInInspector]
    public float changeToOrangeAt = 50;
    [HideInInspector]
    public float changeToRedAt = 10;
    [HideInInspector]
    public Sprite energy_green,energy_orange, energy_red;
    [HideInInspector]
    public Sprite energyBar_green, energyBar_orange, energyBar_red;
    //UI
    [HideInInspector]
    public Text workerEnergyT, moneyEnergyT, clientEnergyT, qualityEnergyT;
    [HideInInspector]
    public GameObject workerEnergyBar, moneyEnergyBar, clientEnergyBar, qualityEnergyBar;
    [HideInInspector]
    public GameObject workerEnergyBarAnim, moneyEnergyBarAnim, clientEnergyBarAnim, qualityEnergyBarAnim;
    [HideInInspector]
    public Text workerEnergyV, moneyEnergyV, clientEnergyV, qualityEnergyV;

    public Animator Worker_20_Anim, Money_20_anim, Officer_20_Anim, Goods_20_Anim;

    
    [HideInInspector]
    public int m_defaultValue;
    public int defaultValue
    {
        get
        {
            return m_defaultValue;
        }
        set
        {
            if (m_defaultValue == value)
                return;
            m_defaultValue = value;

            m_workerEnergy = defaultValue;
            m_moneyEnergy = defaultValue;
            m_clientEnergy = defaultValue;
            m_qualityEnergy = defaultValue;

            workerEnergy = defaultValue;
            moneyEnergy = defaultValue;
            clientEnergy = defaultValue;
            qualityEnergy = defaultValue;

        }
    }

    float m_workerEnergy, m_moneyEnergy, m_clientEnergy, m_qualityEnergy;
    public float workerEnergy
    {
        get
        {
            return m_workerEnergy;
        }

        set
        {
            if (m_workerEnergy == value)
                return;
            if (value <= 0)
            {
                StartCoroutine(GameManager.ShowGameOverText("生產力"));
                //GameManager.ShowGameOverText("生產力");
            }
            float temp = (m_workerEnergy - value) * -1;
            m_workerEnergy = Mathf.Clamp(value, 0, 100);
            //workerEnergyV.text = Mathf.Floor(workerEnergy).ToString() + "%";
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            workerEnergyT.gameObject.SetActive(true);
            workerEnergyT.GetComponent<Animator>().SetTrigger("Trigger");
            if (temp < 0)
            {
                workerEnergyT.text = temp.ToString();
                workerEnergyT.color = new Color(1, 0.3137255f, 0.2627451f);
                Boss_MissionManager.instance.checkPlayerGood(false);
            }
            else
            {
                workerEnergyT.text = "+" + temp.ToString();
                workerEnergyT.color = new Color(1, 1, 1);

                if (temp == GameManager.Increase_highestScore)
                {
                    Boss_MissionManager.instance.checkPlayerGood(true);

                }else if(temp == GameManager.Increase_secondScore)
                {
                    Boss_MissionManager.instance.checkPlayerGood(false);
                }
            }

        }
    }
    public float moneyEnergy
    {
        get
        {
            return m_moneyEnergy;
        }

        set
        {
            if (m_moneyEnergy == value)
                return;
            if (value <= 0)
            {
                StartCoroutine(GameManager.ShowGameOverText("資金"));
                //GameManager.ShowGameOverText("資金");
            }
            float temp = (m_moneyEnergy - value) * -1;
            m_moneyEnergy = Mathf.Clamp(value, 0, 100);
            //moneyEnergyV.text = Mathf.Floor(moneyEnergy).ToString() + "%";
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            moneyEnergyT.gameObject.SetActive(true);

            moneyEnergyT.GetComponent<Animator>().SetTrigger("Trigger");
            if (temp < 0)
            {
                moneyEnergyT.text = temp.ToString();
                moneyEnergyT.color = new Color(1, 0.3137255f, 0.2627451f);
                Boss_MissionManager.instance.checkPlayerGood(false);
            }
            else
            {
                moneyEnergyT.text = "+" + temp.ToString();
                moneyEnergyT.color = new Color(1, 1, 1);
                if (temp == GameManager.Increase_highestScore)
                {
                    Boss_MissionManager.instance.checkPlayerGood(true);
                }
                else if (temp == GameManager.Increase_secondScore)
                {
                    Boss_MissionManager.instance.checkPlayerGood(false);
                }
            }

        }
    }
    public float clientEnergy
    {
        get
        {
            return m_clientEnergy;
        }

        set
        {
            if (m_clientEnergy == value)
                return;
            if (value <= 0)
            {
                StartCoroutine(GameManager.ShowGameOverText("客戶"));
                //GameManager.ShowGameOverText("客戶");
            }
            float temp = (m_clientEnergy - value) * -1;
            m_clientEnergy = Mathf.Clamp(value, 0, 100);
            //clientEnergyV.text = Mathf.Floor(clientEnergy).ToString() + "%";
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            clientEnergyT.gameObject.SetActive(true);

            clientEnergyT.GetComponent<Animator>().SetTrigger("Trigger");
            if (temp < 0)
            {
                clientEnergyT.text = temp.ToString();
                clientEnergyT.color = new Color(1, 0.3137255f, 0.2627451f);
                Boss_MissionManager.instance.checkPlayerGood(false);
            }
            else
            {
                clientEnergyT.text = "+" + temp.ToString();
                clientEnergyT.color = new Color(1, 1, 1);
                if (temp == GameManager.Increase_highestScore)
                {
                    Boss_MissionManager.instance.checkPlayerGood(true);
                }
                else if (temp == GameManager.Increase_secondScore)
                {
                    Boss_MissionManager.instance.checkPlayerGood(false);
                }
            }

        }
    }
    public float qualityEnergy
    {
        get
        {
            return m_qualityEnergy;
        }

        set
        {

            if (m_qualityEnergy == value)
                return;
            if (value <= 0)
            {
                StartCoroutine(GameManager.ShowGameOverText("品質"));
                //GameManager.ShowGameOverText("品質");
            }
            float temp = (m_qualityEnergy - value) * -1;
            m_qualityEnergy = Mathf.Clamp(value, 0, 100);
            //qualityEnergyV.text = Mathf.Floor(qualityEnergy).ToString() + "%";

#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            qualityEnergyT.gameObject.SetActive(true);

            qualityEnergyT.GetComponent<Animator>().SetTrigger("Trigger");
            if (temp < 0)
            {
                qualityEnergyT.text = temp.ToString();
                qualityEnergyT.color = new Color(1, 0.3137255f, 0.2627451f);
                Boss_MissionManager.instance.checkPlayerGood(false);
            }
            else
            {
                qualityEnergyT.text = "+" + temp.ToString();
                qualityEnergyT.color = new Color(1, 1, 1);
                if (temp == GameManager.Increase_highestScore)
                {
                    Boss_MissionManager.instance.checkPlayerGood(true);
                }
                else if (temp == GameManager.Increase_secondScore)
                {
                    Boss_MissionManager.instance.checkPlayerGood(false);
                }
            }

        }
    }

    [HideInInspector]
    public bool canUpdate = false;

    private void Update()
    {
        //EnergyChange();
        if (canUpdate)
        {

        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            workerEnergy -= 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            moneyEnergy -= 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            clientEnergy -= 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            qualityEnergy -= 10;
        }
#endif
    }
    
    IEnumerator LerpObject(GameObject bar, float bar2)
    {
        float traveltime = 0.5f;
        float curTime = 0;
        float normalizedValue;
        Vector3 curPosition = bar.GetComponent<RectTransform>().anchoredPosition;
        while (curTime <= traveltime)
        {
            curTime += Time.deltaTime;
            normalizedValue = curTime / traveltime;
            //print(normalizedValue);
            //print(curPosition);
            //print(bar2);
            bar.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(curPosition, new Vector3((100 - bar2) * -3.3f, 0, 0), normalizedValue);
            yield return null;
        }
        



    }
    public void EnergyChange()
    {

        //workerEnergy
        StartCoroutine(LerpObject(workerEnergyBar, workerEnergy));
        //workerEnergyBar.GetComponent<RectTransform>().anchoredPosition = new Vector3((100 - workerEnergy) * -3.3f, 0, 0);
        if (workerEnergy <= changeToRedAt)
        {
            Worker_20_Anim.SetBool("_red", true);
            workerEnergyBar.GetComponent<Image>().sprite = energy_red;
            workerEnergyBarAnim.GetComponent<Image>().sprite = energyBar_red;
        }

        else if (workerEnergy > changeToRedAt && workerEnergy <= changeToOrangeAt)
        {
            Worker_20_Anim.SetBool("_red", false);
            workerEnergyBar.GetComponent<Image>().sprite = energy_orange;
            workerEnergyBarAnim.GetComponent<Image>().sprite = energyBar_orange;
        }

        else
        {
            workerEnergyBar.GetComponent<Image>().sprite = energy_green;
            workerEnergyBarAnim.GetComponent<Image>().sprite = energyBar_green;
        }
           
        workerEnergyV.text = Mathf.Floor(workerEnergy).ToString() + "%";

        //moneyEnergy
        StartCoroutine(LerpObject(moneyEnergyBar, moneyEnergy));
        //moneyEnergyBar.GetComponent<RectTransform>().anchoredPosition = new Vector3((100 - moneyEnergy) * -3.3f, 0, 0);
        if (moneyEnergy <= changeToRedAt)
        {
            Money_20_anim.SetBool("_red", true);
            moneyEnergyBar.GetComponent<Image>().sprite = energy_red;
            moneyEnergyBarAnim.GetComponent<Image>().sprite = energyBar_red;
        }
            
        else if (moneyEnergy > changeToRedAt && moneyEnergy <= changeToOrangeAt)
        {
            Money_20_anim.SetBool("_red", false);
            moneyEnergyBarAnim.GetComponent<Image>().sprite = energyBar_orange;
            moneyEnergyBar.GetComponent<Image>().sprite = energy_orange;
        }

        else
        {
            moneyEnergyBarAnim.GetComponent<Image>().sprite = energyBar_green;
            moneyEnergyBar.GetComponent<Image>().sprite = energy_green;
        }
         
        moneyEnergyV.text = Mathf.Floor(moneyEnergy).ToString() + "%";

        //clientEnergy
        StartCoroutine(LerpObject(clientEnergyBar, clientEnergy));
        //clientEnergyBar.GetComponent<RectTransform>().anchoredPosition = new Vector3((100 - clientEnergy) * -3.3f, 0, 0);
        if (clientEnergy <= changeToRedAt)
        {
            Officer_20_Anim.SetBool("_red", true);
            clientEnergyBarAnim.GetComponent<Image>().sprite = energyBar_red;
            clientEnergyBar.GetComponent<Image>().sprite = energy_red;
        }
            
        else if (clientEnergy > changeToRedAt && clientEnergy <= changeToOrangeAt)
        {
            Officer_20_Anim.SetBool("_red", false);
            clientEnergyBarAnim.GetComponent<Image>().sprite = energyBar_orange;
            clientEnergyBar.GetComponent<Image>().sprite = energy_orange;
        }

        else
        {
            clientEnergyBarAnim.GetComponent<Image>().sprite = energyBar_green;
            clientEnergyBar.GetComponent<Image>().sprite = energy_green;
        }
            
        clientEnergyV.text = Mathf.Floor(clientEnergy).ToString() + "%";

        //qualityEnergy
        StartCoroutine(LerpObject(qualityEnergyBar, qualityEnergy));
        //qualityEnergyBar.GetComponent<RectTransform>().anchoredPosition = new Vector3((100 - qualityEnergy) * -3.3f, 0, 0);
        if (qualityEnergy <= changeToRedAt)
        {
            Goods_20_Anim.SetBool("_red", true);
            qualityEnergyBarAnim.GetComponent<Image>().sprite = energyBar_red;
            qualityEnergyBar.GetComponent<Image>().sprite = energy_red;
        }
            
        else if (qualityEnergy > changeToRedAt && qualityEnergy <= changeToOrangeAt)
        {
            Goods_20_Anim.SetBool("_red", false);
            qualityEnergyBarAnim.GetComponent<Image>().sprite = energyBar_orange;
            qualityEnergyBar.GetComponent<Image>().sprite = energy_orange;
        }

        else
        {
            qualityEnergyBarAnim.GetComponent<Image>().sprite = energyBar_green;
            qualityEnergyBar.GetComponent<Image>().sprite = energy_green;
        }
            
        qualityEnergyV.text = Mathf.Floor(qualityEnergy).ToString() + "%";
    }
}








