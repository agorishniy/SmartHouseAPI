﻿window.onload = function () {
    // Set same hieght left and right colum
    sameCol();

    $('.btn-add').on('click', addDevice);
    $('.device-div-on-off-cmd-btn').on('click', onoffParam);
    $('.btn-down').on('click', downParam);
    $('.btn-up').on('click', upParam);
    $('.device-status-btn-del').on('click', delDevice);
};

// Set same hieght left and right colum
function sameCol() {
    $('.left-col').height($('.right-col').height() + 9);
}



function addDevice(event) {
    event.preventDefault();

    var imgBtn = $(this).children("img");
    var divStatus = $(this).closest(".device-status");
    var divState = $(this).closest(".device-status").children(".div-state");
    var divOnOff = $(this).closest(".device-div-on-off");

    var sType = $(this).attr('data-type');
    var sName = $(".name-of-new-device").val();

    var devNew = {
        Type: sType,
        Name: sName
    }

    $.ajax({
        url: "/api/values/add",
        type: "POST",
        data: JSON.stringify(devNew),
        contentType: "application/json;charset=utf-8",
        success: function (dev) {
            if (dev != null) {
                $("#RightCol").append('<div class="device-status"></div>');
                var divStatusNew = $("#RightCol .device-status:last");

                displayHeader(divStatusNew, dev.Id, sType, sName);
                displayState(divStatusNew, dev, sType);

                if (dev.State == true) {
                    switch (sType) {
                        case "Fan":
                            divStatusNew.append('<div class="div-status-block div-speed"></div>');
                            displaySpeed(divStatusNew, dev.Id, dev.Speed.Value, sType);
                            break;
                        case "Louvers":
                            divStatusNew.append('<div class="div-status-block div-open"></div>');
                            displayOpen(divStatusNew, dev.Id, dev.Open.Value, sType);
                            break;
                        case "Tv":
                            divStatusNew.append('<div class="div-status-block div-volume"></div>');
                            displayVolume(divStatusNew, dev.Id, dev.Volume.Value, sType);

                            divStatusNew.append('<div class="div-status-block div-program"></div>');
                            displayProgram(divStatusNew, dev.Id, dev.Channel, sType);
                            break;
                    }
                }

                displayOnOff(divStatusNew, dev.Id, dev.State, sType);

                $(".name-of-new-device").val('');
                sameCol();
            }
        }
    });
};

// Decrease parameter's value of device
function downParam(event) {
    changeParam(event, $(this), "down");
}

// Increase parameter's value of device
function upParam(event) {
    changeParam(event, $(this), "up");
}

// Change state of device (On/Off)
function onoffParam(event) {
    changeParam(event, $(this), "onoff");
}

// Change parameter's value of device
function changeParam(event, elem, cmd) {
    event.preventDefault();
    var divParam = elem.closest(".div-status-block");
    var imgParam = divParam.find(".device-div-speed-state");
    var divState = elem.closest(".device-status").children(".div-state");

    var imgBtn = elem.children("img");
    var divStatus = elem.closest(".device-status");
//    var divState = elem.closest(".device-status").children(".div-state");
    var divOnOff = elem.closest(".device-div-on-off");


    var sType = elem.attr('data-type');
    var id = elem.attr('data-id');
    var sParam = elem.attr('data-param');

    var devParam = {
        Id: id,
        Type: sType,
        Param: sParam,
        Cmd: cmd
    }

    $.ajax({
        url: "/api/values/param",
        type: "PUT",
        data: JSON.stringify(devParam),
        contentType: "application/json;charset=utf-8",
        success: function (newValue) {
            if (newValue != -1) {   // if no error
                if (cmd == "onoff") {
                    if (newValue == 1) {    // if device state is ON
                        displayOnStatus(elem);  // display all parameter of device
                    } else {    // if device state is OFF, then remove blocks with parameters of device
                        imgBtn.attr('src', '../Content/images/btn_off.png');
                        divState.children("span").html("State: OFF");
                        divState.children("img").attr('src', '../Content/images/' + sType + '_off.png');
                        switch (sType) {
                            case "Fan":
                                divStatus.children('.div-speed').remove();
                                break;
                            case "Louvers":
                                divStatus.children('.div-open').remove();
                                break;
                            case "Tv":
                                divStatus.children('.div-program').remove();
                                divStatus.children('.div-volume').remove();
                                break;
                        }
                    }
                } else {    // if cmd is "up" or "down"
                    switch (sType) {
                        case "Fan":
                            if (sParam == "Speed") {
                                imgParam.attr('src', '../Content/images/val_' + newValue + '.png');
                                divState.children("img").attr('src', '../Content/images/fan_' + newValue + '.gif');
                            }
                            break;
                        case "Louvers":
                            if (sParam == "Open") {
                                divState.children("img").attr('src', '../Content/images/louvers_' + newValue + '.png');
                                divParam.find('.device-status-name').html("Open: " + 50 * Number(newValue) + "%");
                            }
                            break;
                        case "Tv":
                            if (sParam == "Volume") {
                                imgParam.attr('src', '../Content/images/val_' + newValue + '.png');
                            }
                            if (sParam == "Program") {
                                divState.children("img").attr('src', '../Content/images/tv_' + newValue + '.png');
                                divParam.find('.device-div-prog-state').attr('src', '../Content/images/tv_icon_' + newValue + '.png');
                            }
                            break;
                    }
                }
            }
        }
    });
};


