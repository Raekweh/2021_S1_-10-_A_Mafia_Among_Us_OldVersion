using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class WaitingRoomController : MonoBehaviour
{
    PhotonView myPV;
    [SerializeField] float timeToStart;
    float timerToStart;
    bool readyToStart;
    [SerializeField] GameObject StartButton;
    [SerializeField] Text countDownDisplay;
    [SerializeField] int nextLevel;
    [SerializeField] InputField usernameField;
    List<AU_PlayerController> playersInGame;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        timerToStart = timeToStart;

        playersInGame = AU_PlayerController.playersInGame;
        
    }

    // Update is called once per frame
    void Update()
    {
        StartButton.SetActive(PhotonNetwork.IsMasterClient);

        if(playersInGame==null){
            playersInGame = AU_PlayerController.playersInGame;
        }
        
            if(readyToStart)
            {
                timerToStart -= Time.deltaTime;
                countDownDisplay.text = ((int)timerToStart).ToString();
            }
            else
            {
                timerToStart = timeToStart;
                countDownDisplay.text = "";
            }
        if (PhotonNetwork.IsMasterClient)
        {
            if (timerToStart <= 0)
            {
                timerToStart = 100;
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.LoadLevel(nextLevel);
            }
        }
        
    }
    
    public void Play()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            myPV.RPC("RPC_Play", RpcTarget.All);
        }
    }
    
    [PunRPC]
    void RPC_Play()
    {
        readyToStart = !readyToStart;
    }
}