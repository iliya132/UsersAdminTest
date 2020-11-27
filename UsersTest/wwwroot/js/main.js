"use strict";

let connection = new XMLHttpRequest();
let users = [];
let roles = [];

class User {
    constructor(login, name, email, password, roles) {
        this.id = 'new';
        this.login = login;
        this.name = name;
        this.email = email;
        this.password = password;
        this.roles = roles;
    }
}

class Role {
    constructor(name) {
        this.id = 0;
        this.name = name;
    }
}

function AddUser(newUser) {
    users.push(newUser);
    Refresh();
}

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
window.onload = function () {
    GetUsersAndRoles();
}

function GetUsersAndRoles() {
    connection.open('GET', 'https://localhost:44393/Users/allusers', true);
    connection.onreadystatechange = function () {
        if (connection.readyState == 4) {
            if (connection.status == 200) {
                users = JSON.parse(connection.responseText);
                roles = GetRoles();
                Refresh();
            }
        }
    }
    connection.send();
}

function GetRoles() {
    connection.open('GET', 'https://localhost:44393/Users/roles', true);
    connection.onreadystatechange = function () {
        if (connection.readyState == 4) {
            if (connection.status == 200) {
                return JSON.parse(connection.responseText);
            }
        }
    }
    connection.send();
}

document.getElementById("AddBtn").onclick = function () {
    $("#AddModalWindow").modal('show');
    let loginField = document.getElementById("AddmodalLoginField");
    let nameField = document.getElementById("AddmodalNameField");
    let emailField = document.getElementById("AddmodalEmailField");
    let passwrdField = document.getElementById("AddmodalPswrdField");
    let admCB = document.getElementById("AdminBox");
    let usrCB = document.getElementById("UserBox");
    document.getElementById("ConfirmAdd").onclick = function () {
        if (!ValidateAddedUser()) {
            return;
        }
        let rls = [];
        if (admCB.checked) {
            rls.push(new Role("Admin"));
            admCB.checked = false;
        }
        if (usrCB.checked) {
            rls.push(new Role("User"));
            usrCB.checked = false;
        }
        let newUser = new User(loginField.value, nameField.value, emailField.value, passwrdField.value, rls);
        AddUser(newUser);
    }
    $("#AddModalWindow").modal('hide');
    loginField.value = '';
    nameField.value = '';
    emailField.value = '';
    passwrdField.value = '';
    
}

function ValidateAddedUser() {
    let loginField = document.getElementById("AddmodalLoginField");
    let nameField = document.getElementById("AddmodalNameField");
    let emailField = document.getElementById("AddmodalEmailField");
    let passwrdField = document.getElementById("AddmodalPswrdField");
    if (loginField.value.length < 1) {
        return false;
    }
    return true;
}