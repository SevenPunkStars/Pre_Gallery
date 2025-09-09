using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class AvatarInfosManager : MonoBehaviour
{
    private ExperimentDB db;

    public TMP_Text[] current_names;
    public TMP_Text[] current_roles;

    public TMP_InputField[] names;
    public TMP_Dropdown[] roles;
    public Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        db = FindObjectOfType<ExperimentDB>();
    }

    // Update is called once per frame
    void Update()
    {
        int avatarCpt = db.avatarNB();
        int cpt = 0;
        foreach (Button button in buttons) {
            
            cpt++;
            button.interactable = cpt<=avatarCpt;

            if(cpt <= avatarCpt)
            {

                current_names[cpt - 1].text = db.FindNameByNum(cpt);
                current_roles[cpt - 1].text = db.FindRoleByNum(cpt).ToString();
            }
        }
    }

    public void setAvatarInfoFromNum(int num)
    {

        db.setAvatarInfoFromNum(names[num-1].text, AvatarManager.stringToRole(roles[num-1].options[roles[num-1].value].text),num);
    }
}