// Delete device
function delDevice(event) {
    event.preventDefault();
    var divStatus = $(this).closest(".device-status");

    var devInfo = {
        Id: $(this).attr('data-id'),
        Type: $(this).attr('data-type')
    }

    $.ajax({
        url: "/api/values/del",
        type: "DELETE",
        data: JSON.stringify(devInfo),
        contentType: "application/json;charset=utf-8",
        success: function (statusOk) {
            if (statusOk == 0) {
                divStatus.remove();
                sameCol();
            }
        }
    });
};




// Display state of device to On
function displayOnStatus(elem) {
    var imgBtn = elem.children("img");
    var divStatus = elem.closest(".device-status");
    var divState = elem.closest(".device-status").children(".div-state");
    var divOnOff = elem.closest(".device-div-on-off");
    var sType = elem.attr('data-type');
    var id = elem.attr('data-id');

    $.ajax({
        url: "/api/values/" + id + "/" + sType,
        type: "GET",
        success: function (dev) {
            if (dev != null) {
                imgBtn.attr('src', '../Content/images/btn_on.png');
                divState.children("span").html("State: ON");
                switch (sType) {
                    case "Lamp":
                        divState.children("img").attr('src', '../Content/images/lamp_on.png');
                        break;
                    case "Fan":
                        divState.children("img").attr('src', '../Content/images/fan_' + dev.Speed.Value + '.gif');

                        divOnOff.before('<div class="div-status-block div-speed"></div>');
                        displaySpeed(divStatus, id, dev.Speed.Value, sType);
                        break;
                    case "Louvers":
                        divState.children("img").attr('src', '../Content/images/louvers_' + dev.Open.Value + '.png');

                        divOnOff.before('<div class="div-status-block div-open"></div>');
                        displayOpen(divStatus, id, dev.Open.Value, sType);
                        break;
                    case "Tv":
                        divState.children("img").attr('src', '../Content/images/tv_' + dev.Channel + '.png');

                        divOnOff.before('<div class="div-status-block div-volume"></div>');
                        displayVolume(divStatus, id, dev.Volume.Value, sType);

                        divOnOff.before('<div class="div-status-block div-program"></div>');
                        displayProgram(divStatus, id, dev.Channel, sType);
                        break;
                }
                sameCol();
            }
        }
    });
};




// Display on page block Header of device
function displayHeader(divStatusNew, id, sType, sName) {
    divStatusNew.append('<div class="device-status-header"></div>');
    var divHeader = divStatusNew.children(".device-status-header");

    divHeader.append('<img class="device-status-icon" src="../Content/images/' + sType + '_icon.png" alt="Device icon" />');
    divHeader.append('<span class="device-status-name">' + sName + '</span>');
    divHeader.append('<a class="device-status-btn-del" href="" data-id="' + id + '" data-type="' + sType + '"></a>');
    divHeader.children('.device-status-btn-del').append('<img class="device-status-img-del" src="../Content/images/del_green_2.png" alt="Delete" />');

    divHeader.children('.device-status-btn-del').on('click', delDevice);
}


// Display on page block State of device
function displayState(divStatusNew, dev, sType) {
    divStatusNew.append('<div class="device-div-on-off div-state"></div>');
    var divState = divStatusNew.children(".div-state");

    if (dev.State == true)
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
            if (dev.State == true) { 
                sImgFile = "lamp_on.png"; 
            } else {
                sImgFile = "lamp_off.png"; 
            }
            break;
        case "Fan":
            if (dev.State == true) {
                sImgFile = "fan_" + dev.Speed.Value + ".gif";
            } else {
                sImgFile = "fan_off.png";
            }
            break;
        case "Louvers":
            if (dev.State == true) {
                sImgFile = "louvers_" + dev.Open.Value + ".png";
            } else {
                sImgFile = "louvers_off.png";
            }
            break;
        case "Tv":
            if (dev.State == true) {
                sImgFile = "tv_" + dev.Channel + ".png";
            } else {
                sImgFile = "tv_off.png";
            }
            break;
    }

    divState.append('<img class="device-image" src="../Content/images/' + sImgFile + '" />');
}

