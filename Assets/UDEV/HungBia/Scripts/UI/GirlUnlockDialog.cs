using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Thirdweb;

public class GirlUnlockDialog : Dialog
{
    public Text btnTxt;
    public Image girlHud;
    private Contract contract;

    //Blockchain
    private ThirdwebSDK sdk;
    public TMPro.TextMeshProUGUI claimmingStatusTxt;
    public TMPro.TextMeshProUGUI nftTotalBalanceTxt;
    private string ContractAddress = "0x343480F820B722265C249473A3fFc81dBd280c08";
    private string ContractAddressLevel1 = "0x3C2Cbddc22C6Af8209163Fc4BC64be445be9501a";
    private string ContractAddressLevel2 = "0xF266fdc69625D668000BB25598791d3D5C269852";
    private string ContractAddressLevel3 = "0xf083598Eda88BDfADb21E7331E79746FA0f8421E";
    public GameObject claimNFTBtn;

    public Button playBtn;

    private void Start()
    {
        sdk = ThirdwebManager.Instance.SDK;
    }

    public override void Show(bool isShow)
    {
        base.Show(isShow);
        if (isShow) {
            claimNFTBtn.SetActive(true);
            claimmingStatusTxt.gameObject.SetActive(false);
            nftTotalBalanceTxt.gameObject.SetActive(false);
        }
        if (girlHud)
        {
            girlHud.sprite = GameManager.Ins.CurLevel.unlockImg;
        }
        if (btnTxt)
        {
            btnTxt.text = "Next Challenge !!!";
        }
        if (GameManager.Ins.gameState == GameState.Gameover)
        {
            AudioController.Ins.PlaySound(AudioController.Ins.win);
        }
    }
    public async void GetNFTBalance()
    {
        claimmingStatusTxt.text = "Getting balance...";
        string address = await sdk.Wallet.GetAddress();
        List<NFT> nftList = await contract.ERC721.GetOwned(address);
        claimmingStatusTxt.text = "NFT Total Balance";
        if (nftList.Count > 0)
        {
            nftTotalBalanceTxt.text = nftList.Count.ToString();
        }
        else
        {
            nftTotalBalanceTxt.text = "0";
        }
        claimmingStatusTxt.gameObject.SetActive(true);
        nftTotalBalanceTxt.gameObject.SetActive(true);
    }

    public async void ClaimNFT()
    {
        claimNFTBtn.SetActive(false);
        playBtn.interactable = false;
        claimmingStatusTxt.text = "Claiming NFT...";
        if (GameManager.Ins.CurLevelIdx == 1)
        {
            contract = sdk.GetContract(ContractAddress);
        }
        else if (GameManager.Ins.CurLevelIdx == 2)
        {
            contract = sdk.GetContract(ContractAddressLevel1);
        }
        else if (GameManager.Ins.CurLevelIdx == 3)
        {
            contract = sdk.GetContract(ContractAddressLevel2);
        }
        else
        {
            contract = sdk.GetContract(ContractAddressLevel3);
        }
        var data = await contract.ERC721.Claim(1);
        GetNFTBalance();
        playBtn.interactable = true;
    }
}
