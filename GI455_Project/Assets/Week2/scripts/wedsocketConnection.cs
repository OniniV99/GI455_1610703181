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
            public string nameID;
            public string name;
            public string password;
            public string sendMessage;
            public SocketEvent(string eventName, string data, string nameID, string name, string password, string sendMessage)
            {
                this.eventName = eventName;
                this.data = data;
                this.nameID = nameID;
                this.name = name;
                this.password = password;
                this.sendMessage = sendMessage;


            }
        }

        string tempMessageString;



        public InputField inputText;
        public Text sendText;
        public Text receiveText;
        

        private WebSocket ws;


        //ชื่อผู่ใช้
        string name1;
        //รับค่า
        public Text NameN1;
        public Text UserMe;
        public Text IPnum;
        public Text Portnum;

        public string nameUserLogin;
        //โชว์ข้อความ
        public Text Showstatus;
        
        //ตัวtextข้อความ
        public GameObject textspawnUser;
        public GameObject textspawnYour;
        public GameObject prarent;
        public GameObject spawntextpostUser;
        public GameObject spawntextpostYour;

        //poperror
        public GameObject spawnError;
        public Text error;

        //Room
        public Text UserName;

        

        //เก็บUI
        public string Metalk;


        //---------------------------------------------------------------------//
        public Text nameRoominput;
        public Text joinRoominput;
        public Text nameRoom;
        public Text login_ID_Input;
        public Text login_Pass_Input;
        public Text register_ID_Input;
        public Text register_pass_Input;
        public Text register_name_Input;
        public Text register_re_pass_Input;

        public Text ShowtextCreate;
        public Text ShowtextJoin;

        string login_ID;
        string login_Pass;
        string register_ID;
        string register_Pass;
        string register_name;
        string register_re_pass;
        string NRinput;
        string JRinput;

        public GameObject UI1;
        public GameObject Room;
        public GameObject ConnectUI;
        public GameObject LoginUI;
        public GameObject RegisterUI;

        public List<GameObject> counttext = new List<GameObject>();
        

        private WebSocket websocket;
         
        // Start is called before the first frame update
        void Start()
        {

            //string receiveMessage = "lnw007#hello#red";

            //string[] messageDataSplit = receiveMessage.Split('#');

            //string jsonStr = "}";

            //for (int i = 0; i < messageDataSplit.Length; i++)
            //{
            //    jsonStr += messageDataSplit[i] + ",";
            //}

            //jsonStr += "}";

            //print(jsonStr);

            //MessageData messageData = new MessageData();
            //messageData.username = messageDataSplit[0];
            //messageData.message = messageDataSplit[1];
            //messageData.colorName = messageDataSplit[2];


            //if (messageData.username == "lnw007")
            //{
            //    ShowMessage(messageData.message);
            //}

            

        }

        void ShowMessage(string message)
        {

        }

        public void Connect()
        {
            websocket = new WebSocket("ws://127.0.0.1:24563/");
            websocket.OnMessage += OnMessage;
            websocket.Connect();

            LoginUI.SetActive(true);
            RegisterUI.SetActive(false);
            Room.SetActive(false);
            UI1.SetActive(false);
            ConnectUI.SetActive(false);
        }
        public void ConnectServer()
        {
            
            NRinput = nameRoominput.text;

            CreateRoom(NRinput);

            
        }

        public void Login()
        {
            login_ID = login_ID_Input.text;
            login_Pass = login_Pass_Input.text;
            
            if (websocket.ReadyState == WebSocketState.Open)
            {

                SocketEvent socketEvent = new SocketEvent("Login", "",login_ID,"",login_Pass,"") ;

                string jsonStr = JsonUtility.ToJson(socketEvent);

                print(jsonStr);

                print(login_Pass_Input.text);

                if (login_ID_Input.text == "" || login_Pass_Input.text == "")
                {
                    spawnError.SetActive(true);
                    error.text = "Please enter all information";
                    return;

                }
                websocket.Send(jsonStr);
            }
        }
        public void Register()
        {
            register_ID = register_ID_Input.text;
            register_name = register_name_Input.text;
            register_Pass = register_pass_Input.text;
            register_re_pass = register_re_pass_Input.text;
            if (websocket.ReadyState == WebSocketState.Open)
            {

                SocketEvent socketEvent = new SocketEvent("Register", "", register_ID, register_name, register_Pass,"");

                string jsonStr = JsonUtility.ToJson(socketEvent);

                print(jsonStr);

                if (register_ID_Input.text == "" || register_name_Input.text == "" || register_pass_Input.text == "" || register_re_pass_Input.text == "")
                {

                    spawnError.SetActive(true);
                    error.text = "Please enter all information";
                    return;
                }
                if (register_pass_Input.text != register_re_pass_Input.text)
                {
                    print(register_pass_Input);
                    print(register_re_pass_Input);
                    spawnError.SetActive(true);
                    error.text = "Password not match Re-Password";
                    return;

                }

                websocket.Send(jsonStr);
            }

        }

        public void Redister_Login()
        {
            LoginUI.SetActive(false);
            RegisterUI.SetActive(true);
            Room.SetActive(false);
            UI1.SetActive(false);
            ConnectUI.SetActive(false);

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

                SocketEvent socketEvent = new SocketEvent("JoinRoom", roomName, "","", "","");

                string jsonStr = JsonUtility.ToJson(socketEvent);

                nameRoom.text = roomName;

                websocket.Send(jsonStr);

            }

         }

        public void CreateRoom(string roomName)
        {
            if (websocket.ReadyState == WebSocketState.Open)
            {


                SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName, "", "", "","");

                string jsonStr = JsonUtility.ToJson(socketEvent);

                nameRoom.text = roomName;


                websocket.Send(jsonStr);
            }
        }

        public void LeaveRoom(string roomName)
        {

            if (websocket.ReadyState == WebSocketState.Open)
            {

                SocketEvent socketEvent = new SocketEvent("LeaveRoom", roomName, "", "", "","");

                string jsonStr = JsonUtility.ToJson(socketEvent);


                websocket.Send(jsonStr);

                Room.SetActive(false);
                UI1.SetActive(true);
                ConnectUI.SetActive(false);

                nameRoominput.text = "";
                joinRoominput.text = "";
                sendText.text = "";
                receiveText.text = "";
                inputText.text = "";



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

            UpdateMessageCreate();
            
        }

        public void OnDestroy()
        {

            if (websocket != null)
            {
                websocket.Close();
            }
            //if(websocket != null)
            //{
            //    websocket.Close();
            //}
        }
        public void SendMassage()
        {
            if (inputText.text == "" || websocket.ReadyState != WebSocketState.Open)
            {
                return;
            }

            SocketEvent socketEvent = new SocketEvent("SendMessage", "", "", nameUserLogin, "",inputText.text);
            //newmessageData.username = inputUsername.text;
            

            string toJsonStr = JsonUtility.ToJson(socketEvent);


            websocket.Send(toJsonStr);
            inputText.text = "";

            //Metalk = UserMe.text;

            //websocket.Send(Metalk);


            //for (int i = 0; i < counttext.Count; i++)
            //{
            //    counttext[i].transform.position = new Vector3(counttext[i].transform.position.x, counttext[i].transform.position.y + 30, counttext[i].transform.position.z);

            //}



            //var User = Instantiate(textspawnUser, spawntextpostUser.transform.position, Quaternion.identity, prarent.transform);
            //User.gameObject.GetComponent<Text>().text = Metalk;
            //counttext.Add(User);

            // ShowUserMetext.text = Metalk;

            //นับการกดปุ่ม

        }
        public void CloseError()
        {
            spawnError.SetActive(false);
        }
        public void UpdateMessageCreate()
        {
            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                SocketEvent severOutputJS = JsonUtility.FromJson<SocketEvent>(tempMessageString);
                print(tempMessageString);

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

                        //statusCreate.text = "Room can not Create";
                        spawnError.SetActive(true);
                        error.text = "Can not Create"; 


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

                        //statusJoin.text = "Room can not Join";
                        spawnError.SetActive(true);
                        error.text = "Can not Join";

                    }

                }
                else if (severOutputJS.eventName == "Login")
                {

                    if (severOutputJS.data == "Success")
                    {


                        LoginUI.SetActive(false);
                        RegisterUI.SetActive(false);
                        Room.SetActive(false);
                        UI1.SetActive(true);
                        ConnectUI.SetActive(false);

                        nameUserLogin = severOutputJS.name;

                        UserName.text = "Name: " + nameUserLogin;



                        

                    }
                    else if (severOutputJS.data == "Fail")
                    {

                            //statusJoin.text = "Room can not Join";
                            spawnError.SetActive(true);
                            error.text = "Can not Login";

                    }
                    

                }
                else if (severOutputJS.eventName == "Register")
                {
                    print(severOutputJS);
                    if (severOutputJS.data == "Success")
                    {

                        LoginUI.SetActive(true);
                        RegisterUI.SetActive(false);
                        Room.SetActive(false);
                        UI1.SetActive(false);
                        ConnectUI.SetActive(false);

                    }
                    else if (severOutputJS.data == "Fail")
                    {

                        //statusJoin.text = "Room can not Join";
                        spawnError.SetActive(true);
                        error.text = "Can not Register";

                    }
                }
                else if (severOutputJS.eventName == "SendMessage")
                {
                    print(severOutputJS.sendMessage);
                    if (severOutputJS.name == nameUserLogin)
                    {
                        sendText.text += severOutputJS.name + ": " + severOutputJS.sendMessage + "\n";
                        receiveText.text += "\n";
                    }
                    else
                    {
                        receiveText.text += severOutputJS.name + ": " + severOutputJS.sendMessage + "\n";
                        sendText.text += "\n";
                    }

                }
            }
            tempMessageString = "";
        }
       
        public void OnMessage(object sender, MessageEventArgs messageEventArgs) 
        {

            Debug.Log(messageEventArgs.Data);

            tempMessageString = messageEventArgs.Data;


            //string data = messageEventArgs.Data;
            //Yourtalk = messageEventArgs.Data;

            //if (UserMe.text == Yourtalk)
            //{

            //    return;


            //}


            //Debug.Log("Message from server : " + data);

            ////ShowUserMetext.text = messageEventArgs.Data;

            //for (int i = 0; i < counttext.Count; i++)
            //{

            //    counttext[i].transform.position = new Vector3(counttext[i].transform.position.x, counttext[i].transform.position.y + 30, counttext[i].transform.position.z);

            //}

            //var Your = Instantiate(textspawnYour, spawntextpostYour.transform.position, Quaternion.identity, prarent.transform);
            //Your.gameObject.GetComponent<Text>().text = Yourtalk;
            //counttext.Add(Your);
        }
        
    }
}

