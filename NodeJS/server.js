var wedsocket = require('ws');



var wedsocketServer = new wedsocket.Server({port:24563},()=>{

console.log("oni server is running")

})

var wsList = []

wedsocketServer.on("connection",(ws,rp)=>{

    console.log('client connected.')

    wsList.push(ws)

    ws.on("message",(data)=>{

        console.log("send from client : "+ data)
        Boardcast(data)
    })

    ws.on("close", ()=>{

        wsList = ArrayRemove(wsList,ws)
        
        console.log("client disconnected.")

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