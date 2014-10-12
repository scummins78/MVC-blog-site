var ArchivePostsVM = function ArchivePostsVM() {

    var self = this;
    self.Links = ko.observableArray();
    self.IsLoading = ko.observable(false);

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