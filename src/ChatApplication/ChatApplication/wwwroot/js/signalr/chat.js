"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendChatMessageButton").disabled = true;
const userChatInputElement = document.getElementById("userChatInput");

connection.on("ReceiveMessageClientListener", function (user, message) {
    const li = document.createElement("p");
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
    document.getElementById("chatMessagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendChatMessageButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendChatMessageButton").addEventListener("click", function (event) {
    var username = document.getElementById("chatUsername").textContent;
    var messageToSend = userChatInputElement.value;
    connection.invoke("SendMessageServerListener", username, messageToSend).catch(function (err) {
        return console.error(err.toString());
    });
    userChatInputElement.value = "";
    event.preventDefault();
});
document.getElementById("userChatInput").addEventListener("keydown", function (event) {
    if (event.key == "Enter") {
        const username = document.getElementById("chatUsername").textContent;
        const messageToSend = userChatInputElement.value;
        connection.invoke("SendMessageServerListener", username, messageToSend).catch(function (err) {
            return console.error(err.toString());
        });
        userChatInputElement.value = "";
        event.preventDefault();
    }
})