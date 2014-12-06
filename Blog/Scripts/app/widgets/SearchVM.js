define("SearchVM", ["config"], function (config) {

    var SearchVM = function SearchVM() {

        var self = this;
        self.SearchTerm = ko.observable("");

        self.Search = function(){
            window.location.href = config.baseUrl + "/search/" + self.SearchTerm();
        }
    };

    return SearchVM;
});

require(["SearchVM"], function (SearchVM) {
    var viewModel = new SearchVM();
    ko.applyBindings(viewModel, document.getElementById("searchBox"));
});
