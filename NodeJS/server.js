
const sqlite3 = require('sqlite3').verbose()

var wedsocket = require('ws');

var wedsocketServer = new wedsocket.Server({port:24563},()=>{

console.log("oni server is running")

})

//var wsList = []
var roomList = []

/*
{
    roomName: "XXX",
    wsList: []
}
*/
let db = new sqlite3.Database('./database/chatDB.db', sqlite3.OPEN_CREATE | sqlite3.OPEN_READWRITE, (err)=>{

    
    if(err) throw err;

    console.log('Connected to database.')
    /*
    var sqlSelect = "SELECT * FROM UserData WHERE UserID = '"+userID+"' AND Password ='"+password+"'"//login
    var sqlInsert = "INSERT INTO UserData (UserID, Password, Name, Money) VALUES('"+userID+"', '"+password+"', '"+name+"', '0')"
    var sqlUpdate = "UPDATE UserData SET Money ='200' WHERE UserID= '"+userID+"'"
    */
})

wedsocketServer.on("connection",(ws,rp)=>{

    {



        //Lobbyzone
        ws.on("message",(data)=>{


            console.log(data)

            var toJson = JSON.parse(data)
            console.log(toJson["eventName"])
            console.log("--------")
            if(toJson["eventName"] == "SendMessage")
            {
                Boardcast(ws , data)

            }

            if(toJson["eventName"] == "Register")//Register
            {    
                var sqlInsert = "INSERT INTO UserData (UserID, Password, Name) VALUES('"+toJson["nameID"]+"', '"+toJson["name"]+"', '"+toJson["password"]+"')"
                db.all(sqlInsert,(err,rows)=>{
                         
                    if(err)
                    {
                        var callbackMsg = {
                        eventName:"Register",
                        data:"Fail"
                        }

                        var toJsonStr = JSON.stringify(callbackMsg)
                        console.log("[0]" + toJsonStr)

                        ws.send(toJsonStr)

                    }
                    else
                    {
                        var callbackMsg = {
                        eventName:"Register",
                        data:"Success"
                        }
                            
                        var toJsonStr = JSON.stringify(callbackMsg)
                        console.log("[1]" + toJsonStr)

                        ws.send(toJsonStr)
                    }
                    
                })
            }

            if(toJson["eventName"] == "Login")//Login
            {
                var sqlSelect = "SELECT * FROM UserData WHERE UserID = '"+toJson["nameID"]+"' AND Password ='"+toJson["password"]+"'"
                
                db.all(sqlSelect,(err , rows)=>{
                    if(toJson["eventName"] == "Login")//Login
                    {
                        if(err)
                        {
                            
                    
                            console.log("[0]" + err)
                        }
                        else
                        {
                            if(rows.length > 0)
                            {
                                console.log("========[1]========")
                                console.log(rows)
                                console.log("========[1]========")
                                var callbackMsg = {
                                    eventName:"Login",
                                    data:"Success",
                                    nameID:"",
                                    name:rows[0].Name,
                                    password:"",
                                    
                                }

                                var toJsonStr = JSON.stringify(callbackMsg)
                                console.log("[2]" + toJsonStr)

                                console.log(callbackMsg)

                                ws.send(toJsonStr)

                                console.log(toJsonStr)

                            }
                            else
                            {
                                var callbackMsg = {
                                    eventName:"Login",
                                    data:"Fail",
                                    nameID:"",
                                    name:"",
                                    password:"",
                                    

                                }

                                var toJsonStr = JSON.stringify(callbackMsg)
                                console.log("[2]" + toJsonStr)

                                ws.send(toJsonStr)

                            }
                            //console.log(rows)
                    
                        }
                    }    
                })
                
            }

            if(toJson["eventName"] == "CreateRoom")//CreateRoom
            {
                for(var i = 0; i < roomList.length; i++)
                {
                    console.log(roomList[i].roomName)
                }
                console.log("Client request CreateRoom ["+toJson.data+"]")
                var isFoundRoom = false
                for(var i = 0; i < roomList.length; i++)
                {
                    if(roomList[i].roomName == toJson.data)
                    {
                        isFoundRoom = true
                        break
                    }
                }

                if(isFoundRoom)
                {
                    //callback to client : room is exist
                    console.log("Create room: room is found")

                    var resultData = {
                        eventName: toJson.eventName,
                        data: "Fail"
                    }

                    var toJsonStr = JSON.stringify(resultData)

                    ws.send(JSON.stringify(resultData))
                }
                else
                {
                    console.log("Create room: room is not found")
                    //CreateRoom here
                    var newRoom = {roomName: toJson.data,wsList:[]}
                    newRoom.wsList.push(ws) //ws = ฉัน

                    roomList.push(newRoom);

                    var resultData = {
                        eventName: toJson.eventName,
                        data: "Success",
                        nameID:"",
                        name:toJson.data,
                        password:"",
                    }

                    var toJsonStr = JSON.stringify(resultData)
                    


                    ws.send(toJsonStr)
                }

                
                
            }
            else if(toJson["eventName"] == "JoinRoom")//JoinRoom
            {
                console.log("Client join request :  ["+toJson.data+"]")
                

                var isFoundRoom = false
                for(var i = 0; i < roomList.length; i++)
                {
                    if(roomList[i].roomName == toJson.data)
                    {
                    
                        isFoundRoom = true

                        roomList[i].wsList.push(ws)

                        break
                    }

                }

                if(isFoundRoom)
                {
                    //callback to client : room is exist
                    console.log("join room: joining room")

                    var resultData = {
                        eventName: toJson.eventName,
                        data: "Success",
                        nameID:"",
                        name:toJson.data,
                        password:"",}

                    var toJsonStr = JSON.stringify(resultData)

                    ws.send(JSON.stringify(resultData))
                }
                else
                {
                    console.log("Join room: not join room")
                    

                    var newRoom = {roomName: toJson.data,wsList: []}
                    

                    //roomList.push(newRoom);

                    var resultData = {eventName: toJson.eventName,data: "Fail"}

                    var toJsonStr = JSON.stringify(resultData)
                    
                    ws.send(toJsonStr)
                }

                
            }
            else if(toJson["eventName"] == "LeaveRoom")
            {
                //============ Find client in room for remove client out of room ================
                var isLeaveSuccess = false;//Set false to default.
                for(var i = 0; i < roomList.length; i++)//Loop in roomList
                {
                    for(var j = 0; j < roomList[i].wsList.length; j++)//Loop in wsList in roomList
                    {
                        if(ws == roomList[i].wsList[j])//If founded client.
                        {
                            roomList[i].wsList.splice(j, 1);//Remove at index one time. When found client.

                            if(roomList[i].wsList.length <= 0)//If no one left in room remove this room now.
                            {
                                roomList.splice(i, 1);//Remove at index one time. When room is no one left.
                            }
                            isLeaveSuccess = true;
                            break;
                        }
                    }
                }
                //===============================================================================

                if(isLeaveSuccess)
                {
                    //========== Send callback message to Client ============

                    //ws.send("LeaveRoomSuccess");

                    //I will change to json string like a client side. Please see below
                    var resultData = {
                        eventName:"LeaveRoom",
                        data:"Success"
                    }
                    var toJsonStr = JSON.stringify(resultData);
                    ws.send(toJsonStr);
                    //=======================================================

                    console.log("leave room success");
                }
                else
                {
                    //========== Send callback message to Client ============

                    //ws.send("LeaveRoomFail");

                    //I will change to json string like a client side. Please see below
                    var resultData = {
                        eventName:"LeaveRoom",
                        data:"Fail"
                    }
                    var toJsonStr = JSON.stringify(resultData);
                    ws.send(toJsonStr);
                    //=======================================================

                    console.log("leave room fail");
            
                }

            }
    
        })
      
    }

    console.log('client connected.')

    //wsList.push(ws)

    ws.on("close", ()=>{
        
    

        console.log("client disconnected.")
            //============ Find client in room for remove client out of room ================
            var isLeaveSuccess = false;//Set false to default.
            for(var i = 0; i < roomList.length; i++)//Loop in roomList
            {
                for(var j = 0; j < roomList[i].wsList.length; j++)//Loop in wsList in roomList
                {
                    if(ws == roomList[i].wsList[j])//If founded client.
                    {
                        roomList[i].wsList.splice(j, 1);//Remove at index one time. When found client.

                        if(roomList[i].wsList.length <= 0)//If no one left in room remove this room now.
                        {
                            roomList.splice(i, 1);//Remove at index one time. When room is no one left.
                        }
                        isLeaveSuccess = true;
                        break;
                    }
                }
            }
            //===============================================================================

            if(isLeaveSuccess)
            {
                //========== Send callback message to Client ============

                //ws.send("LeaveRoomSuccess");

                //I will change to json string like a client side. Please see below
                var resultData = {
                    eventName:"LeaveRoom",
                    data:"Success"
                }
                var toJsonStr = JSON.stringify(resultData);
                ws.send(toJsonStr);
                //=======================================================

                console.log("leave room success");
            }
            else
            {
                //========== Send callback message to Client ============

                //ws.send("LeaveRoomFail");

                //I will change to json string like a client side. Please see below
                var resultData = {
                    eventName:"LeaveRoom",
                    data:"Fail"
                }
                var toJsonStr = JSON.stringify(resultData);
                ws.send(toJsonStr);
                //=======================================================

                console.log("leave room fail");
        
            }
        


    })

})




function ArrayRemove(arr, value)
{
    return arr.filter((element)=>{

        return element != value
    })
}

function Boardcast(ws,data)
{
    var selectRoomIndex = -1;

    for(var i = 0; i < roomList.length;i++)
    {
        for(var j = 0; j < roomList[i].wsList.length;j++)
        {
            if(ws == roomList[i].wsList[j])
            {
                selectRoomIndex = i;
            }
        }
    }

    for(var i = 0; i < roomList[selectRoomIndex].wsList.length; i++)
    {

        roomList[selectRoomIndex].wsList[i].send(data)
    }
}