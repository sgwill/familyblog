﻿(function(n,t){"use strict";var f,e,i,r,u;if(typeof n!="function")throw"SignalR: jQuery not found. Please ensure jQuery is referenced before the SignalR.js file.";if(!t.JSON)throw"SignalR: No JSON parser found. Please ensure json2.js is referenced before the SignalR.js file if you need to support clients without native JSON parsing support, e.g. IE<8.";i={onStart:"onStart",onStarting:"onStarting",onSending:"onSending",onReceived:"onReceived",onError:"onError",onReconnect:"onReconnect",onDisconnect:"onDisconnect"},r=function(n,i){if(i===!1)return;var r;if(typeof t.console=="undefined")return;r="["+(new Date).toTimeString()+"] SignalR: "+n,t.console.debug?t.console.debug(r):t.console.log&&t.console.log(r)},f=function(n,t,i){return new f.fn.init(n,t,i)},f.fn=f.prototype={init:function(n,t,i){this.url=n,this.qs=t,typeof i=="boolean"&&(this.logging=i)},logging:!1,reconnectDelay:2e3,start:function(r,u){var e=this,o={transport:"auto"},h,s=n.Deferred();return e.transport?(s.resolve(e),s):(n.type(r)==="function"?u=r:n.type(r)==="object"&&(n.extend(o,r),n.type(o.callback)==="function"&&(u=o.callback)),n(e).bind(i.onStart,function(){n.type(u)==="function"&&u.call(e),s.resolve(e)}),h=function(t,r){r=r||0;if(r>=t.length){e.transport||s.reject("SignalR: No transport could be initialized successfully. Try specifying a different transport or none at all for auto initialization.");return}var u=t[r],o=n.type(u)==="object"?u:f.transports[u];o.start(e,function(){e.transport=o,n(e).trigger(i.onStart)},function(){h(t,r+1)})},t.setTimeout(function(){n.ajax(e.url+"/negotiate",{global:!1,type:"POST",data:{},dataType:"json",error:function(t){n(e).trigger(i.onError,[t]),s.reject("SignalR: Error during negotiation request: "+t)},success:function(t){e.appRelativeUrl=t.Url,e.id=t.ConnectionId,e.webSocketServerUrl=t.WebSocketServerUrl;if(!t.ProtocolVersion||t.ProtocolVersion!=="1.0"){n(e).trigger(i.onError,"SignalR: Incompatible protocol version."),s.reject("SignalR: Incompatible protocol version.");return}n(e).trigger(i.onStarting);var u=[],r=[];n.each(f.transports,function(n){if(n==="webSockets"&&!t.TryWebSockets)return!0;r.push(n)}),n.isArray(o.transport)?n.each(o.transport,function(){var t=this;n.type(t)!=="object"&&(n.type(t)!=="string"||n.inArray(""+t,r)<0)||u.push(n.type(t)==="string"?""+t:t)}):n.type(o.transport)!=="object"&&n.inArray(o.transport,r)<0?u=r:u.push(o.transport),h(u)}})},0),s)},starting:function(t){var r=this,u=n(r);return u.bind(i.onStarting,function(){t.call(r),u.unbind(i.onStarting)}),r},send:function(n){var t=this;if(!t.transport)throw"SignalR: Connection must be started before data can be sent. Call .start() before .send()";return t.transport.send(t,n),t},sending:function(t){var r=this;return n(r).bind(i.onSending,function(){t.call(r)}),r},received:function(t){var r=this;return n(r).bind(i.onReceived,function(n,i){t.call(r,i)}),r},error:function(t){var r=this;return n(r).bind(i.onError,function(n,i){t.call(r,i)}),r},disconnected:function(t){var r=this;return n(r).bind(i.onDisconnect,function(){t.call(r)}),r},reconnected:function(t){var r=this;return n(r).bind(i.onReconnect,function(){t.call(r)}),r},stop:function(){var t=this;return t.transport&&(t.transport.stop(t),t.transport=null),delete t.messageId,delete t.groups,n(t).trigger(i.onDisconnect),t},log:r},f.fn.init.prototype=f.fn,u={addQs:function(t,i){return i.qs?typeof i.qs=="object"?t+"&"+n.param(i.qs):typeof i.qs=="string"?t+"&"+i.qs:t+"&"+escape(i.qs.toString()):t},getUrl:function(n,i,r){var u=n.url,f="transport="+i+"&connectionId="+t.escape(n.id);return n.data&&(f+="&connectionData="+t.escape(n.data)),r?(n.messageId&&(f+="&messageId="+n.messageId),n.groups&&(f+="&groups="+t.escape(JSON.stringify(n.groups)))):u=u+"/connect",u+="?"+f,u=this.addQs(u,n)},ajaxSend:function(r,u){var f=r.url+"/send?transport="+r.transport.name+"&connectionId="+t.escape(r.id);f=this.addQs(f,r),n.ajax(f,{global:!1,type:"POST",dataType:"json",data:{data:u},success:function(t){t&&n(r).trigger(i.onReceived,[t])},error:function(t,u){if(u==="abort")return;n(r).trigger(i.onError,[t])}})},processMessages:function(t,u){var f=n(t);if(u){if(u.Disconnect){r("Disconnect command received from server",t.logging),t.stop(),f.trigger(i.onDisconnect);return}u.Messages&&n.each(u.Messages,function(){try{f.trigger(i.onReceived,[this])}catch(u){r("Error raising received "+u,t.logging),n(t).trigger(i.onError,[u])}}),t.messageId=u.MessageId,t.groups=u.TransportData.Groups}},foreverFrame:{count:0,connections:{}}},f.transports={webSockets:{name:"webSockets",send:function(n,t){n.socket.send(t)},start:function(u,f,e){var o,h=!1,s;t.MozWebSocket&&(t.WebSocket=t.MozWebSocket);if(!t.WebSocket){e();return}u.socket||(u.webSocketServerUrl?o=u.webSocketServerUrl:(s=document.location.protocol==="https:"?"wss://":"ws://",o=s+document.location.host+u.appRelativeUrl),n(u).trigger(i.onSending),o+=u.data?"?connectionData="+u.data+"&transport=webSockets&connectionId="+u.id:"?transport=webSockets&connectionId="+u.id,u.socket=new t.WebSocket(o),u.socket.onopen=function(){h=!0,f&&f()},u.socket.onclose=function(t){h?typeof t.wasClean!="undefined"&&t.wasClean===!1&&n(u).trigger(i.onError):e&&e(),u.socket=null},u.socket.onmessage=function(f){var e=t.JSON.parse(f.data),o;e&&(o=n(u),e.Messages?n.each(e.Messages,function(){try{o.trigger(i.onReceived,[this])}catch(n){r("Error raising received "+n,u.logging)}}):o.trigger(i.onReceived,[e]))})},stop:function(n){n.socket!==null&&(n.socket.close(),n.socket=null)}},serverSentEvents:{name:"serverSentEvents",timeOut:3e3,start:function(f,e,o){var s=this,l=!1,c=n(f),h=!e,v,a;f.eventSource&&f.stop();if(!t.EventSource){o&&o();return}c.trigger(i.onSending),v=u.getUrl(f,this.name,h);try{f.eventSource=new t.EventSource(v)}catch(y){r("EventSource failed trying to connect with error "+y.Message,f.logging),o?o():(c.trigger(i.onError,[y]),h&&(r("EventSource reconnecting",f.logging),s.reconnect(f)));return}a=t.setTimeout(function(){l===!1&&(r("EventSource timed out trying to connect",f.logging),o&&o(),h?(r("EventSource reconnecting",f.logging),s.reconnect(f)):s.stop(f))},s.timeOut),f.eventSource.addEventListener("open",function(){r("EventSource connected",f.logging),a&&t.clearTimeout(a),l===!1&&(l=!0,e&&e(),h&&c.trigger(i.onReconnect))},!1),f.eventSource.addEventListener("message",function(n){if(n.data==="initialized")return;u.processMessages(f,t.JSON.parse(n.data))},!1),f.eventSource.addEventListener("error",function(n){if(!l){o&&o();return}r("EventSource readyState: "+f.eventSource.readyState,f.logging),n.eventPhase===t.EventSource.CLOSED?f.eventSource.readyState===t.EventSource.CONNECTING?(r("EventSource reconnecting due to the server connection ending",f.logging),s.reconnect(f)):(r("EventSource closed",f.logging),s.stop(f)):(r("EventSource error",f.logging),c.trigger(i.onError))},!1)},reconnect:function(n){var i=this;t.setTimeout(function(){i.stop(n),i.start(n)},n.reconnectDelay)},send:function(n,t){u.ajaxSend(n,t)},stop:function(n){n&&n.eventSource&&(n.eventSource.close(),n.eventSource=null,delete n.eventSource)}},foreverFrame:{name:"foreverFrame",timeOut:3e3,start:function(f,e,o){var h=this,l=u.foreverFrame.count+=1,c,a,s=n("<iframe data-signalr-connection-id='"+f.id+"' style='position:absolute;width:0;height:0;visibility:hidden;'></iframe>");if(t.EventSource){o&&o();return}n(f).trigger(i.onSending),c=u.getUrl(f,this.name),c+="&frameId="+l,s.prop("src",c),u.foreverFrame.connections[l]=f,s.bind("readystatechange",function(){n.inArray(this.readyState,["loaded","complete"])<0||(r("Forever frame iframe readyState changed to "+this.readyState+", reconnecting",f.logging),h.reconnect(f))}),f.frame=s[0],f.frameId=l,e&&(f.onSuccess=e),n("body").append(s),a=t.setTimeout(function(){f.onSuccess&&(h.stop(f),o&&o())},h.timeOut)},reconnect:function(n){var i=this;t.setTimeout(function(){var r=n.frame,t=u.getUrl(n,i.name,!0)+"&frameId="+n.frameId;r.src=t},n.reconnectDelay)},send:function(n,t){u.ajaxSend(n,t)},receive:u.processMessages,stop:function(t){t.frame&&(t.frame.stop?t.frame.stop():t.frame.document&&t.frame.document.execCommand&&t.frame.document.execCommand("Stop"),n(t.frame).remove(),delete u.foreverFrame.connections[t.frameId],t.frame=null,t.frameId=null,delete t.frame,delete t.frameId)},getConnection:function(n){return u.foreverFrame.connections[n]},started:function(t){t.onSuccess?(t.onSuccess(),t.onSuccess=null,delete t.onSuccess):n(t).trigger(i.onReconnect)}},longPolling:{name:"longPolling",reconnectDelay:3e3,start:function(r,f){var o=this;r.pollXhr&&r.stop(),r.messageId=null,t.setTimeout(function(){(function e(f,s){n(f).trigger(i.onSending);var a=f.messageId,l=a===null,v=u.getUrl(f,o.name,!l),c=null,h=!1;f.pollXhr=n.ajax(v,{global:!1,type:"GET",dataType:"json",success:function(r){var c=0,o=!1;s===!0&&h===!1&&(n(f).trigger(i.onReconnect),h=!0),u.processMessages(f,r),r&&n.type(r.TransportData.LongPollDelay)==="number"&&(c=r.TransportData.LongPollDelay),r&&r.TimedOut&&(o=r.TimedOut),c>0?t.setTimeout(function(){e(f,o)},c):e(f,o)},error:function(u,o){if(o==="abort")return;c&&clearTimeout(c),n(f).trigger(i.onError,[u]),t.setTimeout(function(){e(f,!0)},r.reconnectDelay)}}),s===!0&&(c=t.setTimeout(function(){h===!1&&(n(f).trigger(i.onReconnect),h=!0)},o.reconnectDelay))})(r),t.setTimeout(f,150)},250)},send:function(n,t){u.ajaxSend(n,t)},stop:function(n){n.pollXhr&&(n.pollXhr.abort(),n.pollXhr=null,delete n.pollXhr)}}},f.noConflict=function(){return n.connection===f&&(n.connection=e),f},n.connection&&(e=n.connection),n.connection=n.signalR=f})(window.jQuery,window),jQuery.easing.jswing=jQuery.easing.swing,jQuery.extend(jQuery.easing,{def:"easeOutQuad",swing:function(n,t,i,r,u){return jQuery.easing[jQuery.easing.def](n,t,i,r,u)},easeInQuad:function(n,t,i,r,u){return r*(t/=u)*t+i},easeOutQuad:function(n,t,i,r,u){return-r*(t/=u)*(t-2)+i},easeInOutQuad:function(n,t,i,r,u){return(t/=u/2)<1?r/2*t*t+i:-r/2*(--t*(t-2)-1)+i},easeInCubic:function(n,t,i,r,u){return r*(t/=u)*t*t+i},easeOutCubic:function(n,t,i,r,u){return r*((t=t/u-1)*t*t+1)+i},easeInOutCubic:function(n,t,i,r,u){return(t/=u/2)<1?r/2*t*t*t+i:r/2*((t-=2)*t*t+2)+i},easeInQuart:function(n,t,i,r,u){return r*(t/=u)*t*t*t+i},easeOutQuart:function(n,t,i,r,u){return-r*((t=t/u-1)*t*t*t-1)+i},easeInOutQuart:function(n,t,i,r,u){return(t/=u/2)<1?r/2*t*t*t*t+i:-r/2*((t-=2)*t*t*t-2)+i},easeInQuint:function(n,t,i,r,u){return r*(t/=u)*t*t*t*t+i},easeOutQuint:function(n,t,i,r,u){return r*((t=t/u-1)*t*t*t*t+1)+i},easeInOutQuint:function(n,t,i,r,u){return(t/=u/2)<1?r/2*t*t*t*t*t+i:r/2*((t-=2)*t*t*t*t+2)+i},easeInSine:function(n,t,i,r,u){return-r*Math.cos(t/u*(Math.PI/2))+r+i},easeOutSine:function(n,t,i,r,u){return r*Math.sin(t/u*(Math.PI/2))+i},easeInOutSine:function(n,t,i,r,u){return-r/2*(Math.cos(Math.PI*t/u)-1)+i},easeInExpo:function(n,t,i,r,u){return t==0?i:r*Math.pow(2,10*(t/u-1))+i},easeOutExpo:function(n,t,i,r,u){return t==u?i+r:r*(-Math.pow(2,-10*t/u)+1)+i},easeInOutExpo:function(n,t,i,r,u){return t==0?i:t==u?i+r:(t/=u/2)<1?r/2*Math.pow(2,10*(t-1))+i:r/2*(-Math.pow(2,-10*--t)+2)+i},easeInCirc:function(n,t,i,r,u){return-r*(Math.sqrt(1-(t/=u)*t)-1)+i},easeOutCirc:function(n,t,i,r,u){return r*Math.sqrt(1-(t=t/u-1)*t)+i},easeInOutCirc:function(n,t,i,r,u){return(t/=u/2)<1?-r/2*(Math.sqrt(1-t*t)-1)+i:r/2*(Math.sqrt(1-(t-=2)*t)+1)+i},easeInElastic:function(n,t,i,r,u){var o=1.70158,f=0,e=r;return t==0?i:(t/=u)==1?i+r:(f||(f=u*.3),e<Math.abs(r)?(e=r,o=f/4):o=f/(2*Math.PI)*Math.asin(r/e),-(e*Math.pow(2,10*(t-=1))*Math.sin((t*u-o)*2*Math.PI/f))+i)},easeOutElastic:function(n,t,i,r,u){var o=1.70158,f=0,e=r;return t==0?i:(t/=u)==1?i+r:(f||(f=u*.3),e<Math.abs(r)?(e=r,o=f/4):o=f/(2*Math.PI)*Math.asin(r/e),e*Math.pow(2,-10*t)*Math.sin((t*u-o)*2*Math.PI/f)+r+i)},easeInOutElastic:function(n,t,i,r,u){var o=1.70158,f=0,e=r;if(t==0)return i;if((t/=u/2)==2)return i+r;return f||(f=u*.3*1.5),e<Math.abs(r)?(e=r,o=f/4):o=f/(2*Math.PI)*Math.asin(r/e),t<1?-.5*e*Math.pow(2,10*(t-=1))*Math.sin((t*u-o)*2*Math.PI/f)+i:e*Math.pow(2,-10*(t-=1))*Math.sin((t*u-o)*2*Math.PI/f)*.5+r+i},easeInBack:function(n,t,i,r,u,f){return f==undefined&&(f=1.70158),r*(t/=u)*t*((f+1)*t-f)+i},easeOutBack:function(n,t,i,r,u,f){return f==undefined&&(f=1.70158),r*((t=t/u-1)*t*((f+1)*t+f)+1)+i},easeInOutBack:function(n,t,i,r,u,f){return f==undefined&&(f=1.70158),(t/=u/2)<1?r/2*t*t*(((f*=1.525)+1)*t-f)+i:r/2*((t-=2)*t*(((f*=1.525)+1)*t+f)+2)+i},easeInBounce:function(n,t,i,r,u){return r-jQuery.easing.easeOutBounce(n,u-t,0,r,u)+i},easeOutBounce:function(n,t,i,r,u){return(t/=u)<1/2.75?r*7.5625*t*t+i:t<2/2.75?r*(7.5625*(t-=1.5/2.75)*t+.75)+i:t<2.5/2.75?r*(7.5625*(t-=2.25/2.75)*t+.9375)+i:r*(7.5625*(t-=2.625/2.75)*t+.984375)+i},easeInOutBounce:function(n,t,i,r,u){return t<u/2?jQuery.easing.easeInBounce(n,t*2,0,r,u)*.5+i:jQuery.easing.easeOutBounce(n,t*2-u,0,r,u)*.5+r*.5+i}}),jQuery.fn.not_exists=function(){return jQuery(this).length==0},jQuery.fn.jqcollapse=function(n){var n=jQuery.extend({slide:!0,speed:300,easing:""},n);$(this).each(function(){var t=$(this).attr("id");$("#"+t+" li > ul").each(function(){var i=$(this).parent("li"),r=$(this).remove();i.children("a").not_exists()&&i.wrapInner("<a/>"),i.find("a").addClass("jqcNode").css("cursor","pointer").click(function(){n.slide==!0?r.slideToggle(n.speed,n.easing):r.toggle()}),i.append(r)}),$("#"+t+" ul").hide()})},function(n){n.jGrowl=function(t,i){n("#jGrowl").size()==0&&n('<div id="jGrowl"></div>').addClass(i&&i.position?i.position:n.jGrowl.defaults.position).appendTo("body"),n("#jGrowl").jGrowl(t,i)},n.fn.jGrowl=function(t,i){if(n.isFunction(this.each)){var r=arguments;return this.each(function(){var u=this;n(this).data("jGrowl.instance")==undefined&&(n(this).data("jGrowl.instance",n.extend(new n.fn.jGrowl,{notifications:[],element:null,interval:null})),n(this).data("jGrowl.instance").startup(this)),n.isFunction(n(this).data("jGrowl.instance")[t])?n(this).data("jGrowl.instance")[t].apply(n(this).data("jGrowl.instance"),n.makeArray(r).slice(1)):n(this).data("jGrowl.instance").create(t,i)})}},n.extend(n.fn.jGrowl.prototype,{defaults:{pool:0,header:"",group:"",sticky:!1,position:"top-right",glue:"after",theme:"default",themeState:"highlight",corners:"10px",check:250,life:3e3,closeDuration:"normal",openDuration:"normal",easing:"swing",closer:!0,closeTemplate:"&times;",closerTemplate:"<div>[ close all ]</div>",log:function(){},beforeOpen:function(){},afterOpen:function(){},open:function(){},beforeClose:function(){},close:function(){},animateOpen:{opacity:"show"},animateClose:{opacity:"hide"}},notifications:[],element:null,interval:null,create:function(t,i){var i=n.extend({},this.defaults,i);typeof i.speed!="undefined"&&(i.openDuration=i.speed,i.closeDuration=i.speed),this.notifications.push({message:t,options:i}),i.log.apply(this.element,[this.element,t,i])},render:function(t){var r=this,u=t.message,i=t.options,t;i.themeState=i.themeState==""?"":"ui-state-"+i.themeState,t=n('<div class="jGrowl-notification '+i.themeState+" ui-corner-all"+(i.group!=undefined&&i.group!=""?" "+i.group:"")+'"><div class="jGrowl-close">'+i.closeTemplate+'</div><div class="jGrowl-header">'+i.header+'</div><div class="jGrowl-message">'+u+"</div></div>").data("jGrowl",i).addClass(i.theme).children("div.jGrowl-close").bind("click.jGrowl",function(){n(this).parent().trigger("jGrowl.close")}).parent(),n(t).bind("mouseover.jGrowl",function(){n("div.jGrowl-notification",r.element).data("jGrowl.pause",!0)}).bind("mouseout.jGrowl",function(){n("div.jGrowl-notification",r.element).data("jGrowl.pause",!1)}).bind("jGrowl.beforeOpen",function(){i.beforeOpen.apply(t,[t,u,i,r.element])!=!1&&n(this).trigger("jGrowl.open")}).bind("jGrowl.open",function(){i.open.apply(t,[t,u,i,r.element])!=!1&&(i.glue=="after"?n("div.jGrowl-notification:last",r.element).after(t):n("div.jGrowl-notification:first",r.element).before(t),n(this).animate(i.animateOpen,i.openDuration,i.easing,function(){n.browser.msie&&(parseInt(n(this).css("opacity"),10)===1||parseInt(n(this).css("opacity"),10)===0)&&this.style.removeAttribute("filter"),n(this).data("jGrowl")!=null&&(n(this).data("jGrowl").created=new Date),n(this).trigger("jGrowl.afterOpen")}))}).bind("jGrowl.afterOpen",function(){i.afterOpen.apply(t,[t,u,i,r.element])}).bind("jGrowl.beforeClose",function(){i.beforeClose.apply(t,[t,u,i,r.element])!=!1&&n(this).trigger("jGrowl.close")}).bind("jGrowl.close",function(){n(this).data("jGrowl.pause",!0),n(this).animate(i.animateClose,i.closeDuration,i.easing,function(){n.isFunction(i.close)?i.close.apply(t,[t,u,i,r.element])!==!1&&n(this).remove():n(this).remove()})}).trigger("jGrowl.beforeOpen"),i.corners!=""&&n.fn.corner!=undefined&&n(t).corner(i.corners),n("div.jGrowl-notification:parent",r.element).size()>1&&n("div.jGrowl-closer",r.element).size()==0&&this.defaults.closer!=!1&&n(this.defaults.closerTemplate).addClass("jGrowl-closer "+this.defaults.themeState+" ui-corner-all").addClass(this.defaults.theme).appendTo(r.element).animate(this.defaults.animateOpen,this.defaults.speed,this.defaults.easing).bind("click.jGrowl",function(){n(this).siblings().trigger("jGrowl.beforeClose"),n.isFunction(r.defaults.closer)&&r.defaults.closer.apply(n(this).parent()[0],[n(this).parent()[0]])})},update:function(){n(this.element).find("div.jGrowl-notification:parent").each(function(){n(this).data("jGrowl")!=undefined&&n(this).data("jGrowl").created!=undefined&&n(this).data("jGrowl").created.getTime()+parseInt(n(this).data("jGrowl").life)<+new Date&&n(this).data("jGrowl").sticky!=!0&&(n(this).data("jGrowl.pause")==undefined||n(this).data("jGrowl.pause")!=!0)&&n(this).trigger("jGrowl.beforeClose")}),this.notifications.length>0&&(this.defaults.pool==0||n(this.element).find("div.jGrowl-notification:parent").size()<this.defaults.pool)&&this.render(this.notifications.shift()),n(this.element).find("div.jGrowl-notification:parent").size()<2&&n(this.element).find("div.jGrowl-closer").animate(this.defaults.animateClose,this.defaults.speed,this.defaults.easing,function(){n(this).remove()})},startup:function(t){this.element=n(t).addClass("jGrowl").append('<div class="jGrowl-notification"></div>'),this.interval=setInterval(function(){n(t).data("jGrowl.instance").update()},parseInt(this.defaults.check)),n.browser.msie&&parseInt(n.browser.version)<7&&!window.XMLHttpRequest&&n(this.element).addClass("ie6")},shutdown:function(){n(this.element).removeClass("jGrowl").find("div.jGrowl-notification").remove(),clearInterval(this.interval)},close:function(){n(this.element).find("div.jGrowl-notification").each(function(){n(this).trigger("jGrowl.beforeClose")})}}),n.jGrowl.defaults=n.fn.jGrowl.prototype.defaults}(jQuery),function(){var n;n=function(n){return window[n]=window[n]||{}},n("Utility"),Utility.notify=function(n,t){var r;window.webkitNotifications?(r=window.webkitNotifications.createNotification("",n,t),r.show(),setTimeout(function(){return r.cancel()},"5000")):$.jGrowl(t,{header:n,position:position})}}.call(this),function(){var n;window.onerror=function(t){n(t,arguments.callee.trace())},n=function(n,t){var i,r;if(n===null)return;if(logErrorUrl===null){alert("logErrorUrl must be defined.");return}return r=n.fileName===!0?n.fileName:document.location,t===null&&n.stack!==null&&(t=n.stack),i=n.message===!0?n.name+": "+n.message:n,i+=": at document path '"+r+"'.",t!==null&&(i+="\n  at "+t.join("\n  at ")),$.ajax({type:"POST",url:logErrorUrl,data:{message:i}})},Function.prototype.trace=function(){var n,t;t=[],n=this;while(n)t.push(n.signature()),n=n.caller;return t},Function.prototype.getName=function(){var t,n;if(this.name)return this.name;return t=this.toString().split("\n")[0],n=/^function ([^\s(]+).+/,n.test(t)?t.split("\n")[0].replace(n,"$1")||"anonymous":"anonymous"},Function.prototype.signature=function(){var r,i,n,u,t;i={name:this.getName(),params:[],toString:function(){var n;return n=this.params.length>0?"'"+this.params.join("', '")+"'":"",this.name+"("+n+")"}};if(this.arguments)for(t=this.arguments.length,n=0,u=t.length;n<u;n++)r=t[n],i.params.push(r);return i}}.call(this)