using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security;
using System.Threading;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Microsoft.AspNetCore.SignalR.Client;
using ScreenConnect;

public class SignalRClientAsyncSessionEventTrigger : IAsyncDynamicSessionEventTrigger
{
    // ProcessEventAsync checks for when a host ends a support session
	public async Task ProcessEventAsync(SessionEventTriggerEvent sessionEventTriggerEvent)
	{
		if (

			sessionEventTriggerEvent.Event.EventType == SessionEventType.DeletedSession &&
			sessionEventTriggerEvent.Session.SessionType == SessionType.Support
		)
		{
			try
			{
                Microsoft.AspNetCore.SignalR.Client.HubConnection connection = new HubConnectionBuilder()
                    .WithUrl("https://biffkittz.screenconnect.com/chatHub")
                    .Build();

                await connection.InvokeAsync("SendMessage", userTextBox.Text, messageTextBox.Text);

				//var sessionDetails = await SessionManagerPool.Demux.GetSessionDetailsAsync(sessionEventTriggerEvent.Session.SessionID);
			}
			catch (Exception ex)
			{
				// TODO: Log error to streaming?
			}
		}
	}
}
