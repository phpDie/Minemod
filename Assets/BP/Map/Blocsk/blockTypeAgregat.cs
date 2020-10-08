using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyProg;

public class blockTypeAgregat : MonoBehaviour
{

    public invData invInput;
    public invData invOutput;
    public agregatUi agrUi;

    public float wifiEnergyGive = 1f;
    public float wifiEnergy = 0f;
    public float wifiEnergyMax = 40f;

    public void btnGo()
    { 
        isOn = !isOn;
    }


    public void open()
    {
        agrUi = Global.Links.getSui().winAgregat;


        agrUi.myAgr = this;

        agrUi.myInvUiInput.openCargo(invInput);
        agrUi.myInvUiOut.openCargo(invOutput);


        agrUi.txtTitle.text = GetComponent<BlockController>().myData.nameView;


        agrUi.transform.Find("in/btn").gameObject.SetActive(btnText != "not");
        if (btnText != "not")
        {
            agrUi.txtBtn.text = btnText;

        }
        else
        {
            isOn = true;
        }


        slowUpd(0f);

    }


    string inputInd = "Core:dirt";
    string inputWifi = "electro";
    string outputInd = "Core:brevno";
    string outputWifi = "not";
    float timeBake = 3f;
    string btnText = "Перегнать";


    public void init()
    {

        IniFile MyIni = new IniFile(GetComponent<BlockController>().myData.iniFilePath);



        invInput = gameObject.AddComponent<invData>();
        invInput.init(MyIni.ReadInt("inputSize", "agregat", 1));


        invOutput = gameObject.AddComponent<invData>();
        invOutput.init(MyIni.ReadInt("outputSize", "agregat", 1));


        btnText = MyIni.Read("btnText", "agregat", "not");
        outputWifi = MyIni.Read("outputWifi", "agregat", "not");
        outputInd = MyIni.Read("outputInd", "agregat", "not");
        inputWifi = MyIni.Read("inputWifi", "agregat", "not");
        inputInd = MyIni.Read("inputInd", "agregat", "not");

        timeBake = MyIni.ReadInt("timeBake", "agregat", 3000)/1000f;


        wifiEnergyGive = MyIni.ReadInt("wifiEnergyGive", "agregat", 10)/10f;
        wifiEnergyMax = MyIni.ReadInt("wifiEnergyMax", "agregat", 10)/10f;


        agrUi = Global.Links.getSui().winAgregat;


    }


    void zarydka()
    {
      
        if (wifiEnergy > 0f)
        {
            int batSearch = invOutput.searchBat(false);
            if (batSearch > -1)
            {
                float give = invOutput.items[batSearch].infoItemSave.stackSize - invOutput.items[batSearch].count;
                give = give * 10f;
                give = Mathf.Min(give, wifiEnergy);

                invOutput.items[batSearch].count += Mathf.RoundToInt(give/10f);
                wifiEnergy -= give;
                invOutput.ReRender();
            }
        }
        

        //вход
        if (wifiEnergy <= 0f)
        {
            int batSearch = invInput.searchBat(true, Mathf.Abs(Mathf.RoundToInt( wifiEnergyGive/10f)));
            if (batSearch > -1)
            {

                float give = invInput.items[batSearch].count*10f;
                give = Mathf.Min(give, wifiEnergyMax - wifiEnergy );

                //print(give);

                invInput.items[batSearch].count -= Mathf.RoundToInt(give/10f);
                wifiEnergy += give;
                invInput.ReRender();
            }
        }

    }

    public bool isOn = false;
    void tickAction()
    {
        zarydka();


        //Если забор энергии
        if (wifiEnergyGive < 0)
        {
            if (wifiEnergy + wifiEnergyGive < 0)
            {
                isOn = false;
                return;
            }
        }

        //Если генерация
        if (wifiEnergyGive > 0)
        {
            if (wifiEnergy >= wifiEnergyMax)
            {
                isOn = false;
                return;
            }
        }
 


            if (invInput.giveFromInd(inputInd, 1))
        {
            invOutput.itemAdd(outputInd, 1,true);
            wifiEnergy += wifiEnergyGive;
        }
        else
        {
            isOn = false;
        }
    }


    float timeerBaker = 0f;
    void slowUpd(float deltaTime)
    {
        zarydka();
        timeerBaker += deltaTime;

         
        if (timeerBaker >= timeBake)
        {

            tickAction();
            timeerBaker = 0f;
        }



        if (agrUi != null)
        {
            if (!agrUi.gameObject.active)
            {
                agrUi = null;
                return;

            }
            agrUi.barProgress.fillAmount = 1f / timeBake * timeerBaker;
            agrUi.barBatarey.fillAmount = 1f / wifiEnergyMax * wifiEnergy;
        }

    }

    float timeSlowUpd=1f;
    void Update()
    {
        if (isOn)
        {
            timeSlowUpd -= Time.deltaTime;
            if (timeSlowUpd <= 0f)
            {
                slowUpd(0.2f);
                timeSlowUpd = 0.2f;
            }
        }
        
    }
}
