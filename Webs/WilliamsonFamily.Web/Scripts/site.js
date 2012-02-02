(function() {
  var module;

  module = function(name) {
    return window[name] = window[name] || {};
  };

  module('Utility');

  Utility.notify = function(header, message, placement) {
    var notification;
    if (window.webkitNotifications) {
      notification = window.webkitNotifications.createNotification('', header, message);
      notification.show();
      setTimeout(function() {
        return notification.cancel();
      }, '5000');
    } else {
      $.jGrowl(message, {
        header: header,
        position: position
      });
    }
  };

}).call(this);
