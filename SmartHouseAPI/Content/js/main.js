window.onload = function () {
    // set same hieght left and right colum
    sameCol();

    $('.device-div-on-off-cmd-btn').on('click', sendOnOff);
    $('.btn-down').on('click', sendDown);
    $('.btn-up').on('click', sendUp);
    $('.device-status-btn-del').on('click', sendDel);
    $('.btn-add').on('click', sendAdd);
};

// set same hieght left and right colum
function sameCol() {
    $('.left-col').height($('.right-col').height() + 9);
}

function sendOnOff(event) {
    event.preventDefault();
    var imgBtn = $(this).children("img");
    var divStatus = $(this).closest(".device-status");
    var divState = $(this).closest(".device-status").children(".div-state");
    var divOnOff = $(this).closest(".device-div-on-off");
    var sType = $(this).attr('data-type');
    var id = $(this).attr('data-id');

    $.ajax({
        url: "/api/values/onoff/" + id,
        type: "POST",
        data: JSON.stringify(sType),
        contentType: "application/json;charset=utf-8",
        success: function (param) {
            if (param.State == "True") {
                imgBtn.attr('src', '../Content/images/btn_on.png');
                divState.children("span").html("State: ON");
            } else {
                imgBtn.attr('src', '../Content/images/btn_off.png');
                divState.children("span").html("State: OFF");
                divState.children("img").attr('src', '../Content/images/' + sType + '_off.png');
            }
            switch (sType) {
                case "Lamp":
                    if (param.State == "True") {
                        divState.children("img").attr('src', '../Content/images/lamp_on.png');
                    }
                    break;
                case "Fan":
                    if (param.State == "True") {
                        divState.children("img").attr('src', '../Content/images/fan_' + param.Speed + '.gif');

                        divOnOff.before('<div class="div-status-block div-speed"></div>');
                        putSpeed(divStatus, id, param.Speed, sType);
                    } else {
                        divStatus.children('.div-speed').remove();
                    }
                    break;
                case "Louvers":
                    if (param.State == "True") {
                        divState.children("img").attr('src', '../Content/images/louvers_' + param.Open + '.png');

                        divOnOff.before('<div class="div-status-block div-open"></div>');
                        putOpen(divStatus, id, param.Open, sType);
                    } else {
                        divStatus.children('.div-open').remove();
                    }
                    break;
                case "Tv":
                    if (param.State == "True") {
                        divState.children("img").attr('src', '../Content/images/tv_' + param.Channel + '.png');

                        divOnOff.before('<div class="div-status-block div-volume"></div>');
                        putVolume(divStatus, id, param.Volume, sType);

                        divOnOff.before('<div class="div-status-block div-program"></div>');
                        putProgram(divStatus, id, param.Channel, sType);
                    } else {
                        divStatus.children('.div-program').remove();
                        divStatus.children('.div-volume').remove();
                    }
                    break;
            }
            sameCol();
        }
    });
};

function sendDown(event) {
    sendParam(event, $(this), "down");
}

function sendUp(event) {
    sendParam(event, $(this), "up");
}

function sendParam(event, elem, cmd) {
    event.preventDefault();
    var divParam = elem.closest(".div-status-block");
    var imgParam = divParam.find(".device-div-speed-state");
    var divState = elem.closest(".device-status").children(".div-state");

    var sType = elem.attr('data-type');
    var id = elem.attr('data-id');
    var sParam = elem.attr('data-param');

    $.ajax({
        url: "/api/values/param/" + id + "/" + sType + "/" + cmd,
        type: "POST",
        data: JSON.stringify(sParam),
        contentType: "application/json;charset=utf-8",
        success: function (param) {
            switch (sType) {
                case "Fan":
                    if (sParam == "Speed") {
                        imgParam.attr('src', '../Content/images/val_' + param.Speed + '.png');
                        divState.children("img").attr('src', '../Content/images/fan_' + param.Speed + '.gif');
                    }
                    break;
                case "Louvers":
                    if (sParam == "Open") {
                        divState.children("img").attr('src', '../Content/images/louvers_' + param.Open + '.png');
                        divParam.find('.device-status-name').html("Open: " + 50 * Number(param.Open) + "%");
                    }
                    break;
                case "Tv":
                    if (sParam == "Volume") {
                        imgParam.attr('src', '../Content/images/val_' + param.Volume + '.png');
                    }
                    if (sParam == "Program") {
                        divState.children("img").attr('src', '../Content/images/tv_' + param.Channel + '.png');
                        divParam.find('.device-div-prog-state').attr('src', '../Content/images/icon_' + param.Channel + '.png');
                    }
                    break;
            }
        }
    });
};


