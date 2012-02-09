window.onerror = (msg, url, line) ->
	logError(msg, arguments.callee.trace())
	return

logError = (ex, stack) ->
	if (ex is null) 
		return
	if (logErrorUrl is null)
		alert('logErrorUrl must be defined.')
		return

	url = if ex.fileName is not null then ex.fileName else document.location
	if stack == null && ex.stack != null
		stack = ex.stack

	# format output
	out = if ex.message is not null then ex.name + ": " + ex.message else ex
	out += ": at document path '" + url + "'."
	if stack != null
		out += "\n  at " + stack.join("\n  at ")

	# send error message
	$.ajax({
		type: 'POST',
		url: logErrorUrl,
		data: { message: out }
	})

Function.prototype.trace = ->
	trace = []
	current = this
	while (current)
		trace.push(current.signature())
		current = current.caller
	return trace

Function.prototype.getName = ->
	if this.name
		return this.name
	definition = this.toString().split("\n")[0]
	exp = /^function ([^\s(]+).+/
	if exp.test(definition)
		return definition.split("\n")[0].replace(exp, "$1") || "anonymous"
	return "anonymous"

Function.prototype.signature = ->
	signature = 
		name: this.getName()
		params: []
		toString: ->
			params = if this.params.length > 0 then "'" + this.params.join("', '") + "'" else ""
			return this.name + "(" + params + ")"

	if this.arguments 
		signature.params.push arg for arg in this.arguments.length

	return signature