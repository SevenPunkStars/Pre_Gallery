using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Normal.Realtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using TMPro;
using Oculus.Interaction.Input;
using Unity.VisualScripting;


public class AvatarController : MonoBehaviour
{

    public GameObject watch;
    [HideInInspector] public Transform originalHips;
    public Transform hips;

    //[HideInInspector] public OVRHand rightHand;
    [HideInInspector] public Transform indexJoint1;
    [HideInInspector] public Transform indexJoint2;
    [HideInInspector] public Transform middleJoint;

    [HideInInspector] public Hand rh;
    public GameObject pointer;
    public GameObject tshirt;
    public RealtimeTransform skeleton;

    private RealtimeView _realtimeView;
    private ExperimentDB db;

    public String user_name;
    String old_user_name = "";

    [HideInInspector] public Material pointer_color;
    [HideInInspector] public Material tshirt_color;

    public AvatarManager.Role role;
    [HideInInspector] public AvatarManager.Role old_role;

    public TextMeshPro NameTM;
    public Transform nameHolder;


    public bool isPointingB;

    public Transform headPos;

    private Camera camera;

    GameObject environment_control_panel;
    GameObject ovrManager; 

    // Start is called before the first frame update
    private void Start()
    {
        camera = Camera.main;
        environment_control_panel = GameObject.FindWithTag("ControlPanel");
        db = FindObjectOfType<ExperimentDB>();

        // Call LocalStart() only if this instance is owned by the local client
        if (_realtimeView.isOwnedLocallyInHierarchy) {
        
            // Request ownership of the Player and the character RealtimeTransforms
            GetComponent<RealtimeTransform>().RequestOwnership();
            ownSkeleton(skeleton);

            user_name = role.ToString() + "#" + getRoleNb().ToString();
            
            db.Add(user_name, role.ToString(), _realtimeView.ownerIDSelf.ToString());

        }

        updateName();
        updateRole();


    }

    private int getRoleNb() {

        return db.RoleNb(role);
    }



    private void Awake()
    {
        // Set physics timestep to 60hz
        Time.fixedDeltaTime = 1.0f / 60.0f;


        // Store a reference to the RealtimeView for easy access
        _realtimeView = GetComponent<RealtimeView>();

        //Application.quitting += removeFromList;
    }


    //private void LocalStart()
    //{
    //    db = FindObjectOfType<AvatarDB>();
    //    db.Add(user_name, role.ToString(), _realtimeView.ownerIDSelf.ToString());

    //    // Request ownership of the Player and the character RealtimeTransforms
    //    GetComponent<RealtimeTransform>().RequestOwnership();
    //    ownSkeleton(skeleton);



    //}

    void OnDestroy()
    {
        removeFromList();

    }
    

    void removeFromList()
    {

            db.Remove(_realtimeView.ownerIDSelf.ToString());
    }


    void ownSkeleton(RealtimeTransform bone)
    {

        bone.RequestOwnership();
        foreach(Transform child in bone.transform.transform)
        {
            if (child.GetComponent<RealtimeTransform>() != null) ownSkeleton(child.GetComponent<RealtimeTransform>());
        }
    }



    // Update is called once per frame
    void Update()
    {
        watch.SetActive(role == AvatarManager.Role.Psychatrist || role == AvatarManager.Role.ArtCommunicator);
        if (db == null) { db = FindObjectOfType<ExperimentDB>(); }
        ;
        
        if (GetComponent<RealtimeView>().isOwnedLocallyInHierarchy)
        {
            environment_control_panel.SetActive(role == AvatarManager.Role.Psychatrist || role == AvatarManager.Role.Psychatrist);

            
            UpdateSkeletton(hips, originalHips);


            if (isPointing() != db.FindPointingById(_realtimeView.ownerIDSelf.ToString()))
            {
                db.setPointingFromID(_realtimeView.ownerIDSelf.ToString(), isPointing());

            }

            rotateAllNames();
            
        }

        checkUpdateName();
        checkUpdateRole();
        checkUpdatePointer();

    }

