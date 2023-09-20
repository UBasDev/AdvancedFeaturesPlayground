"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendChatMessageButton").disabled = true;
const userChatInputElement = document.getElementById("userChatInput");

connection.on("ReceiveMessageClientListener", function (user, message) {
    if (user && message) {
        const li = document.createElement("p");
        li.textContent = `${user} says ${message}`;
        document.getElementById("chatMessagesList").appendChild(li);
        alertifySuccessNotifier(`Received: ${message}`, 'bottom-right', 2);
    }
});

connection.start().then(function () {
    document.getElementById("sendChatMessageButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendChatMessageButton").addEventListener("click", function (event) {
    var username = document.getElementById("chatUsername").textContent;
    var messageToSend = userChatInputElement.value;
    if (messageToSend && username) {
        connection.invoke("SendMessageServerListener", username, messageToSend).catch(function (err) {
            return console.error(err.toString());
        });
        alertifyNotifyNotifier(`Sent: ${userChatInputElement.value}`, 'bottom-right', 1);
        userChatInputElement.value = "";
    }
    event.preventDefault();
});
document.getElementById("userChatInput").addEventListener("keydown", function (event) {
    if (event.key == "Enter") {
        const username = document.getElementById("chatUsername").textContent;
        const messageToSend = userChatInputElement.value;
        if (username && messageToSend) {
            connection.invoke("SendMessageServerListener", username, messageToSend).catch(function (err) {
                return console.error(err.toString());
            });
            alertifyNotifyNotifier(`Sent: ${userChatInputElement.value} NE`, 'bottom-right', 1);
            userChatInputElement.value = "";  
        }
        event.preventDefault();
    }
})
const alertifySuccessNotifier = function (content, position, duration){
    alertify.set('notifier', 'position', position);
    alertify.success(content, duration);
}
const alertifyMessageNotifier = function (content, position, duration){
    alertify.set('notifier', 'position', position);
    alertify.message(content, duration);
}
const alertifyNotifyNotifier = function (content, position, duration) {
    alertify.set('notifier', 'position', position);
    alertify.notify(content, duration);
}