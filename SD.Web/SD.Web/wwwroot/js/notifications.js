"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function (message) {

	var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = msg;
	var li = document.createElement("nav-item");
	li.setAttribute('dropdown-item', true)
	li.textContent = encodedMsg;
	console.log('are be')
	console.log(li);
	document.getElementById("notificationsList").appendChild(li);
	//if (document.getElementById("notificationsList").childElementCount > 5) {
	//	document.getElementById("notificationsList").removeChild(document.getElementById("notificationsList").firstChild);
	//}


	document.getElementById("notificationsCount").innerHTML = Number(document.getElementById("notificationsList").childElementCount);
});

connection.start().catch(function (err) {
	return console.error(err.toString());
});