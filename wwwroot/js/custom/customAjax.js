//Button loader to show button spinner and disable button click while loading
function BeginButtonLoad(buttonID, loaderId = null)
{
    buttonID = EnsureJQueryId(buttonID);

    $(loaderId).removeClass('hidden');
    $(buttonID).attr("disabled", true);
    $(buttonID).addClass('actionButtonBusy');
}

//Button loader to hide button spinner and enable button click
function EndButtonLoad(buttonID, loaderId = null)
{
    buttonID = EnsureJQueryId(buttonID);

    $(loaderId).addClass('hidden');
    $(buttonID).attr("disabled", false);
    $(buttonID).removeClass('actionButtonBusy');
}

function PerformAction5(actionURL, methodType, payload, actionBtnId, btnLoaderId, responseCallback)
{
    $.ajax({
        type: methodType,
        url: actionURL,
        data: payload,
        beforeSend: function ()
        {
            if(actionBtnId != null && btnLoaderId != null)
            {
                BeginButtonLoad(actionBtnId, btnLoaderId);
            }
        },
        success: function (response)
        {
            if(actionBtnId != null && btnLoaderId != null)
            {
                EndButtonLoad(actionBtnId, btnLoaderId);
            }
            if(responseCallback != responseCallback)
            {
                responseCallback(true, response);
            }
        },
        error: function (response)
        {
            if(actionBtnId != null && btnLoaderId != null)
            {
                EndButtonLoad(actionBtnId, btnLoaderId);
            }
            if(responseCallback != responseCallback)
            {
                responseCallback(false, response);
            }
        }
    });
}