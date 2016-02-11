"use strict";

var my = my || {};

my.viewModel = (function () {
    var userId = ko.observable();
    var token = ko.observable();

    var generate = function () {
        var url = "/OneTimePassword/Generate";
        $.getJSON(url, {
            userId: userId()
        }).done(function (data) {
            token(data);
        }).fail(function (xhr) {
            var error = xhr.responseText;
            Console.log(error);
        });
    };

    var validate = function () {

    };

    var getJson = function (url, parameters, done) {

    };

    return {
        userId: userId,
        token: token,
        generate: generate,
        validate: validate,
    }
}());

$(document).ready(function () {
    ko.applyBindings(my.viewModel);
});