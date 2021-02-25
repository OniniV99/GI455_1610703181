const sqlite3 = require('sqlite3').verbose()

let db = new sqlite3.Database('./database/chatDB.db', sqlite3.OPEN_CREATE | sqlite3.OPEN_READWRITE, (err)=>{

    if(err) throw err;

    console.log('Connected to database.')

    var dataFromClient = {
        eventName:"Login",
        data:"T5#100#T55" //UserID#Password
    }

    var splitStr = dataFromClient.data.split('#')
    var userID = splitStr[0]
    var password = splitStr[1]
    var name = splitStr[2]
    //var addedMoney = parseInt(splitStr[1])

    

    var sqlSelect = "SELECT * FROM UserData WHERE UserID = '"+userID+"' AND Password ='"+password+"'"//login
    var sqlInsert = "INSERT INTO UserData (UserID, Password, Name, Money) VALUES('"+userID+"', '"+password+"', '"+name+"', '0')"
    //var sqlUpdate = "UPDATE UserData SET Money ='200' WHERE UserID= '"+userID+"'"

    /*db.all("SELECT Money FROM UserData WHERE UserID='"+userID+"'",(err,rows)=>{
        if(err)
        {
            var callbackMsg = {
                eventName:"AddMoney",
                data:"Fail"
            }

            var toJsonStr = JSON.stringify(callbackMsg)
            console.log("[0]" + toJsonStr)

        }
        else
        {
           console.log(rows)
           if(rows.length > 0)
           {
                var currentMoney = rows[0].Money
                currentMoney += addedMoney
                db.all("UPDATE UserData SET Money= '"+currentMoney+"' WHERE UserID= '"+userID+"'",(err,rows)=>{

                    if(err)
                    {
                        var callbackMsg = {
                            eventName:"AddMoney",
                            data:"Fail"
                        }
            
                        var toJsonStr = JSON.stringify(callbackMsg)
                        console.log("[1]" + toJsonStr)
                    }
                    else
                    {
                        var callbackMsg = {
                            eventName:"AddMoney",
                            data:currentMoney.toString()
                        }
            
                        var toJsonStr = JSON.stringify(callbackMsg)
                        console.log("[2]" + toJsonStr)

                        
                        
                    }

                })
           }
           else
           {
            var callbackMsg = {
                eventName:"AddMoney",
                data:"Fail"
            }

            var toJsonStr = JSON.stringify(callbackMsg)
            console.log("[3]" + toJsonStr)
           }
        }
        



    })*/

    /*
    db.all(sqlInsert,(err,rows)=>{
        if(err)
        {
            var callbackMsg = {
                eventName:"Register",
                data:"Fail"
            }

            var toJsonStr = JSON.stringify(callbackMsg)
            console.log("[0]" + toJsonStr)

        }
        else
        {
            var callbackMsg = {
                eventName:"Register",
                data:"Success"
            }

            var toJsonStr = JSON.stringify(callbackMsg)
            console.log("[1]" + toJsonStr)
        }
    })*/


    
    db.all(sqlSelect,(err , rows)=>{
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
                    data:rows[0].Name
                }

                var toJsonStr = JSON.stringify(callbackMsg)
                console.log("[2]" + toJsonStr)

                console.log(callbackMsg)



            }
            else
            {
                var callbackMsg = {
                    eventName:"Login",
                    data:"Fail"
                }

                var toJsonStr = JSON.stringify(callbackMsg)
                console.log("[2]" + toJsonStr)

            }
            //console.log(rows)
            
        }
    })
    

})