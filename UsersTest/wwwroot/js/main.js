"use strict";

let connection = new XMLHttpRequest();
let apiUrl = 'https://localhost:44393/Users/';
let tokenUrl = 'https://localhost:44393/jwt/token';
let users = [];
let roles = [];
let ActiveToken;

class User {
    constructor(login, name, email, password, roles) {
        this.id = 0;
        this.login = login;
        this.name = name;
        this.email = email;
        this.password = password;
        this.roles = roles;
    }
}

class Role {
    constructor(id, name) {
        this.id = id;
        this.name = name;
    }
}

window.onload = function () {
    //в тестовой реализации токен запрашивается один раз. 
    //В проме следует реализовать метод для направления запроса, 
    //в котором бы производилась проверка на статус запроса и получение нового токена в случае необходимости
    GetToken(); 
    GetUsersAndRoles(); //загрузить первичные данные
    let addform = document.getElementById('AddForm');
    let editform = document.getElementById('editForm');
    //предотвратить перезагрузку и отправку формы при сабмите
    addform.addEventListener("submit", function (event) { 
        event.preventDefault();
    });
    editform.addEventListener("submit", function (event) {
        event.preventDefault();
    });
    document.getElementById("AddBtn").onclick = AddButtonClick;
    document.getElementById("ConfirmAdd").onclick = ConfirmAddButtonClick;
}

//Получить JWT token
function GetToken() {
    // Тестовая реализация запроса токена. Пользовательские данные указаны хардкодно!
    // Запрос выполняется синхронно что бы предотвратить дальнейшие запросы до получения токена
    let body = JSON.stringify({ username: "TestUser", password: "testPasswrd" });
    connection.open('POST', tokenUrl, false);
    connection.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    connection.onreadystatechange = function () {
        if (connection.readyState == 4) {
            if (connection.status == 200) {
                let token = JSON.parse(connection.responseText);
                ActiveToken = token.token;
            }
        }
    }
    connection.send(body);
}

