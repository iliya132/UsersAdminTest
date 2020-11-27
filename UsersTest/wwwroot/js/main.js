"use strict";

let connection = new XMLHttpRequest();
let users = [];

function EditUser(user) {
    let nameField = document.getElementById('modalNameField');
    let emailField = document.getElementById('modalEmailField');
    user.name = nameField.value;
    user.email = emailField.value;
    Refresh();
}

function DeleteUser(user) {
    users.splice(users.findIndex(v => v == user), 1);
    Refresh();
}

function CreateElement(type, innerHtml, owner) {
    let newElem = document.createElement(type);
    newElem.innerHTML = innerHtml;
    owner.appendChild(newElem);
    return newElem;
}

function Refresh() {
    let tableBody = document.getElementById("UserRows");
    tableBody.innerHTML = '';
    users.forEach(function (itm, i, arr) {
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
        editBtn.setAttribute("class", "btn btn-secondary mr-2");
        editBtn.appendChild(document.createTextNode("Edit"));
        newRow.appendChild(editBtn);
        editBtn.onclick = function () {

            $("#editModalWindow").modal('show');
            document.getElementById('modalNameField').value = users[i].name;
            document.getElementById('modalEmailField').value = users[i].email;
            ConfirmEdit.onclick = function () { EditUser(users[i]); }
            $("#editModalWindow").modal('hide');
        }
        let deleteBtn = document.createElement("button");
        deleteBtn.setAttribute("class", "btn btn-danger");

        deleteBtn.appendChild(document.createTextNode("Delete"));
        newRow.appendChild(deleteBtn);
        deleteBtn.onclick = function () {
            DeleteUser(users[i]);
            
        }
        tableBody.appendChild(newRow);
    });
}

window.onload = function(){
    connection.open('GET', 'https://localhost:44393/Users/allusers', true);
    connection.onreadystatechange = function(){
        if(connection.readyState == 4){
            if(connection.status == 200){
                users = JSON.parse(connection.responseText);
                Refresh();
        }
    }
    }
    connection.send();
}



