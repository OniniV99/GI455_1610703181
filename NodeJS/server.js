var wedsocket = require('ws');



var wedsocketServer = new wedsocket.Server({port:24563},()=>{

console.log("oni server is running")

})

var wsList = []
var roomList = []

/*
{
    roomName: "XXX",
    wsList: []
}
*/

wedsocketServer.on("connection",(ws,rp)=>{

    {
        //Lobbyzone
        ws.on("message",(data)=>{


            console.log(data)

            var toJson = JSON.parse(data)

            console.log(toJson["eventName"])
            
            
            if(toJson["eventName"] == "CreateRoom")//CreateRoom
            {
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
                        data: "Success"
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

                    var resultData = {eventName: toJson.eventName,data: "Success"}

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
        

        //wsList = ArrayRemove(wsList,ws)
        
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

function Boardcast(data)
{
    for(var i = 0; i < wsList.length;i++)
    {
        wsList[i].send(data);
    }
}