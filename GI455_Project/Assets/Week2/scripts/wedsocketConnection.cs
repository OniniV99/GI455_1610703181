using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;

namespace Week2
{

    public class wedsocketConnection : MonoBehaviour
    {
        //ชื่อผู่ใช้
        string name1;
        //รับค่า
        public Text NameN1;
        public Text UserMe;
        //โชว์ข้อความ
        //public Text ShowUserMetext;
        //public Text ShowUserYourtext;
        //เก็บเป็นข้อความ
        string Metalk;
        string Yourtalk;
        //ตัวtextข้อความ
        public GameObject textspawnUser;
        public GameObject textspawnYour;
        public GameObject prarent;
        public GameObject spawntextpostUser;
        public GameObject spawntextpostYour;


        public List<GameObject> counttext = new List<GameObject>();
        

        private WebSocket websocket;

        // Start is called before the first frame update
         void Start()
        {


            websocket = new WebSocket("ws://127.0.0.1:24563/");

            websocket.OnMessage += OnMessage;

            websocket.Connect();

            
            
        }

        // Update is called once per frame
        void Update()
        {
           

            if (Input.GetKeyDown(KeyCode.Return))
            {
                // websocket.Send("Number : " + Random.Range(0,9999));

                websocket.Send(Metalk);
                
                

            }
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


        public void OnMessage(object sender, MessageEventArgs messageEventArgs) 
        {
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



       

       void Login() 
       {

            name1 = NameN1.text;
            print("Name : " + name1);


       }





    }
}

