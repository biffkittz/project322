SC.event.addGlobalHandler(SC.event.PreRender, signalRClientPreRenderHandler);

var signalRClientPreRenderHandler = function () {
    SC.util.includeStyleSheet(extensionContext.baseUrl + 'clients\\signalr.css');
    SC.util.includeScript(extensionContext.baseUrl + 'clients\\jquery.min.js', null, runWhenjQueryLoadedProc());
}

var runWhenjQueryLoadedProc = function () {
    console.log('jquery.min.js loaded');

    SC.util.includeScript(extensionContext.baseUrl + 'clients\\signalr.js', null, runWhenSignalRClientLoadedProc());
}

var runWhenSignalRClientLoadedProc = function () {
    console.log('signalr.js loaded');

    var connection = $.hubConnection("https://screentalk.cloud/chatHub", { useDefaultPath: false });
    var chatHubProxy = connection.createHubProxy('chatHub');

    chatHubProxy.on('ReceiveMessage', function(userName, message) {
        console.log(userName + ' ' + message);
    });

    connection.start(
        { transport: ['webSockets', 'longPolling'] }
    ).done(function() {
        // TODO: Add logic to control when to send a message vs once upon load for testing
        //chatHubProxy.invoke('sendMessage', 'userNameFromScContext', 'theMessageFromUser');

        console.log('Now connected, connection ID=' + connection.id);
    }).fail(function() {
        console.log('Could not connect');
    });

    var showSignalRChatModal = function (proxy) {
        SC.dialog.showModalButtonDialog(
            'SignalRChat',
            'SignalRChat Client',
            'Send Message',
            'Default',
            function (container) {
                SC.ui.setContents(container, [
                    $div([
                        $label({ _textResource: 'SignalRClient.MessageLabelText' }, [
                            SC.ui.createTextBox({ id: 'signalRMessage' }, true),
                        ]),
                    ]),
                ]);
            },
            function (eventArgs) {
                proxy.invoke('SendMessage', 'userNameFromScContext', $('signalRMessage').value)
                    .done(function () {
                        console.log ('Invocation of SendMessage succeeded');
                        
                    })
                    .fail(function (error) {
                        console.log('Invocation of SendMessage failed. Error: ' + error);
                    });
                
                SC.dialog.hideModalDialog();

            }
        );
    };

    showSignalRChatModal(chatHubProxy);
}