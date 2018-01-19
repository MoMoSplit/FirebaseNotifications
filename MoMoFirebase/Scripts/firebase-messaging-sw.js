importScripts('firebase-app.js');
importScripts('firebase-messaging.js');

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

messaging.setBackgroundMessageHandler(function (payload) {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    const notificationTitle = 'Background Message Title';
    const notificationOptions = {
        body: 'Background Message body.',
        icon: '/firebase-logo.png'
    };

    return self.registration.showNotification(notificationTitle,
        notificationOptions);
});
