using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyProg;
using System;

public enum agregatType
{
    none = 0,
    bat=1,
    gen=2
}
public class blockTypeAgregat : MonoBehaviour
{

    public invData invInput;
    public invData invOutput;
    public agregatUi agrUi;
    public ModLoader mod;


    public float wifiBatCount = 0f;
    public float wifiBatMax = 40f;

    public void btnGo()
    {
        isOn = !isOn;
    }


    public void open()
    {
        mod = Global.Links.getModLoader();
        agrUi = Global.Links.getSui().winAgregat;


        agrUi.myAgr = this;

        agrUi.myInvUiInput.openCargo(invInput);
        agrUi.myInvUiOut.openCargo(invOutput);


        agrUi.txtTitle.text = GetComponent<BlockController>().myData.nameView;






        RenderUi();

        slowUpd(0f);

    }


    string wifiInput = "electro";
    public float wifiInputCount = 1f; //количество треб энергии

    string wifiOutput = "not";
    public float wifiOutputCount = 1f; //количество энерги на выходе


    string wifiBatType = "not";

    string inputInd = "Core:dirt";
    string outputInd = "Core:brevno";
    float timeBake = 3f;
    string btnText = "Перегнать";
    agregatType typeAgregat = agregatType.none;

    public void init()
    {

        IniFile MyIni = new IniFile(GetComponent<BlockController>().myData.iniFilePath);



        invInput = gameObject.AddComponent<invData>();
        invInput.init(MyIni.ReadInt("inputSize", "agregat", 1));


        invOutput = gameObject.AddComponent<invData>();
        invOutput.init(MyIni.ReadInt("outputSize", "agregat", 1));


        btnText = MyIni.Read("btnText", "agregat", "not");


        typeAgregat = (agregatType)Enum.Parse(typeof(agregatType), MyIni.Read("typeAgregat", "agregat", "none"));


        wifiBatType = MyIni.Read("wifiBatType", "agregat", "not"); //типа батареи внутри агрегата. Агрегат работает на этой энергии

        wifiBatMax = MyIni.ReadInt("wifiBatMax", "agregat", 10) / 10f;//размер батареи


        wifiOutput = MyIni.Read("wifiOutput", "agregat", "not");//вход энергия
        wifiInput = MyIni.Read("wifiInput", "agregat", "not");//принимает эту энергию, копит если она в батареи


        wifiInputCount = MyIni.ReadInt("wifiInputCount", "agregat", 10) / 10f; //колв энергии для тика
        wifiOutputCount = MyIni.ReadInt("wifiOutputCount", "agregat", 10) / 10f; //колв энергии для тика


        outputInd = MyIni.Read("outputInd", "agregat", "not");

        inputInd = MyIni.Read("inputInd", "agregat", "not");

        timeBake = MyIni.ReadInt("timeBake", "agregat", 3000) / 1000f;


        wifiBatMax = MyIni.ReadInt("wifiBatMax", "agregat", 10) / 10f;


        agrUi = Global.Links.getSui().winAgregat;


    }


    void zarydka()
    {

        //Заряжаем предметы
        if (wifiBatCount > 0f)
        {
            int batSearch = invOutput.searchBat(wifiBatType, false);
            if (batSearch > -1)
            {
                float give = invOutput.items[batSearch].infoItemSave.stackSize - invOutput.items[batSearch].count;
                give = give * 10f;
                give = Mathf.Min(give, wifiBatCount);

                invOutput.items[batSearch].count += Mathf.RoundToInt(give / 10f);
                wifiBatCount -= give;
                invOutput.ReRender();
            }
        }


        //вход. Зарядка от батерейки
        if (wifiBatCount < wifiBatMax)
        {
            int batSearch = invInput.searchBat(wifiBatType, true, Mathf.Abs(Mathf.RoundToInt(wifiInputCount / 10f)));
            if (batSearch > -1)
            {

                float give = invInput.items[batSearch].count * 10f;
                give = Mathf.Min(give, wifiBatMax - wifiBatCount);

                //print(give);

                invInput.items[batSearch].count -= Mathf.RoundToInt(give / 10f);
                wifiBatCount += give;
                invInput.ReRender();
            }
        }

    }

    public void actionWifiOutputAll(bool toBat=true) {

        
        if (wifiBatCount <= 0f) return;

      

        Vector3 location = transform.position + new Vector3(1, 0, 0);


        RaycastHit[] hits = Physics.BoxCastAll(transform.position, transform.localScale * 2f, Vector3.up * -0.2f, transform.rotation, 3f);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)

