using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class EnvironmentManager : MonoBehaviour
{
    public enum Environment
    {
        Ga,
        SGJ,

    }



    Environment currentPress = Environment.Ga;
    bool isPressed = false;
    float timmer = 0;
    float threshold = 1.5f;

    public GameObject Ga;
    public GameObject SGJ;


    public ExperimentDB db;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void setEnvironment(string code){
        db.updateEnvironment(CodeToEnvironment(code));
    }

    // Update is called once per frame
    void Update()
    {



        if (isPressed)
        {
            timmer += Time.deltaTime;
            if (timmer > threshold)
            {
                db.updateEnvironment(currentPress);
                isPressed = false;
            }
        }
        //if (code == "PST") db.updateEnvironment(Environment.PST);
        //else if (code == "SN") db.updateEnvironment(Environment.SN);
        //else if (code == "W") db.updateEnvironment(Environment.W);
        //else if (code == "Gl") db.updateEnvironment(Environment.Gl);
        //else db.updateEnvironment(Environment.Ga);



        Ga.SetActive(db.getEnvironment() == Environment.Ga);
        SGJ.SetActive(db.getEnvironment() == Environment.SGJ);

    }


    public void pressStart(string code)
    {   
        if (!isPressed)
        {
            currentPress = CodeToEnvironment(code);
            isPressed = true;
            timmer = 0;
        }
        
    }

    public void pressStop(string code)
    {
        isPressed = false;
        

    }

    private Environment CodeToEnvironment(string code)
    {
        if (code == "SGJ") return Environment.SGJ;
        else return Environment.Ga;
    }

    

}
