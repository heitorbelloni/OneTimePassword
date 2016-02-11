"use strict";

var my = my || {};

my.viewModel = (function () {
    var userId = ko.observable();
    var token = ko.observable();
    var message = ko.observable();

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
            //
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
            var error = xhr.responseText;
            message(error);
        });
    };

    var clear = function() {
        message("");
    };

    return {
        userId: userId,
        token: token,
        message: message,
        generate: generate,
        validate: validate,
    }
}());

$(document).ready(function () {
    ko.applyBindings(my.viewModel);
});