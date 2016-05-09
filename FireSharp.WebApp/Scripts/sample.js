(function () {
    // Get a reference to the root of the chat data.
    var dataRef = new Firebase("https://firesharp.firebaseio.com/chat");
    //dataRef.set("I am now writing data into Firebase!");

    // When the user presses enter on the message input, write the message to firebase.
    $("#messageInput")
        .keypress(function (e) {
            if (e.keyCode == 13) {
                var name = $("#nameInput").val();
                var text = $("#messageInput").val();
                dataRef.push({ name: name, text: text });
                $("#messageInput").val("");
            }
        });

    // Add a callback that is triggered for each chat message.
    dataRef.limit(10)
        .on("child_added",
            function (snapshot) {
                var message = snapshot.val();
                $("<div/>")
                    .text(message.text)
                    .prepend($("<em/>")
                        .text(message.name + ": "))
                    .appendTo($("#messagesDiv"));
                $("#messagesDiv")[0].scrollTop = $("#messagesDiv")[0].scrollHeight;
            });

    function faker(c) {
        var result = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (var i = 0; i < (c || 5) ; i++) {
            result += possible.charAt(Math.floor(Math.random() * possible.length));
        }

        return result;
    }

    setInterval(function () {
        dataRef.push({ name: faker(), text: faker(10) });
    },
        100);
})();