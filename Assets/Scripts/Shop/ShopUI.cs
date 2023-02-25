using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShopSystem
{
    public class ShopUI : MonoBehaviour
    {

        [SerializeField] public static int totalFame;
        [SerializeField] private SaveLoadData saveLoadData;

        public GameObject[] weaponModels;                       //list to all the 3D models of items
        public ShopData shopData;                       //ref to ShopSaveScriptable asset
        public Text unlockBtnText, upgradeBtnText, levelText, weaponNameText, selectBtnText; //ref to important text components
        public Text damageText, totalFameText, newUpgradeDamageText;
        public Button unlockBtn, upgradeBtn, nextBtn, previousButton, selectBtn;   //ref to important Buttons
        public GameObject lockedImage;

        public int currentIndex = 0;                       //index of current item showing in the shop 
        public int selectedIndex;                          //actual selected item index
        public int currentLevel;
        public static int weaponDamage;
        public static string weaponName;
        [SerializeField]
        private AudioSource unlockSound;
        [SerializeField]
        private AudioSource upgradeSound;
        [SerializeField]
        private AudioSource prevnextSound;



        private void Start()
        {
            saveLoadData.Initialize();                      //Initialize , load or save default data and load data
            Reminder();
            PlayerPrefs.SetInt("WeaponDamage", weaponDamage);
            selectedIndex = PlayerPrefs.GetInt("SelectedItem", 0);  //get the selectedIndex from PlayerPrefs
            currentIndex = selectedIndex;                           //set the currentIndex
            totalFameText.text = "" + totalFame;



            SetWeaponInfo();

            unlockBtn.onClick.AddListener(() => UnlockButton());      //add listner to button
            upgradeBtn.onClick.AddListener(() => UpgradeButton());          //add listner to button
            selectBtn.onClick.AddListener(() => SelectButton());      //add listner to button
            nextBtn.onClick.AddListener(() => NextButton());                //add listner to button
            previousButton.onClick.AddListener(() => PreviousButton());     //add listner to button

            if (currentIndex == 0) previousButton.interactable = false;     //dont interact previousButton if currentIndex is 0
                                                                            //dont interact previousButton if currentIndex is shopItemList.shopItems.Length - 1
            if (currentIndex == shopData.shopItems.Length - 1) nextBtn.interactable = false;

            weaponModels[currentIndex].SetActive(true);                         //activate the object at currentIndex
            UnlockButtonStatus();
            UpgradeButtonStatus();
            SelectButtonStatus();
        }

        void SetWeaponInfo()
        {
            /*
            theTarget = initialObject;

            initialMesh = initialObject.GetComponent<MeshFilter>().mesh;
            swapMesh = swapObject.GetComponent<MeshFilter>().mesh;

            theTarget.GetComponent<MeshFilter>().mesh = swapMesh;
            */

            weaponNameText.text = shopData.shopItems[currentIndex].itemName;
            currentLevel = shopData.shopItems[currentIndex].unlockedLevel;
            levelText.text = "Level:" + (currentLevel + 1); //level start from zero we add 1
            damageText.text = "Damage:" + shopData.shopItems[currentIndex].weaponLevel[currentLevel].damage;
            if(shopData.shopItems[currentIndex].weaponLevel.Length > currentLevel+1)
            {
                newUpgradeDamageText.text = "DMG. +" + (shopData.shopItems[currentIndex].weaponLevel[currentLevel + 1].damage - shopData.shopItems[currentIndex].weaponLevel[currentLevel].damage);
            }
            else
            {
                newUpgradeDamageText.text = "DMG";

            }

            //weaponDamage = shopData.shopItems[currentIndex].weaponLevel[currentLevel].damage;
            ChangeWeaponDamage(shopData.shopItems[currentIndex].weaponLevel[currentLevel].damage);
            // eskiden bilgileri burda alıyordu onu aşşağıya taşıdım

        }

        /// <summary>
        /// Method called on Next button click
        /// </summary>
        public void NextButton()
        {
            //check if currentIndex is less than the total shope items we have - 1
            if (currentIndex < shopData.shopItems.Length - 1)
            {
                weaponModels[currentIndex].SetActive(false);                     //deactivate old model
                currentIndex++;                                             //increase count by 1
                weaponModels[currentIndex].SetActive(true);                      //activate the new model
                SetWeaponInfo();                                               //set car information
                PrevNextSound();

                //check if current index is equal to total items - 1
                if (currentIndex == shopData.shopItems.Length - 1)
                {
                    nextBtn.interactable = false;                           //then set nextBtn interactable false
                }

                if (!previousButton.interactable)                           //if previousButton is not interactable
                {
                    previousButton.interactable = true;                     //then set it interactable
                }

                UnlockButtonStatus();
                UpgradeButtonStatus();
                SelectButtonStatus();
            }
        }

        /// <summary>
        /// Method called on Previous button click
        /// </summary>
        public void PreviousButton()
        {
            if (currentIndex > 0)                           //we check is currentIndex i more than 0
            {
                weaponModels[currentIndex].SetActive(false);     //deactivate old model
                currentIndex--;                             //reduce count by 1
                weaponModels[currentIndex].SetActive(true);      //activate the new model
                SetWeaponInfo();                               //set car information
                PrevNextSound();


                if (currentIndex == 0)                      //if currentIndex is 0
                {
                    previousButton.interactable = false;    //set previousButton interactable to false
                }

                if (!nextBtn.interactable)                  //if nextBtn interactable is false
                {
                    nextBtn.interactable = true;            //set nextBtn interactable to true
                }
                UnlockButtonStatus();
                UpgradeButtonStatus();
                SelectButtonStatus();
            }
        }

        /// <summary>
        /// Method called on Unlock button click
        /// </summary>
        /// 
        private void SelectButton()
        {
            selectBtn.interactable = false;
            selectBtnText.text = "Selected";
            selectedIndex = currentIndex;                       //set the selectedIndex to currentIndex
            PlayerPrefs.SetInt("SelectedItem", selectedIndex);  //save the selectedIndex
            ChangeWeaponName(weaponModels[currentIndex].name);
        }
        private void UnlockButton()
        {
            bool yesUnlocked = false;   //local bool
            if (shopData.shopItems[currentIndex].isUnlocked)    //if shop item at currentIndex is already unlocked
            {
                yesUnlocked = true;                             //set yesSelected to true
            }
            else if (!shopData.shopItems[currentIndex].isUnlocked)  //if shop item at currentIndex is not unlocked
            {
                //check if we have enough coins to unlock it
                if (totalFame >= shopData.shopItems[currentIndex].unlockCost)
                {
                    //if yes then reduce the cost coins from our total coins
                    //totalFame -= shopData.shopItems[currentIndex].unlockCost;
                    ChangeTotalFame(-shopData.shopItems[currentIndex].unlockCost);
                    totalFameText.text = "" + totalFame;          //set the coins text
                    yesUnlocked = true;                             //set yesSelected to true
                    shopData.shopItems[currentIndex].isUnlocked = true; //mark the shop item unlocked
                    UnlockSound();
                    UpgradeButtonStatus();
                }
            }

            if (yesUnlocked)
            {
                                
                
                unlockBtn.gameObject.SetActive(false);
                lockedImage.gameObject.SetActive(false);
                upgradeBtn.gameObject.SetActive(true);
                selectBtn.gameObject.SetActive(true);
                unlockBtn.interactable = false;                     //set unlockBtn interactable to false
                                                                    //weaponName =  weaponModels[currentIndex].name;
               // ChangeWeaponName(weaponModels[currentIndex].name);
            }

        }

        /// <summary>
        /// Method called on Upgrade button click
        /// </summary>
        private void UpgradeButton()//upgrade button is interactable only if we have any level left to upgrade
        {
            //get the next level index
            int nextLevelIndex = shopData.shopItems[currentIndex].unlockedLevel + 1;
            //we check if we have enough coins
            if (totalFame >= shopData.shopItems[currentIndex].weaponLevel[nextLevelIndex].unlockCost)
            {
                //totalFame -= shopData.shopItems[currentIndex].weaponLevel[nextLevelIndex].unlockCost;
                ChangeTotalFame(-shopData.shopItems[currentIndex].weaponLevel[nextLevelIndex].unlockCost);
                totalFameText.text = "" + totalFame;          //set the coins text
                                                              //if yes we increate the unlockedLevel by 1
                shopData.shopItems[currentIndex].unlockedLevel++;
                //weaponDamage = shopData.shopItems[currentIndex].weaponLevel[currentLevel].damage;
                ChangeWeaponDamage(shopData.shopItems[currentIndex].weaponLevel[currentLevel].damage);
                UpgradeSound();

                //we check if are not at max level
                if (shopData.shopItems[currentIndex].unlockedLevel < shopData.shopItems[currentIndex].weaponLevel.Length - 1)
                {
                    upgradeBtnText.text = shopData.shopItems[currentIndex].weaponLevel[nextLevelIndex + 1].unlockCost.ToString();

                }
                else    //we check if we are at max level
                {
                    upgradeBtn.interactable = false;            //set upgradeBtn interactable to false
                    upgradeBtnText.text = "MAX";    //set the btn text
                    newUpgradeDamageText.text = "DMG. MAX";
                }

                SetWeaponInfo();
            }
        }

        /// <summary>
        /// This method is called when we are changing the current item in the shop
        /// This method set the interactablity and text of unlock btn
        /// </summary>
        private void UnlockButtonStatus()
        {
            //if current item is unlocked
            if (shopData.shopItems[currentIndex].isUnlocked)
            {
                lockedImage.gameObject.SetActive(false);
                unlockBtn.gameObject.SetActive(false);
                upgradeBtn.gameObject.SetActive(true);
                selectBtn.gameObject.SetActive(true);
                
                
            }
            else if (!shopData.shopItems[currentIndex].isUnlocked) //if current item is not unlocked
            {
                lockedImage.gameObject.SetActive(true);
                unlockBtn.gameObject.SetActive(true);
                upgradeBtn.gameObject.SetActive(false);
                selectBtn.gameObject.SetActive(false);
                unlockBtn.interactable = true;  //set the unlockbtn interactable
                unlockBtnText.text = shopData.shopItems[currentIndex].unlockCost + ""; //set the text as cost of item
            }
        }
        private void SelectButtonStatus()
        {
            if (shopData.shopItems[currentIndex].isUnlocked)
            {
                selectBtn.interactable = selectedIndex != currentIndex ? true : false;
                selectBtnText.text = selectedIndex == currentIndex ? "Selected" : "Select";
            }
            else if (!shopData.shopItems[currentIndex].isUnlocked) //if current item is not unlocked
            {
                selectBtn.interactable = true;  //set the unlockbtn interactable
                selectBtnText.text = "Select";
            }

        }

        /// <summary>
        /// Method to set Upgrade button interactable and text sttus
        /// </summary>
        private void UpgradeButtonStatus()
        {
            //if current item is unlocked
            if (shopData.shopItems[currentIndex].isUnlocked)
            {
                //if unlockLevel of current item is less than its max level
                if (shopData.shopItems[currentIndex].unlockedLevel < shopData.shopItems[currentIndex].weaponLevel.Length - 1)
                {
                    upgradeBtn.interactable = true;                     //make upgradeBtn interactable true
                    int nextLevelIndex = shopData.shopItems[currentIndex].unlockedLevel + 1;
                    //set the next level as value of upgrade button text
                    upgradeBtnText.text = shopData.shopItems[currentIndex].weaponLevel[nextLevelIndex].unlockCost.ToString();
                    
                }
                else   //if unlockLevel of current item is equal to max level
                {
                    upgradeBtn.interactable = false;                    //make upgradeBtn interactable false
                    upgradeBtnText.text = "MAX";
                }
            }
            else if (!shopData.shopItems[currentIndex].isUnlocked)  //if current item is not unlocked
            {
                upgradeBtn.interactable = false;                        //make upgradeBtn interactable false
                upgradeBtnText.text = "Locked";
            }
        }

        private void ChangeTotalFame(int change)
        {
            totalFame += change;
            PlayerPrefs.SetInt("TotalFame", totalFame);
        }

        private void ChangeWeaponDamage(int newDamage)
        {
            weaponDamage = newDamage;
            PlayerPrefs.SetInt("WeaponDamage", weaponDamage);
        }

        private void ChangeWeaponName(string newName)
        {
            weaponName = newName;
            PlayerPrefs.SetString("WeaponName", weaponName);
        }

        private void Reminder()
        {
            if (!PlayerPrefs.HasKey("TotalFame"))
            {
                totalFame = 999;
                PlayerPrefs.SetInt("TotalFame", 999);
            }
            else
            {
                totalFame = PlayerPrefs.GetInt("TotalFame");
            }

            if (!PlayerPrefs.HasKey("WeaponDamage"))
            {
                weaponDamage = 31;
                PlayerPrefs.SetInt("WeaponDamage", 31);
            }
            else
            {
                weaponDamage = PlayerPrefs.GetInt("WeaponDamage");
            }


            if (!PlayerPrefs.HasKey("WeaponName"))
            {
                weaponName = "axe1H1";
                PlayerPrefs.SetString("WeaponName", "axe1H1");
            }
            else
            {
                weaponName = PlayerPrefs.GetString("WeaponName");
            }
        }
        public void UnlockSound()
        {
            unlockSound.Play();
        }
        public void UpgradeSound()
        {
            upgradeSound.Play();
        }
        public void PrevNextSound()
        {
            prevnextSound.Play();
        }
    }


}