    private void rotateAllNames()
    {

        //AvatarController[] avatarsInScene = FindObjectsByType<AvatarController>(FindObjectsSortMode.None);
        AvatarController[] avatarsInScene = FindObjectsOfType<AvatarController>();
        foreach (AvatarController avatar in avatarsInScene) {


            //avatar.nameHolder.transform.position = Vector3.zero;
            if (avatar != this)
            {
                avatar.nameHolder.transform.position = avatar.headPos.position + new Vector3(0, 0.4f, 0);
                avatar.nameHolder.transform.LookAt(camera.transform.position);
            }
            else
            {
                avatar.nameHolder.transform.localScale = Vector3.zero;
            }


            //avatar.nameHolder.transform.position = avatar.headPos.position+ new Vector3(0,0.3f,0);
            //avatar.nameHolder.transform.LookAt(camera.transform.position);
            //avatar.nameHolder.transform.rotation = Quaternion.AngleAxis(180, Vector3.up) * transform.rotation;

        }
    }

    void checkUpdatePointer()
    {
        

        if (isPointingB != db.FindPointingById(_realtimeView.ownerIDSelf.ToString()))
        {
            updatePointing();
        }

    }

    void updatePointing()
    {
        isPointingB = db.FindPointingById(_realtimeView.ownerIDSelf.ToString());
        pointer.SetActive(db.FindPointingById(_realtimeView.ownerIDSelf.ToString()));
    }

    void checkUpdateName()
    {


        if (old_user_name != getName())
        {


            updateName();
        }
    }
    void updateName()
    {
        user_name = getName();
        old_user_name = user_name;

        NameTM.text = user_name;

    }

    private string getName()
    {

        return db.FindNameById(_realtimeView.ownerIDSelf.ToString());
    }




    void checkUpdateRole()
    {
        
        if (old_role != getRole())
        {
            updateRole();
        }
        
    }
    void updateRole()
    {
        role = getRole();
        old_role = role;

        string roleName = ((role == AvatarManager.Role.Psychatrist) || (role == AvatarManager.Role.ArtCommunicator)) ? "Coordinator" : ((role == AvatarManager.Role.Teenager) ? "Teenager" : "Eldery");
        string pointer_name = roleName;
        string tshirt_name = roleName + " #" + getRoleNb().ToString();

        pointer_color = Resources.Load<Material>("PointerColor/" + pointer_name);
        tshirt_color = Resources.Load<Material>("TshirtColor/" + tshirt_name);

        Renderer renderer = gameObject.GetComponent<Renderer>();

        pointer.GetComponent<Renderer>().material = pointer_color;
        tshirt.GetComponent<Renderer>().material = tshirt_color;
    }

    private AvatarManager.Role getRole()
    {
        return db.FindRoleById(_realtimeView.ownerIDSelf.ToString());
    }



    void UpdateSkeletton(Transform bone, Transform original)
    {


        String[] toIgnore = new string[] { "wristwatch", "pointer", "Canvas", "Cube", "Text (TMP)", "Text Holder" };

        bone.rotation = original.rotation;
        bone.position = original.position;


        foreach (Transform child in bone)
        {
            if (!contains (toIgnore, child.name))
            {
                UpdateSkeletton(child, original.Find(child.name));
            }
            
        }
    }

    public static bool contains(String[] array, String s)
    {
        foreach (String item in array)
        {
            if (item.Equals(s))
            {
                return true;
            }
        }
        return false;
    }

    //public float getClosure(OVRHand.HandFinger finger)
    //{

    //    float pinchStrength = rightHand.GetFingerPinchStrength(finger);
    //    //Debug.Log(rh.GetFingerPinchStrength(HandFinger.Index));
    //    return pinchStrength;
    //}

    public bool isPointing()
    {
        return (indexJoint1.localEulerAngles.z > 320 || indexJoint1.localEulerAngles.z < 20) && (indexJoint2.localEulerAngles.z > 320 || indexJoint2.localEulerAngles.z < 20);
        //return middleJoint.localEulerAngles.z < 310 && indexJoint.localEulerAngles.z > 320;
        
        //Debug.Log(getClosure(OVRHand.HandFinger.Index) + " " + getClosure(OVRHand.HandFinger.Middle) + " " + getClosure(OVRHand.HandFinger.Thumb));
        //return (getClosure(OVRHand.HandFinger.Index) == 0 && getClosure(OVRHand.HandFinger.Middle) > 0.1f && getClosure(OVRHand.HandFinger.Thumb) > 0.1f);


    }
}
