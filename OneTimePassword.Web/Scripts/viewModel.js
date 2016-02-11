"use strict";

var my = my || {};

my.viewModel = (function () {
    var userId = ko.observable("");
    var token = ko.observable("");
    var message = ko.observable("");

    var statusEnum = {
        NONE: 0,
        VALID: 1,
        INVALID: 2,
        ERROR: 3
    }
    var status = ko.observable(statusEnum.NONE);

    var generate = function () {
        var url = "/OneTimePassword/Generate";
        var parameters = { userId: userId() };
        var done = function (result) {
            token(result);
        };
        getJson(url, parameters, done);
    };

    var validate = function () {
        var url = "/OneTimePassword/Validate";
        var parameters = {
            userId: userId(),
            token: token()
        };
        var done = function (result) {
            if (result) {
                status(statusEnum.VALID);
            }
            else {
                status(statusEnum.INVALID);
            }
        };
        getJson(url, parameters, done);
    };

    var getJson = function (url, parameters, done) {
        clear();

        $.getJSON(url, parameters)
        .done(function (result) {
            done(result);
        })
        .fail(function (xhr) {
            status(statusEnum.ERROR);
            var error = xhr.responseText;
            message(error);
        });
    };

    var clear = function() {
        status(statusEnum.NONE);
        message("");
    };

    return {
        userId: userId,
        token: token,
        message: message,
        generate: generate,
        validate: validate,
        status: status,
        statusEnum: statusEnum,
    }
}());

$(document).ready(function () {
    ko.applyBindings(my.viewModel);
});