                if (hits[i].transform.tag == "block")
                {
                    if (hits[i].transform.GetComponent<blockTypeAgregat>() != null)
                    {
                        blockTypeAgregat b = hits[i].transform.GetComponent<blockTypeAgregat>();

                       

                        if ((toBat && b.typeAgregat == agregatType.bat) || b.typeAgregat == agregatType.none)
                        {
                            if (b.wifiBatType == wifiBatType)
                            {
                                float give = Math.Min(wifiBatCount, b.wifiBatMax - b.wifiBatCount);
                                if (give < 0f)
                                {
                                    print("ОШИБКА ЦИФРЫ");
                                    return;
                                } 
                                b.wifiBatCount += give;
                                wifiBatCount -= give;
                                if (wifiBatCount <= 0f) break;
                            }
                        }

                    }


                }
        }

    }

    public void actionBat()
    {

    }


    public bool isOn = false;
    void tickAction()
    {
        zarydka();


        if (typeAgregat==agregatType.bat)
        {
            if (isOn)
            {
                actionWifiOutputAll();
            }
            return;
        }

        if (typeAgregat == agregatType.gen)
        {
            actionWifiOutputAll();
        }

        if (wifiBatType == wifiInput)
        {
            if (wifiInputCount > 0f)
            {
                if (wifiBatCount - wifiInputCount < 0f)
                {
                    //не хватает энергии для работы
                    isOn = false;
                    return;
                }
            }
        }


        if (wifiBatType == wifiOutput)
        {
            if (wifiOutputCount > 0f)
            {

                if (wifiBatCount >= wifiBatMax)
                {
                    //мы заряжены на полную
                    isOn = false;
                    return;
                }
            }
        }



        if (invInput.giveFromInd(inputInd, 1))
        {
            if (wifiBatType == wifiInput) wifiBatCount -= wifiInputCount;
            if (wifiBatType == wifiOutput) wifiBatCount += wifiOutputCount;

            invOutput.itemAdd(outputInd, 1, true);
        }
        else
        {
            isOn = false;
        }


     

    }


    float timeerBaker = 0f;
    void slowUpd(float deltaTime)
    {
        if (isOn)
        {
            zarydka();
            timeerBaker += deltaTime;
            if (timeerBaker >= timeBake)
            {

                tickAction();
                timeerBaker = 0f;
            }
        }

        RenderUi();
    }


    float timeSlowUpd = 1f;
    void Update()
    {
      
            timeSlowUpd -= Time.deltaTime;
            if (timeSlowUpd <= 0f)
            {
                slowUpd(0.2f);
                timeSlowUpd = 0.2f;
            }
        

    }

    void RenderUi()
    {
        if (agrUi != null)
            if (agrUi != null)
            {
                if (!agrUi.gameObject.active)
                {
                    agrUi = null;
                    return;

                }
            }
        if (agrUi == null) return;


        agrUi.barProgress.fillAmount = 1f / timeBake * timeerBaker;
        agrUi.barBatarey.fillAmount = 1f / wifiBatMax * wifiBatCount;


        string _in = "";
        if (wifiInput != "not")
        {
            _in = $"{ mod.wifiGetFromInd(wifiInput).name} {wifiInputCount} {mod.wifiGetFromInd(wifiInput).ed}";
        }
        agrUi.transform.Find("in/input/h1").GetComponent<Text>().text = $"Вход {_in}";


        _in = "";
        if (wifiOutput != "not")
        {
            _in = $"{ mod.wifiGetFromInd(wifiOutput).name} {wifiInputCount} {mod.wifiGetFromInd(wifiOutput).ed}";
        }
        agrUi.transform.Find("in/out/h1").GetComponent<Text>().text = $"Выход {_in}";


        _in = "";
        if (wifiBatType != "not")
        {
            _in = $"{ mod.wifiGetFromInd(wifiBatType).name} {Mathf.RoundToInt(wifiBatCount)} / {Mathf.RoundToInt(wifiBatMax)} {mod.wifiGetFromInd(wifiBatType).ed}";
        }
        agrUi.transform.Find("in/en/txt").GetComponent<Text>().text = $" {_in}";


        if (isOn)
        {
            agrUi.txtBtn.text = $"{ btnText}:вкл ";

        }
        else
        {
            agrUi.txtBtn.text = $"{ btnText}:выкл";
        }


    }
}