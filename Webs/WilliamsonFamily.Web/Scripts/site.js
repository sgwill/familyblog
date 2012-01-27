function notify(message, header, position) {
    if (window.webkitNotifications) {
        var notification = window.webkitNotifications.createNotification('', header, message);
        notification.show();
        setTimeout(function () {
            notification.cancel();
        }, '5000');
    }
    else {
        $.jGrowl(message, { header: header, position: position });
    }
}