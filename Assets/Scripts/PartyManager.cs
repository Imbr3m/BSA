using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] private PartyMemberInfo[] allMembers;
    [SerializeField] private List<PartyMember> currentParty;

    [SerializeField] private PartyMemberInfo defaultPartyMember;

    private static GameObject instance;

    private void Awake()
    {
        if (currentParty == null) currentParty = new List<PartyMember>();

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this.gameObject;
            AddMemberToPartyByName(defaultPartyMember.MemberName);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void AddMemberToPartyByName(string memberName)
    {
        if (currentParty == null) currentParty = new List<PartyMember>();
        // Prevent duplicates (avoids spawning the same overworld visual multiple times).
        for (int j = 0; j < currentParty.Count; j++)
        {
            if (currentParty[j].MemberName == memberName) return;
        }

        for (int i = 0; i < allMembers.Length; i++)
        {
            if (allMembers[i].MemberName == memberName)
            {
                PartyMember newPartyMember = new PartyMember();
                newPartyMember.MemberName = allMembers[i].MemberName;
                newPartyMember.MemberOverworldVisualPrefab = allMembers[i].MemberOverworldVisualPrefab;

                currentParty.Add(newPartyMember);
            }
        }
    }

    public List<PartyMember> GetCurrentParty()
    {
        return currentParty;
    }
}

[System.Serializable]
public class PartyMember
{
    public string MemberName;
    public GameObject MemberOverworldVisualPrefab;
}
