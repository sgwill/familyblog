module = (name) ->
  window[name] = window[name] or {}

module 'Utility'

Utility.notify = (header, message, placement) ->
	if window.webkitNotifications
		notification = window.webkitNotifications.createNotification('', header, message)
		notification.show()
		setTimeout(
			-> notification.cancel(),
		'5000')
		return
	else
		$.jGrowl(message, { header: header, position: position })
		return