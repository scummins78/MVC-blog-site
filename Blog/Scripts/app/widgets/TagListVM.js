define("TagListVM", ["DataService"], function (dataService) {

    var TagListVM = function TagListVM() {

        var tagColumns = 2;

        var self = this;
        self.Tags = ko.observableArray();
        self.IsLoading = ko.observable(false);

        // column array of links
        self.TagRows = ko.computed(function () {
            var items = [];
            var row;

            // loop through items and push into a row array
            for (var i = 0, c = self.Tags().length; i < c; i++) {
                if (i % tagColumns === 0) {
                    if (row) {
                        items.push(row);
                    }
                    row = [];
                }
                row.push(self.Tags()[i]);
            }

            // push the final row
            if (row) {
                items.push(row);
            }

            return items;
        }, this);

        self.Load = function () {
            this.IsLoading(true);

            dataService.getTags().done(function (data) {
                self.Tags(data);
            });

            self.IsLoading(false);
        };

        // initialize
        this.Load();
    };

    return TagListVM;
});

require(["TagListVM"], function (TagListVM) {
    var viewModel = new TagListVM();
    ko.applyBindings(viewModel, document.getElementById("tagList"));
});
