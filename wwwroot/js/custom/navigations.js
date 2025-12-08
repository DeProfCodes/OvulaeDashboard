
function ShowLoader(loaderType, message)
{
    var loaderType = loaderType.toLowerCase() == 'main' ? "Main" : "Secondary";

    var loaderId = `#${loaderType}Loader`;
    var loaderImgId = `${loaderId}Image`;
    var loaderMsgId = `${loaderId}Message`;
    
    $(loaderId).css("visibility", "visible");
    $(loaderImgId).css("visibility", "visible");
    $(loaderMsgId).text(message);
}

function HideLoader(loaderType)
{
    var loaderType = loaderType.toLowerCase() == 'main' ? "Main" : "Secondary";

    var loaderId = `#${loaderType}Loader`;
    var loaderImgId = `${loaderId}Image`;

    $(loaderId).css("visibility", "hidden");
    $(loaderImgId).css("visibility", "hidden");
}

function FinishedLoading(Destination, HTMLContent) 
{
   $(Destination).empty();
   $(Destination).html(HTMLContent);    
}

function LoadPartialView(pageUrl, pageTitle, destinationDiv = "#MainPageArea")
{
    destinationDiv = EnsureJQueryId(destinationDiv);
    return new Promise(function (resolve) 
    {
        $.ajax({
            url: pageUrl,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function (res)
            {
                document.title = pageTitle;
                FinishedLoading(destinationDiv, res);
                resolve(true);
            },
            error: function (res)
            {
                console.log(res);
                console.error("Response Text:", res.responseText);
                resolve(false);
            }
        });
    });
}

function LoadPartialViewWithLoader(url, destinationDiv, loaderDiv)
{
    destinationDiv = EnsureJQueryId(destinationDiv);
    loaderDiv = EnsureJQueryId(loaderDiv);

    $.ajax({
        url: url,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        beforeSend: function()
        {
            $(destinationDiv).css("opacity", "0.3");
            $(loaderDiv).css("visibility", "visible");
        },
        success: function (res)
        {
            FinishedLoading(destinationDiv, res);   
            $(loaderDiv).css("visibility", "hidden");
            HideShowElementVisibity("#SecondaryLoaderImage", HideShow.HIDE);
            $(destinationDiv).css("opacity", "1");
        },
        error: function (res)
        {
            //toastr.error("Error");
            $(loaderDiv).css("visibility", "hidden");
            $(destinationDiv).css("opacity", "1");
        }
    });
}

//Button loader to show button spinner and disable button click while loading
function BeginButtonLoad(buttonID, loaderId = null)
{
    loaderId = loaderId == null ? "#actionBtnLoader" : EnsureJQueryId(loaderId);

    buttonID = EnsureJQueryId(buttonID);

    $(loaderId).removeClass('hidden');
    $(buttonID).attr("disabled", true);
    $(buttonID).addClass('actionButtonBusy');
}

//Button loader to hide button spinner and enable button click
function EndButtonLoad(buttonID, loaderId = null)
{
    loaderId = loaderId == null ? "#actionBtnLoader" : EnsureJQueryId(loaderId);

    buttonID = EnsureJQueryId(buttonID);

    $(loaderId).addClass('hidden');
    $(buttonID).attr("disabled", false);
    $(buttonID).removeClass('actionButtonBusy');
}

function PerformAction(actionURL, methodType, payload, responseCallback, actionBtnId, btnLoaderId)
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
            responseCallback(true, response);
        },
        error: function (response)
        {
            if(actionBtnId != null && btnLoaderId != null)
            {
                EndButtonLoad(actionBtnId, btnLoaderId);
            }
            responseCallback(false, response);
        }
        
    });
}

function OpenPage(pageUrl, pageTitle)
{
    LoadPageArea(pageUrl, pageTitle);

    document.title = pageTitle;
}

function LoadPageArea(pageUrl, pageTitle)
{
    LoadPartialView(pageUrl, "#MainPageArea");

    document.title = pageTitle;
}

function LoadPageAreaURL(url, pageName)
{
    ShowLoader("Main", "Loading " + pageName);

    setTimeout(function ()
    {
        HideLoader("Main");
        LoadPartialView(url, "#MainPageArea");
    }, 0);
}