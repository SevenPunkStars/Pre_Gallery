using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;
using System.Runtime.CompilerServices;
public class ExperimentDB : RealtimeComponent<ExperimentDBModel>
{
    //public string lvn;
    //public AvatarManager.Role role;
    //public bool tocheck;

    public void Update()
    {
        //Debug.Log(model.avatarDB);

        //if (tocheck)
        //{
        //    tocheck = false;
        //    setAvatarInfoFromNum(lvn, role, 1);
        //}
    }

    public EnvironmentManager.Environment getEnvironment()
    {
        if (model.environment == "SGJ") return EnvironmentManager.Environment.SGJ;

        return EnvironmentManager.Environment.Ga;
    }


    public void updateEnvironment(EnvironmentManager.Environment env)
    {
        model.environment = env.ToString();

    }

    public void Add(string name, string role, string id)
    {

        model.avatarDB += ";" + name + "|" + role + "|" + id + "|" + "f";
    }

    public void Remove(string id)
    {
        string[] parts = model.avatarDB.Split(';');

        string toRemove = "";
        foreach (string part in parts)
        {
            if (!part.Equals(""))
            {
                string thisId = part.Split('|')[2];
                if (id == thisId) toRemove = part;
            }
        }
        if (!toRemove.Equals(""))
        {
            model.avatarDB = model.avatarDB.Replace(";" + toRemove, "");

        }
    }

    public int RoleNb(AvatarManager.Role role)
    {
        int cpt = 0;
        string[] parts = model.avatarDB.Split(';');
        foreach (string part in parts)
        {
            if (!part.Equals(""))
            {
                if (role.ToString() == part.Split('|')[1]) cpt++;

            }
        }

        return cpt;
    }

    public string FindNameById(string id)
    {
        string[] parts = model.avatarDB.Split(';');


        foreach (string part in parts)
        {
            if (!part.Equals(""))
            {
                string thisId = part.Split('|')[2];
                if (id == thisId) return part.Split('|')[0];
            }

        }
        return "noName";
    }

    public AvatarManager.Role FindRoleById(string id)
    {
        string[] parts = model.avatarDB.Split(';');


        foreach (string part in parts)
        {
            if (!part.Equals(""))
            {
                string thisId = part.Split('|')[2];
                if (id == thisId) return AvatarManager.stringToRole(part.Split('|')[1]);
            }

        }
        return AvatarManager.Role.Psychatrist;
    }

    public bool FindPointingById(string id)
    {

        string[] parts = model.avatarDB.Split(';');


        foreach (string part in parts)
        {
            if (!part.Equals(""))
            {
                string thisId = part.Split('|')[2];
                if (id == thisId)
                {
                    return (part.Split('|')[3]).Equals("t") ? true : false;
                }
            }

        }
        return false;
    }

    public void setPointingFromID(string id, bool b)
    {
        string[] parts = model.avatarDB.Split(';');

        string theGuyToFind_old = "";
        string theGuyToFind_new = "";

        foreach (string part in parts)
        {
            if (!part.Equals(""))
            {
                string thisId = part.Split('|')[2];
                if (id == thisId)
                {
                    theGuyToFind_old = part;
                    theGuyToFind_new = part.Replace('|' + part.Split('|')[3], '|' + (b ? "t" : "f"));

                }
            }
        }

        if (theGuyToFind_old != "") model.avatarDB = model.avatarDB.Replace(theGuyToFind_old, theGuyToFind_new);

    }

    public void setNameFromNum(int num, string new_name)
    {


        string[] parts = model.avatarDB.Split(';');

        string theGuyToFind_old = "";
        string theGuyToFind_new = "";

        for (int i = 1; i <= parts.Length; i++)
        {
            if (num == i)
            {
                theGuyToFind_old = parts[i];
                int index = parts[i].IndexOf('|');

                string sub = (index >= 0 && index + 1 < parts[i].Length)
                    ? parts[i].Substring(index)
                    : "";

                theGuyToFind_new = new_name+ sub;
                    
            }
        }


        if (theGuyToFind_old != "") model.avatarDB = model.avatarDB.Replace(";" + theGuyToFind_old, ";" + theGuyToFind_new);
    }

    public void setRoleFromNum(int num, AvatarManager.Role role)
    {
        string[] parts = model.avatarDB.Split(';');

        string theGuyToFind_old = "";
        string theGuyToFind_new = "";


        for (int i = 1; i <= parts.Length; i++)
        {
            if (num == i)
            {
                theGuyToFind_old = parts[i];
                theGuyToFind_new = parts[i].Replace('|' + parts[i].Split('|')[1] + '|', '|' + role.ToString() + '|');
            }
        }

        if (theGuyToFind_old != "") model.avatarDB = model.avatarDB.Replace(theGuyToFind_old, theGuyToFind_new);
    }

    public int avatarNB()
    {
        return model.avatarDB.Split(';').Length - 1;
    }

    public string FindNameByNum(int num){

        return model.avatarDB.Split(';')[num].Split('|')[0];
        
    }

    public AvatarManager.Role FindRoleByNum(int num)
    {
        return AvatarManager.stringToRole(model.avatarDB.Split(';')[num].Split('|')[1]);

    }

    public void setAvatarInfoFromNum(string new_name, AvatarManager.Role role, int num) {


        setNameFromNum(num, new_name);
        setRoleFromNum(num, role);

    }

}