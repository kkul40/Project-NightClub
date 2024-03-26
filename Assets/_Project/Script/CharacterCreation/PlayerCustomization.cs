using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Script.NewSystem.Data;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCustomization : MonoBehaviour
{
    public static PlayerCustomization Instance;

    //TODO CustomizationData dan cek bu degiskenleri
    public int playerGenderIndex;
    public int playerHairIndex;
    public int playerBeardIndex;
    public int playerAttachmentIndex;
    public int playerEaringIndex;
    
    [Header("Customization Variables")] 
    [SerializeField] private PlayerCustomizationDataSo playerCDS;
    [SerializeField] private SkinnedMeshRenderer playerGenderHolder;
    [SerializeField] private Transform playerHairHolder;
    [SerializeField] private Transform playerBeardHolder;
    [SerializeField] private Transform playerAttachmentHolder;
    [SerializeField] private Transform playerEaringHolder;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadCustomizedPlayer();
    }

    private void LoadCustomizedPlayer()
    {
        PlayerCustomizationIndexData data = SaveSystem.LoadCustomizedPlayer();
        if (data != null)
        {
            playerGenderIndex = data.playerGenderIndex;
            playerHairIndex = data.playerHairIndex;
            playerBeardIndex = data.playerBeardIndex;
            playerAttachmentIndex = data.playerAttachmentIndex;
            playerEaringIndex = data.playerEaringIndex;
        }

        playerGenderHolder.sharedMesh = playerCDS.playerGenders[playerGenderIndex];
        ChangePart(playerHairHolder, playerCDS.playerHairPrefabs[playerHairIndex]);
        ChangePart(playerBeardHolder, playerCDS.playerBeardPrefabs[playerBeardIndex]);
        ChangePart(playerAttachmentHolder, playerCDS.playerAttachtmentPrefabs[playerAttachmentIndex]);
        ChangePart(playerEaringHolder, playerCDS.playerEaringPrefabs[playerEaringIndex]);
    }

    public void OnMaleButton()
    {
        playerGenderIndex = 0;
        playerGenderHolder.sharedMesh = playerCDS.playerGenders[playerGenderIndex];
    }

    public void OnFemaleButton()
    {
        playerGenderIndex = 1;
        playerGenderHolder.sharedMesh = playerCDS.playerGenders[playerGenderIndex];
    }

    private void ChangePart(Transform partHolder,GameObject partPrefab)
    {
        for (int i = partHolder.childCount - 1; i >= 0; i--)
        {
            Debug.Log("Removed");
            var child = partHolder.GetChild(i).gameObject;
            Destroy(child);
        }

        if (partPrefab != null)
        {
            var newPart = Instantiate(partPrefab, partHolder);
        }
    }   

    public void OnHairPreviousButton()
    {
        playerHairIndex--;
        if (playerHairIndex < 0)
            playerHairIndex = playerCDS.playerHairPrefabs.Count - 1;
        
        ChangePart(playerHairHolder, playerCDS.playerHairPrefabs[playerHairIndex]);
    }

    public void OnHairNextButton()
    {
        playerHairIndex++;
        if (playerHairIndex > playerCDS.playerHairPrefabs.Count - 1)
            playerHairIndex = 0;
        
        ChangePart(playerHairHolder, playerCDS.playerHairPrefabs[playerHairIndex]);
    }

    public void OnBeardPreviousButton()
    {
        playerBeardIndex--;
        if (playerBeardIndex < 0)
            playerBeardIndex = playerCDS.playerBeardPrefabs.Count - 1;
        
        ChangePart(playerBeardHolder, playerCDS.playerBeardPrefabs[playerBeardIndex]);
    }

    public void OnBeardNextButton()
    {
        playerBeardIndex++;
        if (playerBeardIndex > playerCDS.playerBeardPrefabs.Count - 1)
            playerBeardIndex = 0;
        
        ChangePart(playerBeardHolder, playerCDS.playerBeardPrefabs[playerBeardIndex]);
    }

    public void OnAttachmentPreviousButton()
    {
        playerAttachmentIndex--;                                               
        if (playerAttachmentIndex < 0)                                         
            playerAttachmentIndex = playerCDS.playerAttachtmentPrefabs.Count - 1;                     
                                                                  
        ChangePart(playerAttachmentHolder, playerCDS.playerAttachtmentPrefabs[playerAttachmentIndex]);      
    }

    public void OnAttachmentNextButton()
    {
        playerAttachmentIndex++;                                                          
        if (playerAttachmentIndex > playerCDS.playerAttachtmentPrefabs.Count - 1)                                
            playerAttachmentIndex = 0;                                                    
                                                                            
        ChangePart(playerAttachmentHolder, playerCDS.playerAttachtmentPrefabs[playerAttachmentIndex]);                 
    }

    public void OnEaringPreviousButton()
    {
        playerEaringIndex--;                                                                
        if (playerEaringIndex < 0)                                                          
            playerEaringIndex = playerCDS.playerEaringPrefabs.Count - 1;                                
                                                                                        
        ChangePart(playerEaringHolder, playerCDS.playerEaringPrefabs[playerEaringIndex]);           
    }

    public void OnEaringNextButton()
    {
        playerEaringIndex++;                                                                   
        if (playerEaringIndex > playerCDS.playerEaringPrefabs.Count - 1)                                   
            playerEaringIndex = 0;                                                             
                                                                                           
        ChangePart(playerEaringHolder, playerCDS.playerEaringPrefabs[playerEaringIndex]);              
    }

    public void FinishUpCustomization()
    {
        SaveSystem.SaveCustomizedPlayer(this);
    }
}
