"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");

    // Create span for the timestamp
    var timestampSpan = document.createElement("span");
    timestampSpan.style.color = "gray"; // Set timestamp color to red
    timestampSpan.textContent = `[${new Date().toLocaleTimeString()}]: `;

    // Create span for the user
    var userSpan = document.createElement("span");
    userSpan.style.color = "#5dd15d";
    timestampSpan.style.fontWeight = "bold";
    userSpan.textContent = `${user}: `;

    // Create span for the message
    var messageSpan = document.createElement("span");
    messageSpan.style.color = "gray"; // Set message color to gray
    messageSpan.textContent = `${message}`;

    // Append spans to the list item
    li.appendChild(timestampSpan);
    li.appendChild(userSpan);
    li.appendChild(messageSpan);

    // Append the list item to the messages list
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});