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
        for (var i=0, c = self.Links().length; i < c; i++){
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

        $.getJSON("/Blog/widget/archivelist", function (returnData) {

            if (returnData.Success) {
                self.Links(returnData.Data);
            }

            self.IsLoading(false);
        });
    };

    // initialize
    this.Load();
};
// TODO:  move this code to a central initialize area.  
$(document).ready(function () {
    var viewModel = new ArchivePostsVM();
    ko.applyBindings(viewModel, document.getElementById("archiveList"));
});