function sendDel(event) {
    event.preventDefault();
    var divStatus = $(this).closest(".device-status");

    var sType = $(this).attr('data-type');
    var id = $(this).attr('data-id');

    $.ajax({
        url: "/api/values/del/" + id + "/" + sType,
        type: "GET",
        success: function (answer) {
            divStatus.remove();
            sameCol();
        }
    });
};


function sendAdd(event) {
    event.preventDefault();

    var imgBtn = $(this).children("img");
    var divStatus = $(this).closest(".device-status");
    var divState = $(this).closest(".device-status").children(".div-state");
    var divOnOff = $(this).closest(".device-div-on-off");

    var sType = $(this).attr('data-type');
    var sName = $(".name-of-new-device").val();

    $.ajax({
        url: "/api/values/add/" + sType,
        type: "POST",
        data: JSON.stringify(sName),
        contentType: "application/json;charset=utf-8",
        success: function (param) {
            $("#RightCol").append('<div class="device-status"></div>');
            var divStatusNew = $("#RightCol .device-status:last");

            putHeader(divStatusNew, param.Id, sType, sName);
            putState(divStatusNew, param, sType);

            if (param.State == "True") {
                switch (sType) {
                    case "Fan":
                        divStatusNew.append('<div class="div-status-block div-speed"></div>');
                        putSpeed(divStatusNew, param.Id, param.Speed, sType);
                        break;
                    case "Louvers":
                        divStatusNew.append('<div class="div-status-block div-open"></div>');
                        putOpen(divStatusNew, param.Id, param.Open, sType);
                        break;
                    case "Tv":
                        divStatusNew.append('<div class="div-status-block div-volume"></div>');
                        putVolume(divStatusNew, param.Id, param.Volume, sType);

                        divStatusNew.append('<div class="div-status-block div-program"></div>');
                        putProgram(divStatusNew, param.Id, param.Program, sType);
                        break;
                }
            }

            putOnOff(divStatusNew, param.Id, param.State, sType);

            $(".name-of-new-device").val('');
            sameCol();
        }
    });
};

function putHeader(divStatusNew, id, sType, sName) {
    divStatusNew.append('<div class="device-status-header"></div>');
    var divHeader = divStatusNew.children(".device-status-header");

    divHeader.append('<img class="device-status-icon" src="../Content/images/' + sType + '_3.png" alt="Device icon" />');
    divHeader.append('<span class="device-status-name">' + sName + '</span>');
    divHeader.append('<a class="device-status-btn-del" href="" data-id="' + id + '" data-type="' + sType + '"></a>');
    divHeader.children('.device-status-btn-del').append('<img class="device-status-img-del" src="../Content/images/del_green_2.png" alt="Delete" />');

    divHeader.children('.device-status-btn-del').on('click', sendDel);
}



function putState(divStatusNew, param, sType) {
    divStatusNew.append('<div class="device-div-on-off div-state"></div>');
    var divState = divStatusNew.children(".div-state");

    if (param.State == "True")
    {
        divState.append('<span class="device-state">State: ON</span>');
    }
    else
    {
        divState.append('<span class="device-state">State: OFF</span>');
    }

    var sImgFile = "";
    switch (sType) {
        case "Lamp":
            if (param.State == "True") { 
                sImgFile = "lamp_on.png"; 
            } else {
                sImgFile = "lamp_off.png"; 
            }
            break;
        case "Fan":
            if (param.State == "True") { 
                sImgFile = "fan_" + param.Speed + ".gif";
            } else {
                sImgFile = "fan_off.png";
            }
            break;
        case "Louvers":
            if (param.State == "True") { 
                sImgFile = "louvers_" + param.Open + ".png";
            } else {
                sImgFile = "louvers_off.png";
            }
            break;
        case "Tv":
            if (param.State == "True") { 
                sImgFile = "tv_" + param.Program + ".png";
            } else {
                sImgFile = "tv_off.png";
            }
            break;
    }

    divState.append('<img class="device-image" src="../Content/images/' + sImgFile + '" />');
}


