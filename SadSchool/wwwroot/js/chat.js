"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("LoadChatHistory", (messages) => {
    document.getElementById("messagesList").innerHTML = "";  // Clear old messages
    messages.forEach((msg) => appendMessage(msg));
});

// 📌 Append message to chat window


connection.on("ReceiveMessage", (message) => {
    appendMessage(message);
});

function appendMessage(message) {
    var li = document.createElement("li");

    // Create span for the timestamp
    var timestampSpan = document.createElement("span");
    timestampSpan.style.fontSize = "11px";
    timestampSpan.style.color = "gray"; // Set timestamp color to red
    timestampSpan.textContent = `${message.timeStamp}: `;

    // Create span for the user
    var userSpan = document.createElement("span");
    userSpan.style.color = "#5dd15d";
    userSpan.textContent = `${message.user}: `;

    // Create span for the message
    var messageSpan = document.createElement("span");
    messageSpan.style.color = "gray"; // Set message color to gray
    messageSpan.textContent = `${message.messageText}`;

    // Append spans to the list item
    li.appendChild(timestampSpan);
    li.appendChild(userSpan);
    li.appendChild(messageSpan);

    // Append the list item to the messages list
    document.getElementById("messagesList").appendChild(li);
}

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

document.getElementById("clearButton").addEventListener("click", function (event) {
    document.getElementById("messagesList").replaceChildren();
    connection.invoke("ClearChat").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});