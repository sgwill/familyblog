(function() {
  var logError;

  window.onerror = function(msg, url, line) {
    logError(msg, arguments.callee.trace());
  };

  logError = function(ex, stack) {
    var out, url;
    if (ex === null) return;
    if (logErrorUrl === null) {
      alert('logErrorUrl must be defined.');
      return;
    }
    url = ex.fileName === !null ? ex.fileName : document.location;
    if (stack === null && ex.stack !== null) stack = ex.stack;
    out = ex.message === !null ? ex.name + ": " + ex.message : ex;
    out += ": at document path '" + url + "'.";
    if (stack !== null) out += "\n  at " + stack.join("\n  at ");
    return $.ajax({
      type: 'POST',
      url: logErrorUrl,
      data: {
        message: out
      }
    });
  };

  Function.prototype.trace = function() {
    var current, trace;
    trace = [];
    current = this;
    while (current) {
      trace.push(current.signature());
      current = current.caller;
    }
    return trace;
  };

  Function.prototype.getName = function() {
    var definition, exp;
    if (this.name) return this.name;
    definition = this.toString().split("\n")[0];
    exp = /^function ([^\s(]+).+/;
    if (exp.test(definition)) {
      return definition.split("\n")[0].replace(exp, "$1") || "anonymous";
    }
    return "anonymous";
  };

  Function.prototype.signature = function() {
    var arg, signature, _i, _len, _ref;
    signature = {
      name: this.getName(),
      params: [],
      toString: function() {
        var params;
        params = this.params.length > 0 ? "'" + this.params.join("', '") + "'" : "";
        return this.name + "(" + params + ")";
      }
    };
    if (this.arguments) {
      _ref = this.arguments.length;
      for (_i = 0, _len = _ref.length; _i < _len; _i++) {
        arg = _ref[_i];
        signature.params.push(arg);
      }
    }
    return signature;
  };

}).call(this);
