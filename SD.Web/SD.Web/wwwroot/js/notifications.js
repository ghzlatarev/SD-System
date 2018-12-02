"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function (message) {
	var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = msg;
	var li = document.createElement("li");
	li.textContent = encodedMsg;
	document.getElementById("notificationsList").appendChild(li);
});

connection.start().catch(function (err) {
	return console.error(err.toString());
});