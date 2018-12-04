"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function (message) {

	var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = msg;

	var lis = document.createElement("li");
	lis.setAttribute('class', "nav-link");
	var li = document.createElement("nav-item");
	li.setAttribute('dropdown-item', true)
	li.textContent = encodedMsg;
	lis.appendChild(li);
	document.getElementById("notificationsList").appendChild(lis);
	document.getElementById("notificationsCount").innerHTML = document.getElementById("notificationsList").childElementCount - 1;
});

connection.start().catch(function (err) {
	return console.error(err.toString());
});