function putSpeed(divStatusNew, id, speed, sType) {
    var divSpeed = divStatusNew.find('.div-speed');
    divSpeed.append('<div class="div-param-text"><span class="device-status-name">Speed</span></div>');
    divSpeed.append('<div class="div-param-btn"></div>');
    var divBtn = divSpeed.find('.div-param-btn');
    divBtn.append('<a class="btn-down" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Speed"><img class="device-div-speed-img" src="../Content/images/btn_down.png" /></a>');
    divBtn.append('<img class="device-div-speed-state" src="../Content/images/val_' + speed + '.png")" />');
    divBtn.append('<a class="btn-up" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Speed"><img class="device-div-speed-img" src="../Content/images/btn_up.png" /></a>');

    divBtn.children('.btn-down').on('click', sendDown);
    divBtn.children('.btn-up').on('click', sendUp);
}



function putOpen(divStatusNew, id, open, sType) {
    var divOpen = divStatusNew.find('.div-open');
    divOpen.append('<div class="div-param-text"><span class="device-status-name">Open: ' + 50 * open + '%</span></div>');
    divOpen.append('<div class="div-param-btn"></div>');
    var divBtn = divOpen.find('.div-param-btn');
    divBtn.append('<a class="btn-down" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Open"><img class="device-div-speed-img" src="../Content/images/btn_down.png" /></a>');
    divBtn.append('<a class="btn-up" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Open"><img class="device-div-speed-img" src="../Content/images/btn_up.png" /></a>');

    divBtn.children('.btn-down').on('click', sendDown);
    divBtn.children('.btn-up').on('click', sendUp);
}

function putVolume(divStatusNew, id, volume, sType) {
    var divVolume = divStatusNew.find('.div-volume');
    divVolume.append('<div class="div-param-text"><span class="device-status-name">Volume</span></div>');
    divVolume.append('<div class="div-param-btn"></div>');
    var divBtn = divVolume.find('.div-param-btn');
    divBtn.append('<a class="btn-down" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Volume"><img class="device-div-speed-img" src="../Content/images/btn_down.png" /></a>');
    divBtn.append('<img class="device-div-speed-state" src="../Content/images/val_' + volume + '.png")" />');
    divBtn.append('<a class="btn-up" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Volume"><img class="device-div-speed-img" src="../Content/images/btn_up.png" /></a>');

    divBtn.children('.btn-down').on('click', sendDown);
    divBtn.children('.btn-up').on('click', sendUp);
}


function putProgram(divStatusNew, id, channel, sType) {
    var divProgram = divStatusNew.find('.div-program');
    divProgram.append('<div class="div-param-text"><span class="device-status-name">Program</span><img class="device-div-prog-state" src="../Content/images/icon_' + channel + '.png")" /></div>');
    divProgram.append('<div class="div-param-btn"></div>');
    var divBtn = divProgram.find('.div-param-btn');
    divBtn.append('<a class="btn-down" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Program"><img class="device-div-speed-img" src="../Content/images/btn_down.png" /></a>');
    divBtn.append('<a class="btn-up" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Program"><img class="device-div-speed-img" src="../Content/images/btn_up.png" /></a>');

    divBtn.children('.btn-down').on('click', sendDown);
    divBtn.children('.btn-up').on('click', sendUp);
}

function putOnOff(divStatusNew, id, state, sType) {
    divStatusNew.append('<div class="device-div-on-off"></div>');
    var divOnOff = divStatusNew.find('.device-div-on-off:last');

    divOnOff.append('<div class="device-div-on-off-left"><span class="device-status-name">Turn on/off</span></div>');
    divOnOff.append('<div class="device-div-on-off-right"><a class="device-div-on-off-cmd-btn" href="" data-id="' + id + '" data-type="' + sType + '"></a></div>');
    var btn = divOnOff.find('.device-div-on-off-cmd-btn');

    if (state = "True") {
        btn.append('<img class="device-div-on-off-cmd-img" src="../Content/images/btn_on.png" />');
    } else {
        btn.append('<img class="device-div-on-off-cmd-img" src="../Content/images/btn_off.png" />');
    }

    btn.on('click', sendOnOff);
}
