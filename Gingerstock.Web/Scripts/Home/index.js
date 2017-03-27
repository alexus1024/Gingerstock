if (!serverData)
    throw "DEV! Please setup serverData var in Razor view";

function init()
{
    function getBuySellInfo() {
        return {
            LotForEdit: {
                Price: ko.observable(),
                Quantity: ko.observable(),
                BrokerEmail: ko.observable(),
            },
            Lots: ko.observableArray(),
            IsLoading: ko.observable(true),
        };
    }

    var viewModel = {
        BuyInfo: getBuySellInfo(),
        SellInfo: getBuySellInfo(),

        Transactions: ko.observableArray(),
        IsTransactionLoading: ko.observable(false),

    };


    viewModel.buy = function (lotInfoVm) {
        var model = ko.mapping.toJSON(lotInfoVm);
        vanillaPost(serverData.urlForBuyLots, model, viewModel.loadData);

    }

    viewModel.sell = function (lotInfoVm) {
        var model = ko.mapping.toJSON(lotInfoVm);
        vanillaPost(serverData.urlForSellLots, model, viewModel.loadData);

    }

    viewModel.loadData = function () {

        viewModel.BuyInfo.IsLoading(true);
        vanillaGet(serverData.urlForBuyLots, function(data) {
            ko.mapping.fromJSON(data, null, viewModel.BuyInfo.Lots);
            viewModel.BuyInfo.IsLoading(false);
        });

        viewModel.SellInfo.IsLoading(true);
        vanillaGet(serverData.urlForSellLots, function (data) {
            ko.mapping.fromJSON(data, null, viewModel.SellInfo.Lots);
            viewModel.SellInfo.IsLoading(false);
        });

        viewModel.IsTransactionLoading(true);
        vanillaGet(serverData.urlForTransactions, function (data) {
            ko.mapping.fromJSON(data, null, viewModel.Transactions);
            viewModel.IsTransactionLoading(false);
        });
    }

    viewModel.loadData();


    ko.applyBindings(viewModel);

    window.viewModel = viewModel;
}


function vanillaPost(url, body, success, error) {
    var stringBody = body;
    if (typeof(body) !== "string")
        stringBody = JSON.stringify(body);

    vanillaAjax(url, "POST", stringBody, success, error);
}

function vanillaGet(url, success, error) {
    vanillaAjax(url, "GET", null, success, error);
}

function vanillaAjax(url, method, jsonData, success, error) {
    // попробуем ajax без jQuery. Просто из интереса.
    var xmlhttp = new XMLHttpRequest();

    xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState === XMLHttpRequest.DONE) {
            if (xmlhttp.status >= 200 && xmlhttp.status < 300) {
                if(success)
                    success(xmlhttp.response);
            } else {
                if(error)
                    error(xmlhttp.status, xmlhttp.response);
            }
        }
    };

    xmlhttp.open(method, url);
    xmlhttp.setRequestHeader("content-type", "application/json");
    xmlhttp.setRequestHeader("accept", "application/json");

    xmlhttp.send(jsonData);
}

document.addEventListener('DOMContentLoaded', init, false);


