using UnityEngine;
using UnityEngine.Animations;
using Normal.Realtime;
using Oculus.Interaction.Input;


public class AvatarManager : MonoBehaviour
{
    
    public enum Role
    {
        Psychatrist,
        ArtCommunicator,
        Eldery,
        Teenager

    }
    private Realtime _realtime;
    public Role role;
    private AvatarController avatarController;


    public Transform originalHips;
    //public OVRHand rightHand;

    public Transform indexJoint1;
    public Transform indexJoint2;
    public Transform middleJoint;



    public Transform[] spawn_positions;
    public Transform tracking_origin;

    private void Awake()
    {
        // Get the Realtime component on this game object
        _realtime = GetComponent<Realtime>();

        // Notify us when Realtime successfully connects to the room
        _realtime.didConnectToRoom += DidConnectToRoom;


    }

    Transform getSpawnPosition()
    {
        string roleStr = role.ToString();
        string nb = (FindObjectOfType<ExperimentDB>().RoleNb(role) +1).ToString();

        Debug.Log((roleStr + " #" + nb));
        foreach (Transform t in spawn_positions)
        {
            if (t.name == (roleStr + " #" + nb)) return t;
        }
        
        return spawn_positions[0];
    }

    private void DidConnectToRoom(Realtime realtime)
    {
        Transform spawnPosition = getSpawnPosition();
        tracking_origin.position = spawnPosition.position;
        tracking_origin.rotation = spawnPosition.rotation;

        // Instantiate the Player for this client once we've successfully connected to the room
        GameObject playerGameObject = Realtime.Instantiate(prefabName: "Avatar 2.0",  // Prefab name
                                                                      ownedByClient: true,      // Make sure the RealtimeView on this prefab is owned by this client
                                                           preventOwnershipTakeover: true,      // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                                                                        useInstance: realtime); // Use the instance of Realtime that fired the didConnectToRoom event.

        avatarController =  playerGameObject.GetComponent<AvatarController>();
        //avatarController.watch.SetActive(role==Role.Psychatrist);
        avatarController.originalHips = originalHips;

        //avatarController.rightHand = rightHand;
        avatarController.indexJoint1 = indexJoint1;
        avatarController.indexJoint2 = indexJoint2;
        avatarController.middleJoint = middleJoint;



        avatarController.role = role;
        avatarController.user_name = "test";





    }

    public static Role stringToRole( string s)
    {
        if (s == "Psychatrist") return Role.Psychatrist;
        if (s == "Eldery") return Role.Eldery;
        if (s == "Teenager") return Role.Teenager;
        if (s == "ArtCommunicator") return Role.ArtCommunicator;
        return Role.Psychatrist;
    }

}