//Первичная загрузка пользователей и ролей
function GetUsersAndRoles() {
    connection.open('GET', apiUrl + 'allusers', true);
    connection.setRequestHeader("Authorization", "Bearer " + ActiveToken);
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

//Обновить представление
function Refresh() {
    let tableBody = document.getElementById("UserRows");
    tableBody.innerHTML = '';
    users.forEach(function (itm, i, arr) {
        let newRow = document.createElement("tr");
        CreateElement("th", itm.id, newRow);
        CreateElement("th", itm.login, newRow);
        CreateElement("th", itm.name, newRow);
        CreateElement("th", itm.email, newRow);
        let roles = itm.roles.map(function (rl) { return rl.name; }).join(', ');
        CreateElement("th", roles, newRow);
        let editBtn = document.createElement("button");
        editBtn.setAttribute("class", "btn btn-secondary mr-2");
        editBtn.appendChild(document.createTextNode("Edit"));
        newRow.appendChild(editBtn);
        editBtn.onclick = function () { EditButtonClick(i); };
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

//Нажатие на кнопку "Редактировать"
function EditButtonClick(id) {
    $("#editModalWindow").modal('show');
    document.getElementById('modalNameField').value = users[id].name;
    document.getElementById('modalEmailField').value = users[id].email;
    document.getElementById('EditAdminBox').checked = users[id].roles.map(i => i.name).some(nm => nm == 'Admin');
    document.getElementById('EditUserBox').checked = users[id].roles.map(i => i.name).some(nm => nm == 'User');
    ConfirmEdit.onclick = function () {
        if (EditUser(users[id])) {
            $("#editModalWindow").modal('hide');
        }
    }
}

//Добавление нового пользователя
function AddUser(newUser) {
    users.push(newUser);
    let conn = new XMLHttpRequest();
    conn.open('POST', apiUrl + 'adduser', true);
    conn.setRequestHeader('Content-Type', 'application/json');
    conn.setRequestHeader("Authorization", "Bearer " + ActiveToken);
    conn.onreadystatechange = function () {
        if (conn.readyState == 4) {
            if (conn.status == 200) {
                newUser.id = conn.response;
                Refresh();
            }
        }
    }
    conn.send(JSON.stringify(newUser));
    Refresh();
}

//валидация отредактированного пользователя и изменение объекта в памяти
function EditUser(user) {
    if (!ValidateEditedUser()) {
        return false;
    }
    let nameField = document.getElementById('modalNameField');
    let emailField = document.getElementById('modalEmailField');
    let admCB = document.getElementById("EditAdminBox");
    let usrCB = document.getElementById("EditUserBox");
    user.name = nameField.value;
    user.email = emailField.value;
    user.roles.splice(0);
    if (admCB.checked) {
        user.roles.push(new Role(1, "Admin"));
        admCB.checked = false;
    }
    if (usrCB.checked) {
        user.roles.push(new Role(2, "User"));
        usrCB.checked = false;
    }

    CommitEdit(user);
    Refresh();
    return true;
}

//Удаление объекта в памяти
function DeleteUser(user) {
    users.splice(users.findIndex(v => v == user), 1);
    CommitDelete(user);
    Refresh();
}

//Отправка изменений на сервер
function CommitEdit(user) {
    let conn = new XMLHttpRequest();
    conn.open('PUT', apiUrl + 'edituser', true);
    conn.setRequestHeader('Content-Type', 'application/json');
    conn.setRequestHeader("Authorization", "Bearer " + ActiveToken);
    conn.onreadystatechange = function () {
        if (connection.readyState == 4) {
            if (connection.status == 200) {
                //TODO Здесь нужно бы было проверить ответ от сервера
            }
        }
    }
    conn.send(JSON.stringify(user));
}

//Отправка изменений на сервер
function CommitDelete(user) {
    let conn = new XMLHttpRequest();
    conn.open('DELETE', apiUrl + 'deleteuser', true);
    conn.setRequestHeader('Content-Type', 'application/json');
    conn.setRequestHeader("Authorization", "Bearer " + ActiveToken);
    conn.onreadystatechange = function () {
        if (connection.readyState == 4) {
            if (connection.status == 200) {
                //TODO Здесь нужно бы было проверить ответ от сервера
            }
        }
    }
    conn.send(JSON.stringify(user));
}

//Создание нового html элемента с заданными параметрами
function CreateElement(type, innerHtml, owner) {
    let newElem = document.createElement(type);
    newElem.innerHTML = innerHtml;
    owner.appendChild(newElem);
    return newElem;
}

//запрос всех ролей с сервера
function GetRoles() {
    connection.open('GET', apiUrl + 'roles', true);
    connection.setRequestHeader("Authorization", "Bearer " + ActiveToken);
    connection.onreadystatechange = function () {
        if (connection.readyState == 4) {
            if (connection.status == 200) {
                return JSON.parse(connection.responseText);
            }
        }
    }
    connection.send();
}

//Нажатие на кнопку добавить
function AddButtonClick() {
    $("#AddModalWindow").modal('show');
    let loginField = document.getElementById("AddmodalLoginField");
    let nameField = document.getElementById("AddmodalNameField");
    let emailField = document.getElementById("AddmodalEmailField");
    let passwrdField = document.getElementById("AddmodalPswrdField");
    loginField.value = '';
    nameField.value = '';
    emailField.value = '';
    passwrdField.value = '';
}

//Нажатие на кнопку подтвердить добавление (валидация + создание объекта)
function ConfirmAddButtonClick() {   
    if (!ValidateAddedUser()) {
        return;
    }
    let admCB = document.getElementById("AdminBox");
    let usrCB = document.getElementById("UserBox");
    let loginField = document.getElementById("AddmodalLoginField");
    let nameField = document.getElementById("AddmodalNameField");
    let emailField = document.getElementById("AddmodalEmailField");
    let passwrdField = document.getElementById("AddmodalPswrdField");
    let rls = [];
    if (admCB.checked) {
        rls.push(new Role(1, "Admin"));
        admCB.checked = false;
    }
    if (usrCB.checked) {
        rls.push(new Role(2, "User"));
        usrCB.checked = false;
    }
    let newUser = new User(loginField.value, nameField.value, emailField.value, passwrdField.value, rls);
    AddUser(newUser);
    $("#AddModalWindow").modal('hide');
}

function ValidateEditedUser() {
    return document.getElementById("editForm").checkValidity();
}

function ValidateAddedUser() {
    return document.getElementById("AddForm").checkValidity();
}