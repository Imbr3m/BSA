using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject joinPopup;
    [SerializeField] private TextMeshProUGUI joinPopupText;
    [SerializeField] private GameObject agustinIndicatorUI;

    private bool infrontOfPartyMember;
    private GameObject joinableMember;
    private PlayerControls playerControls;
    private List<GameObject> overworldCharacters = new List<GameObject>();

    private const string PARTY_JOINED_MESSAGE = " Joined The Party!";
    private const string NPC_JOINABLE_TAG = "NPCJoinable";

    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    // Start is called before the first frame update testing
    void Start()
    {
        playerControls.Player.Interact.performed += _ => Interact();
        if (agustinIndicatorUI != null) agustinIndicatorUI.SetActive(false);
        SpawnOverworldMembers();
        
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void Interact()
    {
        if (infrontOfPartyMember == true && joinableMember != null)
        {
            MemberJoined(joinableMember.GetComponent<JoinableCharacterScript>().MemberToJoin);//add member
            infrontOfPartyMember = false;
            joinableMember = null;
        }
    }

    private void MemberJoined(PartyMemberInfo partyMember)
    {
        GameObject.FindFirstObjectByType<PartyManager>().AddMemberToPartyByName(partyMember.MemberName);// add party member
        joinableMember.GetComponent<JoinableCharacterScript>().CheckIfJoined();// disable joinable member
        // join pop up
        joinPopup.SetActive(true);
        joinPopupText.text = partyMember.MemberName + PARTY_JOINED_MESSAGE;

        if (partyMember.MemberName == "Soldier") 
        {
            if (agustinIndicatorUI != null) agustinIndicatorUI.SetActive(true);
        }

        SpawnOverworldMembers(); // adding an overworld member
    }

 
    private void SpawnOverworldMembers()
    {
        // 1. Destroy old followers, but DO NOT destroy the Player!
        for (int i = 0; i < overworldCharacters.Count; i++)
        {
            if (overworldCharacters[i] != gameObject) 
            {
                Destroy(overworldCharacters[i]);
            }
        }
        overworldCharacters.Clear();

        PartyManager partyManager = GameObject.FindFirstObjectByType<PartyManager>();
        if (partyManager == null) return;

        List<PartyMember> currentParty = partyManager.GetCurrentParty();
        if (currentParty == null) return;

        for (int i = 0; i < currentParty.Count; i++)
        {
            if (i == 0) 
            {
                overworldCharacters.Add(gameObject); 
            }
            else 
            {
                Vector3 positionToSpawn = transform.position;
                positionToSpawn.x -= i * 2.0f;

                GameObject tempFollower = Instantiate(currentParty[i].MemberOverworldVisualPrefab, positionToSpawn, Quaternion.identity);

                MemberFollowAI followerAI = tempFollower.GetComponent<MemberFollowAI>();
                if (followerAI == null) followerAI = tempFollower.GetComponentInChildren<MemberFollowAI>();
                
                if (followerAI != null)
                {
                    followerAI.enabled = true;
                    // Tell them to follow the person in front of them
                    followerAI.SetFollowTarget(overworldCharacters[i - 1].transform);
                }
                
                overworldCharacters.Add(tempFollower); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == NPC_JOINABLE_TAG)
        {
            //enable our prompt
            infrontOfPartyMember = true;
            joinableMember = other.gameObject;
            joinableMember.GetComponent<JoinableCharacterScript>().ShowInteractPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == NPC_JOINABLE_TAG)
        {
            //disable our prompt
            infrontOfPartyMember = false;
            joinableMember.GetComponent<JoinableCharacterScript>().ShowInteractPrompt(false);
            joinableMember = null;
        }
    }
}
