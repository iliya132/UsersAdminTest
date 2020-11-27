"use strict";

let connection = new XMLHttpRequest();
let users = [];

function EditUser(user) {
    alert('Edited ' + user.name);
}

function CreateElement(type, innerHtml, owner) {
    let newElem = document.createElement(type);
    newElem.innerHTML = innerHtml;
    owner.appendChild(newElem);
    return newElem;
}

window.onload = function(){
    connection.open('GET', 'https://localhost:44393/Users/allusers', true);
    
    connection.onreadystatechange = function(){
        if(connection.readyState == 4){
            
            if(connection.status == 200){

            users = JSON.parse(connection.responseText);
            users.forEach(function(itm, i, arr){
                let tableBody = document.getElementById("UserRows");
                let newRow = document.createElement("tr");
                CreateElement("th", itm.id, newRow);
                CreateElement("th", itm.login, newRow);
                CreateElement("th", itm.name, newRow);
                CreateElement("th", itm.email, newRow);
                let roles = itm.roles.map(function (rl) {
                    return rl.name;
                }).join(', ');
                CreateElement("th", roles, newRow);
                let editBtn = document.createElement("button");
                editBtn.setAttribute("class", "btn btn-primary");
                editBtn.appendChild(document.createTextNode("Edit"));
                newRow.appendChild(editBtn);
                editBtn.onclick = function () {
                    EditUser(users[i]);
                    tableBody.removeChild(newRow);
                }
                let deleteBtn = document.createElement("button");
                deleteBtn.setAttribute("class", "btn btn-primary");
                deleteBtn.appendChild(document.createTextNode("Delete"));
                newRow.appendChild(deleteBtn);
                
                tableBody.appendChild(newRow);
            });
        }
    }
    }
    
    connection.send();
    
    
}



