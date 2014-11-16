define("ArchivePostsVM", ["DataService"], function (dataService) {

    var ArchivePostsVM = function ArchivePostsVM() {

        var linkColumns = 2;

        var self = this;
        self.Links = ko.observableArray();
        self.IsLoading = ko.observable(false);

        // column array of links
        self.LinkRows = ko.computed(function () {
            var items = [];
            var row;

            // loop through items and push into a row array
            for (var i = 0, c = self.Links().length; i < c; i++) {
                if (i % linkColumns === 0) {
                    if (row) {
                        items.push(row);
                    }
                    row = [];
                }
                row.push(self.Links()[i]);
            }

            // push the final row
            if (row) {
                items.push(row);
            }

            return items;
        }, this);

        self.Load = function () {
            this.IsLoading(true);

            dataService.getArchiveItems().done(function (data) {
                self.Links(data);
            });
            self.IsLoading(false);
        };

        // initialize
        this.Load();
    };

    return ArchivePostsVM;
});

require(["ArchivePostsVM"], function (ArchivePostsVM) {
    var viewModel = new ArchivePostsVM();
    ko.applyBindings(viewModel, document.getElementById("archiveList"));
});

