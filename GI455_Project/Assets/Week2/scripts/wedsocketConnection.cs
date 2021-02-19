using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System;

namespace Week2
{

    public class wedsocketConnection : MonoBehaviour
    {
        

        struct SocketEvent
        {
            public string eventName;
            public string data;

            public SocketEvent(string eventName, string data)
            {
                this.eventName = eventName;
                this.data = data;
            }
        }


        string tempMessageString;



    //ชื่อผู่ใช้
        string name1;
        //รับค่า
        public Text NameN1;
        public Text UserMe;
        public Text IPnum;
        public Text Portnum;
        //โชว์ข้อความ
        public Text Showstatus;
        //public Text ShowUserMetext;
        //public Text ShowUserYourtext;
        //เก็บเป็นข้อความ
        string Metalk;
        string Yourtalk;
        string numIp;
        string numPort;
        //ตัวtextข้อความ
        public GameObject textspawnUser;
        public GameObject textspawnYour;
        public GameObject prarent;
        public GameObject spawntextpostUser;
        public GameObject spawntextpostYour;

    //เซ็ทค่า

    //เก็บUI
    //---------------------------------------------------------------------//
         public Text nameRoominput;
         public Text joinRoominput;
         public Text statusCreate;
         public Text statusJoin;
         public Text nameRoom;

        public Text ShowtextCreate;
        public Text ShowtextJoin;


        string NRinput;
        string JRinput;


        public GameObject UI1;
        public GameObject Room;
        public GameObject ConnectUI;

        public List<GameObject> counttext = new List<GameObject>();
        

        private WebSocket websocket;
         

        // Start is called before the first frame update
        void Start()
        {


            

            
            
        }

        public void Connect()
        {
            websocket = new WebSocket("ws://127.0.0.1:24563/");



            websocket.OnMessage += OnMessage;

            websocket.Connect();

            Room.SetActive(true);
            UI1.SetActive(true);
            ConnectUI.SetActive(false);
        }



        public void ConnectServer()
        {
            

            NRinput = nameRoominput.text;

            CreateRoom(NRinput);

            //โชว์ชื่อห้อง
            nameRoom.text = NRinput;

        }

        public void JoinSever()
        {

           

            JRinput = joinRoominput.text;

            nameRoom.text = JRinput;

            JoinRoom(JRinput);

        }

         public void JoinRoom(string roomName) 
         {

            if (websocket.ReadyState == WebSocketState.Open)
            {

                SocketEvent socketEvent = new SocketEvent("JoinRoom", roomName);

                string jsonStr = JsonUtility.ToJson(socketEvent);


                websocket.Send(jsonStr);




            }


        }





        public void CreateRoom(string roomName)
        {
            if (websocket.ReadyState == WebSocketState.Open)
            {


                SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName);

                string jsonStr = JsonUtility.ToJson(socketEvent);

                websocket.Send(jsonStr);


           
            
            }
        }

        public void LeaveRoom(string roomName)
        {

            if (websocket.ReadyState == WebSocketState.Open)
            {

                SocketEvent socketEvent = new SocketEvent("LeaveRoom", roomName);

                string jsonStr = JsonUtility.ToJson(socketEvent);


                websocket.Send(jsonStr);



                Room.SetActive(false);
                UI1.SetActive(true);
                ConnectUI.SetActive(false);

            }
                



            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                SocketEvent severOutputJS = JsonUtility.FromJson<SocketEvent>(tempMessageString);



                print(severOutputJS.data);


                if (severOutputJS.eventName == "LeaveRoom")
                {

                    if (severOutputJS.data == "Success")
                    {
                        Room.SetActive(false);
                        UI1.SetActive(false);
                        ConnectUI.SetActive(true);
                        print("Leaver Success");

                    }
                    else if (severOutputJS.data == "Fail")
                    {

                        print("LeaveRoom Fail");

                    }

                }
                    


            }

                tempMessageString = "";

            

        }


    // Update is called once per frame
        void Update()
        {
           

            if (Input.GetKeyDown(KeyCode.Return))
            {


                websocket.Send(Metalk);

            }

            UpdateMessageCreate();
        }

        public void OnDestroy()
        {
            if(websocket != null)
            {
                websocket.Close();
            }
        }
        public void SendMassage()
        {
            Metalk = UserMe.text;

            websocket.Send(Metalk);



            for (int i = 0; i < counttext.Count; i++)
            {
                counttext[i].transform.position = new Vector3(counttext[i].transform.position.x, counttext[i].transform.position.y + 30, counttext[i].transform.position.z);

            }



            var User = Instantiate(textspawnUser, spawntextpostUser.transform.position, Quaternion.identity, prarent.transform);
            User.gameObject.GetComponent<Text>().text = Metalk;
            counttext.Add(User);

          

            
            // ShowUserMetext.text = Metalk;

            //นับการกดปุ่ม

            
        }
            

        public void UpdateMessageCreate()
        {
            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                SocketEvent severOutputJS = JsonUtility.FromJson<SocketEvent>(tempMessageString);



                print(severOutputJS.data);


                if (severOutputJS.eventName == "CreateRoom")
                {

                    if (severOutputJS.data == "Success") 
                    {
                        UI1.SetActive(false);
                        Room.SetActive(true);
                        print("in room");

                    }
                    else if (severOutputJS.data == "Fail")
                    {

                        statusCreate.text = "Room can not Create";

                    }

                }
                else if (severOutputJS.eventName == "JoinRoom")
                {
                    if (severOutputJS.data == "Success")
                    {

                        UI1.SetActive(false);
                        Room.SetActive(true);
                        print("in room");

                    }
                    else if (severOutputJS.data == "Fail")
                    {

                        statusJoin.text = "Room can not Join";

                    }


                }


            }

            tempMessageString = "";
        }

       

        public void OnMessage(object sender, MessageEventArgs messageEventArgs) 
        {

            Debug.Log(messageEventArgs.Data);

            tempMessageString = messageEventArgs.Data;




            string data = messageEventArgs.Data;
            Yourtalk = messageEventArgs.Data;

            if (UserMe.text == Yourtalk)
            {

                return;


            }



            Debug.Log("Message from server : " + data);

            //ShowUserMetext.text = messageEventArgs.Data;

            for (int i = 0; i < counttext.Count; i++)
            {

                counttext[i].transform.position = new Vector3(counttext[i].transform.position.x, counttext[i].transform.position.y + 30, counttext[i].transform.position.z);

            }

            var Your = Instantiate(textspawnYour, spawntextpostYour.transform.position, Quaternion.identity, prarent.transform);
            Your.gameObject.GetComponent<Text>().text = Yourtalk;
            counttext.Add(Your);






        }



       

        public void Login() 
        {


            //name1 = NameN1.text;
            //print("Name : " + name1);


           
            if (IPnum.text == "127.0.0.1"&& Portnum.text == "24563")
            {
                UI1.SetActive(false);
            }
            else
            {
                Showstatus.text ="login False Try Again";

            }







        }





    }
}

