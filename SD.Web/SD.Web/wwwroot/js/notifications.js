"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function (message) {

	var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = msg;
	var li = document.createElement("nav-item");
	li.setAttribute('dropdown-item', true)
	li.textContent = encodedMsg;
	document.getElementById("notificationsList").appendChild(li);
	document.getElementById("notificationsCount").innerHTML = document.getElementById("notificationsList").childElementCount - 1;
});

connection.start().catch(function (err) {
	return console.error(err.toString());
});