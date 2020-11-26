"use strict";

let connection = new XMLHttpRequest();
let users = [];

window.onload = function(){
    connection.open('GET', 'https://localhost:44393/Users/allusers', true);
    
    connection.onreadystatechange = function(){
        if(connection.readyState == 4){
            
            if(connection.status == 200){

            users = JSON.parse(connection.responseText);
            users.forEach(function(itm, i, arr){
                let tableBody = document.getElementById("UserRows");
                let newRow = document.createElement("tr");
                let idCol = document.createElement("th");
                idCol.innerHTML = itm.id;
                newRow.appendChild(idCol);
                let NameCol = document.createElement("th");
                NameCol.innerHTML = itm.name;
                newRow.appendChild(NameCol);
                tableBody.appendChild(newRow);
            });
        }
    }
    }
    
    connection.send();
    
    
}



