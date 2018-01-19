if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register(fcmSw)
        .then(function (registration) {

            var config = {
                apiKey: "AIzaSyBsaSobS9hGyaWF4rukfhLU1VQlRQQPS54",
                authDomain: "momofirebase.firebaseapp.com",
                databaseURL: "https://momofirebase.firebaseio.com",
                projectId: "momofirebase",
                storageBucket: "momofirebase.appspot.com",
                messagingSenderId: "247572470053"
            };

            firebase.initializeApp(config);

            const messaging = firebase.messaging();
            messaging.useServiceWorker(registration);

            messaging.requestPermission()
                .then(function () {
                    return messaging.getToken();
                })
                .then(function (token) {
                    SaveTokenOnServer(token);
                    console.log("FCMToken value:", token);
                })
                .catch(function (err) {
                    console.log('Error while request premission.', err);
                });

            messaging.onTokenRefresh(function () {
                messaging.getToken()
                    .then(function (refreshedToken) {
                        SaveTokenOnServer(refreshedToken);
                    })
                    .catch(function (err) {
                        console.log('Unable to retrieve refreshed token ', err);
                    });
            });

            messaging.onMessage(function (payload) {
                console.log("Message received. ", payload);
            });


        }, function (err) {
            console.log('ServiceWorker registration failed: ', err);
        });

    function SaveTokenOnServer(token) {
        $.ajax({
            url: saveTokenUrl,
            type: "POST",
            data: {
                tokenValue: token
            },
        }).fail(error => {
            alert(error.responseText);
        });
    }
}

