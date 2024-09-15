using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlockchainManager : MonoBehaviour
{
    public string Address { get; private set; }

    public Button nftButton;
    public Button playButton;

    public Text nftButtonText;

    string NFTAddressSmartContract = "0xC2CaEEAB81cC8f120E11818a403A8E5fc1201445";

    private void Start()
    {
        nftButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
    }

    public async void Login()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Debug.Log(Address);
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContract);
        List<NFT> nftList = await contract.ERC721.GetOwned(Address);
        if (nftList.Count == 0)
        {
            nftButton.gameObject.SetActive(true);
        }
        else
        {
            playButton.gameObject.SetActive(true);
        }
    }

    public async void ClaimNFTPass()
    {
        nftButtonText.text = "Claiming...";
        nftButton.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContract);
        var result = await contract.ERC721.ClaimTo(Address, 1);
        nftButtonText.text = "Claimed NFT Pass!";
        nftButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
    }

    public void PlayGame()
    {
        
    }
}
