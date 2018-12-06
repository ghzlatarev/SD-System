"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("UpdateGauges", function (dataValue) {

	var dv = dataValue;
	document.getElementById("gauge-value").innerHTML = dataValue;
});

connection.start().catch(function (err) {
	return console.error(err.toString());
});