// Display on page block Speed of device
function displaySpeed(divStatusNew, id, speed, sType) {
    var divSpeed = divStatusNew.find('.div-speed');
    divSpeed.append('<div class="div-param-text"><span class="device-status-name">Speed</span></div>');
    divSpeed.append('<div class="div-param-btn"></div>');
    var divBtn = divSpeed.find('.div-param-btn');
    divBtn.append('<a class="btn-down" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Speed"><img class="device-div-speed-img" src="../Content/images/btn_down.png" /></a>');
    divBtn.append('<img class="device-div-speed-state" src="../Content/images/val_' + speed + '.png")" />');
    divBtn.append('<a class="btn-up" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Speed"><img class="device-div-speed-img" src="../Content/images/btn_up.png" /></a>');

    divBtn.children('.btn-down').on('click', downParam);
    divBtn.children('.btn-up').on('click', upParam);
}


// Display on page block Open of device
function displayOpen(divStatusNew, id, open, sType) {
    var divOpen = divStatusNew.find('.div-open');
    divOpen.append('<div class="div-param-text"><span class="device-status-name">Open: ' + 50 * open + '%</span></div>');
    divOpen.append('<div class="div-param-btn"></div>');
    var divBtn = divOpen.find('.div-param-btn');
    divBtn.append('<a class="btn-down" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Open"><img class="device-div-speed-img" src="../Content/images/btn_down.png" /></a>');
    divBtn.append('<a class="btn-up" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Open"><img class="device-div-speed-img" src="../Content/images/btn_up.png" /></a>');

    divBtn.children('.btn-down').on('click', downParam);
    divBtn.children('.btn-up').on('click', upParam);
}


// Display on page block Volume of device
function displayVolume(divStatusNew, id, volume, sType) {
    var divVolume = divStatusNew.find('.div-volume');
    divVolume.append('<div class="div-param-text"><span class="device-status-name">Volume</span></div>');
    divVolume.append('<div class="div-param-btn"></div>');
    var divBtn = divVolume.find('.div-param-btn');
    divBtn.append('<a class="btn-down" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Volume"><img class="device-div-speed-img" src="../Content/images/btn_down.png" /></a>');
    divBtn.append('<img class="device-div-speed-state" src="../Content/images/val_' + volume + '.png")" />');
    divBtn.append('<a class="btn-up" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Volume"><img class="device-div-speed-img" src="../Content/images/btn_up.png" /></a>');

    divBtn.children('.btn-down').on('click', downParam);
    divBtn.children('.btn-up').on('click', upParam);
}

// Display on page block Program (channel) of device
function displayProgram(divStatusNew, id, channel, sType) {
    var divProgram = divStatusNew.find('.div-program');
    divProgram.append('<div class="div-param-text"><span class="device-status-name">Program</span><img class="device-div-prog-state" src="../Content/images/tv_icon_' + channel + '.png")" /></div>');
    divProgram.append('<div class="div-param-btn"></div>');
    var divBtn = divProgram.find('.div-param-btn');
    divBtn.append('<a class="btn-down" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Program"><img class="device-div-speed-img" src="../Content/images/btn_down.png" /></a>');
    divBtn.append('<a class="btn-up" href="" data-id="' + id + '" data-type="' + sType + '" data-param="Program"><img class="device-div-speed-img" src="../Content/images/btn_up.png" /></a>');

    divBtn.children('.btn-down').on('click', downParam);
    divBtn.children('.btn-up').on('click', upParam);
}

// Display on page block with button On/Off of device
function displayOnOff(divStatusNew, id, state, sType) {
    divStatusNew.append('<div class="device-div-on-off"></div>');
    var divOnOff = divStatusNew.find('.device-div-on-off:last');

    divOnOff.append('<div class="device-div-on-off-left"><span class="device-status-name">Turn on/off</span></div>');
    divOnOff.append('<div class="device-div-on-off-right"><a class="device-div-on-off-cmd-btn" href="" data-id="' + id + '" data-type="' + sType + '"></a></div>');
    var btn = divOnOff.find('.device-div-on-off-cmd-btn');

    if (state = true) {
        btn.append('<img class="device-div-on-off-cmd-img" src="../Content/images/btn_on.png" />');
    } else {
        btn.append('<img class="device-div-on-off-cmd-img" src="../Content/images/btn_off.png" />');
    }

    btn.on('click', onoffParam);
}
